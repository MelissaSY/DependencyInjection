namespace DependencyInjectionTests
{
    public class Tests
    {
        private DependenciesConfiguration dependencies;
        [SetUp]
        public void Setup()
        {
            dependencies = new DependenciesConfiguration();
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}