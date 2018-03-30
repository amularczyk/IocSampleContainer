using System;
using System.Collections.Generic;
using System.Linq;

namespace IocSampleContainer
{
    public class ListContainer : IContainer
    {
        private readonly List<ContainerMember> _types = new List<ContainerMember>();

        public void Register<TIn, TOut>(RegistrationKind registrationKind)
        {
            _types.Add(new ContainerMember { InputType = typeof(TIn), DestType = typeof(TOut), RegistrationKind = registrationKind });
        }

        public T Resolve<T>()
        {
            var registeredType = _types.First(t => t.InputType == typeof(T));
            return GetObject<T>(registeredType);
        }

        public void Register<T>(RegistrationKind registrationKind)
        {
            Register<T, T>(registrationKind);
        }

        public T Resolve<T>(IScope scope)
        {
            var type = typeof(T);

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

        private T GetObject<T>(ContainerMember registeredType)
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

        private static object GetNewInstance(Type type)
        {
            return Activator.CreateInstance(type);
        }

        public IScope StartNewScope()
        {
            return new Scope();
        }
    }

    public class ContainerMember
    {
        public Type InputType { get; set; }
        public Type DestType { get; set; }
        public object Value { get; set; }
        public RegistrationKind RegistrationKind { get; set; }
    }
}