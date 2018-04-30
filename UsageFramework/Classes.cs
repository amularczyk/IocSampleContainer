using Autofac.Features.AttributeFilters;

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

    public class TransientClass2 : ITransientClass
    {
    }

    public class SampleClass : ISampleClass
    {
        public SampleClass([KeyFilter("Name")]ITransientClass transientClass)
        {

        }
    }

    public interface ISampleClass
    {
    }
}