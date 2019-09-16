using Penguin.Entities;

namespace Penguin.Persistence.Repositories.Interfaces
{
    /// <summary>
    /// An interface representing the base repository for all entities that inherit from "Entity"
    /// </summary>
    /// <typeparam name="T">Any Entity Type</typeparam>
    public interface IEntityRepositoryI<in T> : IKeyedObjectRepositoryI<T>, IEntityRepository where T : Entity
    {
    }
}