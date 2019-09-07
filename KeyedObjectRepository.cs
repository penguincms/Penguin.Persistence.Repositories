using Penguin.Messaging.Abstractions.Interfaces;
using Penguin.Messaging.Core;
using Penguin.Messaging.Persistence.Messages;
using Penguin.Persistence.Abstractions.Interfaces;
using Penguin.Persistence.Abstractions.Models.Base;
using Penguin.Persistence.Repositories.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Penguin.Persistence.Repositories
{
    /// <summary>
    /// Base repository for any objects inheriting from "Keyed Object"
    /// </summary>
    /// <typeparam name="T">Any object type inheriting from "KeyedObject" </typeparam>
    public class KeyedObjectRepository<T> : IKeyedObjectRepository<T>, IMessageHandler where T : KeyedObject
    {
        ////This is needed to ensure that the assembly is marked as referenced in the manifest
        //private readonly Type x = typeof(System.Data.Entity.SqlServer.SqlProviderServices);

        /// <summary>
        /// Returns the (possibly) overridden IQueriable used to access database by the underlying persistence context
        /// </summary>
        public virtual IQueryable<T> All => this.Context.All;

        IQueryable IRepository.All => this.All;

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
        /// Constructs a new instance of this repository
        /// </summary>
        /// <param name="context">A persistence context allowing for persistence of the objects this repository manages</param>
        /// <param name="messageBus">An optional message bus for sending out persistence event messages</param>
        public KeyedObjectRepository(IPersistenceContext<T> context, MessageBus messageBus = null)
        {
            if (context is null)
            {
                throw new ArgumentNullException($"Can not create instance of {this.GetType().FullName} with null context of type {this.GetType().FullName}");
            }

            this.MessageBus = messageBus;
            this.Context = context;
        }

        /// <summary>
        /// This should add a new object to the underlying data store
        /// </summary>
        /// <param name="o">The object(s) to add to the data store</param>
        public virtual void Add(params T[] o) => this.Context.Add(o);

        void IRepository.Add(params object[] o) => this.Add(o.Cast<T>().ToArray());

        /// <summary>
        /// This should add a new object to the data store, or update an existing matching object
        /// </summary>
        /// <param name="o">The object(s) to add or update</param>
        public virtual void AddOrUpdate(params T[] o) => this.Context.AddOrUpdate(o);

        void IRepository.AddOrUpdate(params object[] o) => this.AddOrUpdate(o.Cast<T>().ToArray());

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
        /// Should handle any pre-create persistence messages for the type this repository represents
        /// </summary>
        /// <param name="create">A creatomg message containing the object being created</param>
        public virtual void Create(Creating<T> create)
        {
        }

        /// <summary>
        /// This should remove objects from the underlying data store, or make them inaccessible (if deleting is not prefered)
        /// </summary>
        /// <param name="o">The object(s) to remove from the data store</param>
        public virtual void Delete(params T[] o) => this.Context.Delete(o);

        /// <summary>
        /// Should handle any pre-Delete persistence messages for the type this repository represents
        /// </summary>
        /// <param name="delete">A delete message containing the object being deleted</param>
        public virtual void Delete(Deleting<T> delete)
        {
        }

        void IRepository.Delete(params object[] o) => this.Delete(o.Cast<T>().ToArray());

        /// <summary>
        /// Gets an IEnumerable of objects containing any that match the requested ID's
        /// </summary>
        /// <param name="Ids">The Ids to check for</param>
        /// <returns>an IEnumerable of objects containing any that match the requested ID's</returns>
        public virtual IEnumerable<T> Get(params int[] Ids) => this.Where(t => Ids.Contains(t._Id));

        /// <summary>
        /// Gets an object by its ID property
        /// </summary>
        /// <param name="Id">The ID property to get</param>
        /// <returns>An object (or null) matching the ID</returns>
        public virtual T Get(int Id) => this.Get(new[] { Id }).SingleOrDefault();

        object IKeyedObjectRepository.Get(int Id) => this.Get(new[] { Id }).SingleOrDefault();

        IEnumerable IKeyedObjectRepository.Get(params int[] Ids) => this.Get(Ids);

        List<object> IRepository.Get() => this.Context.ToList().Cast<object>().ToList();

        IList<KeyedObject> IKeyedObjectRepository.Get() => this.Context.ToList().Cast<KeyedObject>().ToList();

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
        /// This should update any objects that already exist in the underlying data store
        /// </summary>
        /// <param name="o">The objects to update from the underlying data store</param>
        public virtual void Update(params T[] o) => this.Context.Update(o);

        /// <summary>
        /// This should handle any pre-update messages on derived repositories
        /// </summary>
        /// <param name="update">An update message containing the object being updated</param>
        public virtual void Update(Updating<T> update)
        {
        }

        void IRepository.Update(params object[] o) => this.Update(o.Cast<T>().ToArray());

        /// <summary>
        /// Allows for a "Where" call on a non generic instance by converting the provided expression tree to the implemented type
        /// </summary>
        /// <typeparam name="T1">An assumed type/base for this non-generic instance of the repository</typeparam>
        /// <param name="predicate">The Expression to pass to the underlying IQueriable</param>
        /// <returns>The results of evaluating the expression against the underlying IQueriable</returns>
        public IEnumerable<T1> Where<T1>(Expression<Func<T1, bool>> predicate) where T1 : class
        {
            Expression<Func<T, bool>> derivedExpr = Expression.Lambda<Func<T, bool>>(predicate.Body, predicate.Parameters);

            return this.Context.Where(derivedExpr).ToList().Cast<T1>();
        }

        /// <summary>
        /// Returns a new write context for the underlying persistence context
        /// </summary>
        /// <returns> a new write context for the underlying persistence context</returns>
        public IWriteContext WriteContext() => this.Context.WriteContext();

        /// <summary>
        /// An optional message bus for sending out persistence event messages
        /// </summary>
        protected MessageBus MessageBus { get; set; }
    }
}