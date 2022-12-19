namespace DependencyInjectionTests
{
    public class TestResolve
    {
        private DependenciesConfiguration _dependencies;
        [SetUp]
        public void Setup()
        {
            _dependencies = new DependenciesConfiguration();
        }

        [Test]
        public void SimpleResolve_Test()
        {
            _dependencies.Register<IService1, Service1>();
            _dependencies.Register<AbstractService2, Service2>();
            _dependencies.Register<Service3, Service3>();

            var provider = new DependencyProvider(_dependencies);

            var service1 = provider.Resolve<IService1>();
            var service2 = provider.Resolve<AbstractService2>(); 
            var service3 = provider.Resolve<Service3>();

            Assert.That(service1, Is.Not.Null);
            Assert.That(service2, Is.Not.Null);
            Assert.That(service3, Is.Not.Null);
        }
        [Test]
        public void Dependency_into_dependencyConstructor_Test()
        {
            _dependencies.Register<IService1, ServiceRep>();
            _dependencies.Register<IRepository, Repository>();

            var provider = new DependencyProvider(_dependencies);

            var service1 = provider.Resolve<IService1>();

            Assert.That(service1, Is.Not.Null);
        }
        [Test]
        public void Singleton_Test()
        {

        }
        [Test]
        public void MultipleImplementationsGet_Test()
        {
            _dependencies.Register<IService1, ServiceImpl1>();
            _dependencies.Register<IService1, ServiceImpl2>();
            var provider = new DependencyProvider(_dependencies);

            IEnumerable<IService1?>? services = provider.Resolve<IEnumerable<IService1>>();

            Assert.That(services, Is.Not.Null);
            Assert.That(services.Count(), Is.EqualTo(2));

            Assert.That(services.ElementAt(0), Is.Not.Null);
            Assert.That(services.ElementAt(1), Is.Not.Null);

            Assert.That(services.ElementAt(0).GetType(), Is.AnyOf(typeof(ServiceImpl1), typeof(ServiceImpl2)));
            Assert.That(services.ElementAt(1).GetType(), Is.AnyOf(typeof(ServiceImpl1), typeof(ServiceImpl2)));
            Assert.That(services.ElementAt(0).GetType(), Is.Not.EqualTo(services.ElementAt(1).GetType()));
        }
        [Test]
        public void GenericDependency_Standart_Form_Test()
        {
            _dependencies.Register<IRepository, MySqlRepository>();
            _dependencies.Register<IService<IRepository>, ServiceImpl<IRepository>>();

            var provider = new DependencyProvider(_dependencies);

            var service = provider.Resolve<IService<IRepository>>();

            Assert.That(service, Is.Not.Null);
        }
        [Test]
        public void OpenGenerics_Form_Test()
        {
            _dependencies.Register(typeof(IService<>), typeof(ServiceImpl<>));
            _dependencies.Register<IMySqlRepository, MySqlRepository>();
            _dependencies.Register<IRepository, Repository>();

            var provider = new DependencyProvider(_dependencies);

            var service = provider.Resolve<IService<IMySqlRepository>>() as ServiceImpl<IMySqlRepository>;
            var repository = service?.Repository as MySqlRepository;

            Assert.That(service, Is.Not.Null);
            Assert.That(repository, Is.Not.Null);
        }
        [Test]
        public void NamedDependency_In_Registration_Test()
        {
            _dependencies.Register<IService1, ServiceImpl1>(ServiceImplementations.First);
            _dependencies.Register<IService1, ServiceImpl2>(ServiceImplementations.Second);

            var provider = new DependencyProvider(_dependencies);

            var service1 = provider.Resolve<IService1>(ServiceImplementations.First) as ServiceImpl1;
            var service2 = provider.Resolve<IService1>(ServiceImplementations.Second) as ServiceImpl2;


            Assert.That(service1, Is.Not.Null);
            Assert.That(service2, Is.Not.Null);
        }
        [Test]
        public void NamedDependency_Constructor_Test()
        {
            _dependencies.Register<IService1, ServiceImpl1>(ServiceImplementations.First);
            _dependencies.Register<IService1, ServiceImpl2>(ServiceImplementations.Second);

            _dependencies.Register<IRepository, SomeRepository>();

            var provider = new DependencyProvider(_dependencies);

            var repository = provider.Resolve<IRepository>() as SomeRepository;
            var secondService = repository?.service as ServiceImpl2;

            Assert.That(repository, Is.Not.Null);
            Assert.That(secondService, Is.Not.Null);
        }
    }
}