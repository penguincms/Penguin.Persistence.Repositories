﻿using Penguin.Messaging.Core;
using Penguin.Persistence.Abstractions;
using Penguin.Persistence.Abstractions.Interfaces;
using Penguin.Persistence.Repositories.Interfaces;
using Penguin.Reflection.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Penguin.Persistence.Repositories.Repositories
{
    /// <summary>
    /// Base repository for any objects inheriting from "Keyed Object"
    /// </summary>
    /// <typeparam name="T">Any object type inheriting from "KeyedObject" </typeparam>
    public class KeyedObjectRepository<T> : IKeyedObjectRepository<T> where T : KeyedObject
    {
        ////This is needed to ensure that the assembly is marked as referenced in the manifest
        //private readonly Type x = typeof(System.Data.Entity.SqlServer.SqlProviderServices);

        /// <summary>
        /// Returns the (possibly) overridden IQueryable used to access database by the underlying persistence context
        /// </summary>
        public virtual IQueryable<T> All => Context.All;

        IQueryable ICrud.All => All;

        /// <summary>
        /// The underlying persistence context that handles saving of the data this repository is accessing
        /// </summary>
        public IPersistenceContext<T> Context { get; internal set; }

        /// <summary>
        /// Returns the element type of the underlying persistence context
        /// </summary>
        public Type ElementType => All.ElementType;

        /// <summary>
        /// Returns the current expressions of the underlying persistence context
        /// </summary>
        public Expression Expression => All.Expression;

        /// <summary>
        /// Returns a bool indicating whether or not the underlying persistence context contains a set for storing the
        /// type represented by this repository
        /// </summary>
        public bool IsValid => Context.IsValid;

        /// <summary>
        /// Returns the Provider of the underlying PersistenceContext
        /// </summary>
        public IQueryProvider Provider => All.Provider;

        /// <summary>
        /// An optional message bus for sending out persistence event messages
        /// </summary>
        protected MessageBus MessageBus { get; set; }

        /// <summary>
        /// Constructs a new instance of this repository
        /// </summary>
        /// <param name="context">A persistence context allowing for persistence of the objects this repository manages</param>
        /// <param name="messageBus">An optional message bus for sending out persistence event messages</param>
        public KeyedObjectRepository(IPersistenceContext<T> context, MessageBus messageBus = null)
        {
            if (context is null)
            {
                throw new ArgumentNullException($"Can not create instance of {GetType()} with null context of type {typeof(IPersistenceContext<T>)}");
            }

            MessageBus = messageBus;
            Context = context;
        }

        /// <summary>
        /// Adds a new object to the context
        /// </summary>
        /// <param name="o">The object to add</param>
        public virtual void Add(T o)
        {
            Context.Add(o);
        }

        void ICrud.Add(object o)
        {
            Add((T)o);
        }

        /// <summary>
        /// ShallowClones an object, resets the key, and then calls Add
        /// </summary>
        /// <param name="o">The object to clone</param>
        public virtual void AddCopy(T o)
        {
            T newObject = o.ShallowClone();

            newObject._Id = 0;

            Add(newObject);
        }

        /// <summary>
        /// ShallowClones an object, resets the key, and then calls Add
        /// </summary>
        /// <param name="o">The object to clone</param>
        void IRepository.AddCopy(object o)
        {
            AddCopy((T)o);
        }

        /// <summary>
        /// Updates an object or adds it if it does not exist
        /// </summary>
        /// <param name="o">The object to add or update</param>
        public virtual void AddOrUpdate(T o)
        {
            Context.AddOrUpdate(o);
        }

        void ICrud.AddOrUpdate(object o)
        {
            AddOrUpdate((T)o);
        }

        /// <summary>
        /// ShallowClones an object, resets the key, and then calls AddOrUpdate
        /// </summary>
        /// <param name="o">The object to clone</param>
        public virtual void AddOrUpdateCopy(T o)
        {
            T newObject = o.ShallowClone();

            newObject._Id = 0;

            AddOrUpdate(newObject);
        }

        /// <summary>
        /// ShallowClones an object, resets the key, and then calls AddOrUpdate
        /// </summary>
        /// <param name="o">The object to clone</param>
        void IRepository.AddOrUpdateCopy(object o)
        {
            AddOrUpdateCopy((T)o);
        }

        /// <summary>
        /// Adds a range of new objects or updates existing objects if they already exist
        /// </summary>
        /// <param name="o">The objects to add or update</param>
        public virtual void AddOrUpdateRange(IEnumerable<T> o)
        {
            Context.AddOrUpdateRange(o);
        }

        void ICrud.AddOrUpdateRange(IEnumerable o)
        {
            AddOrUpdateRange(o.Cast<T>());
        }

        /// <summary>
        /// Adds a range of new objects
        /// </summary>
        /// <param name="o">The objects to add</param>
        public virtual void AddRange(IEnumerable<T> o)
        {
            Context.AddRange(o);
        }

        void ICrud.AddRange(IEnumerable o)
        {
            AddRange(o.Cast<T>());
        }

        /// <summary>
        /// Cancels any open write contexts and prevents changes from persisting
        /// </summary>
        public void CancelWrite()
        {
            Context.CancelWrite();
        }

        /// <summary>
        /// If all WriteContexts have been deregistered, this should persist any changes to the underlying data store
        /// </summary>
        /// <param name="writeContext">The IWriteContext that has finished making changes</param>
        public void Commit(IWriteContext writeContext)
        {
            Context.Commit(writeContext);
        }

        /// <summary>
        /// If all WriteContexts have been deregistered, this should persist any changes to the underlying data store in an ASYNC manner
        /// </summary>
        /// <param name="writeContext">The IWriteContext that has finished making changes</param>
        public Task CommitASync(IWriteContext writeContext)
        {
            return Context.CommitASync(writeContext);
        }

        /// <summary>
        /// Deletes an object from the persistence context
        /// </summary>
        /// <param name="o">The object to delete</param>
        public virtual void Delete(T o)
        {
            Context.Delete(o);
        }

        void ICrud.Delete(object o)
        {
            Delete((T)o);
        }

        /// <summary>
        /// Deletes multiple objects from the persistence context
        /// </summary>
        /// <param name="o">The objects to delete</param>
        public virtual void DeleteRange(IEnumerable<T> o)
        {
            Context.DeleteRange(o);
        }

        void ICrud.DeleteRange(IEnumerable o)
        {
            DeleteRange(o.Cast<T>());
        }

        /// <summary>
        /// Gets an IEnumerable of objects from the Persistence Context that match the provided list. Useful for refreshing from the context
        /// </summary>
        /// <param name="o">The matching objects to return</param>
        /// <returns>The matching objects</returns>
        public virtual IEnumerable<T> Find(IEnumerable<T> o)
        {
            return Context.FindRange(o);
        }

        /// <summary>
        /// Finds a KeyedObject by its int Id
        /// </summary>
        /// <param name="Id">The Id to find</param>
        /// <returns>An object with a matching ID or null</returns>
        public virtual T Find(int Id)
        {
            return Context.Find(Id);
        }

        /// <summary>
        /// Finds the object by the key
        /// </summary>
        /// <param name="Key">The key to find</param>
        /// <returns>Any matching object or null</returns>
        public virtual T Find(object Key)
        {
            return Find((int)Key);
        }

        object ICrud.Find(object Key)
        {
            return Find((int)Key);
        }

        KeyedObject IKeyedObjectRepository.Find(int Id)
        {
            return Find(Id);
        }

        /// <summary>
        /// Finds a number of objects with matching Ids
        /// </summary>
        /// <param name="Ids">The Ids to find</param>
        /// <returns>Objects that match the Id</returns>
        public virtual IEnumerable<T> FindRange(IEnumerable<int> Ids)
        {
            return Context.FindRange(Ids);
        }

        IEnumerable<T> ICrud<T>.FindRange(IEnumerable Keys)
        {
            return FindRange(Keys.Cast<int>());
        }

        IEnumerable ICrud.FindRange(IEnumerable Key)
        {
            return FindRange(Key.Cast<int>());
        }

        IEnumerable<KeyedObject> IKeyedObjectRepository.FindRange(IEnumerable<int> Ids)
        {
            return FindRange(Ids);
        }

        /// <summary>
        /// This returns the Enumerator for the underlying IQueryable
        /// </summary>
        /// <returns>The Enumerator for the underlying IQueryable</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return Context.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Context.GetEnumerator();
        }

        /// <summary>
        /// Returns objects from the repository of the specified type, for repositories where more than one type exist
        /// </summary>
        /// <typeparam name="TDerived">The type to return</typeparam>
        /// <returns></returns>
        public IQueryable<TDerived> OfType<TDerived>() where TDerived : T
        {
            return Context.OfType<TDerived>();
        }

        /// <summary>
        /// ShallowClones an object and resets its key
        /// </summary>
        /// <param name="o">The object to clone</param>
        /// <returns>The ShallowClone</returns>
        public virtual T ShallowClone(T o)
        {
            T newObject = o.ShallowClone();

            newObject._Id = 0;

            return newObject;
        }

        /// <summary>
        /// ShallowClones an object and resets its Key
        /// </summary>
        /// <param name="o">The object to shallow clone</param>
        /// <returns></returns>
        public virtual object ShallowClone(object o)
        {
            return ShallowClone((T)o);
        }

        object IRepository.ShallowClone(object o)
        {
            return ShallowClone((T)o);
        }

        /// <summary>
        /// Updates an object but does not add it to the context if it does not exist
        /// </summary>
        /// <param name="o">The object to update</param>
        public virtual void Update(T o)
        {
            Context.Update(o);
        }

        void ICrud.Update(object o)
        {
            Update((T)o);
        }

        /// <summary>
        /// Updates a range of objects but does not add them to the context if they do not exist
        /// </summary>
        /// <param name="o">The objects to update</param>
        public virtual void UpdateRange(IEnumerable<T> o)
        {
            Context.UpdateRange(o);
        }

        void ICrud.UpdateRange(IEnumerable o)
        {
            UpdateRange(o.Cast<T>());
        }

        /// <summary>
        /// Allows for a "Where" call on a non generic instance by converting the provided expression tree to the implemented type
        /// </summary>
        /// <typeparam name="T1">An assumed type/base for this non-generic instance of the repository</typeparam>
        /// <param name="predicate">The Expression to pass to the underlying IQueryable</param>
        /// <returns>The results of evaluating the expression against the underlying IQueryable</returns>
        public IEnumerable<T1> Where<T1>(Expression<Func<T1, bool>> predicate) where T1 : class
        {
            if (predicate is null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            Expression<Func<T, bool>> derivedExpr = Expression.Lambda<Func<T, bool>>(predicate.Body, predicate.Parameters);

            return Context.Where(derivedExpr).ToList().Cast<T1>();
        }

        /// <summary>
        /// Returns a new write context for the underlying persistence context
        /// </summary>
        /// <returns> a new write context for the underlying persistence context</returns>
        public IWriteContext WriteContext()
        {
            return Context.WriteContext();
        }
    }
}