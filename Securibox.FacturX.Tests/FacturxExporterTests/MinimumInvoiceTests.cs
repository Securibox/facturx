using NUnit.Framework;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Securibox.FacturX.Tests.FacturxExporterTests
{
    internal class MinimumInvoiceTests
    {
        private readonly string _mainDir = Path.Combine(System.IO.Directory.GetCurrentDirectory()?.Split("bin").First()!, "Invoices", "Custom");
        private readonly string _invoiceName = "2023-6026_facture_facturx_minimum.pdf";

        [SetUp]
        public void Setup()
        {
            TestContext.WriteLine(_mainDir);
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            var outputPath = Path.Combine(_mainDir, _invoiceName);
            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }
        }

        public static SpecificationModels.Minimum.CrossIndustryInvoice GetInvoice()
        {
            Securibox.FacturX.SpecificationModels.Minimum.CrossIndustryInvoice invoice = new Securibox.FacturX.SpecificationModels.Minimum.CrossIndustryInvoice()
            {
                ExchangedDocument = new SpecificationModels.Minimum.ExchangedDocument()
                {
                    ID = new SpecificationModels.Minimum.ID() { Value = "2023-6026" },
                    IssueDateTime = new SpecificationModels.Minimum.IssueDateTime() { DateTimeString = new SpecificationModels.Minimum.DateTimeString() { Value = "20230928", Format = "102" } },
                    TypeCode = "380"
                },
                ExchangedDocumentContext = new SpecificationModels.Minimum.ExchangedDocumentContext()
                {
                    BusinessProcessSpecifiedDocumentContextParameter = new SpecificationModels.Minimum.DocumentContextParameter()
                    {
                        ID = new SpecificationModels.Minimum.ID() { Value = "A1" },
                    },
                    GuidelineSpecifiedDocumentContextParameter = new SpecificationModels.Minimum.DocumentContextParameter()
                    {
                        ID = new SpecificationModels.Minimum.ID() { Value = "urn:factur-x.eu:1p0:minimum" }
                    },
                },
                SupplyChainTradeTransaction = new SpecificationModels.Minimum.SupplyChainTradeTransaction()
                {
                    ApplicableHeaderTradeAgreement = new SpecificationModels.Minimum.HeaderTradeAgreement()
                    {
                        BuyerTradeParty = new SpecificationModels.Minimum.TradeParty()
                        {
                            Name = "Securibox SARL",
                            SpecifiedLegalOrganization = new SpecificationModels.Minimum.LegalOrganization()
                            {
                                ID = new SpecificationModels.Minimum.ID() { Value = "50000371000034", SchemeID = "0002" },
                            },
                        },
                        SellerTradeParty = new SpecificationModels.Minimum.TradeParty()
                        {
                            Name = "Société Hôtelière du Pacano",
                            SpecifiedLegalOrganization = new SpecificationModels.Minimum.LegalOrganization()
                            {
                                ID = new SpecificationModels.Minimum.ID()
                                {
                                    Value = "12345682400016",
                                    SchemeID = "0002"
                                },
                            },
                            PostalTradeAddress = new SpecificationModels.Minimum.TradeAddress()
                            {
                                CountryID = "FR"
                            }
                        },
                    },
                    ApplicableHeaderTradeDelivery = new SpecificationModels.Minimum.HeaderTradeDelivery() { },
                    ApplicableHeaderTradeSettlement = new SpecificationModels.Minimum.HeaderTradeSettlement()
                    {
                        InvoiceCurrencyCode = "EUR",
                        SpecifiedTradeSettlementHeaderMonetarySummation = new SpecificationModels.Minimum.TradeSettlementHeaderMonetarySummation()
                        {
                            TaxBasisTotalAmount = new SpecificationModels.Minimum.Amount() { Value = 207.55m },
                            TaxTotalAmount = new SpecificationModels.Minimum.Amount() { Value = 20.59m, CurrencyID = "EUR" },
                            GrandTotalAmount = new SpecificationModels.Minimum.Amount()
                            {
                                Value = 228.14m,
                            },
                            DuePayableAmount = new SpecificationModels.Minimum.Amount()
                            {
                                Value = 228.14m,
                            },
                        },
                    },
                }
            };

            return invoice;
        }

        [Test]
        [Order(1)]
        public async Task WriteData_Minimum_SUCCESS()
        {
            var outputPath = Path.Combine(_mainDir, _invoiceName);

            Securibox.FacturX.SpecificationModels.Minimum.CrossIndustryInvoice invoiceToExport = GetInvoice();
            FacturxExporter exporter = new FacturxExporter();

            using (var stream = exporter.CreateFacturXStream(
                Path.Combine(_mainDir, "2023-6026_facture_minimum.pdf"),
                invoiceToExport,
                $"SEPEM: Invoice ",
                $"Invoice "))
            {
                using (var fileStream = new FileStream(outputPath, FileMode.Create))
                {
                    await stream.CopyToAsync(fileStream);
                }
            }
        }

        [Test]
        [Order(2)]
        public void AssertWrittenData_Minimum_SUCCESS()
        {
            var invoicePath = Path.Combine(_mainDir, "2023-6026_facture_facturx_minimum.pdf");

            var importer = new FacturxImporter(invoicePath);
            var minimumInvoice = importer.ImportDataWithDeserialization() as Securibox.FacturX.SpecificationModels.Minimum.CrossIndustryInvoice;

            Assert.That(minimumInvoice is not null);

            Assert.That(minimumInvoice!.ExchangedDocument.ID.Value, Is.EqualTo("2023-6026"));
            Assert.That(minimumInvoice!.ExchangedDocument.IssueDateTime.DateTimeString.Value, Is.EqualTo("20230928"));
            Assert.That(minimumInvoice!.ExchangedDocument.IssueDateTime.DateTimeString.Format, Is.EqualTo("102"));
            Assert.That(minimumInvoice!.ExchangedDocument.TypeCode, Is.EqualTo("380"));

            Assert.That(minimumInvoice!.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameter.ID.Value, Is.EqualTo("A1"));
            Assert.That(minimumInvoice!.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameter.ID.Value, Is.EqualTo("urn:factur-x.eu:1p0:minimum"));

            Assert.That(minimumInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.Name, Is.EqualTo("Securibox SARL"));
            Assert.That(minimumInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.Value, Is.EqualTo("50000371000034"));
            Assert.That(minimumInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.SchemeID, Is.EqualTo("0002"));
            Assert.That(minimumInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.Name, Is.EqualTo("Société Hôtelière du Pacano"));
            Assert.That(minimumInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.Value, Is.EqualTo("12345682400016"));
            Assert.That(minimumInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.SchemeID, Is.EqualTo("0002"));
            Assert.That(minimumInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CountryID, Is.EqualTo("FR"));
            Assert.That(minimumInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration is null);

            Assert.That(minimumInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.InvoiceCurrencyCode, Is.EqualTo("EUR"));
            Assert.That(minimumInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxBasisTotalAmount.Value, Is.EqualTo(207.55m));
            Assert.That(minimumInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount.Value, Is.EqualTo(20.59m));
            Assert.That(minimumInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount.CurrencyID, Is.EqualTo("EUR"));
            Assert.That(minimumInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.GrandTotalAmount.Value, Is.EqualTo(228.14m));
            Assert.That(minimumInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.DuePayableAmount.Value, Is.EqualTo(228.14m));
        }
    }
}