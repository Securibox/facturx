using System.Globalization;
using System.Xml;

namespace Securibox.FacturX.Core
{
    public class XmlParsingHelpers
    {
        //public static string? ExtractString(XmlNode? xmlNode)
        //{
        //    return xmlNode?.InnerText;
        //}

        public static decimal ExtractDecimal(XmlNode xmlNode)
        {
            var decimalString = xmlNode.InnerText;
            
            decimal.TryParse(decimalString, NumberStyles.Currency, CultureInfo.InvariantCulture, out decimal decimalValue);
            return decimalValue;
        }

        public static int? ExtractInteger(XmlNode? xmlNode)
        {
            var integerString = xmlNode?.InnerText;

            var isSuccess = int.TryParse(integerString, out int integerValue);
            if (!isSuccess)
                return null;

            return integerValue;
        }

        //public static bool? ExtractBoolFromChild(XmlNode? baseNode, string childName)
        //{
        //    var xmlNode = baseNode?.SelectSingleNode($"*[local-name() = '{childName}']");
        //    return ExtractBool(xmlNode);
        //}

        public static bool? ExtractBool(XmlNode? xmlNode)
        {
            var boolString = xmlNode?.InnerText;
            return bool.TryParse(boolString, out bool boolValue) ? boolValue : null;
        }

        public static bool? ExtractBool(string boolString)
        {
            return bool.TryParse(boolString, out bool boolValue) ? boolValue : null;
        }

        public static DateTime ExtractDateTime(XmlNode dateTimeNode)
        {
            //Extract DateTime format (default should be 102)
            //TODO: Look for other formats!
            var dateTimeFormat = "102";
            if (dateTimeNode != null && dateTimeNode.Attributes != null &&
                dateTimeNode.Attributes["format"] != null &&
                !string.IsNullOrEmpty(dateTimeNode.Attributes["format"]!.Value))
            {
                dateTimeFormat = dateTimeNode.Attributes["format"]!.Value;

                if (dateTimeFormat == "102")
                {
                    if (DateTime.TryParseExact(dateTimeNode.InnerText, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime extractedDateTime))
                    {
                        return extractedDateTime;
                    }
                }

                if (dateTimeFormat == "203")
                {
                    if (DateTime.TryParseExact(dateTimeNode.InnerText, "yyyyMMddHHmm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime extractedDateTime))
                    {
                        return extractedDateTime;
                    }
                }
            }

            return DateTime.MinValue;
        }

        public static string? ExtractAttribute(XmlNode? currentNode, string schemeName)
        {
            if (currentNode != null && currentNode.Attributes != null && !string.IsNullOrWhiteSpace(currentNode.Attributes[schemeName]?.Value))
            {
                return currentNode.Attributes[schemeName]!.Value;
            }

            return null;
        }

       
    }
}
