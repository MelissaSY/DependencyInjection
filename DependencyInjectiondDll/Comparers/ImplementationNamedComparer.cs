using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionDll.Comparers
{
    public class ImplementationNamedComparer : IEqualityComparer<ImplementationType>
    {
        public bool Equals(ImplementationType? x, ImplementationType? y)
        {
            if(x == null&& y == null) return true;
            if(y == null || x == null) return false;
            if (x.namedDependency == null && y.namedDependency == null) return true;
            if (x.namedDependency == null || y.namedDependency == null) return false;
            return x.namedDependency.Equals(y.namedDependency);
        }

        public int GetHashCode([DisallowNull] ImplementationType obj)
        {
            throw new NotImplementedException();
        }
    }
}
