namespace UsageCore
{
    public class SingletonClass : ISingletonClass
    {
    }

    public interface ISingletonClass   
    {
    }

    public class TransientClass : ITransientClass
    {
    }

    public interface ITransientClass
    {
    }

    public class ScopedClass : IScopedClass
    {
        public ScopedClass(ITransientClass transientClass)
        {
            
        }
    }

    public interface IScopedClass
    {
    }

    public class SampleClass : ISampleClass
    {
    }

    public interface ISampleClass
    {
    }
}