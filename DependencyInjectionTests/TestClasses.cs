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
    public interface IService1 { }
    public interface IRepository
    { }
    public class Repository : IRepository
    {
        public Repository() { }
    }
    public interface IMySqlRepository : IRepository { }
    public class MySqlRepository : IMySqlRepository
    {
        public MySqlRepository() { }
    }
    public class ServiceImpl<TRepository> : IService<TRepository>
        where TRepository : IRepository
    {
        public TRepository Repository { get; }
        public ServiceImpl(TRepository repository)
        {
            Repository = repository;
        }
    }
    public class ServiceImpl1 : IService1
    {  }
    public class ServiceImpl2 : IService1
    {  }
    public abstract class AbstractService2 { }
    public class Service1 : IService1 { };
    public class Service2 : AbstractService2 { };
    public class Service3 { }
    public class ServiceRep : IService1
    {
        public ServiceRep(IRepository repository) { }
    }
    public class SomeRepository : IRepository
    {
        public IService1 service;
        public SomeRepository([DependencyKey(ServiceImplementations.Second)] IService1 service) 
        {
            this.service = service;
        }
    }
}
