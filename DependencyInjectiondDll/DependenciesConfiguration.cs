using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionDll
{
    public class DependenciesConfiguration
    {
        private Dictionary<Type, Dependency> _dependencies;
        public DependenciesConfiguration() 
        {
            _dependencies = new Dictionary<Type, Dependency>();
        }
        public void Register<T, K>(bool isSingleton = false)
            where K : T
        {
            Register(typeof(T), typeof(K), isSingleton);
        }
        public void Register<T, K>(object namedDependency, bool isSingleton = false)
            where K : T
        {
            Register(typeof(T), typeof(K), namedDependency, isSingleton);
        }
        public void Register(Type dependencyType, Type implementationType, bool isSingleton = false)
        {
            if(dependencyType.IsAssignableFrom(implementationType) || (implementationType.IsGenericTypeDefinition && dependencyType.IsGenericTypeDefinition))
            {
                if (!_dependencies.ContainsKey(dependencyType))
                {
                    _dependencies.Add(dependencyType, new Dependency(dependencyType));
                }
                _dependencies[dependencyType].AddImplementationType(implementationType, isSingleton);
            }
        }
        public void Register(Type dependencyType, Type implementationType, object namedDependency, bool isSingleton = false)
        {
            if (dependencyType.IsAssignableFrom(implementationType))
            {
                if (!_dependencies.ContainsKey(dependencyType))
                {
                    _dependencies.Add(dependencyType, new Dependency(dependencyType));
                }
                _dependencies[dependencyType].AddNamedDependency(namedDependency, implementationType, isSingleton);
            }
        }
        public Type? GetNamedDependency(Type dependencyType, object namedDependency)
        {
            Type? implementationType = null;
            if(_dependencies.ContainsKey(dependencyType))
            {
                implementationType = _dependencies[dependencyType].GetNamedDependencyType(namedDependency);
            }
            return implementationType;
        }
        public Type? GetDependencyType(Type dependencyType)
        { 
            if(!_dependencies.ContainsKey(dependencyType))
            {
                return null;
            }
            return _dependencies[dependencyType].dependencyType;
        }
        public Type? GetFirstImplementation(Type type, object? namedDependency = null)
        {
            Type? implementationType = null;
            if(_dependencies.ContainsKey(type))
            {
                implementationType = _dependencies[type].GetImplentationFirstType(namedDependency);
            } 
            else
            {
                var dependencies = _dependencies.Keys.ToArray();
                var dependecyType = (from dependency in dependencies
                                     where type.IsAssignableFrom(dependency)
                                     select dependency);
                if(dependecyType != null && dependecyType.Any())
                {
                    implementationType = _dependencies[dependecyType.First()].GetImplentationFirstType(namedDependency);
                }
                else if(type.IsGenericType)
                {
                    if(!type.IsGenericTypeDefinition)
                    {
                        var parameters = type.GetGenericArguments();
                        type = type.GetGenericTypeDefinition();
                        implementationType = TrySearchDependency(type, namedDependency);
                        if(implementationType!= null)
                        {
                            implementationType = implementationType.MakeGenericType(parameters);
                        }
                    }
                    else
                    {
                        implementationType = TrySearchDependency(type.GetGenericTypeDefinition(), namedDependency);
                        if (implementationType != null)
                        {
                            implementationType = implementationType.MakeGenericType(type.GetGenericArguments());
                        }
                    }
                }
            }
            return implementationType;
        }
        private Type? TrySearchDependency(Type type, object? namedDependency)
        {
            Type? implementationType = null;
            var dependencies = _dependencies.Keys.ToArray();
            var dependecyType = (from dependency in dependencies
                                 where type.IsAssignableFrom(dependency)
                                 select dependency);
            if (dependecyType != null && dependecyType.Any())
            {
                implementationType = _dependencies[dependecyType.First()].GetImplentationFirstType(namedDependency);
            }
            return implementationType;
        }
        public IEnumerable<Type> GetAllImplementationTypes(Type type)
        {
            IEnumerable<Type> implentationTypes = new List<Type>();
            if(_dependencies.ContainsKey(type))
            {
                implentationTypes = _dependencies[type].GetAllImplentationTypes();
            }
            return implentationTypes;
        }
        public bool ContainsDependency(Type dependencyType)
        {
            return _dependencies.ContainsKey(dependencyType);
        }
        public bool ImplementationIsSingleton(Type dependencyType, Type implementationType)
        {
            bool isSingleton = false;
            if(_dependencies.ContainsKey(dependencyType) && _dependencies[dependencyType].GetAllImplentationTypes().Contains(implementationType))
            {
                isSingleton = _dependencies[dependencyType].ImplementationIsSingleton(implementationType);
            }
            return isSingleton;
        }
        public object? GetImplementationObject(Type dependencyType, Type implementationType)
        {
            object? implementationObject = null;
            if (_dependencies.ContainsKey(dependencyType) && _dependencies[dependencyType].GetAllImplentationTypes().Contains(implementationType))
            {
                implementationObject = _dependencies[dependencyType].GetImplementationObject(implementationType);
            }
            return implementationObject;
        }
        public void SetImplementationObject(Type dependencyType, Type implementationType, object implementationObject)
        {
            if (_dependencies.ContainsKey(dependencyType) && _dependencies[dependencyType].GetAllImplentationTypes().Contains(implementationType))
            {
                _dependencies[dependencyType].SetImplementationObject(implementationType, implementationObject);
            }
        }
    }
}
