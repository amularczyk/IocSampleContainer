using System;
using System.Collections.Generic;

namespace IocSampleContainer
{
    public class Container : IContainer
    {
        private readonly Dictionary<IScope, Dictionary<Type, object>> _scopes =
            new Dictionary<IScope, Dictionary<Type, object>>();

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
            ValidateType<T>(type);

            var registeredType = _types[type];

            return GetObject<T>(registeredType);
        }

        public T Resolve<T>(IScope scope)
        {
            var type = typeof(T);
            ValidateType<T>(type);

            var registeredType = _types[type];

            if (registeredType.RegistrationKind == RegistrationKind.Scope)
            {
                var registeredTypesForScope = _scopes[scope];

                if (!registeredTypesForScope.ContainsKey(type))
                {
                    registeredTypesForScope[type] = GetNewInstance(registeredType.DestType);
                }

                return (T)registeredTypesForScope[type];
            }

            return GetObject<T>(registeredType);
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

        private void ValidateType<T>(Type type)
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

        public IScope StartNewScope()
        {
            var scope = new Scope();
            _scopes.Add(scope, new Dictionary<Type, object>());
            return scope;
        }

        public void FinishScope(IScope scope)
        {
            _scopes.Remove(scope);
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

    public interface IScope
    {
        Guid Id { get; }
    }

    public class Scope : IScope
    {
        public Scope()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; }
    }
}