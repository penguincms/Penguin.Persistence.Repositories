using Penguin.Persistence.Abstractions.Interfaces;
using Penguin.Persistence.Abstractions.Models.Base;

namespace Penguin.Persistence.Repositories.Interfaces
{
    /// <summary>
    /// An interface representing a repository that manages objects deriving from KeyedObject
    /// </summary>
    /// <typeparam name="T">Any KeyedObject type</typeparam>
    public interface IKeyedObjectRepositoryI<in T> : IRepositoryI<T>, IKeyedObjectRepository where T : KeyedObject
    {
    }
}