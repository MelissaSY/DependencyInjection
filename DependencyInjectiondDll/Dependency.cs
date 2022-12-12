using DependencyInjectionDll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectiondDll
{
    public class Dependency
    {
        public Type dependencyType { get; }
        private List<ImplementationType> _implementations;
        private Dictionary<int, ImplementationType> _namedDepencies;
        public Dependency(Type dependencyType) 
        {
            this._namedDepencies = new Dictionary<int, ImplementationType>();
            this.dependencyType = dependencyType;
            _implementations = new List<ImplementationType>();
        }
        public IEnumerable<ImplementationType>? GetImplementationTypes(Type implementationType)
        {
            var implementations = from implementation in _implementations
                       where implementation.implementationType == implementationType
                       select implementation;
            return implementations;
        }
        public void AddImplementationType(Type implementationType, bool isSingleton)
        {
            ImplementationType implementation = new ImplementationType(implementationType, isSingleton);
            if(!_implementations.Contains(implementation, ImplementationType.equalityComparer))
            {
                _implementations.Add(implementation);
            }
        }
        public void AddNamedDependency(int namedDependcyNum, Type implementationType, bool isSingleton)
        {
            ImplementationType implementation = new ImplementationType(implementationType, isSingleton);
            if(!_namedDepencies.ContainsKey(namedDependcyNum))
            {
                _namedDepencies.Add(namedDependcyNum, implementation);
            }
            else
            {
                _namedDepencies[namedDependcyNum] = implementation;
            }
        }
        public void AddImplementation(Type implementationType, object implementation)
        {
        }
        public object? GetImplementationObject(Type implementationType)
        {
            object? result = from implementation in _implementations
                             where implementation.implementationType == implementationType
                             select implementation.implementationObject;
            return result;
        }
    }
}
