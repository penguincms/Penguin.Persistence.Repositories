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
        /// Gets an IEnumerable of objects containing any that match the requested ID's
        /// </summary>
        /// <param name="Ids">The Ids to check for</param>
        /// <returns>an IEnumerable of objects containing any that match the requested ID's</returns>
        IEnumerable FindRange(IEnumerable<int> Ids);
    }
}