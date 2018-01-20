using System;
using System.Collections.Generic;

namespace IocSampleContainer
{
    public class Container : IContainer
    {
        private readonly List<Type> _types = new List<Type>();

        public void Register<T>()
        {
            _types.Add(typeof(T));
        }

        public T Resolve<T>()
        {
            if (_types.Contains(typeof(T)))
            {
                var obj = Activator.CreateInstance<T>();
                return obj;
            }

            throw new Exception($"Type {typeof(T)} is not registered.");
        }
    }
}