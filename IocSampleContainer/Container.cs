using System;
using System.Collections.Generic;

namespace IocSampleContainer
{
    public class Container : IContainer
    {
        private readonly Dictionary<Type, RegisteredType> _types = new Dictionary<Type, RegisteredType>();

        public void Register<T>(RegistrationKind registrationKind)
        {
            _types.Add(typeof(T), new RegisteredType {DestType = typeof(T), RegistrationKind = registrationKind});
        }

        public void Register<TIn, TOut>(RegistrationKind registrationKind)
        {
            _types.Add(typeof(TIn), new RegisteredType {DestType = typeof(TOut), RegistrationKind = registrationKind});
        }

        public T Resolve<T>()
        {
            var type = typeof(T);
            if (!_types.ContainsKey(type))
            {
                throw new Exception($"Type {type} is not registered.");
            }

            var registeredType = _types[type];

            if (registeredType.RegistrationKind == RegistrationKind.Singleton)
            {
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
        public RegistrationKind RegistrationKind { get; set; }
    }

    public enum RegistrationKind
    {
        Transient,
        Singleton,
        Scope
    }
}