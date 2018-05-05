using System;
using System.Collections.Generic;

namespace IocSampleContainer
{
    public class Container : IContainer
    {
        private readonly List<Type> _singletons = new List<Type>();
        private readonly Dictionary<Type, RegisteredType> _types = new Dictionary<Type, RegisteredType>();
        
        public void Register<TIn, TOut>(bool isSingleton)
        {
            if (isSingleton)
            {
                _singletons.Add(typeof(TIn));
            }

            _types.Add(typeof(TIn), new RegisteredType { DestType = typeof(TOut) });
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
                var registeredType = _types[type];

                if (registeredType.Value == null)
                {
                    registeredType.Value = GetNewInstance(registeredType.DestType);
                }

                return (T)registeredType.Value;
            }

            var obj = GetNewInstance(_types[type].DestType);
            return (T)obj;
        }

        private static object GetNewInstance(Type type)
        {
            return Activator.CreateInstance(type);
        }
    }

    public class RegisteredType
    {
        public Type DestType { get; set; }
        public object Value { get; set; }
    }
}