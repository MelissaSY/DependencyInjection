using DependencyInjectionDll;
using DependencyInjectionDll.Comparers;
using System.Collections.Concurrent;
using System.Reflection;
using System.Reflection.Metadata;

namespace DependencyInjectionDll
{
    public class DependencyProvider
    {
        private DependenciesConfiguration _configuration;
        public DependencyProvider(DependenciesConfiguration configuration)
        { 
            _configuration = configuration;
        }
        public T? Resolve<T>(object? namedDependency = null)
        {
            return (T?)Resolve(typeof(T), namedDependency);
        }
        public object? Resolve(Type type, object? namedDependency = null)
        {
            object? result = null;

            if (namedDependency == null)
            {
                namedDependency = GetDependencyFromAttribute(type);
            }

            Type? implementationType = _configuration.GetFirstImplementation(type, namedDependency);
            
            if (implementationType != null)
            {
                result  = TryFullResolve(type, implementationType);
            }
            else if(type.IsGenericType &&
                typeof(IEnumerable<>).IsAssignableFrom(type.GetGenericTypeDefinition()))
            {
                Type dependecnyType = type.GetGenericArguments()[0];
                if (_configuration.ContainsDependency(dependecnyType))
                {
                    var implementations = _configuration.GetAllImplementationTypes(dependecnyType);
                    var implementationObjects = (object[]?)Activator.CreateInstance(dependecnyType.MakeArrayType(), 
                        new object[] { implementations.Count() });
                    if (implementationObjects != null && implementations != null)
                    {
                        for (int i = 0; i < implementations.Count(); i++)
                        {
                            implementationObjects[i] = TryFullResolve(dependecnyType, implementations.ElementAt(i));
                        }
                    }
                    result = implementationObjects;
                }
            }
            return result;
        }
        private object? GetDependencyFromAttribute(Type type)
        {
            object? result = null;
            DependencyKeyAttribute? attribute = type.GetCustomAttribute<DependencyKeyAttribute>();
            if(attribute != null)
            {
                result = attribute.NamedDependency;
            }

            return result;
        }
        private object? GetDependencyFromAttribute(ParameterInfo member)
        {
            object? result = null;
            DependencyKeyAttribute? attribute = member.GetCustomAttribute<DependencyKeyAttribute>();
            if (attribute != null)
            {
                result = attribute.NamedDependency;
            }

            return result;
        }
        private bool SuitableParameter(ParameterInfo parameter)
        {
            var parameterType = parameter.ParameterType;
            bool isSuitable = _configuration.ContainsDependency(parameterType);

            var customAttribute = parameter.GetCustomAttribute(typeof(DependencyKeyAttribute)) as DependencyKeyAttribute;
            if (customAttribute != null && isSuitable)
            {
                isSuitable = _configuration.GetNamedDependency(parameterType, customAttribute.NamedDependency) != null;
            }

            return isSuitable;
        }
        private object? TryFullResolve(Type dependencyType, Type implementationType)
        {
            object? result;
            bool isSingleton = _configuration.ImplementationIsSingleton(dependencyType, implementationType);
            object? implementationObject = TryCreateImplementation(implementationType);
            if (isSingleton)
            {
                if(implementationObject!= null)
                {
                    _configuration.SetImplementationObject(dependencyType, implementationType, implementationObject);
                }
                result = _configuration.GetImplementationObject(dependencyType, implementationType);
            }
            else
            {
                result = implementationObject;
            }
            return result;
        }
        private object? TryCreateImplementation(Type implementationType)
        {
            object? result = null;
            List<ConstructorInfo> suitableConstructors = new List<ConstructorInfo>();
            ConstructorInfo[] constructors = implementationType.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
            foreach (ConstructorInfo constructor in constructors)
            {
                bool isSuitable = true;
                ParameterInfo[] parameters = constructor.GetParameters();
                foreach (ParameterInfo parameter in parameters)
                {
                    isSuitable &= SuitableParameter(parameter);
                }
                if (isSuitable)
                {
                    suitableConstructors.Add(constructor);
                }
            }
            if (suitableConstructors.Count > 0)
            {
                suitableConstructors.Sort(new DependencyConstructorComparer(suitableConstructors));
                bool isActivated = false;
                while (suitableConstructors.Count > 0 && !isActivated)
                {
                    isActivated = TryActivate(implementationType, suitableConstructors.First(), out result);
                    suitableConstructors.Remove(suitableConstructors.First());
                }
            }
            return result;
        }
        private bool TryActivate(Type t, ConstructorInfo constructor, out object? activated)
        {
            bool isActivated = false;
            activated = null;
            ParameterInfo[] parameters = constructor.GetParameters();
            object?[] objParameters = new object[parameters.Length];

            for(int i = 0; i < parameters.Length; i++)
            {
                try
                {
                    object? namedDependency = GetDependencyFromAttribute(parameters[i]);
                    objParameters[i] = Resolve(parameters[i].ParameterType, namedDependency);
                }
                catch { }
            }
            try
            {
                activated = Activator.CreateInstance(t, objParameters);
                isActivated = true;
            }
            catch { }
            return isActivated;
        }
    }
}