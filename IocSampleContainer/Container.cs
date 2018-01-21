using System;
using System.Collections.Generic;

namespace IocSampleContainer
{
    public class Container : IContainer
    {
        private readonly List<Type> _singletons = new List<Type>();
        private readonly Dictionary<Type, object> _types = new Dictionary<Type, object>();

        public void Register<T>(bool isSingleton)
        {
            if (isSingleton)
            {
                _singletons.Add(typeof(T));
            }

            _types.Add(typeof(T), null);
        }

        public T Resolve<T>()
        {
            var type = typeof(T);
            if (!_types.ContainsKey(type))
            {
                throw new Exception($"Type {type} is not registered.");
            }

            if (_singletons.Contains(type))
            {
                if (_types[type] == null)
                {
                    _types[type] = GetNewInstance<T>();
                }

                return (T)_types[type];
            }

            var obj = GetNewInstance<T>();
            return obj;
        }

        private static T GetNewInstance<T>()
        {
            return Activator.CreateInstance<T>();
        }
    }
}