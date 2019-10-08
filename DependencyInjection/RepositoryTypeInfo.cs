using System;
using System.Collections.Generic;
using System.Text;

namespace Penguin.Persistence.Repositories.DependencyInjection
{
    internal class RepositoryTypeInfo
    {
        public RepositoryTypeInfo(Type repositoryType, Type objectType)
        {
            RepositoryType = repositoryType;
            ObjectType = objectType;
        }
        public Type RepositoryType { get; set; }
        public Type ObjectType { get; set; }
    }
}
