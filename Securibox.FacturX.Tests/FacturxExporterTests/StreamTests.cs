using System;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using Securibox.FacturX.SpecificationModels.Minimum;

namespace Securibox.FacturX.Tests.FacturxExporterTests
{
    internal class StreamTests
    {
        private readonly string _mainDir = Path.Combine(
            System.IO.Directory.GetCurrentDirectory(),
            "Invoices",
            "Custom"
        );
        private readonly string _invoiceName = "2023-6013_facture_facturx_basic.pdf";

        public static SpecificationModels.Basic.CrossIndustryInvoice GetHotelInvoice_SpecificationModels()
        {
            Securibox.FacturX.SpecificationModels.Basic.CrossIndustryInvoice invoice =
                new Securibox.FacturX.SpecificationModels.Basic.CrossIndustryInvoice()
                {
                    ExchangedDocument = new SpecificationModels.BasicWL.ExchangedDocument()
                    {
                        ID = new SpecificationModels.Minimum.ID { Value = "2023-6013" },
                        IssueDateTime = new SpecificationModels.Minimum.IssueDateTime()
                        {
                            DateTimeString = new SpecificationModels.Minimum.DateTimeString()
                            {
                                Value = "20230920",
                                Format = "102",
                            },
                        },
                        TypeCode = "380",
                        IncludedNote = new SpecificationModels.BasicWL.Note[]
                        {
                            new SpecificationModels.BasicWL.Note()
                            {
                                Content = "SASU au capital de 200000€",
                                SubjectCode = "REG",
                            },
                            new SpecificationModels.BasicWL.Note()
                            {
                                Content = "R.C.S Paris 123 456 789",
                                SubjectCode = "ABL",
                            },
                            new SpecificationModels.BasicWL.Note()
                            {
                                Content =
                                    "2, rue de la Paix – 75000 Paris – Tel: +33 1 01 12 34 56",
                                SubjectCode = "AAI",
                            },
                            new SpecificationModels.BasicWL.Note()
                            {
                                Content = "APE 5510Z – TVA FR40123456824",
                                SubjectCode = "AAI",
                            },
                            new SpecificationModels.BasicWL.Note()
                            {
                                Content =
                                    "La loi n°92/1442 du 31 décembre 1992 nous fait l’obligation de vous indiquer que le non-respect des conditions de paiement entraine des intérêts de retard suivant modalités et taux défini par la loi.",
                                SubjectCode = "PMD",
                            },
                            new SpecificationModels.BasicWL.Note()
                            {
                                Content =
                                    "Une indemnité forfaitaire de 40€ sera due pour frais de recouvrement en cas de retard de paiement.",
                                SubjectCode = "PMT",
                            },
                        },
                    },
                    ExchangedDocumentContext =
                        new SpecificationModels.Minimum.ExchangedDocumentContext()
                        {
                            BusinessProcessSpecifiedDocumentContextParameter =
                                new SpecificationModels.Minimum.DocumentContextParameter()
                                {
                                    ID = new SpecificationModels.Minimum.ID() { Value = "A1" },
                                },
                            GuidelineSpecifiedDocumentContextParameter =
                                new SpecificationModels.Minimum.DocumentContextParameter()
                                {
                                    ID = new SpecificationModels.Minimum.ID()
                                    {
                                        Value =
                                            "urn:cen.eu:en16931:2017#compliant#urn:factur-x.eu:1p0:basic",
                                    },
                                },
                        },
                    SupplyChainTradeTransaction =
                        new SpecificationModels.Basic.SupplyChainTradeTransaction()
                        {
                            IncludedSupplyChainTradeLineItem =
                                new SpecificationModels.Basic.SupplyChainTradeLineItem[]
                                {
                                    new SpecificationModels.Basic.SupplyChainTradeLineItem()
                                    {
                                        AssociatedDocumentLineDocument =
                                            new SpecificationModels.Basic.DocumentLineDocument()
                                            {
                                                LineID = new SpecificationModels.Minimum.ID
                                                {
                                                    Value = "1",
                                                },
                                            },
                                        SpecifiedTradeProduct =
                                            new SpecificationModels.Basic.TradeProduct()
                                            {
                                                Name = "Chambre du 09/08/2023 au 15/08/2023",
                                            },
                                        SpecifiedLineTradeAgreement =
                                            new SpecificationModels.Basic.LineTradeAgreement()
                                            {
                                                NetPriceProductTradePrice =
                                                    new SpecificationModels.Basic.TradePrice()
                                                    {
                                                        ChargeAmount =
                                                            new SpecificationModels.Minimum.Amount
                                                            {
                                                                Value = 205.9000m,
                                                            },
                                                        BasisQuantity =
                                                            new SpecificationModels.Basic.Quantity()
                                                            {
                                                                UnitCode = "C62",
                                                                Value = 6.0000m,
                                                            },
                                                    },
                                            },
                                        SpecifiedLineTradeSettlement =
                                            new SpecificationModels.Basic.LineTradeSettlement()
                                            {
                                                ApplicableTradeTax =
                                                    new SpecificationModels.BasicWL.TradeTax()
                                                    {
                                                        TypeCode = "VAT",
                                                        CategoryCode = "S",
                                                        RateApplicablePercent = 10.00m,
                                                    },
                                                SpecifiedTradeSettlementLineMonetarySummation =
                                                    new SpecificationModels.Basic.TradeSettlementLineMonetarySummation()
                                                    {
                                                        LineTotalAmount =
                                                            new SpecificationModels.Minimum.Amount
                                                            {
                                                                Value = 1235.40m,
                                                            },
                                                    },
                                            },
                                        SpecifiedLineTradeDelivery =
                                            new SpecificationModels.Basic.LineTradeDelivery()
                                            {
                                                BilledQuantity =
                                                    new SpecificationModels.Basic.Quantity()
                                                    {
                                                        UnitCode = "C62",
                                                        Value = 6.0000m,
                                                    },
                                            },
                                    },
                                    new SpecificationModels.Basic.SupplyChainTradeLineItem()
                                    {
                                        AssociatedDocumentLineDocument =
                                            new SpecificationModels.Basic.DocumentLineDocument()
                                            {
                                                LineID = new ID { Value = "2" },
                                            },
                                        SpecifiedTradeProduct =
                                            new SpecificationModels.Basic.TradeProduct()
                                            {
                                                Name = "Forfait taxe de séjour",
                                            },
                                        SpecifiedLineTradeAgreement =
                                            new SpecificationModels.Basic.LineTradeAgreement()
                                            {
                                                NetPriceProductTradePrice =
                                                    new SpecificationModels.Basic.TradePrice()
                                                    {
                                                        ChargeAmount =
                                                            new SpecificationModels.Minimum.Amount
                                                            {
                                                                Value = 1.65m,
                                                            },
                                                        BasisQuantity =
                                                            new SpecificationModels.Basic.Quantity()
                                                            {
                                                                UnitCode = "C62",
                                                                Value = 6.0000m,
                                                            },
                                                    },
                                            },
                                        SpecifiedLineTradeSettlement =
                                            new SpecificationModels.Basic.LineTradeSettlement()
                                            {
                                                ApplicableTradeTax =
                                                    new SpecificationModels.BasicWL.TradeTax()
                                                    {
                                                        TypeCode = "VAT",
                                                        CategoryCode = "Z",
                                                        RateApplicablePercent = 0.00m,
                                                    },
                                                SpecifiedTradeSettlementLineMonetarySummation =
                                                    new SpecificationModels.Basic.TradeSettlementLineMonetarySummation()
                                                    {
                                                        LineTotalAmount =
                                                            new SpecificationModels.Minimum.Amount
                                                            {
                                                                Value = 9.90m,
                                                            },
                                                    },
                                            },
                                        SpecifiedLineTradeDelivery =
                                            new SpecificationModels.Basic.LineTradeDelivery()
                                            {
                                                BilledQuantity =
                                                    new SpecificationModels.Basic.Quantity()
                                                    {
                                                        UnitCode = "C62",
                                                        Value = 6.0000m,
                                                    },
                                            },
                                    },
                                },
                            ApplicableHeaderTradeAgreement =
                                new SpecificationModels.BasicWL.HeaderTradeAgreement()
                                {
                                    SellerTradeParty = new SpecificationModels.BasicWL.TradeParty()
                                    {
                                        Name = "Société Hôtelière du Pacano",
                                        SpecifiedLegalOrganization =
                                            new SpecificationModels.BasicWL.LegalOrganization()
                                            {
                                                ID = new SpecificationModels.Minimum.ID()
                                                {
                                                    Value = "12345682400016",
                                                    SchemeID = "0002",
                                                },
                                            },
                                        PostalTradeAddress =
                                            new SpecificationModels.BasicWL.TradeAddress()
                                            {
                                                CountryID = "FR",
                                                PostcodeCode = "75000",
                                                LineOne = "2, rue de la Paix",
                                                CityName = "PARIS",
                                            },
                                        URIUniversalCommunication =
                                            new SpecificationModels.BasicWL.UniversalCommunication()
                                            {
                                                URIID = new SpecificationModels.Minimum.ID()
                                                {
                                                    Value = "info@hotel-du-pacano.fr",
                                                    SchemeID = "EM",
                                                },
                                            },
                                        SpecifiedTaxRegistration =
                                            new SpecificationModels.Minimum.TaxRegistration()
                                            {
                                                ID = new SpecificationModels.Minimum.ID()
                                                {
                                                    Value = "FR40123456824",
                                                    SchemeID = "VA",
                                                },
                                            },
                                    },
                                    BuyerTradeParty = new SpecificationModels.BasicWL.TradeParty()
                                    {
                                        Name = "Securibox SARL",
                                        SpecifiedLegalOrganization =
                                            new SpecificationModels.BasicWL.LegalOrganization()
                                            {
                                                ID = new SpecificationModels.Minimum.ID()
                                                {
                                                    Value = "50000371000034",
                                                    SchemeID = "0002",
                                                },
                                            },
                                        SpecifiedTaxRegistration =
                                            new SpecificationModels.Minimum.TaxRegistration
                                            {
                                                ID = new SpecificationModels.Minimum.ID()
                                                {
                                                    Value = "FR38500003710",
                                                    SchemeID = "VA",
                                                },
                                            },
                                        PostalTradeAddress =
                                            new SpecificationModels.BasicWL.TradeAddress()
                                            {
                                                CountryID = "FR",
                                                PostcodeCode = "75008",
                                                LineOne = "27, Rue de Bassano",
                                                CityName = "Paris",
                                            },
                                    },
                                },
                            ApplicableHeaderTradeDelivery =
                                new SpecificationModels.BasicWL.HeaderTradeDelivery() { },
                            ApplicableHeaderTradeSettlement =
                                new SpecificationModels.BasicWL.HeaderTradeSettlement()
                                {
                                    InvoiceCurrencyCode = "EUR",
                                    SpecifiedTradeSettlementPaymentMeans =
                                    [
                                        new()
                                        {
                                            TypeCode = "30",
                                            PayeePartyCreditorFinancialAccount = new()
                                            {
                                                IBANID = new SpecificationModels.Minimum.ID
                                                {
                                                    Value = "FR7430003000402964223654P78",
                                                },
                                            },
                                        },
                                    ],
                                    ApplicableTradeTax = new SpecificationModels.BasicWL.TradeTax[]
                                    {
                                        new SpecificationModels.BasicWL.TradeTax()
                                        {
                                            CalculatedAmount =
                                                new SpecificationModels.Minimum.Amount
                                                {
                                                    Value = 123.54m,
                                                },
                                            TypeCode = "VAT",
                                            CategoryCode = "S",
                                            BasisAmount = new SpecificationModels.Minimum.Amount
                                            {
                                                Value = 1235.40m,
                                            },
                                            RateApplicablePercent = 10.00m,
                                        },
                                        new SpecificationModels.BasicWL.TradeTax()
                                        {
                                            CalculatedAmount =
                                                new SpecificationModels.Minimum.Amount
                                                {
                                                    Value = 0.00m,
                                                },
                                            TypeCode = "VAT",
                                            CategoryCode = "Z",
                                            BasisAmount = new SpecificationModels.Minimum.Amount
                                            {
                                                Value = 9.90m,
                                            },
                                            RateApplicablePercent = 0.00m,
                                        },
                                    },
                                    SpecifiedTradePaymentTerms =
                                        new SpecificationModels.BasicWL.TradePaymentTerms()
                                        {
                                            DueDateDateTime =
                                                new SpecificationModels.Minimum.IssueDateTime()
                                                {
                                                    DateTimeString =
                                                        new SpecificationModels.Minimum.DateTimeString()
                                                        {
                                                            Value = "20231019",
                                                            Format = "102",
                                                        },
                                                },
                                            Description = "30 jours net",
                                        },
                                    SpecifiedTradeSettlementHeaderMonetarySummation =
                                        new SpecificationModels.BasicWL.TradeSettlementHeaderMonetarySummation()
                                        {
                                            LineTotalAmount =
                                                new SpecificationModels.Minimum.Amount()
                                                {
                                                    Value = 1245.30m,
                                                },
                                            TaxBasisTotalAmount =
                                                new SpecificationModels.Minimum.Amount()
                                                {
                                                    Value = 1245.30m,
                                                },
                                            TaxTotalAmount =
                                                new SpecificationModels.Minimum.Amount[]
                                                {
                                                    new SpecificationModels.Minimum.Amount()
                                                    {
                                                        Value = 123.54m,
                                                        CurrencyID = "EUR",
                                                    },
                                                },
                                            GrandTotalAmount =
                                                new SpecificationModels.Minimum.Amount()
                                                {
                                                    Value = 1368.84m,
                                                },
                                            TotalPrepaidAmount =
                                                new SpecificationModels.Minimum.Amount()
                                                {
                                                    Value = 452.98m,
                                                },
                                            DuePayableAmount =
                                                new SpecificationModels.Minimum.Amount()
                                                {
                                                    Value = 915.86m,
                                                },
                                        },
                                },
                        },
                };

            return invoice;
        }

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

        [Test]
        [Order(1)]
        public async Task Exporter_File_Input_Should_Be_Disposed()
        {
            // Arrange
            var outputPath = Path.Combine(_mainDir, _invoiceName);
            var fileInputToDelete = Path.GetTempFileName();
            File.Copy(Path.Combine(_mainDir, "2023-6013_facture.pdf"), fileInputToDelete, true);

            // Act
            Securibox.FacturX.SpecificationModels.Basic.CrossIndustryInvoice invoice =
                GetHotelInvoice_SpecificationModels();
            FacturxExporter exporter = new FacturxExporter();

            using (
                var stream = exporter.CreateFacturXStream(
                    fileInputToDelete,
                    invoice,
                    $"SEPEM: Invoice "
                )
            )
            {
                using (var fileStream = new FileStream(outputPath, FileMode.Create))
                {
                    await stream.CopyToAsync(fileStream);
                    fileStream.Close();
                }
            }

            // Assert
            Assert.DoesNotThrow(() => File.Delete(fileInputToDelete));
        }
    }
}
