namespace DependencyInjectionDll
{
    public class ImplementationType
    {
        public Type implementationType { get; }
        private object _lock;
        private object? _implementationObject;
        public object? implementationObject
        {
            get 
            { 
                return _implementationObject; 
            }
            set
            {
                if (_implementationObject == null)
                {
                    lock (_lock)
                    {
                        if (_implementationObject == null)
                        {
                            _implementationObject = value;
                        }
                    }
                }
            }
        }
        public bool isSingleton { get; }
        public ImplementationType(Type implementationType, bool isSingleton)
        {
            this.implementationType = implementationType;
            this._lock = new object();
            this.implementationObject = null;
            this.isSingleton = isSingleton;
        }
    }
}
