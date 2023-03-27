using Penguin.Persistence.Abstractions.Interfaces;
using Penguin.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using Loxifi;

namespace Penguin.Persistence.Repositories.DependencyInjection
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    public static class RepositoryHelper
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        /// <summary>
        /// Reflects over all current assembly types to find anything implementing the IRepository&lt;T&gt; interface
        /// </summary>
        /// <returns>A list of tuples containing the Repository Type as well as the Type of the entity it manages</returns>
        internal static IEnumerable<RepositoryTypeInfo> GetRepositoriesAndBaseTypes()
        {
            foreach (Type t in TypeFactory.Default.GetAllTypes(true))
            {
                if (IsValidRepositoryType(t))
                {
                    yield return new RepositoryTypeInfo(t, t.BaseType.GenericTypeArguments.First());
                }
            }
        }

        private static bool IsValidRepositoryType(Type t)
        {
            if (t.BaseType == null)
            {
                return false;
            }

            if (t.BaseType.IsAbstract)
            {
                return false;
            }

            if (t.BaseType.IsInterface)
            {
                return false;
            }

            if (!t.BaseType.IsGenericType)
            {
                return false;
            }

            return t.GetInterface(nameof(IRepository)) != null && t.BaseType.GenericTypeArguments.Length == 1;
        }
    }
}