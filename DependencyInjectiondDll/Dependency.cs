using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectiondDll
{
    public class Dependency
    {
        public Type dependencyType { get; }
        private Dictionary<Type, object?> singletonImplementations;
        public bool isSingleton;
        public Dependency(Type dependencyType) 
        {
            this.dependencyType = dependencyType;
            singletonImplementations = new Dictionary<Type, object?>();
        }
        public bool ContainsImplementations(Type implementationType)
        {
            return singletonImplementations.ContainsKey(implementationType);
        }
        public void AddImplementationType(Type implementationType)
        {
            if(!singletonImplementations.ContainsKey(implementationType))
            {
                singletonImplementations.Add(implementationType, null);
            }
        }
        public void AddImplementation(Type implementationType, object implementation)
        {
            if(singletonImplementations.ContainsKey(implementationType))
            {
                singletonImplementations[implementationType] = implementation;
            }
        }
        public object? GetImplementationObject(Type implementationType)
        {
            object? result = null;
            if(singletonImplementations.ContainsKey(implementationType))
            {
                result = singletonImplementations[implementationType];
            }
            return result;
        }
    }
}
