using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionDll.Comparers
{
    public class DependencyConstructorComparer : IComparer<ConstructorInfo>
    {
        List<ConstructorInfo> constructors;
        public DependencyConstructorComparer(List<ConstructorInfo> constructors)
        {
            this.constructors = constructors;
        }
        public int Compare(ConstructorInfo? x, ConstructorInfo? y)
        {
            if (x == null || y == null)
            {
                return 0;
            }
            if (!constructors.Contains(x) || !constructors.Contains(y))
            {
                return 0;
            }
            return (-x.GetParameters().Length + y.GetParameters().Length);
        }
    }
}
