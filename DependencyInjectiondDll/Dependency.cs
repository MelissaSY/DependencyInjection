using DependencyInjectionDll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionDll
{
    public class Dependency
    {
        public Type dependencyType { get; }
        private Dictionary<Type, ImplementationType> _implementations;
        private Dictionary<object, ImplementationType> _namedDepencies;
        public Dependency(Type dependencyType) 
        {
            this._namedDepencies = new Dictionary<object, ImplementationType>();
            this.dependencyType = dependencyType;
            _implementations = new Dictionary<Type, ImplementationType>();
        }
        private ImplementationType? GetImplementation(Type implementationType)
        {
            ImplementationType? implementation = null;
            if (_implementations.ContainsKey(implementationType))
            {
                implementation = _implementations[implementationType];
            }
            return implementation;
        }
        public void AddImplementationType(Type implementationType, bool isSingleton)
        {
            ImplementationType implementation = new ImplementationType(implementationType, isSingleton);
            if(!_implementations.ContainsKey(implementationType))
            {
                _implementations.Add(implementationType, implementation);
            }
        }
        public void AddNamedDependency(object namedDependcyNum, Type implementationType, bool isSingleton)
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
            AddImplementationType(implementationType, isSingleton);
        }
        public void AddImplementationObject(Type implementationType, object implementationObject)
        {
            var implementation = GetImplementation(implementationType);
            if(implementation!= null)
            {
                implementation.implementationObject = implementationObject;
            }
        }
        public object? GetImplementationObject(Type implementationType)
        {
            object? result = null;
            if(_implementations.ContainsKey(implementationType))
            {
                result = _implementations[implementationType];
            }
            return result;
        }
        public Type? GetImplentationFirstType(object? namedDependency = null)
        {
            if(namedDependency == null)
            {
                return _implementations.Values.First().implementationType;
            }
            else
            {
                return GetNamedDependencyType(namedDependency);
            }
        }
        public IEnumerable<Type> GetAllImplentationTypes()
        {
            IEnumerable<Type> implementationTypes = new List<Type>();
            foreach(ImplementationType implementationType in _implementations.Values)
            {
                implementationTypes = implementationTypes.Append(implementationType.implementationType);
            }
            return implementationTypes;
        }
        public Type? GetNamedDependencyType(object namedDependency)
        {
            Type? implementationType = null;
            if(_namedDepencies.ContainsKey(namedDependency))
            {
                implementationType = _namedDepencies[namedDependency].implementationType;
            }
            return implementationType;
        }
        public bool ImplementationIsSingleton(Type implementationType)
        {
            ImplementationType? implementation = GetImplementation(implementationType);
            if( implementation == null) return false;
            return implementation.isSingleton;
        }
        public void SetImplementationObject(Type implementationType, object implementationObject)
        {
            ImplementationType? implementation = GetImplementation(implementationType);
            if( implementation == null) return;
            implementation.implementationObject = implementationObject;
        }
    }
}
