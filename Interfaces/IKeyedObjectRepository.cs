using Penguin.Persistence.Abstractions.Interfaces;
using Penguin.Persistence.Abstractions.Models.Base;
using System.Collections;
using System.Collections.Generic;

namespace Penguin.Persistence.Repositories.Interfaces
{
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
        object Find(int Id);

        /// <summary>
        /// Gets all objects in the repository
        /// </summary>
        /// <returns>All objects in the repository</returns>
        new IList<KeyedObject> Find();

        /// <summary>
        /// Gets an IEnumerable of objects containing any that match the requested ID's
        /// </summary>
        /// <param name="Ids">The Ids to check for</param>
        /// <returns>an IEnumerable of objects containing any that match the requested ID's</returns>
        IEnumerable Find(params int[] Ids);

        /// <summary>
        /// Resets the object unique fields and adds a copy to the context
        /// </summary>
        /// <param name="o">The object to copy and add</param>
        void AddCopy(object o);

        /// <summary>
        /// Resets the object unique fields and adds or updates a copy to the context
        /// </summary>
        /// <param name="o">The object to copy and add</param>
        void AddOrUpdateCopy(object o);

        /// <summary>
        /// Creates a shallow clone of an object with new keys
        /// </summary>
        /// <param name="o">The object to clone</param>
        object ShallowClone(object o);
    }
}