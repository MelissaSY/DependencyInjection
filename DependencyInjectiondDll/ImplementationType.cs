using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionDll
{
    public class ImplementationType
    {
        public readonly static IEqualityComparer<ImplementationType> equalityComparer = new ImplementationTypeComparer();
        public Type implementationType { get; }
        public object? implementationObject;
        public bool isSingleton;
        public ImplementationType(Type implementationType, bool isSingleton)
        {
            this.implementationType = implementationType;
            this.implementationObject = null;
            this.isSingleton = isSingleton;
        }
    }
}
