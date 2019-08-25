using Penguin.Entities;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Penguin.Persistence.Repositories.Interfaces
{
    /// <summary>
    /// An interface representing the base repository for all entities that inherit from "Entity"
    /// </summary>
    /// <typeparam name="T">Any Entity Type</typeparam>
    public interface IEntityRepository<T> : IEntityRepositoryI<T>, IEntityRepositoryO<T>, IEntityRepositoryIO<T>, IEntityRepository where T : Entity
    {
    }

    /// <summary>
    /// An nongeneric interface representing the base repository for all entities that inherit from "Entity"
    /// </summary>
    public interface IEntityRepository : IKeyedObjectRepository
    {
        #region Methods

        /// <summary>
        /// Gets an entity by its guid
        /// </summary>
        /// <param name="guid">The guid to get</param>
        /// <returns>An entity matching the Guid, or null</returns>
        object Get(Guid guid);

        /// <summary>
        /// Gets an entity by its external ID
        /// </summary>
        /// <param name="ExternalId">The external Id of the entity to get</param>
        /// <returns>An entity with a matching external Id, or null</returns>
        object Get(string ExternalId);

        /// <summary>
        /// Gets an IEnumerable of entities where the Guid is found in the provided list
        /// </summary>
        /// <param name="guids">The list of Guids to search for</param>
        /// <returns>an IEnumerable of entities where the Guid is found in the provided list</returns>
        IEnumerable Get(params Guid[] guids);

        /// <summary>
        /// Gets an IEnumerable of entities where the External Id is found in the provided list
        /// </summary>
        /// <param name="ExternalIds">The list of External Ids to search for</param>
        /// <returns>an IEnumerable of entities where the External Id is found in the provided list</returns>
        IEnumerable Get(params string[] ExternalIds);

        /// <summary>
        /// Gets all objects in the repository
        /// </summary>
        /// <returns>All objects in the repository</returns>
        new IList<Entity> Get();

        #endregion Methods
    }
    /// <summary>
    /// An interface representing the base repository for all entities that inherit from "Entity"
    /// </summary>
    /// <typeparam name="T">Any Entity Type</typeparam>
    public interface IEntityRepositoryI<in T> : IKeyedObjectRepositoryI<T>, IEntityRepository where T : Entity
    {
    }
    /// <summary>
    /// An interface representing the base repository for all entities that inherit from "Entity"
    /// </summary>
    /// <typeparam name="T">Any Entity Type</typeparam>
    public interface IEntityRepositoryIO<T> : IKeyedObjectRepositoryIO<T>, IEntityRepository where T : Entity
    {
        #region Methods

        /// <summary>
        /// Gets a new instance from the internal method of storage of any entities that match the provided list
        /// </summary>
        /// <param name="o">Any entity(s) to return from the internal method of storage</param>
        /// <returns></returns>
        IEnumerable<T> Get(params T[] o);

        /// <summary>
        /// Gets a single entity from the internal method of storage matching the provided entity
        /// </summary>
        /// <param name="o">The entity to search for</param>
        /// <returns>A matching entity from the internal storage</returns>
        T Get(T o);

        #endregion Methods
    }
    /// <summary>
    /// An interface representing the base repository for all entities that inherit from "Entity"
    /// </summary>
    /// <typeparam name="T">Any Entity Type</typeparam>
    public interface IEntityRepositoryO<out T> : IKeyedObjectRepositoryO<T>, IEntityRepository where T : Entity
    {
        #region Methods
        /// <summary>
        /// Gets an entity by its guid
        /// </summary>
        /// <param name="guid">The guid to get</param>
        /// <returns>An entity matching the Guid, or null</returns>
        new T Get(Guid guid);

        /// <summary>
        /// Gets an entity by its external ID
        /// </summary>
        /// <param name="ExternalId">The external Id of the entity to get</param>
        /// <returns>An entity with a matching external Id, or null</returns>
        new T Get(string ExternalId);

        /// <summary>
        /// Gets an IEnumerable of entities where the Guid is found in the provided list
        /// </summary>
        /// <param name="guids">The list of Guids to search for</param>
        /// <returns>an IEnumerable of entities where the Guid is found in the provided list</returns>
        new IEnumerable<T> Get(params Guid[] guids);

        /// <summary>
        /// Gets an IEnumerable of entities where the External Id is found in the provided list
        /// </summary>
        /// <param name="ExternalIds">The list of External Ids to search for</param>
        /// <returns>an IEnumerable of entities where the External Id is found in the provided list</returns>
        new IEnumerable<T> Get(params string[] ExternalIds);

        #endregion Methods
    }
}