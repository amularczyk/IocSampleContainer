using System;
using System.Collections.Generic;

namespace IocSampleContainer
{
    public class ManyResolvesContainer : IContainer
    {
        private readonly Dictionary<Type, Dictionary<string, RegisteredType>> _types =
            new Dictionary<Type, Dictionary<string, RegisteredType>>();

        public void Register<TIn, TOut>(RegistrationKind registrationKind, string name)
        {
            var registrationType = new RegisteredType { DestType = typeof(TOut), RegistrationKind = registrationKind };
            if (_types.ContainsKey(typeof(TIn)))
            {
                _types[typeof(TIn)].Add(name, registrationType);
            }
            else
            {
                _types.Add(typeof(TIn), new Dictionary<string, RegisteredType> { { name, registrationType } });
            }
        }

        public T Resolve<T>(string name)
        {
            var registeredType = _types[typeof(T)];
            return GetObject<T>(registeredType[name]);
        }

        public void Register<TIn, TOut>(RegistrationKind registrationKind)
        {
            _types.Add(typeof(TIn), new RegisteredType {DestType = typeof(TOut), RegistrationKind = registrationKind});
        }

        public void Register<T>(RegistrationKind registrationKind)
        {
            _types.Add(typeof(T), new RegisteredType {DestType = typeof(T), RegistrationKind = registrationKind});
        }

        public T Resolve<T>()
        {
            var type = typeof(T);
            ValidateType(type);

            var registeredType = _types[type];

            return GetObject<T>(registeredType);
        }

        public T Resolve<T>(IScope scope)
        {
            var type = typeof(T);
            ValidateType(type);

            var registeredType = _types[type];

            if (registeredType.RegistrationKind == RegistrationKind.Scope)
            {
                var objectFromScpe = scope.GetObject(type);

                if (objectFromScpe == null)
                {
                    objectFromScpe = GetNewInstance(registeredType.DestType);
                    scope.AddObject(type, objectFromScpe);
                }

                return (T)objectFromScpe;
            }

            return GetObject<T>(registeredType);
        }

        public IScope StartNewScope()
        {
            return new Scope();
        }

        private T GetObject<T>(RegisteredType registeredType)
        {
            if (registeredType.RegistrationKind == RegistrationKind.Singleton)
            {
                if (registeredType.Value == null)
                {
                    registeredType.Value = GetNewInstance(registeredType.DestType);
                }

                return (T)registeredType.Value;
            }

            var obj = GetNewInstance(registeredType.DestType);
            return (T)obj;
        }

        private void ValidateType(Type type)
        {
            if (!_types.ContainsKey(type))
            {
                throw new Exception($"Type {type} is not registered.");
            }
        }

        private static object GetNewInstance(Type type)
        {
            return Activator.CreateInstance(type);
        }
    }
}