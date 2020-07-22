using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.LoadingSystems.Utilities
{
    public static class TypeExtensions
    {
        public static TAttribute GetEnumMemberAttribute<TAttribute>(this Type enumType, string enumMemberName)
            where TAttribute : Attribute
        {
            if (string.IsNullOrEmpty(enumMemberName))
            {
                throw new ArgumentException($"{nameof(enumMemberName)} cannot be null or empty.", nameof(enumMemberName));
            }

            if (!enumType.IsEnum)
            {
                throw new ArgumentException($"Cannot get attribute on enum member of type '{enumType.FullName}' because this type is not an Enum.", nameof(enumType));
            }

            var enumMember = enumType.GetMember(enumMemberName).FirstOrDefault();
            if (enumMember == null)
            {
                throw new ArgumentException($"'{enumType.FullName}' doesn't have member '{enumMemberName}'.", nameof(enumMemberName));
            }

            var attributeType = typeof(TAttribute);
            ICollection<TAttribute> attributes = enumMember.GetCustomAttributes(attributeType, false).Cast<TAttribute>().ToList();
            if (attributes.Count != 1)
            {
                throw new ArgumentException($"Enum member '{enumMember.Name}' is expected to have 1 attribute of type '{attributeType.Name}', " +
                                            $"but had {attributes.Count} instead.", nameof(enumType));
            }

            return attributes.First();
        }
    }
}