using System;
using System.Collections.Generic;
using System.Linq;

namespace Securibox.FacturX.Compatibility
{
#if NET462
    /// <summary>
    /// Extensions de compatibilit� pour .NET Framework 4.6.2
    /// </summary>
    internal static class CompatibilityExtensions
    {
        /// <summary>
        /// Polyfill pour ArgumentNullException.ThrowIfNull disponible depuis .NET 6
        /// </summary>
        public static void ThrowIfNull(object argument, string paramName = null)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        /// <summary>
        /// Polyfill pour string.StartsWith(char) disponible depuis .NET 5
        /// </summary>
        public static bool StartsWithChar(this string str, char value)
        {
            return !string.IsNullOrEmpty(str) && str[0] == value;
        }

        /// <summary>
        /// Polyfill pour string.EndsWith(char) disponible depuis .NET 5
        /// </summary>
        public static bool EndsWithChar(this string str, char value)
        {
            return !string.IsNullOrEmpty(str) && str[str.Length - 1] == value;
        }

        /// <summary>
        /// Polyfill pour string.Join(char, IEnumerable) disponible depuis .NET Core
        /// </summary>
        public static string JoinChar(char separator, IEnumerable<string> values)
        {
            return string.Join(separator.ToString(), values);
        }

        /// <summary>
        /// Polyfill pour string.Replace(string, string, StringComparison)
        /// </summary>
        public static string Replace(
            this string str,
            string oldValue,
            string newValue,
            StringComparison comparisonType
        )
        {
            if (
                comparisonType == StringComparison.Ordinal
                || comparisonType == StringComparison.CurrentCulture
            )
            {
                return str.Replace(oldValue, newValue);
            }

            var result = str;
            var index = result.IndexOf(oldValue, comparisonType);
            while (index >= 0)
            {
                result = result.Remove(index, oldValue.Length);
                result = result.Insert(index, newValue);
                index = result.IndexOf(oldValue, index + newValue.Length, comparisonType);
            }
            return result;
        }
    }

    /// <summary>
    /// Classe statique pour ArgumentNullException.ThrowIfNull
    /// </summary>
    internal static class ArgumentNullExceptionCompat
    {
        public static void ThrowIfNull(object argument, string paramName = null)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }
    }
#endif
}
