using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionTests
{
    public class TestRegistration
    {
        private DependenciesConfiguration _dependencies;
        [SetUp]
        public void SetUp()
        {
            _dependencies = new DependenciesConfiguration();
        }
        [Test]
        public void Generic_Register_IService_ServiceImpl_Test() 
        {
            _dependencies.Register<IService1, Service1>();
            _dependencies.Register<AbstractService2, Service2>();
            _dependencies.Register<Service3, Service3>();

            var iService1Impl = _dependencies.GetFirstImplementation(typeof(IService1));
            var abstractService2Impl = _dependencies.GetFirstImplementation(typeof(AbstractService2));
            var service3SelfImpl = _dependencies.GetFirstImplementation(typeof(Service3));

            Assert.That(iService1Impl, Is.EqualTo(typeof(Service1)));
            Assert.That(abstractService2Impl, Is.EqualTo(typeof(Service2)));
            Assert.That(service3SelfImpl, Is.EqualTo(typeof(Service3)));
        }
        [Test]
        public void Multiple_ServiceImpl_Test()
        {
            _dependencies.Register<IService1, ServiceImpl1>();
            _dependencies.Register<IService1, ServiceImpl2>();

            IEnumerable<Type> services = _dependencies.GetAllImplementationTypes(typeof(IService1));

            Assert.That(services.Count, Is.EqualTo(2));
            Assert.That(services.ElementAt(0), Is.AnyOf(typeof(ServiceImpl1), typeof(ServiceImpl2)));
            Assert.That(services.ElementAt(1), Is.AnyOf(typeof(ServiceImpl1), typeof(ServiceImpl2)));
            Assert.That(services.ElementAt(0), Is.Not.EqualTo(services.ElementAt(1)));
        }
        [Test]
        public void AddGenericDependencies_Test()
        {
            _dependencies.Register<IRepository, MySqlRepository>();
            _dependencies.Register<IService<IRepository>, ServiceImpl<IRepository>>();

            IEnumerable<Type> services = _dependencies.GetAllImplementationTypes(typeof(IService<IRepository>));

            Assert.That(services.Count, Is.EqualTo(1));
            Assert.That(services.ElementAt(0), Is.EqualTo(typeof(ServiceImpl<IRepository>)));
        }
        [Test]
        public void AddOpenGeneric_Test()
        {
            _dependencies.Register(typeof(IService<>), typeof(ServiceImpl<>));
            IEnumerable<Type> services = _dependencies.GetAllImplementationTypes(typeof(IService<>));
            Assert.That(services.Count, Is.EqualTo(1));
            Assert.That(services.ElementAt(0), Is.EqualTo(typeof(ServiceImpl<>)));
        }
        [Test]
        public void AddNamedDependencies_Test()
        {
            _dependencies.Register<IService1, ServiceImpl1>(ServiceImplementations.First);
            _dependencies.Register<IService1, ServiceImpl2>(ServiceImplementations.Second);

            Type? fisrtImpl = _dependencies.GetNamedDependency(typeof(IService1), ServiceImplementations.First);
            Type? secondImpl = _dependencies.GetNamedDependency(typeof(IService1), ServiceImplementations.Second);

            Assert.That(fisrtImpl, Is.Not.Null);
            Assert.That(secondImpl, Is.Not.Null);
            Assert.That(fisrtImpl, Is.EqualTo(typeof(ServiceImpl1)));
            Assert.That(secondImpl, Is.EqualTo(typeof(ServiceImpl2)));
        }
        public void AddSingleton_Test()
        {
            _dependencies.Register<IService1, ServiceImpl1>(true);

            bool isSingleton = _dependencies.ImplementationIsSingleton(typeof(IService1), typeof(ServiceImpl1));

            Assert.That(isSingleton, Is.True);
        }
    }
}
