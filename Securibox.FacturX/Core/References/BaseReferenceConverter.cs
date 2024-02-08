using Securibox.FacturX.Core.References;
using Securibox.FacturX.Models;
using Securibox.FacturX.Models.EN16931;
using Securibox.FacturX.Models.Enums;
using System.Xml;

namespace Securibox.FacturX.Core
{
    internal abstract class BaseReferenceConverter
    {
        protected readonly FacturXConformanceLevelType ConformanceLevelType;

        protected readonly XmlNode? BuyerAccountingReference;
        protected readonly XmlNode? BuyerReference;
        protected readonly XmlNode? ContractReference;
        protected readonly XmlNode? DeliveryNoteReference;
        protected readonly XmlNode? DespatchAdviceReference;
        protected readonly XmlNode? InvoicedObjectIdentifier;
        protected readonly XmlNodeList? PreviousInvoiceReferenceNodeList;
        protected readonly XmlNode? ProjectReference;
        protected readonly XmlNode? PurchaseOrderReference;
        protected readonly XmlNode? QuotationReference;
        protected readonly XmlNode? ReceivingAdviceReference;
        protected readonly XmlNode? SalesOrderReference;
        protected readonly XmlNode? TenderOrLotReference;
        protected readonly IEnumerable<XmlNode>? SupportingDocumentReferenceNodeList;
        protected readonly XmlNodeList? UltimateCustomerOrderReferenceNodeList;


        protected BaseReferenceConverter(FacturXConformanceLevelType conformanceLevelType, XmlDocument xmlDocument)
        {
            ConformanceLevelType = conformanceLevelType;

            var tradeAgreementNode = xmlDocument.SelectSingleNode("//*[local-name() = 'ApplicableHeaderTradeAgreement']")!;
            BuyerReference = tradeAgreementNode.SelectSingleNode("*[local-name() = 'BuyerReference']");
            ContractReference = tradeAgreementNode.SelectSingleNode("*[local-name() = 'ContractReferencedDocument']");
            ProjectReference = tradeAgreementNode.SelectSingleNode("*[local-name() = 'SpecifiedProcuringProject']");
            PurchaseOrderReference  = tradeAgreementNode.SelectSingleNode("*[local-name() = 'BuyerOrderReferencedDocument']");
            QuotationReference = tradeAgreementNode.SelectSingleNode("*[local-name() = 'QuotationReferencedDocument']");
            SalesOrderReference = tradeAgreementNode.SelectSingleNode("*[local-name() = 'SellerOrderReferencedDocument']");

            UltimateCustomerOrderReferenceNodeList = tradeAgreementNode.SelectNodes("*[local-name() = 'UltimateCustomerOrderReferencedDocument']");

            var additionalDocumentReferenceList = tradeAgreementNode?.SelectNodes("*[local-name() = 'AdditionalReferencedDocument']");

            SupportingDocumentReferenceNodeList = additionalDocumentReferenceList?
               .Cast<XmlNode>()
               .Where(x => x.SelectSingleNode("*[local-name() = 'TypeCode']")?.InnerText == "916");

            InvoicedObjectIdentifier = additionalDocumentReferenceList?
                .Cast<XmlNode>()
                .FirstOrDefault(x => x.SelectSingleNode("*[local-name() = 'TypeCode']")?.InnerText == "130");

            TenderOrLotReference = additionalDocumentReferenceList?
                .Cast<XmlNode>()
                .FirstOrDefault(x => x.SelectSingleNode("*[local-name() = 'TypeCode']")?.InnerText == "50");



            var tradeSettlementNode = xmlDocument.SelectSingleNode("//*[local-name() = 'ApplicableHeaderTradeSettlement']")!;

            BuyerAccountingReference = tradeSettlementNode.SelectSingleNode("*[local-name() = 'ReceivableSpecifiedTradeAccountingAccount']");

            PreviousInvoiceReferenceNodeList = tradeSettlementNode?.SelectNodes("*[local-name() = 'InvoiceReferencedDocument']");

            var tradeDeliveryNode = xmlDocument.SelectSingleNode("//*[local-name() = 'ApplicableHeaderTradeDelivery']");
            DeliveryNoteReference = tradeDeliveryNode?.SelectSingleNode("*[local-name() = 'DeliveryNoteReferencedDocument']");
            DespatchAdviceReference = tradeDeliveryNode?.SelectSingleNode("*[local-name() = 'DespatchAdviceReferencedDocument']");
            ReceivingAdviceReference = tradeDeliveryNode?.SelectSingleNode("*[local-name() = 'ReceivingAdviceReferencedDocument']");

        }

        internal virtual IReference? GetReference()
        {
            return null;
        }

        internal virtual IEnumerable<IReference>? GetReferenceList() 
        {
            return null;
        }

        protected string? GetName(XmlNode referenceNode)
        {
            return referenceNode.SelectSingleNode("*[local-name() = 'Name']")?.InnerText;
        }

        protected IEnumerable<string>? GetNameList(XmlNode referenceNode)
        {
            var nameNodeList = referenceNode.SelectNodes("*[local-name() = 'Name']");
            if (nameNodeList == null)
                return null;

            return nameNodeList.Cast<XmlNode>().Select(x => x.InnerText).ToList();
        }

        protected string? GetLineId(XmlNode referenceNode)
        {
            return referenceNode.SelectSingleNode("*[local-name() = 'LineID']")?.InnerText;
        }

        protected string? GetId(XmlNode referenceNode)
        {
            var idNode = referenceNode.SelectSingleNode("*[local-name() = 'ID']");
            if (idNode == null)
                return null;

            return idNode.InnerText;
        }

        protected virtual string? GetAssignedId(XmlNode referenceNode)
        {
            var assignedIdNode = referenceNode.SelectSingleNode("*[local-name() = 'IssuerAssignedID']");
            if (assignedIdNode == null)
                return null;

            return assignedIdNode.InnerText;
        }

        protected virtual string? GetTypeCode(XmlNode referenceNode)
        {
            var typeCodeNode = referenceNode.SelectSingleNode("*[local-name() = 'TypeCode']");
            if (typeCodeNode == null)
                return null;

            return typeCodeNode.InnerText;
        }

        protected virtual DateTime? GetIssueDate(XmlNode referenceNode)
        {
            var issueDateNode = referenceNode.SelectSingleNode("*[local-name() = 'FormattedIssueDateTime']");
            if (issueDateNode == null)
                return null;

            var dateStringNode = issueDateNode.SelectSingleNode("*[local-name() = 'DateTimeString']")!;
            return XmlParsingHelpers.ExtractDateTime(dateStringNode);
        }

        protected Attachment? GetAttachment(XmlNode referenceNode)
        {
            var attachmentNode = referenceNode.SelectSingleNode("*[local-name() = 'AttachmentBinaryObject']");
            if (attachmentNode == null)
                return null;

            var attachment = new Attachment();

            if (attachmentNode.Attributes != null)
            {
                attachment.MimeCode = attachmentNode.Attributes["mimeCode"]?.Value;
                attachment.Filename = attachmentNode.Attributes["filename"]?.Value;
            }

            return attachment;
        }

        protected string? GetURRID(XmlNode referenceNode)
        {
            var externalLocationNode = referenceNode.SelectSingleNode("*[local-name() = 'URIID']");
            return externalLocationNode?.InnerText;
        }

        protected string? GetReferenceTypeCode(XmlNode referenceNode)
        {
            return referenceNode.SelectSingleNode("*[local-name() = 'ReferenceTypeCode']")?.InnerText;
        }
    }
}
