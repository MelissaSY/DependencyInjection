using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionTests
{
    public interface IService
    { }
    public interface IRepository
    { }
    public class Repository : IRepository
    {
        public Repository() { }
    }
    public class ServiceImpl : IService
    {
        public ServiceImpl(IRepository repository) { }
    }
    public class ServiceImpl1 : IService
    {  }
    public class ServiceImpl2<TRepository>
        where TRepository : IRepository
    {
        public ServiceImpl2(TRepository repository) { }
    }
}
