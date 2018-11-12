using System;
using System.Linq;

namespace Hotels.Utilities
{
    public class ObjectValidate
    {
        public static bool IsAnyNullOrEmpty(object obj)
        {
            if (Object.ReferenceEquals(obj, null))
                return false;

            return obj.GetType().GetProperties().Any(s => IsNullOrEmpty(s.GetValue(obj)));
        }

        public static bool IsNullOrEmpty(object value)
        {
            if (Object.ReferenceEquals(value, null)) return false;

            var type = value.GetType();
            return type.IsValueType && object.Equals(value, Activator.CreateInstance(type));
        }
    }
}