using Penguin.Entities;

namespace Penguin.Persistence.Repositories.Interfaces
{
    /// <summary>
    /// An interface representing the base repository for all entities that inherit from "Entity"
    /// </summary>
    /// <typeparam name="T">Any Entity Type</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Justification = "<Pending>")]
    public interface IEntityRepository<T> : IEntityRepositoryI<T>, IEntityRepositoryO<T>, IEntityRepositoryIO<T>, IEntityRepository where T : Entity
    {
    }
}