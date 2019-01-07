using System;
using System.Linq;
using System.Reflection;

namespace TestFactory.Internal
{
    internal static class TypeExtensions
    {
        internal static string GetFormattedName(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (!type.GetTypeInfo().IsGenericType)
            {
                return type.Name;
            }

            return $"{type.Name.Substring(0, type.Name.IndexOf('`'))}<{string.Join(", ", type.GetTypeInfo().GenericTypeArguments.Select(t => t.GetFormattedName()))}>";
        }
    }
}