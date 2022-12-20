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
            ImplementationType? implementation = _implementations.Where(type => type.implementationType == implementationType)
                                                                 .FirstOrDefault();
            return implementation;
        }
        public void AddImplementationType(Type implementationType, bool isSingleton, object? namedDependency = null)
        {
            ImplementationType implementation = new ImplementationType(implementationType, isSingleton, namedDependency);
            if(!_implementations.Where(impl => impl.implementationType == implementationType)
                                .Any())
            {
                _implementations.Add(implementation);
            }
        }
        public void AddNamedDependency(object namedDependcyNum, Type implementationType, bool isSingleton)
        {

            if (!_implementations.Exists(implementation => namedDependcyNum.Equals(implementation.namedDependency)))
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
            ImplementationType? implementation = _implementations
                                                .FirstOrDefault(impl => impl.implementationType == implementationType);
            if (implementation != null)
            {
                result = implementation.implementationObject;
            }
            return result;
        }
        public Type? GetImplentationFirstType(object? namedDependency = null)
        {
            if(namedDependency == null)
            {
                return _implementations.First().implementationType;
            }
            else
            {
                return GetNamedDependencyType(namedDependency);
            }
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
            var implementation = _implementations
                                           .Where(impl => namedDependency.Equals(impl.namedDependency))
                                           .First();
            Type? implementationType = null;

            if(implementation != null)
            {
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
            if( implementation == null) return;
            implementation.implementationObject = implementationObject;
        }

    }
}
