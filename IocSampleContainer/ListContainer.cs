using System;
using System.Collections.Generic;
using System.Linq;

namespace IocSampleContainer
{
    public class ListContainer
    {
        private readonly List<RegisteredTypeWithInputType> _types = new List<RegisteredTypeWithInputType>();

        public void Register<TIn, TOut>(RegistrationKind registrationKind)
        {
            _types.Add(new RegisteredTypeWithInputType
            {
                InputType = typeof(TIn),
                DestType = typeof(TOut),
                RegistrationKind = registrationKind
            });
        }

        public T Resolve<T>()
        {
            var registeredType = _types.First(t => t.InputType == typeof(T));
            return GetObject<T>(registeredType);
        }

        #region Hide
        
        private T GetObject<T>(RegisteredTypeWithInputType registeredType)
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

    public class RegisteredTypeWithInputType
    {
        public Type InputType { get; set; }
        public Type DestType { get; set; }
        public object Value { get; set; }
        public RegistrationKind RegistrationKind { get; set; }
    }
}