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
        private List<ImplementationType> _implementations;
        public Dependency(Type dependencyType) 
        {
            this.dependencyType = dependencyType;
            _implementations = new List<ImplementationType>();
        }
        private ImplementationType? GetImplementation(Type implementationType)
        {
            ImplementationType? implementation = null;
            if (_implementations.Any(type => type.implementationType == implementationType))
            {
                implementation = _implementations.First(type => type.implementationType == implementationType);
            }
            return implementation;
        }
        public void AddImplementationType(Type implementationType, bool isSingleton, object? namedDependency = null)
        {
            ImplementationType implementation = new ImplementationType(implementationType, isSingleton, namedDependency);
            if(!_implementations.Any(impl => impl.implementationType == implementationType))
            {
                _implementations.Add(implementation);
            }
        }
        public void AddNamedDependency(object namedDependcyNum, Type implementationType, bool isSingleton)
        {

            if (!_implementations.Any(implementation => namedDependcyNum.Equals(implementation.namedDependency)))
            {
                ImplementationType implementation = new ImplementationType(implementationType, isSingleton, namedDependcyNum);
                AddImplementationType(implementationType, isSingleton, namedDependcyNum);
            }
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
            if (_implementations.Any(impl => impl.implementationType == implementationType))
            {
                ImplementationType? implementation = _implementations
                                                    .First(impl => impl.implementationType == implementationType);
                result = implementation.implementationObject;
            }
            return result;
        }
        public Type? GetImplentationFirstType(object? namedDependency = null)
        {
            if(_implementations.Count() > 0)
            {
                if (namedDependency == null)
                {
                    return _implementations.First().implementationType;
                }
                else
                {
                    return GetNamedDependencyType(namedDependency);
                }
            }
            return null;
        }
        public IEnumerable<Type> GetAllImplentationTypes()
        {
            IEnumerable<Type> implementationTypes = new List<Type>();
            foreach(ImplementationType implementationType in _implementations)
            {
                implementationTypes = implementationTypes.Append(implementationType.implementationType);
            }
            return implementationTypes;
        }
        public Type? GetNamedDependencyType(object namedDependency)
        {
            Type? implementationType = null;

            if(_implementations.Any(impl => namedDependency.Equals(impl.namedDependency)))
            {
                var implementation = _implementations.First(impl => namedDependency.Equals(impl.namedDependency));
                implementationType = implementation.implementationType;
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
            if(implementation == null) return;
            implementation.implementationObject = implementationObject;
        }

    }
}
