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
            object? result = null;
            if(_configuration.GetDependencyType(type) != null)
            {
                List<ConstructorInfo> suitableConstructors = new List<ConstructorInfo>();
                ConstructorInfo[] constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
                foreach(ConstructorInfo constructor in constructors)
                {
                    bool isSuitable = true;
                    ParameterInfo[] parameters = constructor.GetParameters();
                    foreach(ParameterInfo parameter in parameters)
                    {
                        isSuitable &= _configuration.ContainsDependency(parameter.ParameterType);
                        SuitableParameter(parameter);
                    }
                    if(isSuitable)
                    {
                        suitableConstructors.Add(constructor);
                    }
                }
                if(suitableConstructors.Count > 0)
                {
                    Dictionary<ConstructorInfo, int> constructorParameters = new Dictionary<ConstructorInfo, int>();
                }
            }
            return result;
        }
        private bool SuitableParameter(ParameterInfo parameter)
        {
            var customAttributes = parameter.CustomAttributes;
            return false;
        }

    }
}