using System.Collections.Concurrent;
using System.Reflection;

namespace DependencyInjectiondDll
{
    public class DependencyProvider
    {
        private DependenciesConfiguration _configuration;
        public DependencyProvider(DependenciesConfiguration configuration)
        { 
            _configuration = configuration;
        }
        public T? Resolve<T>()
        {
            return (T?)Resolve(typeof(T));
        }
        public object? Resolve(Type type)
        {
            if(_configuration.GetDependency(type) != null)
            {
                return Resolve(type);
            }
            return null;
        }
    }
}