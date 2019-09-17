using Penguin.Persistence.Abstractions.Models.Base;

namespace Penguin.Persistence.Repositories.Interfaces
{
    /// <summary>
    /// An interface representing a repository that manages objects deriving from KeyedObject
    /// </summary>
    /// <typeparam name="T">Any KeyedObject type</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Justification = "<Pending>")]
    public interface IKeyedObjectRepository<T> : IKeyedObjectRepositoryI<T>, IKeyedObjectRepositoryIO<T>, IKeyedObjectRepositoryO<T> where T : KeyedObject
    {
        /// <summary>
        /// Resets the object unique fields and adds a copy to the context
        /// </summary>
        /// <param name="o">The object to copy and add</param>
        void AddCopy(T o);

        /// <summary>
        /// Resets the object unique fields and adds or updates a copy to the context
        /// </summary>
        /// <param name="o">The object to copy and add</param>
        void AddOrUpdateCopy(T o);

        /// <summary>
        /// Creates a shallow clone of an object with new keys
        /// </summary>
        /// <param name="o">The object to clone</param>
        T ShallowClone(T o);
    }
}