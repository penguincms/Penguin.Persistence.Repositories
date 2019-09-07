using Penguin.Persistence.Abstractions.Interfaces;
using Penguin.Persistence.Abstractions.Models.Base;
using System.Collections;
using System.Collections.Generic;

namespace Penguin.Persistence.Repositories.Interfaces
{
    /// <summary>
    /// An interface representing a repository that manages objects deriving from KeyedObject
    /// </summary>
    /// <typeparam name="T">Any KeyedObject type</typeparam>
    public interface IKeyedObjectRepository<T> : IKeyedObjectRepositoryI<T>, IKeyedObjectRepositoryIO<T>, IKeyedObjectRepositoryO<T> where T : KeyedObject
    {
    }

    /// <summary>
    /// A non-generic interface for a keyed object repository allowing for data access without knowing the underlying object type for the repository
    /// </summary>
    public interface IKeyedObjectRepository : IRepository
    {
        /// <summary>
        /// Gets an object by its ID property
        /// </summary>
        /// <param name="Id">The ID property to get</param>
        /// <returns>An object (or null) matching the ID</returns>
        object Get(int Id);

        /// <summary>
        /// Gets all objects in the repository
        /// </summary>
        /// <returns>All objects in the repository</returns>
        new IList<KeyedObject> Get();

        /// <summary>
        /// Gets an IEnumerable of objects containing any that match the requested ID's
        /// </summary>
        /// <param name="Ids">The Ids to check for</param>
        /// <returns>an IEnumerable of objects containing any that match the requested ID's</returns>
        IEnumerable Get(params int[] Ids);
    }

    /// <summary>
    /// An interface representing a repository that manages objects deriving from KeyedObject
    /// </summary>
    /// <typeparam name="T">Any KeyedObject type</typeparam>
    public interface IKeyedObjectRepositoryI<in T> : IRepositoryI<T>, IKeyedObjectRepository where T : KeyedObject
    {
    }

    /// <summary>
    /// An interface representing a repository that manages objects deriving from KeyedObject
    /// </summary>
    /// <typeparam name="T">Any KeyedObject type</typeparam>
    public interface IKeyedObjectRepositoryIO<T> : IRepository<T>, IKeyedObjectRepository where T : KeyedObject
    {
    }

    /// <summary>
    /// An interface representing a repository that manages objects deriving from KeyedObject
    /// </summary>
    /// <typeparam name="T">Any KeyedObject type</typeparam>
    public interface IKeyedObjectRepositoryO<out T> : IRepositoryO<T>, IKeyedObjectRepository where T : KeyedObject
    {
        /// <summary>
        /// Gets an object by its ID property
        /// </summary>
        /// <param name="Id">The ID property to get</param>
        /// <returns>An object (or null) matching the ID</returns>
        new T Get(int Id);

        /// <summary>
        /// Gets an IEnumerable of objects containing any that match the requested ID's
        /// </summary>
        /// <param name="Ids">The Ids to check for</param>
        /// <returns>an IEnumerable of objects containing any that match the requested ID's</returns>
        new IEnumerable<T> Get(params int[] Ids);
    }
}