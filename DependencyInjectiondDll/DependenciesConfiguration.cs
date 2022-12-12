using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectiondDll
{
    public class DependenciesConfiguration
    {
        private Dictionary<Type, Dependency> _dependencies;
        public DependenciesConfiguration() 
        {
            _dependencies = new Dictionary<Type, Dependency>();
        }
        public void Register<T, K>(bool isSingleton = false)
            where K : T, new()
        {
            Register(typeof(T), typeof(K), isSingleton);
        }
        public void Register(Type dependencyType, Type implementationType, bool isSingleton = false)
        {
            if(!_dependencies.ContainsKey(dependencyType))
            {
                _dependencies.Add(dependencyType, new Dependency(dependencyType));
            }
            _dependencies[dependencyType].AddImplementationType(implementationType);
            _dependencies[dependencyType].isSingleton = isSingleton;
        }
        public Dependency? GetDependency(Type dependencyType)
        { 
            if(!_dependencies.ContainsKey(dependencyType))
            {
                return null;
            }
            return _dependencies[dependencyType]; 
        }
    }
}
