using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Felix.Common.Extensions
{
    public static class ActivatorExtensions
    {
        public static T CreateInstance<T>(object[] parameters) where T : class
        {
            return (T)CreateInstance(typeof(T), parameters);
        }

        public static object CreateInstance(Type type, object[] parameters)
        {
            var parameterizedCtor = type.GetConstructors().FirstOrDefault(c => c.GetParameters().Length > 0);

            return parameterizedCtor != null ? parameterizedCtor.Invoke(parameters != null && parameters.Length > 0 ? parameters : parameterizedCtor.GetParameters()
                .Select(p =>
                    p.HasDefaultValue ? p.DefaultValue :
                    p.ParameterType.IsValueType && Nullable.GetUnderlyingType(p.ParameterType) == null ?
                    Activator.CreateInstance(p.ParameterType) : null
                ).ToArray()) : Activator.CreateInstance(type);
        }
    }
}
