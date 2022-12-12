using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionDll
{
    public class ImplementationTypeComparer : IEqualityComparer<ImplementationType>
    {
        public bool Equals(ImplementationType? x, ImplementationType? y)
        {
            if(x == null && y == null) return true;
            if(y == null || y == null) return false;
            return x.implementationType.Equals(y.implementationType);
        }

        public int GetHashCode([DisallowNull] ImplementationType obj)
        {
            return obj.implementationType.GetHashCode();
        }
    }
}
