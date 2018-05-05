using System;
using System.Collections.Generic;

namespace IocSampleContainer
{
    public class ManyResolvesContainer
    {
        private readonly Dictionary<Type, Dictionary<string, RegisteredType>> _types =
            new Dictionary<Type, Dictionary<string, RegisteredType>>();

        public void Register<TIn, TOut>(RegistrationKind registrationKind, string name = "")
        {
            var registrationType = new RegisteredType {DestType = typeof(TOut), RegistrationKind = registrationKind};
            if (_types.ContainsKey(typeof(TIn)))
            {
                _types[typeof(TIn)].Add(name, registrationType);
            }
            else
            {
                _types.Add(typeof(TIn), new Dictionary<string, RegisteredType> {{name, registrationType}});
            }
        }

        public T Resolve<T>(string name = "")
        {
            var registeredType = _types[typeof(T)];
            return GetObject<T>(registeredType[name]);
        }

        #region Hide

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

        private static object GetNewInstance(Type type)
        {
            return Activator.CreateInstance(type);
        }

        #endregion
    }
}