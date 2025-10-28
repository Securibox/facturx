using System.Globalization;

namespace Securibox.FacturX.Utils
{
    public static class ConversionUtils
    {
        public static string? ToInvariantNumericString(object? value)
        {
            if (value == null)
                return null;

            switch (Type.GetTypeCode(value.GetType()))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return ((IFormattable)value).ToString(null, CultureInfo.InvariantCulture);

                default:
                    return value.ToString();
            }
        }
    }
}
