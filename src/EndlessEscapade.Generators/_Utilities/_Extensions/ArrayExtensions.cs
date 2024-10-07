using System;
using System.Text;

namespace EndlessEscapade.Generators;

public static class ArrayExtensions
{
    public static StringBuilder ToStringArray<T>(this T[] array) {
        var builder = new StringBuilder(2 + array.Length * 8);

        builder.Append('[');

        for (var i = 0; i < array.Length; i++) {
            if (i != 0) {
                builder.Append(", ");
            }

            var value = array[i].ToString();

            if (typeof(T) == typeof(string)) {
                builder.Append('"').Append(value).Append('"');
            }
            else if (typeof(T) == typeof(char)) {
                builder.Append('\'').Append(value).Append('\'');
            }
            else {
                builder.Append(value);
            }
        }

        builder.Append(']');

        return builder;
    }
}
