using Penguin.Debugging;
using Penguin.DependencyInjection.Abstractions.Enums;
using Penguin.DependencyInjection.Abstractions.Interfaces;
using Penguin.Persistence.Abstractions;
using Penguin.Persistence.Abstractions.Interfaces;
using Penguin.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Penguin.Persistence.Repositories.DependencyInjection
{
    /// <summary>
    /// Contains the interface that automatically registers IRepository implementations for KeyedObject types, as well as IPersistenceContexts
    /// </summary>
    public class DependencyRegistrations : IRegisterDependencies
    {
        /// <summary>
        /// Registers the dependencies
        /// </summary>
        public void RegisterDependencies(IServiceRegister serviceRegister)
        {
            if (serviceRegister is null)
            {
                throw new ArgumentNullException(nameof(serviceRegister));
            }

            List<Type> KeyedObjectTypes = TypeFactory.GetDerivedTypes(typeof(KeyedObject)).ToList();

            StaticLogger.Log($"Penguin.Persistence.Database.DependencyInjection: {Assembly.GetExecutingAssembly().GetName().Version}", StaticLogger.LoggingLevel.Call);

            List<Type> PersistenceContextTypes = TypeFactory.GetAllTypes().Where(t => !t.IsInterface && !t.IsAbstract && typeof(IPersistenceContext).IsAssignableFrom(t) && typeof(IPersistenceContext<Penguin.Persistence.Abstractions.KeyedObject>).IsAssignableFrom(t.MakeGenericType(typeof(Penguin.Persistence.Abstractions.KeyedObject))) && t.GetConstructors().Any(c => !c.GetParameters().Any(p => p.ParameterType.IsPrimitive || p.ParameterType == typeof(string)))).ToList();

            if (PersistenceContextTypes.Count == 0)
            {
                throw new Exception("Unable to find valid injectable type inheriting from " + typeof(IPersistenceContext<>));
            }
            else if (PersistenceContextTypes.Count > 1)
            {
                string Message = "Multiple persistence context types found \r\n\r\n";

                foreach (Type t in PersistenceContextTypes)
                {
                    Message += $"{t} ({t.Assembly.Location})\r\n";
                }

                throw new Exception(Message);
            }

            Type PersistenceContextType = PersistenceContextTypes.Single();
            foreach (Type t in KeyedObjectTypes)
            {
                StaticLogger.Log($"PPDI: Registering persistence context for {t}", StaticLogger.LoggingLevel.Call);

                serviceRegister.Register(typeof(IPersistenceContext<>).MakeGenericType(t), PersistenceContextType.MakeGenericType(t), ServiceLifetime.Transient);
            }

            serviceRegister.Register(typeof(IPersistenceContext), PersistenceContextType.MakeGenericType(typeof(KeyedObject)), ServiceLifetime.Transient);

            HashSet<Type> FoundRepositories = new HashSet<Type>();

            foreach (RepositoryTypeInfo ri in RepositoryHelper.GetRepositoriesAndBaseTypes())
            {
                FoundRepositories.Add(ri.ObjectType);

                StaticLogger.Log($"PPDI: Registering repository for {ri.RepositoryType} => {ri.ObjectType}", StaticLogger.LoggingLevel.Call);

                RecursiveRegisterRepository(serviceRegister, ri.RepositoryType);

                Type baseType = ri.RepositoryType.BaseType;
            }

            List<Type> RepositoryImplementations = TypeFactory.GetAllImplementations(typeof(IRepository)).Where(t => t.IsGenericType).OrderByDescending(t => GetHierarchy(t).Count()).ToList();

            foreach (Type keyedObjectType in KeyedObjectTypes)
            {
                if (FoundRepositories.Contains(keyedObjectType))
                { continue; }

                foreach (Type repoType in RepositoryImplementations)
                {
                    if (repoType.GetGenericArguments()[0].GetGenericParameterConstraints()[0].IsAssignableFrom(keyedObjectType))
                    {
                        RecursiveRegisterRepository(serviceRegister, repoType.MakeGenericType(keyedObjectType));
                        break;
                    }
                }
            }

            StaticLogger.Log($"PPDI: Completed registrations", StaticLogger.LoggingLevel.Final);
        }

        private static IEnumerable<Type> GetHierarchy(Type type)
        {
            while (type != null)
            {
                yield return type;
                type = type.BaseType;
            }
        }

        private static void RecursiveRegisterRepository(IServiceRegister serviceRegister, Type RepositoryType)
        {
            serviceRegister.Register(RepositoryType, RepositoryType, ServiceLifetime.Transient);

            RegisterInterfaces(serviceRegister, RepositoryType);

            Type baseType = RepositoryType.BaseType;

            while (baseType != null)
            {
                if (!baseType.IsGenericTypeDefinition)
                {
                    StaticLogger.Log($"PPDI: Registering repository for {baseType} => {RepositoryType}", StaticLogger.LoggingLevel.Call);

                    serviceRegister.Register(baseType, RepositoryType, ServiceLifetime.Transient);
                }

                baseType = baseType.BaseType;
            }
        }

        private static void RegisterInterfaces(IServiceRegister serviceRegister, Type RepositoryType)
        {
            foreach (Type i in RepositoryType.GetInterfaces())
            {
                if (i.IsGenericType && i.IsAssignableFrom(RepositoryType))
                {
                    StaticLogger.Log($"PPDI: Registering repository for {i} => {RepositoryType}", StaticLogger.LoggingLevel.Call);

                    serviceRegister.Register(i, RepositoryType, ServiceLifetime.Transient);
                }
            }
        }
    }
}