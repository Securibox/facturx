using Securibox.FacturX.Models.BasicWL;
using Securibox.FacturX.Models.Enums;
using Securibox.FacturX.Models.Minimum;
using Securibox.FacturX.Models.Minimum.Enum;
using System.Xml;

namespace Securibox.FacturX.Core
{
    internal abstract class TradePartyConverter
    {

        protected readonly FacturXConformanceLevelType ConformanceLevelType;

        protected TradePartyConverter(FacturXConformanceLevelType conformanceLevelType) 
        {
            ConformanceLevelType = conformanceLevelType;
        }

        protected string? GetName(XmlNode tradePartyNode)
        {
            var nameNode = tradePartyNode.SelectSingleNode("*[local-name() = 'Name']");
            return nameNode?.InnerText;
        }

        /// <summary>
        /// Use for 0..1 GlobalId and 0...1 schemeId
        /// </summary>
        /// <param name="parentNode"></param>
        /// <returns></returns>
        internal GlobalIdentification? GetGlobalIdOptionalScheme(XmlNode tradePartyNode)
        {
            var globalIdNode = tradePartyNode.SelectSingleNode("*[local-name() = 'GlobalID']");
            if (globalIdNode == null)
                return null;

            var id = globalIdNode.InnerText;
          
            var schemeId = globalIdNode.Attributes["schemeID"]!.Value;

            return new GlobalIdentification(id, schemeId);
        }


        internal GlobalIdentification GetGlobalIdMandatoryScheme(XmlNode globalIdNode)
        {
            var id = globalIdNode.InnerText;
           
            var schemeId = default(string);

            schemeId = globalIdNode.Attributes["schemeID"]!.Value;

            return new GlobalIdentification(id, schemeId);
        }


        protected GlobalIdentification? GetActorGlobalIdentification(XmlNode tradePartyNode)
        {
            var globalIdentificationNode = tradePartyNode.SelectSingleNode("*[local-name() = 'GlobalID']");
            if (globalIdentificationNode == null)
                return null;

            var id = globalIdentificationNode.InnerText;
            var scheme = GetGlobalIdentificationScheme(globalIdentificationNode);

            return new Models.BasicWL.GlobalIdentification(id, scheme);
        }

        protected GlobalIdentification? GetListGlobalIdentification(XmlNode globalIdentificationNode)
        {
            var id = globalIdentificationNode.InnerText;
            var scheme = GetGlobalIdentificationScheme(globalIdentificationNode);

            return new Models.BasicWL.GlobalIdentification(id, scheme);
        }

        protected string? GetGlobalIdentificationScheme(XmlNode? globalIdNode)
        {
            if (globalIdNode == null)   
                return null;

            var schemeId = XmlParsingHelpers.ExtractAttribute(globalIdNode, "schemeID");
            if (string.IsNullOrWhiteSpace(schemeId))
                return null;

            return schemeId;
            //if (GlobalSchemeId.GetAll<GlobalSchemeId>().All(x => !x.Name.Equals(schemeId)))
            //    return null;

            //return Utils.Enumeration.FromDisplayName<GlobalSchemeId>(schemeId);
        }

        protected IEnumerable<GlobalIdentification>? GetGlobalIdentificationList(XmlNode tradePartyNode)
        {
            var globalIdNodeList = tradePartyNode.SelectNodes("*[local-name() = 'GlobalID']")?.Cast<XmlNode>()?.ToList();
            if (globalIdNodeList == null || globalIdNodeList.Count == 0)
                return null;

            var globalIdentificationList = new List<GlobalIdentification>();
            foreach (XmlNode globalIdNode in globalIdNodeList)
            {
                var globalIdentification = GetGlobalIdMandatoryScheme(globalIdNode);
                globalIdentificationList.Add(globalIdentification);
            }

            return globalIdentificationList;
        }

        protected string? GetId(XmlNode tradePartyNode)
        {
            var id = tradePartyNode.SelectSingleNode("*[local-name() = 'ID']");
            if (id == null)
                return null;

            return id.InnerText;
        }

        protected IEnumerable<string>? GetIdList(XmlNode tradePartyNode)
        {
            var idList = tradePartyNode.SelectNodes("*[local-name() = 'ID']");
            if (idList == null || idList.Count == 0)
                return null;

            return idList.Cast<XmlNode>().Select(x => x.InnerText).ToList();
        }

        protected string? GetLegalRegistrationScheme(XmlNode legalIdNode)
        {
            var schemeId = XmlParsingHelpers.ExtractAttribute(legalIdNode, "schemeID");
            if (string.IsNullOrWhiteSpace(schemeId))
                return null;

            return schemeId;
            //if (LegalSchemeId.GetAll<LegalSchemeId>().All(x => !x.Name.Equals(schemeId)))
            //    return null;

            //return Utils.Enumeration.FromDisplayName<LegalSchemeId>(schemeId);
        }

        #region PostalAddress
        protected string? GetCountryCode(XmlNode postalAddressNode)
        {
            var countryCodeNode = postalAddressNode.SelectSingleNode("*[local-name() = 'CountryID']");
            return countryCodeNode.InnerText;
        }

        protected string? GetAddressLineOne(XmlNode postalAddressNode)
        {
            var addressLineOneNode = postalAddressNode.SelectSingleNode("*[local-name() = 'LineOne']");
            return addressLineOneNode?.InnerText;
        }

        protected string? GetAddressLineTwo(XmlNode postalAddressNode)
        {
            var addressLineTwoNode = postalAddressNode.SelectSingleNode("*[local-name() = 'LineTwo']");
            return addressLineTwoNode?.InnerText;
        }

        protected string? GetAddressLineThree(XmlNode postalAddressNode)
        {
            var addressLineThreeNode = postalAddressNode.SelectSingleNode("*[local-name() = 'LineThree']");
            return addressLineThreeNode?.InnerText;
        }

        protected string? GetCityCode(XmlNode postalAddressNode)
        {
            var cityNode = postalAddressNode.SelectSingleNode("*[local-name() = 'CityName']");
            return cityNode?.InnerText;
        }

        protected string? GetPostcodeCode(XmlNode postalAddressNode)
        {
            var postCodeNode = postalAddressNode.SelectSingleNode("*[local-name() = 'PostcodeCode']");
            return postCodeNode?.InnerText;
        }

        protected string? GetCountrySubdivision(XmlNode postalAddressNode)
        {
            var countrySubdivisionNode = postalAddressNode.SelectSingleNode("*[local-name() = 'CountrySubDivisionName']");
            return countrySubdivisionNode?.InnerText;
        }
        #endregion

        protected Models.Minimum.PostalAddress? GetPostalAddress(XmlNode tradePartyNode)
        {
            var postalTradeAddress = tradePartyNode.SelectSingleNode("*[local-name() = 'PostalTradeAddress']");
            if (postalTradeAddress == null)
                return null;

            var countryCode = GetCountryCode(postalTradeAddress);
            
            var minimumPostalAddress = new Models.Minimum.PostalAddress(countryCode);
            if (ConformanceLevelType == FacturXConformanceLevelType.Minimum)
                return minimumPostalAddress;

            var addressLine1 = GetAddressLineOne(postalTradeAddress);
            var addressLine2 = GetAddressLineTwo(postalTradeAddress);
            var addressLine3 = GetAddressLineThree(postalTradeAddress);
            var addressLines = new PostalAddressLines(addressLine1, addressLine2, addressLine3);

            var city = GetCityCode(postalTradeAddress);
            var postCode = GetPostcodeCode(postalTradeAddress);
            var countrySubdivision = GetCountrySubdivision(postalTradeAddress);

            return new Models.BasicWL.PostalAddress(minimumPostalAddress, postCode, addressLines, city, countrySubdivision);
        }

        protected Models.Minimum.LegalRegistration? GetLegalRegistration(XmlNode tradePartyNode)
        {
            var legalNode = tradePartyNode.SelectSingleNode("*[local-name() = 'SpecifiedLegalOrganization']");
            if (legalNode == null)
                return null;

            var legalIdNode = legalNode.SelectSingleNode("*[local-name() = 'ID']");
            var id = legalIdNode?.InnerText;
            var scheme = XmlParsingHelpers.ExtractAttribute(legalIdNode, "schemeID");

            if (ConformanceLevelType == FacturXConformanceLevelType.Minimum)
            {
                return new Models.Minimum.LegalRegistration
                {
                    Id = id,
                    Scheme = scheme,
                };
            }

            var tradingNameNode = legalNode.SelectSingleNode("*[local-name() = 'TradingBusinessName']");
            var tradingBusinessName = tradingNameNode?.InnerText;

            if (ConformanceLevelType == FacturXConformanceLevelType.BasicWL || ConformanceLevelType == FacturXConformanceLevelType.Basic || ConformanceLevelType == FacturXConformanceLevelType.EN16931
                || ConformanceLevelType == FacturXConformanceLevelType.EN16931)
            {
                return new Models.BasicWL.LegalRegistration
                {
                    Id = id,
                    Scheme = scheme,
                    TradingName = tradingBusinessName,
                };
            }

            var postalAddress = GetPostalAddress(tradePartyNode) as Models.BasicWL.PostalAddress;

            return new Models.Extended.LegalRegistration
            {
                Id = id,
                Scheme = scheme,
                TradingName = tradingBusinessName,
                PostalAddress = postalAddress,
            };
        }

        protected TaxRegistration? GetVatRegistration(XmlNode tradePartyNode)
        {
            var vatNode = tradePartyNode.SelectSingleNode("*[local-name() = 'SpecifiedTaxRegistration']");
            if (vatNode == null)
                return null;

            var idNode = vatNode.SelectSingleNode("*[local-name() = 'ID']");

            var id = idNode.InnerText;
            var schemeId = idNode.Attributes["schemeID"]?.Value;

            return new TaxRegistration { Id = id, Scheme = TaxSchemeId.VA  };
        }

        protected TaxRegistration GetListVatRegistration(XmlNode taxNode)
        {
            var idNode = taxNode.SelectSingleNode("*[local-name() = 'ID']");
            if (idNode == null)
                return new TaxRegistration();

            var schemeId = XmlParsingHelpers.ExtractAttribute(idNode, "schemeID");
            var scheme = default(TaxSchemeId);
            if (schemeId != null && schemeId.Equals("VA"))
            {
                scheme = TaxSchemeId.VA;
            }

            var id = idNode?.InnerText;
            return new TaxRegistration { Id = id, Scheme = scheme };
        }

        protected IEnumerable<TaxRegistration>? GetVatRegistrationList(XmlNode tradePartyNode)
        {
            var taxNodeList = tradePartyNode.SelectNodes("*[local-name() = 'SpecifiedTaxRegistration']");
            if (taxNodeList == null)
                return null;

            var taxIdList = new List<TaxRegistration>();
            foreach (XmlNode taxNode in taxNodeList)
            {
                var taxId = GetListVatRegistration(taxNode);
                taxIdList.Add(taxId);
            }

            return taxIdList;
        }

        #region Contact
        private string? GetPersonName(XmlNode contactNode)
        {
            var personName = contactNode.SelectSingleNode("*[local-name() = 'PersonName']");
            if (personName == null)
                return null;

            return personName?.InnerText;
        }

        private string? GetDepartmentName(XmlNode contactNode)
        {
            var departmentName = contactNode.SelectSingleNode("*[local-name() = 'DepartmentName']");
            if (departmentName == null)
                return null;

            return departmentName?.InnerText;
        }

        protected virtual string? GetContactTelephoneNumber(XmlNode contactNode)
        {
            var phoneNumberNode = contactNode.SelectSingleNode("*[local-name() = 'TelephoneUniversalCommunication']");
            if (phoneNumberNode == null)
                return null;

            var completeNumber = phoneNumberNode.SelectSingleNode("*[local-name() = 'CompleteNumber']");
            if (completeNumber == null)
            {
                return null;
            }

            return completeNumber?.InnerText;
        }

        protected virtual ElectronicAddress? GetContactEmaiAddress(XmlNode contactNode)
        {
            var emailAddress = contactNode.SelectSingleNode("*[local-name() = 'EmailURIUniversalCommunication']");
            if (emailAddress == null)
                return null;

            var uriIdNode = emailAddress.SelectSingleNode("*[local-name() = 'URIID']");
            if (uriIdNode == null)
            {
                return null;
            }

            var id = uriIdNode.InnerText;

            if (uriIdNode.Attributes == null)
            {
                return new ElectronicAddress(id, null);
            }

            var schemeId = uriIdNode.Attributes["schemeID"]?.Value;
            return new ElectronicAddress(id, schemeId);
        }

        private string? GetContactType(XmlNode contactNode)
        {
            var typeNode = contactNode.SelectSingleNode("*[local-name() = 'TypeCode']");
            return typeNode?.InnerText;
        }

        protected virtual string? GetContactFaxNumber(XmlNode contactNode)
        {
            var faxNumber = contactNode.SelectSingleNode("*[local-name() = 'FaxUniversalCommunication']");
            if (faxNumber == null)
                return null;

            var completeNumber = faxNumber.SelectSingleNode("*[local-name() = 'CompleteNumber']");
            if (completeNumber == null)
            {
                return null;
            }

            return completeNumber?.InnerText;
        }

        protected Models.EN16931.Contact? GetContact(XmlNode tradePartyNode)
        {
            var contactNode = tradePartyNode.SelectSingleNode("*[local-name() = 'DefinedTradeContact']");
            if (contactNode == null)
                return null;

            var personName = GetPersonName(contactNode);
            var departmentName = GetDepartmentName(contactNode);
            var completeNumber = GetContactTelephoneNumber(contactNode);
            var electronicAddress = GetContactEmaiAddress(contactNode);

            var en16931Contact = new Models.EN16931.Contact
            {
                PersonName = personName,
                DepartmentName = departmentName,
                PhoneNumber = completeNumber,
                ElectronicAddress = electronicAddress,
            };

            if (ConformanceLevelType == FacturXConformanceLevelType.Extended)
            {
                var type = GetContactType(contactNode);
                var faxNumber = GetContactFaxNumber(contactNode);
                return new Models.Extended.Contact
                {
                    PersonName = personName,
                    DepartmentName = departmentName,
                    PhoneNumber = completeNumber,
                    ElectronicAddress = electronicAddress,
                    Type = type,
                    FaxNumber = faxNumber,
                };
            }

            return en16931Contact;
        }
        #endregion

        protected virtual ElectronicAddress? GetElectronicAddress(XmlNode tradePartyNode)
        {
            var universalCommunicationURIID = tradePartyNode.SelectSingleNode("*[local-name() = 'URIUniversalCommunication']");
            if (universalCommunicationURIID == null)
                return null;

            var urridNode = universalCommunicationURIID.SelectSingleNode("*[local-name() = 'URIID']");
            if (urridNode == null)
                return null;

            var id = urridNode.InnerText;
           
            if (urridNode.Attributes == null)
            {
                return new ElectronicAddress(id, null);   
            }

            var schemeId = urridNode.Attributes["schemeID"]?.Value;
            return new ElectronicAddress(id, schemeId);
        }

        protected TaxRegistration? GetFiscalRegistration(XmlNode tradePartyNode)
        {
            var vatNodes = tradePartyNode.SelectNodes("*[local-name() = 'SpecifiedTaxRegistration']")?.Cast<XmlNode>()?.ToList();
            if (vatNodes == null || vatNodes.Count == 0)
                return null;

            var fiscalNode = vatNodes.Where(x => x.SelectSingleNode("*[local-name() = 'ID']") != null)
                .Where(x => x.Attributes != null && x.Attributes["schemeID"] != null)
                .FirstOrDefault(x => x.Attributes["schemeID"]!.Value.Equals("FC"));

            if (fiscalNode == null)
                return null;

            var fiscalIdNode = fiscalNode.SelectSingleNode("*[local-name() = 'ID']");
            var id = fiscalNode?.InnerText;
            return new TaxRegistration { Id = id, Scheme = TaxSchemeId.FC };
        }
    }
}
