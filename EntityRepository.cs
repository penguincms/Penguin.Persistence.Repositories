using Penguin.Entities;
using Penguin.Messaging.Core;
using Penguin.Persistence.Abstractions.Interfaces;
using Penguin.Persistence.Repositories.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Penguin.Persistence.Repositories
{
    /// <summary>
    /// The base repository responsible for managing all CMS entities
    /// </summary>
    /// <typeparam name="T">Any CMS entity type</typeparam>
    public class EntityRepository<T> : KeyedObjectRepository<T>, IEntityRepository<T> where T : Entity
    {
        /// <summary>
        /// Constructs a new instance of this repository
        /// </summary>
        /// <param name="dbContext">Any persistence context for this entity</param>
        /// <param name="messageBus">An optional message bus to use for persistence messages</param>
        public EntityRepository(IPersistenceContext<T> dbContext, MessageBus messageBus = null) : base(dbContext, messageBus)
        {
        }

        /// <summary>
        /// Gets an IEnumerable of objects based on the Guid
        /// </summary>
        /// <param name="guids">The guids to search for</param>
        /// <returns>A list of entities where their ID was found in the provided list</returns>
        public virtual IEnumerable<T> Get(params Guid[] guids)
        {
            return this.Where(e => guids.Contains(e.Guid));
        }

        /// <summary>
        /// Gets an entity based on its external id
        /// </summary>
        /// <param name="ExternalId">The external ID of the object to retrieve</param>
        /// <returns>An object with the matching ExternalID or null</returns>
        public virtual T Get(string ExternalId) => this.Get(new[] { ExternalId }).SingleOrDefault();

        /// <summary>
        /// Gets an IEnumerable of objects based on the External Ids
        /// </summary>
        /// <param name="ExternalIds">The External Ids to search for</param>
        /// <returns>A list of entities where their ID was found in the provided list</returns>
        public virtual IEnumerable<T> Get(params string[] ExternalIds)
        {
            return this.Where(e => ExternalIds.Contains(e.ExternalId));
        }

        /// <summary>
        /// Gets an IEnumerable of objects from the Persistence Context that match the provided list. Useful for refreshing from the context
        /// </summary>
        /// <param name="o">The matching objects to return</param>
        /// <returns>The matching objects</returns>
        public IEnumerable<T> Get(params T[] o) => this.Get(o.Select(e => e.Guid).ToArray());

        //Id might be 0 for static representations of entities. This should only be true for security groups
        //Which have a guid generated based on their external ID. If both properties are null, you're boned.

        /// <summary>
        /// Gets any individual object based on its Id (if saved) or its Guid (if not)
        /// </summary>
        /// <param name="o">the object to search for</param>
        /// <returns>The persistence context version of the object</returns>
        public T Get(T o) => o._Id == 0 ? this.Get(o.Guid) : this.Get(o._Id);

        /// <summary>
        /// Retrieves an object instance from the persistence context by its Guid
        /// </summary>
        /// <param name="guid">The Guid to look for</param>
        /// <returns>An object instance, or null</returns>
        public virtual T Get(Guid guid) => this.Get(new[] { guid }).SingleOrDefault();

        IList<Entity> IEntityRepository.Get() => this.Context.ToList().Cast<Entity>().ToList();

        /// <summary>
        /// Retrieves an object instance from the persistence context by its Guid
        /// </summary>
        /// <param name="guid">The Guid to look for</param>
        /// <returns>An object instance, or null</returns>
        object IEntityRepository.Get(Guid guid) => this.Get(new[] { guid }).SingleOrDefault();

        /// <summary>
        /// Gets an IEnumerable of objects based on the Guid
        /// </summary>
        /// <param name="guids">The guids to search for</param>
        /// <returns>A list of entities where their ID was found in the provided list</returns>
        IEnumerable IEntityRepository.Get(params Guid[] guids) => this.Get(guids);

        /// <summary>
        /// Gets an IEnumerable of objects based on the External Ids
        /// </summary>
        /// <param name="ExternalIds">The External Ids to search for</param>
        /// <returns>A list of entities where their ID was found in the provided list</returns>
        IEnumerable IEntityRepository.Get(params string[] ExternalIds) => this.Get(ExternalIds);

        /// <summary>
        /// Gets an entity based on its external id
        /// </summary>
        /// <param name="ExternalId">The external ID of the object to retrieve</param>
        /// <returns>An object with the matching ExternalID or null</returns>
        object IEntityRepository.Get(string ExternalId) => this.Get(new[] { ExternalId }).SingleOrDefault();
    }
}