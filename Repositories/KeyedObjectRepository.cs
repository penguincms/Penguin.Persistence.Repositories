using Penguin.Messaging.Abstractions.Interfaces;
using Penguin.Messaging.Core;
using Penguin.Messaging.Persistence.Messages;
using Penguin.Persistence.Abstractions.Interfaces;
using Penguin.Persistence.Abstractions.Models.Base;
using Penguin.Persistence.Repositories.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using Penguin.Reflection.Extensions;
using System.Threading.Tasks;

namespace Penguin.Persistence.Repositories
{
    /// <summary>
    /// Base repository for any objects inheriting from "Keyed Object"
    /// </summary>
    /// <typeparam name="T">Any object type inheriting from "KeyedObject" </typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Justification = "<Pending>")]
    public class KeyedObjectRepository<T> : IKeyedObjectRepository<T> where T : KeyedObject
    {
        ////This is needed to ensure that the assembly is marked as referenced in the manifest
        //private readonly Type x = typeof(System.Data.Entity.SqlServer.SqlProviderServices);

        /// <summary>
        /// Returns the (possibly) overridden IQueriable used to access database by the underlying persistence context
        /// </summary>
        public virtual IQueryable<T> All => this.Context.All;

        IQueryable ICrud.All => this.All;

        /// <summary>
        /// The underlying perisstence context that handles saving of the data this repository is accessing
        /// </summary>
        public IPersistenceContext<T> Context { get; internal set; }

        /// <summary>
        /// Returns the element type of the underlying persistence context
        /// </summary>
        public Type ElementType => this.All.ElementType;

        /// <summary>
        /// Returns the current expressions of the underlying persistence context
        /// </summary>
        public Expression Expression => this.All.Expression;

        /// <summary>
        /// Returns a bool indicating whether or not the underlying persistence context contains a set for storing the
        /// type represented by this repository
        /// </summary>
        public bool IsValid => (this.Context as IPersistenceContext).IsValid;

        /// <summary>
        /// Returns the Provider of the underlying PersistenceContext
        /// </summary>
        public IQueryProvider Provider => this.All.Provider;

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
                throw new ArgumentNullException($"Can not create instance of {this.GetType()} with null context of type {typeof(IPersistenceContext<T>)}");
            }

            this.MessageBus = messageBus;
            this.Context = context;
        }

        /// <summary>
        /// Adds a new object to the context
        /// </summary>
        /// <param name="o">The object to add</param>
        public virtual void Add(T o) => this.Context.Add(o);

        void ICrud.Add(object o) => this.Add((T)o);

        /// <summary>
        /// ShallowClones an object, resets the key, and then calls Add
        /// </summary>
        /// <param name="o">The object to clone</param>
        public virtual void AddCopy(T o)
        {
            T newObject = o.ShallowClone();

            newObject._Id = 0;

            this.Add(newObject);
        }

        /// <summary>
        /// ShallowClones an object, resets the key, and then calls Add
        /// </summary>
        /// <param name="o">The object to clone</param>
        void IRepository.AddCopy(object o) => this.AddCopy((T)o);

        /// <summary>
        /// Updates an object or adds it if it does not exist
        /// </summary>
        /// <param name="o">The object to add or update</param>
        public virtual void AddOrUpdate(T o) => this.Context.AddOrUpdate(o);

        void ICrud.AddOrUpdate(object o) => this.AddOrUpdate((T)o);

        /// <summary>
        /// ShallowClones an object, resets the key, and then calls AddOrUpdate
        /// </summary>
        /// <param name="o">The object to clone</param>
        public virtual void AddOrUpdateCopy(T o)
        {
            T newObject = o.ShallowClone();

            newObject._Id = 0;

            this.AddOrUpdate(newObject);
        }

        /// <summary>
        /// ShallowClones an object, resets the key, and then calls AddOrUpdate
        /// </summary>
        /// <param name="o">The object to clone</param>
        void IRepository.AddOrUpdateCopy(object o) => this.AddOrUpdateCopy((T)o);

        /// <summary>
        /// Adds a range of new objects or updates existing objects if they already exist
        /// </summary>
        /// <param name="o">The objects to add or update</param>
        public virtual void AddOrUpdateRange(IEnumerable<T> o) => this.Context.AddOrUpdateRange(o);

        void ICrud.AddOrUpdateRange(IEnumerable o) => this.AddOrUpdateRange(o.Cast<T>());

        /// <summary>
        /// Adds a range of new objects
        /// </summary>
        /// <param name="o">The objects to add</param>
        public virtual void AddRange(IEnumerable<T> o) => this.Context.AddRange(o);

        void ICrud.AddRange(IEnumerable o) => this.AddRange(o.Cast<T>());

        /// <summary>
        /// Cancels any open write contexts and prevents changes from persisting
        /// </summary>
        public void CancelWrite() => this.Context.CancelWrite();

        /// <summary>
        /// If all WriteContexts have been deregistered, this should persist any changes to the underlying data store
        /// </summary>
        /// <param name="writeContext">The IWriteContext that has finished making changes</param>
        public void Commit(IWriteContext writeContext) => this.Context.Commit(writeContext);

        /// <summary>
        /// If all WriteContexts have been deregistered, this should persist any changes to the underlying data store in an ASYNC manner
        /// </summary>
        /// <param name="writeContext">The IWriteContext that has finished making changes</param>
        public Task CommitASync(IWriteContext writeContext) => this.Context.CommitASync(writeContext);

        /// <summary>
        /// Deletes an object from the persistence context
        /// </summary>
        /// <param name="o">The object to delete</param>
        public virtual void Delete(T o) => this.Context.Delete(o);

        void ICrud.Delete(object o) => this.Delete((T)o);

        /// <summary>
        /// Deletes multiple objects from the persistence context
        /// </summary>
        /// <param name="o">The objects to delete</param>
        public virtual void DeleteRange(IEnumerable<T> o) => this.Context.DeleteRange(o);

        void ICrud.DeleteRange(IEnumerable o) => this.DeleteRange(o.Cast<T>());

        /// <summary>
        /// Gets an IEnumerable of objects from the Persistence Context that match the provided list. Useful for refreshing from the context
        /// </summary>
        /// <param name="o">The matching objects to return</param>
        /// <returns>The matching objects</returns>
        public virtual IEnumerable<T> Find(IEnumerable<T> o) => this.Context.FindRange(o);

        /// <summary>
        /// Finds a KeyedObject by its int Id
        /// </summary>
        /// <param name="Id">The Id to find</param>
        /// <returns>An object with a matching ID or null</returns>
        public virtual T Find(int Id) => this.Context.Find(Id);

        /// <summary>
        /// Finds the object by the key
        /// </summary>
        /// <param name="Key">The key to find</param>
        /// <returns>Any matching object or null</returns>
        public virtual T Find(object Key) => this.Find((int)Key);

        object ICrud.Find(object Key) => this.Find((int)Key);

        KeyedObject IKeyedObjectRepository.Find(int Id) => this.Find(Id);

        /// <summary>
        /// Finds a number of objects with matching Ids
        /// </summary>
        /// <param name="Ids">The Ids to find</param>
        /// <returns>Objects that match the Id</returns>
        public virtual IEnumerable<T> FindRange(IEnumerable<int> Ids) => this.Context.FindRange(Ids);

        IEnumerable<T> ICrud<T>.FindRange(IEnumerable Keys) => this.FindRange(Keys.Cast<int>());

        IEnumerable ICrud.FindRange(IEnumerable Key) => this.FindRange(Key.Cast<int>());

        IEnumerable<KeyedObject> IKeyedObjectRepository.FindRange(IEnumerable<int> Ids) => this.FindRange(Ids);

        /// <summary>
        /// This returns the Enumerator for the underlying IQueriable
        /// </summary>
        /// <returns>The Enumerator for the underlying IQueriable</returns>
        public IEnumerator<T> GetEnumerator() => this.Context.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.Context.GetEnumerator();

        /// <summary>
        /// Returns objects from the repository of the specified type, for repositories where more than one type exist
        /// </summary>
        /// <typeparam name="TDerived">The type to return</typeparam>
        /// <returns></returns>
        public IQueryable<TDerived> OfType<TDerived>() where TDerived : T => this.Context.OfType<TDerived>();

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
        public virtual object ShallowClone(object o) => this.ShallowClone((T)o);

        object IRepository.ShallowClone(object o) => this.ShallowClone((T)o);

        /// <summary>
        /// Updates an object but does not add it to the context if it does not exist
        /// </summary>
        /// <param name="o">The object to update</param>
        public virtual void Update(T o) => this.Context.Update(o);

        void ICrud.Update(object o) => this.Update((T)o);

        /// <summary>
        /// Updates a range of objects but does not add them to the context if they do not exist
        /// </summary>
        /// <param name="o">The objects to update</param>
        public virtual void UpdateRange(IEnumerable<T> o) => this.Context.UpdateRange(o);

        void ICrud.UpdateRange(IEnumerable o) => this.UpdateRange(o.Cast<T>());

        /// <summary>
        /// Allows for a "Where" call on a non generic instance by converting the provided expression tree to the implemented type
        /// </summary>
        /// <typeparam name="T1">An assumed type/base for this non-generic instance of the repository</typeparam>
        /// <param name="predicate">The Expression to pass to the underlying IQueriable</param>
        /// <returns>The results of evaluating the expression against the underlying IQueriable</returns>
        public IEnumerable<T1> Where<T1>(Expression<Func<T1, bool>> predicate) where T1 : class
        {
            Contract.Requires(predicate != null);

            Expression<Func<T, bool>> derivedExpr = Expression.Lambda<Func<T, bool>>(predicate.Body, predicate.Parameters);

            return this.Context.Where(derivedExpr).ToList().Cast<T1>();
        }

        /// <summary>
        /// Returns a new write context for the underlying persistence context
        /// </summary>
        /// <returns> a new write context for the underlying persistence context</returns>
        public IWriteContext WriteContext() => this.Context.WriteContext();
    }
}