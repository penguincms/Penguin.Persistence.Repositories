using Penguin.Entities;
using Penguin.Messaging.Core;
using Penguin.Persistence.Abstractions.Interfaces;
using Penguin.Persistence.Repositories.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Penguin.Persistence.Repositories
{
    /// <summary>
    /// The base repository responsible for managing all CMS entities
    /// </summary>
    /// <typeparam name="T">Any CMS entity type</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Justification = "<Pending>")]
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
        /// ShallowClones and object and resets its Guid/ExternalId/Id
        /// </summary>
        /// <param name="o">The object to clone</param>
        /// <returns>A clone of the object</returns>
        public override T ShallowClone(T o)
        {
            T newObject = base.ShallowClone(o);

            newObject.Guid = Guid.NewGuid();

            newObject.ExternalId = newObject.Guid.ToString();

            return newObject;
        }

        /// <summary>
        /// Gets an IEnumerable of objects based on the Guid
        /// </summary>
        /// <param name="guids">The guids to search for</param>
        /// <returns>A list of entities where their ID was found in the provided list</returns>
        public virtual IEnumerable<T> Find(params Guid[] guids)
        {
            return this.Where(e => guids.Contains(e.Guid));
        }

        /// <summary>
        /// Gets an entity based on its external id
        /// </summary>
        /// <param name="ExternalId">The external ID of the object to retrieve</param>
        /// <returns>An object with the matching ExternalID or null</returns>
        public virtual T Find(string ExternalId) => this.Find(new[] { ExternalId }).SingleOrDefault();

        /// <summary>
        /// Gets an IEnumerable of objects based on the External Ids
        /// </summary>
        /// <param name="ExternalIds">The External Ids to search for</param>
        /// <returns>A list of entities where their ID was found in the provided list</returns>
        public virtual IEnumerable<T> Find(params string[] ExternalIds)
        {
            return this.Where(e => ExternalIds.Contains(e.ExternalId));
        }

        /// <summary>
        /// Gets an IEnumerable of objects from the Persistence Context that match the provided list. Useful for refreshing from the context
        /// </summary>
        /// <param name="o">The matching objects to return</param>
        /// <returns>The matching objects</returns>
        public override IEnumerable<T> Find(params T[] o)
        {
            foreach (T to in o)
            {
                if (to._Id == 0)
                {
                    yield return this.Find(to._Id);
                }
                else
                {
                    yield return this.Find(to.Guid);
                }
            }
        }

        //Id might be 0 for static representations of entities. This should only be true for security groups
        //Which have a guid generated based on their external ID. If both properties are null, you're boned.

        /// <summary>
        /// Gets any individual object based on its Id (if saved) or its Guid (if not)
        /// </summary>
        /// <param name="o">the object to search for</param>
        /// <returns>The persistence context version of the object</returns>
        public T Find(T o)
        {
            Contract.Requires(o != null);
            return o._Id == 0 ? this.Find(o.Guid) : this.Find(o._Id);
        }

        /// <summary>
        /// Retrieves an object instance from the persistence context by its Guid
        /// </summary>
        /// <param name="guid">The Guid to look for</param>
        /// <returns>An object instance, or null</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "<Pending>")]
        public virtual T Find(Guid guid) => this.Find(new[] { guid }).SingleOrDefault();

        IList<Entity> IEntityRepository.Find() => this.Context.ToList().Cast<Entity>().ToList();

        /// <summary>
        /// Retrieves an object instance from the persistence context by its Guid
        /// </summary>
        /// <param name="guid">The Guid to look for</param>
        /// <returns>An object instance, or null</returns>
        object IEntityRepository.Find(Guid guid) => this.Find(new[] { guid }).SingleOrDefault();

        /// <summary>
        /// Gets an IEnumerable of objects based on the Guid
        /// </summary>
        /// <param name="guids">The guids to search for</param>
        /// <returns>A list of entities where their ID was found in the provided list</returns>
        IEnumerable IEntityRepository.Find(params Guid[] guids) => this.Find(guids);

        /// <summary>
        /// Gets an IEnumerable of objects based on the External Ids
        /// </summary>
        /// <param name="ExternalIds">The External Ids to search for</param>
        /// <returns>A list of entities where their ID was found in the provided list</returns>
        IEnumerable IEntityRepository.Find(params string[] ExternalIds) => this.Find(ExternalIds);

        /// <summary>
        /// Gets an entity based on its external id
        /// </summary>
        /// <param name="ExternalId">The external ID of the object to retrieve</param>
        /// <returns>An object with the matching ExternalID or null</returns>
        object IEntityRepository.Find(string ExternalId) => this.Find(new[] { ExternalId }).SingleOrDefault();

    }
}