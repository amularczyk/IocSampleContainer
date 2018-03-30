﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace IocSampleContainer
{
    public class ManyRegistrationContainer : IContainer
    {
        private readonly Dictionary<List<Type>, RegisteredType> _types = new Dictionary<List<Type>, RegisteredType>();

        public void Register<TIn, TOut>(RegistrationKind registrationKind)
        {
            var tmp = _types.Where(t => t.Value.DestType == typeof(TOut)).Select(t => t.Key).FirstOrDefault();
            if (tmp != null)
            {
                tmp.Add(typeof(TIn));
            }
            else
            {
                var registeredType = new RegisteredType {DestType = typeof(TOut), RegistrationKind = registrationKind};
                _types.Add(new List<Type> {typeof(TIn)}, registeredType);
            }
        }

        public T Resolve<T>()
        {
            var registeredType = _types.First(t => t.Key.Contains(typeof(T))).Value;
            return GetObject<T>(registeredType);
        }

        public void Register<T>(RegistrationKind registrationKind)
        {
            Register<T, T>(registrationKind);
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