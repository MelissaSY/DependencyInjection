using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionDll
{

    public class DependencyKeyAttribute : Attribute
    {
        public object NamedDependency { get; }
        public DependencyKeyAttribute(object namedDependency)
        {
            this.NamedDependency = namedDependency;
        }
    }
}
