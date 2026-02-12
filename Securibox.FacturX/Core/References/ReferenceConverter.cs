using System.Xml;
using Securibox.FacturX.Models;
using Securibox.FacturX.Models.Enums;

namespace Securibox.FacturX.Core.References
{
    internal class ReferenceConverter : BaseReferenceConverter
    {
        internal ReferenceConverter(
            FacturXConformanceLevelType conformanceLevelType,
            XmlDocument xmlDocument
        )
            : base(conformanceLevelType, xmlDocument) { }

        internal Models.Minimum.BuyerReference? GetBuyerReference()
        {
            if (BuyerReference == null)
                return null;

            return new Models.Minimum.BuyerReference { Id = BuyerReference.InnerText };
        }

        internal Models.BasicWL.ContractReference? GetContractReference()
        {
            if (ContractReference == null)
                return null;

            var assignedId = GetAssignedId(ContractReference);

            if (
                ConformanceLevelType == FacturXConformanceLevelType.BasicWL
                || ConformanceLevelType == FacturXConformanceLevelType.Basic
                || ConformanceLevelType == FacturXConformanceLevelType.EN16931
            )
                return new Models.BasicWL.ContractReference { AssignedId = assignedId };

            var codeType = GetReferenceTypeCode(ContractReference);

            var issueDate = GetIssueDate(ContractReference);

            return new Models.Extended.ContractReference
            {
                AssignedId = assignedId,
                Type = codeType,
                IssueDate = issueDate,
            };
        }

        internal Models.Extended.DeliveryNoteReference? GetDeliveryNoteReference()
        {
            if (DeliveryNoteReference == null)
                return null;

            var assignedId = GetAssignedId(DeliveryNoteReference);
            var issueDate = GetIssueDate(DeliveryNoteReference);

            return new Models.Extended.DeliveryNoteReference
            {
                AssignedId = assignedId,
                IssueDate = issueDate,
            };
        }

        internal Models.BasicWL.DespacthAdviceReference? GetDespacthAdviceReference()
        {
            if (DespatchAdviceReference == null)
                return null;

            var assignedId = GetAssignedId(DespatchAdviceReference);

            if (ConformanceLevelType == FacturXConformanceLevelType.Extended)
            {
                var issueDate = GetIssueDate(DespatchAdviceReference);

                return new Models.Extended.DespacthAdviceReference
                {
                    AssignedId = assignedId,
                    IssueDate = issueDate,
                };
            }

            return new Models.BasicWL.DespacthAdviceReference { AssignedId = assignedId };
        }

        internal Models.EN16931.InvoicedObjectIdentifier? GetInvoicedObjectIdentifier()
        {
            if (InvoicedObjectIdentifier == null)
                return null;

            var assignedId = GetAssignedId(InvoicedObjectIdentifier);
            var scheme = GetReferenceTypeCode(InvoicedObjectIdentifier);

            if (ConformanceLevelType == FacturXConformanceLevelType.EN16931)
            {
                return new Models.EN16931.InvoicedObjectIdentifier
                {
                    AssignedId = assignedId,
                    Type = Models
                        .EN16931
                        .Enum
                        .AdditionalDocumentReferenceType
                        .InvoicedObjectIdentifier,
                    Scheme = scheme,
                };
            }

            var issueDate = GetIssueDate(InvoicedObjectIdentifier);

            return new Models.Extended.InvoicedObjectIdentifier
            {
                AssignedId = assignedId,
                Type = Models.EN16931.Enum.AdditionalDocumentReferenceType.InvoicedObjectIdentifier,
                Scheme = scheme,
                IssueDate = issueDate,
            };
        }

        internal Models.BasicWL.BuyerAccountingReference? GetBuyerAccountingReference()
        {
            if (BuyerAccountingReference == null)
                return null;

            var id = GetId(BuyerAccountingReference);
            var basicWLBuyerAccountingReference = new Models.BasicWL.BuyerAccountingReference
            {
                Id = id,
            };

            if (ConformanceLevelType == FacturXConformanceLevelType.Extended)
            {
                var type = GetTypeCode(BuyerAccountingReference);
                return new Models.Extended.BuyerAccountingReference { Id = id, Type = type };
            }

            return basicWLBuyerAccountingReference;
        }

        private Models.BasicWL.PreviousInvoiceReference GetPreviousInvoiceReference(
            XmlNode referenceNode
        )
        {
            var assignedId = GetAssignedId(referenceNode);
            var issueDate = GetIssueDate(referenceNode);

            var basicWLPreviousInvoiceReference = new Models.BasicWL.PreviousInvoiceReference
            {
                AssignedId = assignedId,
                IssueDate = issueDate,
            };

            if (ConformanceLevelType == FacturXConformanceLevelType.Extended)
            {
                var type = GetTypeCode(referenceNode);
                return new Models.Extended.PreviousInvoiceReference
                {
                    AssignedId = assignedId,
                    IssueDate = issueDate,
                    Type = type,
                };
            }

            return basicWLPreviousInvoiceReference;
        }

        internal IEnumerable<IReference>? GetPreviousInvoiceReferenceList()
        {
            if (
                PreviousInvoiceReferenceNodeList == null
                || PreviousInvoiceReferenceNodeList.Count == 0
            )
                return null;

            var previousInvoiceReferenceList = new List<IReference>();
            foreach (XmlNode previousInvoiceReferenceNode in PreviousInvoiceReferenceNodeList)
            {
                var previousInvoiceReference = GetPreviousInvoiceReference(
                    previousInvoiceReferenceNode
                );
                previousInvoiceReferenceList.Add(previousInvoiceReference);
            }

            return previousInvoiceReferenceList;
        }

        internal Models.EN16931.ProjectReference? GetProjectReference()
        {
            if (ProjectReference == null)
                return null;

            var id = GetId(ProjectReference);

            var name = GetName(ProjectReference)!;

            return new Models.EN16931.ProjectReference { Id = id, Name = name };
        }

        internal Models.Minimum.PurchaseOrderReference? GetPurchaseOrderReference()
        {
            if (PurchaseOrderReference == null)
                return null;

            if (ConformanceLevelType == FacturXConformanceLevelType.Extended)
            {
                return new Models.Extended.PurchaseOrderReference
                {
                    AssignedId = GetAssignedId(PurchaseOrderReference),
                    IssueDate = GetIssueDate(PurchaseOrderReference),
                };
            }

            return new Models.Minimum.PurchaseOrderReference
            {
                AssignedId = GetAssignedId(PurchaseOrderReference),
            };
        }

        internal Models.Extended.QuotationReference? GetQuotationReference()
        {
            if (QuotationReference == null)
                return null;

            return new Models.Extended.QuotationReference
            {
                AssignedId = GetAssignedId(QuotationReference),
                IssueDate = GetIssueDate(QuotationReference),
            };
        }

        internal Models.EN16931.ReceivingAdviceReference? GetReceivingAdviceReference()
        {
            if (ReceivingAdviceReference == null)
                return null;

            if (ConformanceLevelType == FacturXConformanceLevelType.Extended)
            {
                return new Models.Extended.ReceivingAdviceReference
                {
                    AssignedId = GetAssignedId(ReceivingAdviceReference),
                    IssueDate = GetIssueDate(ReceivingAdviceReference),
                };
            }

            return new Models.EN16931.ReceivingAdviceReference
            {
                AssignedId = GetAssignedId(ReceivingAdviceReference),
            };
        }

        internal Models.EN16931.SalesOrderReference? GetSalesOrderReference()
        {
            if (SalesOrderReference == null)
                return null;

            if (ConformanceLevelType == FacturXConformanceLevelType.EN16931)
            {
                return new Models.EN16931.SalesOrderReference
                {
                    AssignedId = GetAssignedId(SalesOrderReference),
                };
            }

            return new Models.Extended.SalesOrderReference
            {
                AssignedId = GetAssignedId(SalesOrderReference),
                IssueDate = GetIssueDate(SalesOrderReference),
            };
        }

        private Models.EN16931.SupportingDocument GetSupportingDocumentReference(
            XmlNode referenceNode
        )
        {
            if (ConformanceLevelType == FacturXConformanceLevelType.EN16931)
            {
                return new Models.EN16931.SupportingDocument
                {
                    AssignedId = GetAssignedId(referenceNode),
                    Type = Models
                        .EN16931
                        .Enum
                        .AdditionalDocumentReferenceType
                        .AdditionalDocumentReference,
                    Description = GetName(referenceNode),
                    ExternalLocation = GetURRID(referenceNode),
                    AttachedDocument = GetAttachment(referenceNode),
                };
            }

            return new Models.Extended.SupportingDocument
            {
                AssignedId = GetAssignedId(referenceNode),
                Type = Models
                    .EN16931
                    .Enum
                    .AdditionalDocumentReferenceType
                    .AdditionalDocumentReference,
                Description = GetName(referenceNode),
                ExternalLocation = GetURRID(referenceNode),
                AttachedDocument = GetAttachment(referenceNode),
                IssueDate = GetIssueDate(referenceNode),
            };
        }

        internal IEnumerable<IReference>? GetSupportingDocumentReferenceList()
        {
            if (
                SupportingDocumentReferenceNodeList == null
                || !SupportingDocumentReferenceNodeList.Any()
            )
                return null;

            var supportingDocumentList = new List<IReference>();
            foreach (XmlNode supportingDocumentNode in SupportingDocumentReferenceNodeList)
            {
                var supportingDocument = GetSupportingDocumentReference(supportingDocumentNode);
                supportingDocumentList.Add(supportingDocument);
            }

            return supportingDocumentList;
        }

        internal Models.EN16931.TenderOrLotReference? GetTenderOrLotReference()
        {
            if (TenderOrLotReference == null)
                return null;

            if (ConformanceLevelType == FacturXConformanceLevelType.EN16931)
            {
                return new Models.EN16931.TenderOrLotReference
                {
                    AssignedId = GetAssignedId(TenderOrLotReference),
                    Type = Models.EN16931.Enum.AdditionalDocumentReferenceType.TenderOrLotReference,
                };
            }

            return new Models.Extended.TenderOrLotReference
            {
                AssignedId = GetAssignedId(TenderOrLotReference),
                Type = Models.EN16931.Enum.AdditionalDocumentReferenceType.TenderOrLotReference,
                IssueDate = GetIssueDate(TenderOrLotReference),
            };
        }

        private Models.Extended.UltimateCustomerOrderReference GetUltimateCustomerOrderReference(
            XmlNode referenceNode
        )
        {
            return new Models.Extended.UltimateCustomerOrderReference
            {
                AssignedId = GetAssignedId(referenceNode),
                IssueDate = GetIssueDate(referenceNode),
            };
        }

        internal IEnumerable<IReference>? GetUltimateCustomerOrderReferenceList()
        {
            if (
                UltimateCustomerOrderReferenceNodeList == null
                || UltimateCustomerOrderReferenceNodeList.Count == 0
            )
                return null;

            var ultimateCustomerOrderReferenceList = new List<IReference>();
            foreach (
                XmlNode ultimateCustomerOrderReferenceNode in UltimateCustomerOrderReferenceNodeList
            )
            {
                var ultimateCustomerOrderReference = GetUltimateCustomerOrderReference(
                    ultimateCustomerOrderReferenceNode
                );
                ultimateCustomerOrderReferenceList.Add(ultimateCustomerOrderReference);
            }

            return ultimateCustomerOrderReferenceList;
        }

        internal IEnumerable<Models.Extended.LineAdditionalDocument>? GetLineAdditionalDocumentReferenceList(
            XmlNode lineNode
        )
        {
            var documentReferenceNodeList = lineNode
                .SelectSingleNode("*[local-name() = 'SpecifiedLineTradeAgreement']")!
                .SelectNodes("*[local-name() = 'AdditionalReferencedDocument']")
                ?.Cast<XmlNode>()
                .Where(x =>
                    x.SelectSingleNode("*[local-name() = 'TypeCode']")!.InnerText.Equals("916")
                )
                .ToList();

            if (documentReferenceNodeList == null || !documentReferenceNodeList.Any())
                return null;

            var additionalDocumentList = new List<Models.Extended.LineAdditionalDocument>();
            foreach (XmlNode documentNode in documentReferenceNodeList)
            {
                var additionalDocument = new Models.Extended.LineAdditionalDocument();
                additionalDocument.AddAssignedId(GetAssignedId(documentNode)!);
                additionalDocument.AddTypeCode(GetTypeCode(documentNode)!);
                additionalDocument.AddReferenceTypeCode(GetReferenceTypeCode(documentNode));
                additionalDocument.AddExternalLocation(GetURRID(documentNode));
                additionalDocument.AddLineId(GetLineId(documentNode));
                additionalDocument.AddIssueDate(GetIssueDate(documentNode));
                additionalDocument.AddAttachedDocument(GetAttachment(documentNode));
                additionalDocument.AddNameList(GetNameList(documentNode));

                additionalDocumentList.Add(additionalDocument);
            }

            return additionalDocumentList;
        }

        internal Models.BasicWL.BuyerAccountingReference? GetLineBuyerAccountingReference(
            XmlNode lineNode
        )
        {
            var buyerAccountingReference = lineNode
                .SelectSingleNode("*[local-name() = 'SpecifiedLineTradeSettlement']")!
                .SelectSingleNode("*[local-name() = 'ReceivableSpecifiedTradeAccountingAccount']");

            if (buyerAccountingReference == null)
                return null;

            if (ConformanceLevelType == FacturXConformanceLevelType.Extended)
            {
                return new Models.Extended.BuyerAccountingReference
                {
                    Id = GetId(buyerAccountingReference),
                    Type = GetTypeCode(buyerAccountingReference),
                };
            }

            return new Models.BasicWL.BuyerAccountingReference
            {
                Id = GetId(buyerAccountingReference),
            };
        }

        internal Models.Extended.LineContractReference? GetLineContractReference(XmlNode lineNode)
        {
            var contractReferenceNode = lineNode
                .SelectSingleNode("*[local-name() = 'SpecifiedLineTradeAgreement']")!
                .SelectSingleNode("*[local-name() = 'ContractReferencedDocument']");

            if (contractReferenceNode == null)
                return null;

            var contractReference = new Models.Extended.LineContractReference();
            contractReference.AddAssignedId(GetAssignedId(contractReferenceNode));
            contractReference.AddLineId(GetLineId(contractReferenceNode));
            contractReference.AddIssueDate(GetIssueDate(contractReferenceNode));
            return contractReference;
        }

        internal Models.Extended.DeliveryNoteReference? GetLineDeliveryNoteReference(
            XmlNode lineNode
        )
        {
            var deliveryNoteReference = lineNode
                .SelectSingleNode("*[local-name() = 'SpecifiedLineTradeDelivery']")!
                .SelectSingleNode("*[local-name() = 'DeliveryNoteReferencedDocument']");

            if (deliveryNoteReference == null)
                return null;

            return new Models.Extended.LineDeliveryNoteReference
            {
                LineId = GetLineId(deliveryNoteReference),
                AssignedId = GetAssignedId(deliveryNoteReference),
                IssueDate = GetIssueDate(deliveryNoteReference),
            };
        }

        internal Models.Extended.LineDespacthAdviceReference? GetLineDespacthAdviceReference(
            XmlNode lineNode
        )
        {
            var despatchAdviceReference = lineNode
                .SelectSingleNode("*[local-name() = 'SpecifiedLineTradeDelivery']")!
                .SelectSingleNode("*[local-name() = 'DespatchAdviceReferencedDocument']");

            if (despatchAdviceReference == null)
                return null;

            return new Models.Extended.LineDespacthAdviceReference
            {
                LineId = GetLineId(despatchAdviceReference),
                AssignedId = GetAssignedId(despatchAdviceReference),
                IssueDate = GetIssueDate(despatchAdviceReference),
            };
        }

        internal Models.EN16931.InvoicedObjectIdentifier? GetLineInvoicedObjectIdentifier(
            XmlNode lineNode
        )
        {
            var additionalDocumentReferenceList = lineNode
                .SelectSingleNode("*[local-name() = 'SpecifiedLineTradeSettlement']")!
                .SelectNodes("*[local-name() = 'AdditionalReferencedDocument']");

            var invoicedObjectIdentifier = additionalDocumentReferenceList
                ?.Cast<XmlNode>()
                .FirstOrDefault(x =>
                    x.SelectSingleNode("*[local-name() = 'TypeCode']")?.InnerText == "130"
                );

            if (invoicedObjectIdentifier == null)
                return null;

            if (ConformanceLevelType == FacturXConformanceLevelType.EN16931)
            {
                return new Models.EN16931.InvoicedObjectIdentifier
                {
                    AssignedId = GetAssignedId(invoicedObjectIdentifier),
                    Type = Models
                        .EN16931
                        .Enum
                        .AdditionalDocumentReferenceType
                        .InvoicedObjectIdentifier,
                    Scheme = GetReferenceTypeCode(invoicedObjectIdentifier),
                };
            }

            return new Models.Extended.InvoicedObjectIdentifier
            {
                AssignedId = GetAssignedId(invoicedObjectIdentifier),
                Type = Models.EN16931.Enum.AdditionalDocumentReferenceType.InvoicedObjectIdentifier,
                Scheme = GetReferenceTypeCode(invoicedObjectIdentifier),
                IssueDate = GetIssueDate(invoicedObjectIdentifier),
            };
        }

        internal Models.BasicWL.PreviousInvoiceReference? GetLinePreviousInvoiceReference(
            XmlNode lineNode
        )
        {
            var previousInvoiceReferenceNode = lineNode
                .SelectSingleNode("*[local-name() = 'SpecifiedLineTradeSettlement']")!
                .SelectSingleNode("*[local-name() = 'InvoiceReferencedDocument']");

            if (previousInvoiceReferenceNode == null)
                return null;

            if (ConformanceLevelType == FacturXConformanceLevelType.Extended)
            {
                return new Models.Extended.PreviousInvoiceReference
                {
                    AssignedId = GetAssignedId(previousInvoiceReferenceNode),
                    IssueDate = GetIssueDate(previousInvoiceReferenceNode),
                    Type = GetTypeCode(previousInvoiceReferenceNode),
                };
            }

            return new Models.BasicWL.PreviousInvoiceReference
            {
                AssignedId = GetAssignedId(previousInvoiceReferenceNode),
                IssueDate = GetIssueDate(previousInvoiceReferenceNode),
            };
        }

        internal Models.EN16931.LinePurchaseOrderReference? GetLinePurchaseOrderReference(
            XmlNode lineNode
        )
        {
            var purchaseOrderReference = lineNode
                .SelectSingleNode("*[local-name() = 'SpecifiedLineTradeAgreement']")!
                .SelectSingleNode("*[local-name() = 'BuyerOrderReferencedDocument']");

            if (purchaseOrderReference == null)
                return null;

            if (ConformanceLevelType == FacturXConformanceLevelType.EN16931)
            {
                return new Models.EN16931.LinePurchaseOrderReference(
                    GetLineId(purchaseOrderReference)
                );
            }

            var linePurchaseOrderReference = new Models.Extended.LinePurchaseOrderReference(
                GetLineId(purchaseOrderReference)
            );
            linePurchaseOrderReference.AddAssignedId(GetAssignedId(purchaseOrderReference));
            linePurchaseOrderReference.AddIssueDate(GetIssueDate(purchaseOrderReference));
            return linePurchaseOrderReference;
        }

        internal Models.Extended.QuotationReference? GetLineQuotationReference(XmlNode lineNode)
        {
            var quotationReferenceNode = lineNode
                .SelectSingleNode("*[local-name() = 'SpecifiedLineTradeAgreement']")!
                .SelectSingleNode("*[local-name() = 'QuotationReferencedDocument']");

            if (quotationReferenceNode == null)
                return null;

            var quotationReference = new Models.Extended.LineQuotationReference();
            quotationReference.AddLineId(GetLineId(quotationReferenceNode));
            quotationReference.AddAssignedId(GetAssignedId(quotationReferenceNode));
            quotationReference.AddIssueDate(GetIssueDate(quotationReferenceNode));
            return quotationReference;
        }

        internal Models.Extended.LineReceivingAdviceReference? GetLineReceivingAdviceReference(
            XmlNode lineNode
        )
        {
            var receivingAdviceReference = lineNode
                .SelectSingleNode("*[local-name() = 'SpecifiedLineTradeAgreement']")!
                .SelectSingleNode("*[local-name() = 'ReceivingAdviceReferencedDocument']");

            if (receivingAdviceReference == null)
                return null;

            return new Models.Extended.LineReceivingAdviceReference
            {
                LineId = GetLineId(receivingAdviceReference),
                AssignedId = GetAssignedId(receivingAdviceReference),
                IssueDate = GetIssueDate(receivingAdviceReference),
            };
        }

        internal IEnumerable<Models.Extended.UltimateCustomerOrderReference>? GetLineUltimateCustomerOrderReferenceList(
            XmlNode lineNode
        )
        {
            var ultimateCustomerOrderReferenceNodeList = lineNode
                .SelectSingleNode("*[local-name() = 'SpecifiedLineTradeAgreement']")!
                .SelectNodes("*[local-name() = 'UltimateCustomerOrderReferencedDocument']");

            if (
                ultimateCustomerOrderReferenceNodeList == null
                || ultimateCustomerOrderReferenceNodeList.Count == 0
            )
                return null;

            var ultimateCustomerOrderReferenceList =
                new List<Models.Extended.UltimateCustomerOrderReference>();
            foreach (
                XmlNode ultimateCustomerOrderReferenceNode in ultimateCustomerOrderReferenceNodeList
            )
            {
                var ultimateCustomerOrderReference =
                    new Models.Extended.UltimateCustomerOrderReference
                    {
                        AssignedId = GetAssignedId(ultimateCustomerOrderReferenceNode),
                        IssueDate = GetIssueDate(ultimateCustomerOrderReferenceNode),
                    };

                ultimateCustomerOrderReferenceList.Add(ultimateCustomerOrderReference);
            }

            return ultimateCustomerOrderReferenceList;
        }
    }
}
