﻿using Autofac.Features.AttributeFilters;

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
        public ITransientClass TransientClass { get; set; }

        public SampleClass([KeyFilter("name2")]ITransientClass transientClass)
        {
            TransientClass = transientClass;
        }
    }

    public interface ISampleClass
    {
        ITransientClass TransientClass { get; set; }
    }
}