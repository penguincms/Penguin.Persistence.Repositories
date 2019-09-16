using Penguin.Persistence.Abstractions.Interfaces;
using Penguin.Persistence.Abstractions.Models.Base;

namespace Penguin.Persistence.Repositories.Interfaces
{
    /// <summary>
    /// An interface representing a repository that manages objects deriving from KeyedObject
    /// </summary>
    /// <typeparam name="T">Any KeyedObject type</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Justification = "<Pending>")]
    public interface IKeyedObjectRepositoryIO<T> : IRepository<T>, IKeyedObjectRepository where T : KeyedObject
    {
    }
}