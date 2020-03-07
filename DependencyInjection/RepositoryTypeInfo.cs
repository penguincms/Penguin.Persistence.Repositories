using System;

namespace Penguin.Persistence.Repositories.DependencyInjection
{
    internal class RepositoryTypeInfo
    {
        public Type ObjectType { get; set; }

        public Type RepositoryType { get; set; }

        public RepositoryTypeInfo(Type repositoryType, Type objectType)
        {
            this.RepositoryType = repositoryType;
            this.ObjectType = objectType;
        }
    }
}