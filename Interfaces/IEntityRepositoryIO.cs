using Penguin.Entities;
using System.Collections.Generic;

namespace Penguin.Persistence.Repositories.Interfaces
{
    /// <summary>
    /// An interface representing the base repository for all entities that inherit from "Entity"
    /// </summary>
    /// <typeparam name="T">Any Entity Type</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Justification = "<Pending>")]
    public interface IEntityRepositoryIO<T> : IKeyedObjectRepositoryIO<T>, IEntityRepository where T : Entity
    {
        /// <summary>
        /// Gets a new instance from the internal method of storage of any entities that match the provided list
        /// </summary>
        /// <param name="o">Any entity(s) to return from the internal method of storage</param>
        /// <returns></returns>
        IEnumerable<T> Find(params T[] o);

        /// <summary>
        /// Gets a single entity from the internal method of storage matching the provided entity
        /// </summary>
        /// <param name="o">The entity to search for</param>
        /// <returns>A matching entity from the internal storage</returns>
        T Find(T o);
    }
}