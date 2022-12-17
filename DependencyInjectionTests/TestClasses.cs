using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionTests
{
    public enum ServiceImplementations
    {
        First,
        Second
    }
    public interface IService<TRepository>
        where TRepository : IRepository
    { }
    public interface IRepository
    { }
    public class Repository : IRepository
    {
        public Repository() { }
    }
    public class MySqlRepository : IRepository
    {
        public MySqlRepository() { }
    }
    public class ServiceImpl<TRepository> : IService<TRepository>
        where TRepository : IRepository
    {
        public ServiceImpl(TRepository repository) { }
    }
    public class ServiceImpl1 : IService1
    {  }
    public class ServiceImpl2 : IService1
    {  }
    public interface IService1 { }
    public abstract class AbstractService2 { }
    public class Service1 : IService1 { };
    public class Service2 : AbstractService2 { };
    public class Service3 { }
}
