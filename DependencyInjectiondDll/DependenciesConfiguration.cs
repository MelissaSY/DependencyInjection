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
       // private Dictionary<Type, Dependency> _dependencies;
        private List<Dependency> _dependenciesList;
        public DependenciesConfiguration() 
        {
           // _dependencies = new Dictionary<Type, Dependency>();

            _dependenciesList = new List<Dependency>();
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
            if(dependencyType.IsAssignableFrom(implementationType) 
                || (implementationType.IsGenericTypeDefinition && dependencyType.IsGenericTypeDefinition))
            {
                if(!_dependenciesList.Any(dependency => dependency.dependencyType == dependencyType))
                {
                    _dependenciesList.Add(new Dependency(dependencyType));
                }
                var dependency = (from dependencies in _dependenciesList
                                 where dependencies.dependencyType == dependencyType
                                 select dependencies).First();
                dependency.AddImplementationType(implementationType, isSingleton);
            }
        }
        public void Register(Type dependencyType, Type implementationType, object namedDependency, bool isSingleton = false)
        {
            if (dependencyType.IsAssignableFrom(implementationType)
                || (implementationType.IsGenericTypeDefinition && dependencyType.IsGenericTypeDefinition))
            {
                if (!_dependenciesList.Any(dependency => dependency.dependencyType == dependencyType))
                {
                    _dependenciesList.Add(new Dependency(dependencyType));
                }
                var dependency = (from dependencies in _dependenciesList
                                  where dependencies.dependencyType == dependencyType
                                  select dependencies).First();
                dependency.AddNamedDependency(namedDependency, implementationType, isSingleton);
            }
        }
        public Type? GetNamedDependency(Type dependencyType, object namedDependency)
        {
            Type? implementationType = null;
            if (_dependenciesList.Any(dependency => dependency.dependencyType == dependencyType))
            {
                implementationType = _dependenciesList
                    .Where(dependdency=> dependdency.dependencyType == dependencyType)
                    .Select(dependency => dependency.GetNamedDependencyType(namedDependency))
                    .First();
            }
            return implementationType;
        }
        public Type? GetFirstImplementation(Type type, object? namedDependency = null)
        {
            Dependency? dependency = GetDependencyFromType(type);
            Type? implementationType = null;
            if (dependency != null)
            {
                implementationType = dependency.GetImplentationFirstType(namedDependency);
            } 
            else
            {
                var dependencies = _dependenciesList.Select(dependency => dependency.dependencyType);
                  //  _dependencies.Keys.ToArray();
                var dependecyType = (from _dependency in dependencies
                                     where type.IsAssignableFrom(_dependency)
                                     select _dependency);
                if(dependecyType != null && dependecyType.Any())
                {
                    implementationType = GetDependencyFromType(dependecyType.First())?.GetImplentationFirstType(namedDependency);
                }
                else if (type.IsGenericType)
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
            var dependencies = _dependenciesList.Select(dependency => dependency.dependencyType);
            var dependecyType = (from dependency in dependencies
                                 where type.IsAssignableFrom(dependency)
                                 select dependency);
            if (dependecyType != null && dependecyType.Any())
            {
                Dependency? dependency = GetDependencyFromType(dependecyType.First());
                implementationType = dependency?.GetImplentationFirstType(namedDependency);
            }
            return implementationType;
        }
        public IEnumerable<Type> GetAllImplementationTypes(Type type)
        {
            IEnumerable<Type> implentationTypes = new List<Type>();
            Dependency? dependency = GetDependencyFromType(type);
            if (dependency != null)
            {
                implentationTypes = dependency.GetAllImplentationTypes();
            }
            return implentationTypes;
        }
        public bool ContainsDependency(Type dependencyType)
        {
            return _dependenciesList.Any(dependency=>dependency.dependencyType== dependencyType);
            //return _dependencies.ContainsKey(dependencyType);
        }
        public bool ImplementationIsSingleton(Type dependencyType, Type implementationType)
        {
            bool isSingleton = false;
            Dependency? dependency = GetDependencyFromType(dependencyType);
            if (dependency != null
                && dependency.GetAllImplentationTypes().Contains(implementationType))
            {
                isSingleton = dependency.ImplementationIsSingleton(implementationType);
            }
            return isSingleton;
        }
        public object? GetImplementationObject(Type dependencyType, Type implementationType)
        {
            object? implementationObject = null;
            Dependency? dependency = GetDependencyFromType(dependencyType);
            if (dependency != null
                && dependency.GetAllImplentationTypes().Contains(implementationType))
            {
                implementationObject = dependency.GetImplementationObject(implementationType);
            }
            return implementationObject;
        }
        public void SetImplementationObject(Type dependencyType, Type implementationType, object implementationObject)
        {
            Dependency? dependency = GetDependencyFromType(dependencyType); 
            if (dependency != null
                && dependency.GetAllImplentationTypes().Contains(implementationType))
            {
                dependency.SetImplementationObject(implementationType, implementationObject);
            }
        }
        private Dependency? GetDependencyFromType(Type dependencyType)
        {
            Dependency? dependency = null;
            var dependencies = _dependenciesList
                .Where(dependency => dependency.dependencyType == dependencyType);
            if (dependencies.Any())
            {
                dependency = dependencies.FirstOrDefault();
            }
            return dependency;
        }
    }
}
