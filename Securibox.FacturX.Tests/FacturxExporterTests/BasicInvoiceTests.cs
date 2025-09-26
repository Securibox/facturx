using NUnit.Framework;
using Securibox.FacturX.SpecificationModels.Minimum;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Securibox.FacturX.Tests.FacturxExporterTests
{
    internal class BasicInvoiceTests
    {
        private readonly string _mainDir = Path.Combine(System.IO.Directory.GetCurrentDirectory()?.Split("bin").First()!, "Invoices", "Custom");
        private readonly string _invoiceName = "2023-6013_facture_facturx_basic.pdf";

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

        public static SpecificationModels.Basic.CrossIndustryInvoice GetHotelInvoice_SpecificationModels()
        {
            Securibox.FacturX.SpecificationModels.Basic.CrossIndustryInvoice invoice = new Securibox.FacturX.SpecificationModels.Basic.CrossIndustryInvoice()
            {
                ExchangedDocument = new SpecificationModels.BasicWL.ExchangedDocument()
                {
                    ID = new SpecificationModels.Minimum.ID { Value = "2023-6013" },
                    IssueDateTime = new SpecificationModels.Minimum.IssueDateTime() { DateTimeString = new SpecificationModels.Minimum.DateTimeString() { Value = "20230920", Format = "102" } },
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
                            Content = "2, rue de la Paix – 75000 Paris – Tel: +33 1 01 12 34 56",
                            SubjectCode = "AAI",

                        },
                        new SpecificationModels.BasicWL.Note()
                        {
                            Content = "APE 5510Z – TVA FR40123456824",
                            SubjectCode = "AAI",

                        },
                        new SpecificationModels.BasicWL.Note()
                        {
                            Content = "La loi n°92/1442 du 31 décembre 1992 nous fait l’obligation de vous indiquer que le non-respect des conditions de paiement entraine des intérêts de retard suivant modalités et taux défini par la loi.",
                            SubjectCode = "PMD",

                        },
                        new SpecificationModels.BasicWL.Note()
                        {
                            Content = "Une indemnité forfaitaire de 40€ sera due pour frais de recouvrement en cas de retard de paiement.",
                            SubjectCode = "PMT",

                        },
                     },
                },
                ExchangedDocumentContext = new SpecificationModels.Minimum.ExchangedDocumentContext()
                {
                    BusinessProcessSpecifiedDocumentContextParameter = new SpecificationModels.Minimum.DocumentContextParameter()
                    {
                        ID = new SpecificationModels.Minimum.ID() { Value = "A1" },
                    },
                    GuidelineSpecifiedDocumentContextParameter = new SpecificationModels.Minimum.DocumentContextParameter()
                    {
                        ID = new SpecificationModels.Minimum.ID() { Value = "urn:cen.eu:en16931:2017#compliant#urn:factur-x.eu:1p0:basic" }
                    },
                },
                SupplyChainTradeTransaction = new SpecificationModels.Basic.SupplyChainTradeTransaction()
                {
                    IncludedSupplyChainTradeLineItem = new SpecificationModels.Basic.SupplyChainTradeLineItem[]
                     {
                        new SpecificationModels.Basic.SupplyChainTradeLineItem()
                        {
                            AssociatedDocumentLineDocument = new SpecificationModels.Basic.DocumentLineDocument()
                            {
                                LineID = new SpecificationModels.Minimum.ID { Value = "1" },
                            },
                            SpecifiedTradeProduct = new SpecificationModels.Basic.TradeProduct()
                            {
                                Name = "Chambre du 09/08/2023 au 15/08/2023",
                            },
                            SpecifiedLineTradeAgreement = new SpecificationModels.Basic.LineTradeAgreement()
                            {
                                NetPriceProductTradePrice = new SpecificationModels.Basic.TradePrice()
                                {
                                    ChargeAmount = new SpecificationModels.Minimum.Amount { Value =  205.9000m },
                                    BasisQuantity = new SpecificationModels.Basic.Quantity()
                                    {
                                        UnitCode = "C62",
                                        Value = 6.0000m,
                                    }
                                }
                            },
                            SpecifiedLineTradeSettlement = new SpecificationModels.Basic.LineTradeSettlement()
                            {
                                ApplicableTradeTax = new SpecificationModels.BasicWL.TradeTax()
                                {
                                    TypeCode = "VAT",
                                    CategoryCode = "S",
                                    RateApplicablePercent = 10.00m,
                                },
                                SpecifiedTradeSettlementLineMonetarySummation = new SpecificationModels.Basic.TradeSettlementLineMonetarySummation()
                                {
                                    LineTotalAmount = new SpecificationModels.Minimum.Amount { Value = 1235.40m },
                                }
                            },
                            SpecifiedLineTradeDelivery = new SpecificationModels.Basic.LineTradeDelivery()
                            {
                                BilledQuantity = new SpecificationModels.Basic.Quantity()
                                {
                                        UnitCode = "C62",
                                        Value = 6.0000m,
                                }
                            }

                        },
                        new SpecificationModels.Basic.SupplyChainTradeLineItem()
                        {
                            AssociatedDocumentLineDocument = new SpecificationModels.Basic.DocumentLineDocument()
                            {
                                LineID = new ID { Value = "2" },
                            },
                            SpecifiedTradeProduct = new SpecificationModels.Basic.TradeProduct()
                            {
                                Name = "Forfait taxe de séjour",
                            },
                            SpecifiedLineTradeAgreement = new SpecificationModels.Basic.LineTradeAgreement()
                            {
                                NetPriceProductTradePrice = new SpecificationModels.Basic.TradePrice()
                                {
                                    ChargeAmount = new SpecificationModels.Minimum.Amount { Value = 1.65m },
                                    BasisQuantity = new SpecificationModels.Basic.Quantity()
                                    {
                                        UnitCode = "C62",
                                        Value = 6.0000m,
                                    }
                                }
                            },
                            SpecifiedLineTradeSettlement = new SpecificationModels.Basic.LineTradeSettlement()
                            {
                                ApplicableTradeTax = new SpecificationModels.BasicWL.TradeTax()
                                {
                                    TypeCode = "VAT",
                                    CategoryCode = "Z",
                                    RateApplicablePercent = 0.00m,
                                },
                                SpecifiedTradeSettlementLineMonetarySummation = new SpecificationModels.Basic.TradeSettlementLineMonetarySummation()
                                {
                                    LineTotalAmount = new SpecificationModels.Minimum.Amount { Value = 9.90m },
                                }
                            },
                            SpecifiedLineTradeDelivery = new SpecificationModels.Basic.LineTradeDelivery()
                            {
                                BilledQuantity = new SpecificationModels.Basic.Quantity()
                                {
                                    UnitCode = "C62",
                                    Value = 6.0000m,
                                }
                            }
                        },
                     },
                    ApplicableHeaderTradeAgreement = new SpecificationModels.BasicWL.HeaderTradeAgreement()
                    {
                        SellerTradeParty = new SpecificationModels.BasicWL.TradeParty()
                        {
                            Name = "Société Hôtelière du Pacano",
                            SpecifiedLegalOrganization = new SpecificationModels.BasicWL.LegalOrganization()
                            {
                                ID = new SpecificationModels.Minimum.ID()
                                {
                                    Value = "12345682400016",
                                    SchemeID = "0002"
                                },
                            },
                            PostalTradeAddress = new SpecificationModels.BasicWL.TradeAddress()
                            {
                                CountryID = "FR",
                                PostcodeCode = "75000",
                                LineOne = "2, rue de la Paix",
                                CityName = "PARIS",
                            },
                            URIUniversalCommunication = new SpecificationModels.BasicWL.UniversalCommunication()
                            {
                                URIID = new SpecificationModels.Minimum.ID()
                                {
                                    Value = "info@hotel-du-pacano.fr",
                                    SchemeID = "EM"
                                }
                            },
                            SpecifiedTaxRegistration =
                            new SpecificationModels.Minimum.TaxRegistration()
                            {
                                ID = new SpecificationModels.Minimum.ID()
                                {
                                    Value = "FR40123456824",
                                    SchemeID = "VA"
                                }
                            },
                        },
                        BuyerTradeParty = new SpecificationModels.BasicWL.TradeParty()
                        {
                            Name = "Securibox SARL",
                            SpecifiedLegalOrganization = new SpecificationModels.BasicWL.LegalOrganization()
                            {
                                ID = new SpecificationModels.Minimum.ID() { Value = "50000371000034", SchemeID = "0002" },
                            },
                            SpecifiedTaxRegistration = new SpecificationModels.Minimum.TaxRegistration
                            {
                                ID = new SpecificationModels.Minimum.ID()
                                {
                                    Value = "FR38500003710",
                                    SchemeID = "VA"
                                }
                            },
                            PostalTradeAddress = new SpecificationModels.BasicWL.TradeAddress()
                            {
                                CountryID = "FR",
                                PostcodeCode = "75008",
                                LineOne = "27, Rue de Bassano",
                                CityName = "Paris",
                            },
                        },
                    },
                    ApplicableHeaderTradeDelivery = new SpecificationModels.BasicWL.HeaderTradeDelivery()
                    {
                    },
                    ApplicableHeaderTradeSettlement = new SpecificationModels.BasicWL.HeaderTradeSettlement()
                    {
                        InvoiceCurrencyCode = "EUR",
                        SpecifiedTradeSettlementPaymentMeans = new SpecificationModels.BasicWL.TradeSettlementPaymentMeans()
                        {
                            TypeCode = "30",
                            PayeePartyCreditorFinancialAccount = new SpecificationModels.BasicWL.CreditorFinancialAccount[]
                             {
                                new SpecificationModels.BasicWL.CreditorFinancialAccount()
                                {
                                    IBANID = new SpecificationModels.Minimum.ID { Value = "FR7430003000402964223654P78" },
                                }
                             },
                        },
                        ApplicableTradeTax = new SpecificationModels.BasicWL.TradeTax[]
                         {
                            new SpecificationModels.BasicWL.TradeTax()
                            {
                                CalculatedAmount = new SpecificationModels.Minimum.Amount { Value = 123.54m },
                                TypeCode = "VAT",
                                CategoryCode = "S",
                                BasisAmount = new SpecificationModels.Minimum.Amount { Value = 1235.40m },
                                RateApplicablePercent = 10.00m,
                            },
                            new SpecificationModels.BasicWL.TradeTax()
                            {
                                CalculatedAmount = new SpecificationModels.Minimum.Amount { Value = 0.00m },
                                TypeCode = "VAT",
                                CategoryCode = "Z",
                                BasisAmount = new SpecificationModels.Minimum.Amount { Value = 9.90m },
                                RateApplicablePercent = 0.00m,
                            }
                         },
                        SpecifiedTradePaymentTerms = new SpecificationModels.BasicWL.TradePaymentTerms()
                        {
                            DueDateDateTime = new SpecificationModels.Minimum.IssueDateTime()
                            {
                                DateTimeString = new SpecificationModels.Minimum.DateTimeString()
                                {
                                    Value = "20231019",
                                    Format = "102"
                                }
                            },
                            Description = "30 jours net",
                        },
                        SpecifiedTradeSettlementHeaderMonetarySummation = new SpecificationModels.BasicWL.TradeSettlementHeaderMonetarySummation()
                        {
                            LineTotalAmount = new SpecificationModels.Minimum.Amount() { Value = 1245.30m, },
                            TaxBasisTotalAmount = new SpecificationModels.Minimum.Amount() { Value = 1245.30m },
                            TaxTotalAmount = new SpecificationModels.Minimum.Amount[] { new SpecificationModels.Minimum.Amount() { Value = 123.54m, CurrencyID = "EUR" } },
                            GrandTotalAmount = new SpecificationModels.Minimum.Amount() { Value = 1368.84m },
                            TotalPrepaidAmount = new SpecificationModels.Minimum.Amount() { Value = 452.98m },
                            DuePayableAmount = new SpecificationModels.Minimum.Amount() { Value = 915.86m },
                        },
                    },
                }
            };

            return invoice;

        }

        [Test]
        [Order(1)]
        public async Task WriteData_Basic_SUCCESS()
        {
            var outputPath = Path.Combine(_mainDir, _invoiceName);

            Securibox.FacturX.SpecificationModels.Basic.CrossIndustryInvoice invoice = GetHotelInvoice_SpecificationModels();
            FacturxExporter exporter = new FacturxExporter();

            using (var stream = exporter.CreateFacturXStream(
                Path.Combine(_mainDir, "2023-6013_facture.pdf"),
                invoice,
                $"SEPEM: Invoice "))
            {
                using (var fileStream = new FileStream(outputPath, FileMode.Create))
                {
                    await stream.CopyToAsync(fileStream);
                }
            }
        }

        [Test]
        [Order(2)]
        public void AssertWrittenData_Basic_SUCCESS()
        {
            var invoicePath = Path.Combine(_mainDir, "2023-6013_facture_facturx_basic.pdf");

            var importer = new FacturxImporter(invoicePath);
            var basicInvoice = importer.ImportDataWithDeserialization() as Securibox.FacturX.SpecificationModels.Basic.CrossIndustryInvoice;

            Assert.NotNull(basicInvoice);

            Assert.AreEqual("2023-6013", basicInvoice!.ExchangedDocument.ID.Value);
            Assert.AreEqual("20230920", basicInvoice!.ExchangedDocument.IssueDateTime.DateTimeString.Value);
            Assert.AreEqual("102", basicInvoice!.ExchangedDocument.IssueDateTime.DateTimeString.Format);
            Assert.AreEqual("380", basicInvoice!.ExchangedDocument.TypeCode);

            Assert.AreEqual(6, basicInvoice!.ExchangedDocument.IncludedNote.Count());

            Assert.AreEqual("SASU au capital de 200000€", basicInvoice!.ExchangedDocument.IncludedNote.ElementAt(0).Content);
            Assert.AreEqual("REG", basicInvoice!.ExchangedDocument.IncludedNote.ElementAt(0).SubjectCode);

            Assert.AreEqual("R.C.S Paris 123 456 789", basicInvoice!.ExchangedDocument.IncludedNote.ElementAt(1).Content);
            Assert.AreEqual("ABL", basicInvoice!.ExchangedDocument.IncludedNote.ElementAt(1).SubjectCode);

            Assert.AreEqual("2, rue de la Paix – 75000 Paris – Tel: +33 1 01 12 34 56", basicInvoice!.ExchangedDocument.IncludedNote.ElementAt(2).Content);
            Assert.AreEqual("AAI", basicInvoice!.ExchangedDocument.IncludedNote.ElementAt(2).SubjectCode);

            Assert.AreEqual("APE 5510Z – TVA FR40123456824", basicInvoice!.ExchangedDocument.IncludedNote.ElementAt(3).Content);
            Assert.AreEqual("AAI", basicInvoice!.ExchangedDocument.IncludedNote.ElementAt(3).SubjectCode);

            Assert.AreEqual("La loi n°92/1442 du 31 décembre 1992 nous fait l’obligation de vous indiquer que le non-respect des conditions de paiement entraine des intérêts de retard suivant modalités et taux défini par la loi.", basicInvoice!.ExchangedDocument.IncludedNote.ElementAt(4).Content);
            Assert.AreEqual("PMD", basicInvoice!.ExchangedDocument.IncludedNote.ElementAt(4).SubjectCode);

            Assert.AreEqual("Une indemnité forfaitaire de 40€ sera due pour frais de recouvrement en cas de retard de paiement.", basicInvoice!.ExchangedDocument.IncludedNote.ElementAt(5).Content);
            Assert.AreEqual("PMT", basicInvoice!.ExchangedDocument.IncludedNote.ElementAt(5).SubjectCode);

            Assert.AreEqual("A1", basicInvoice!.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameter.ID.Value);
            Assert.AreEqual("urn:cen.eu:en16931:2017#compliant#urn:factur-x.eu:1p0:basic", basicInvoice!.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameter.ID.Value);

            Assert.AreEqual(2, basicInvoice!.SupplyChainTradeTransaction.IncludedSupplyChainTradeLineItem.Count());

            var line1 = basicInvoice!.SupplyChainTradeTransaction.IncludedSupplyChainTradeLineItem.ElementAt(0);
            Assert.AreEqual("1", line1.AssociatedDocumentLineDocument.LineID.Value);
            Assert.AreEqual("Chambre du 09/08/2023 au 15/08/2023", line1.SpecifiedTradeProduct.Name);
            Assert.AreEqual(205.9000m, line1.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value);
            Assert.AreEqual(6.0000m, line1.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.Value);
            Assert.AreEqual("C62", line1.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.UnitCode);
            Assert.AreEqual("VAT", line1.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode);
            Assert.AreEqual("S", line1.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode);
            Assert.AreEqual(10.00m, line1.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent);
            Assert.AreEqual(1235.40m, line1.SpecifiedLineTradeSettlement.SpecifiedTradeSettlementLineMonetarySummation.LineTotalAmount.Value);

            var line2 = basicInvoice!.SupplyChainTradeTransaction.IncludedSupplyChainTradeLineItem.ElementAt(1);
            Assert.AreEqual("2", line2.AssociatedDocumentLineDocument.LineID.Value);
            Assert.AreEqual("Forfait taxe de séjour", line2.SpecifiedTradeProduct.Name);
            Assert.AreEqual(1.65m, line2.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value);
            Assert.AreEqual(6.0000m, line2.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.Value);
            Assert.AreEqual("C62", line2.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.UnitCode);
            Assert.AreEqual("VAT", line2.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode);
            Assert.AreEqual("Z", line2.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode);
            Assert.AreEqual(0.00m, line2.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent);
            Assert.AreEqual(9.90m, line2.SpecifiedLineTradeSettlement.SpecifiedTradeSettlementLineMonetarySummation.LineTotalAmount.Value);

            Assert.AreEqual("Securibox SARL", basicInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.Name);
            Assert.AreEqual("50000371000034", basicInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", basicInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.SchemeID);
            Assert.AreEqual("FR38500003710", basicInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedTaxRegistration.ID.Value);
            Assert.AreEqual("VA", basicInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedTaxRegistration.ID.SchemeID);
            Assert.AreEqual("FR", basicInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.CountryID);
            Assert.AreEqual("75008", basicInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.PostcodeCode);
            Assert.AreEqual("27, Rue de Bassano", basicInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.LineOne);
            Assert.AreEqual("Paris", basicInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.CityName);

            Assert.AreEqual("Société Hôtelière du Pacano", basicInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.Name);
            Assert.AreEqual("12345682400016", basicInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", basicInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.SchemeID);
            Assert.AreEqual("FR", basicInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CountryID);
            Assert.AreEqual("75000", basicInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.PostcodeCode);
            Assert.AreEqual("2, rue de la Paix", basicInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.LineOne);
            Assert.AreEqual("PARIS", basicInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CityName);
            Assert.AreEqual("info@hotel-du-pacano.fr", basicInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.URIUniversalCommunication.URIID.Value);
            Assert.AreEqual("EM", basicInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.URIUniversalCommunication.URIID.SchemeID);
            Assert.AreEqual("FR40123456824", basicInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.Value);
            Assert.AreEqual("VA", basicInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.SchemeID);

            Assert.AreEqual("EUR", basicInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.InvoiceCurrencyCode);

            Assert.AreEqual("30", basicInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementPaymentMeans.TypeCode);

            Assert.AreEqual(1, basicInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementPaymentMeans.PayeePartyCreditorFinancialAccount.Count());
            Assert.AreEqual("FR7430003000402964223654P78", basicInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementPaymentMeans.PayeePartyCreditorFinancialAccount.ElementAt(0).IBANID.Value);

            Assert.AreEqual(2, basicInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.Count());

            Assert.AreEqual(123.54m, basicInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.ElementAt(0).CalculatedAmount.Value);
            Assert.AreEqual("VAT", basicInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.ElementAt(0).TypeCode);
            Assert.AreEqual(1235.40m, basicInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.ElementAt(0).BasisAmount.Value);
            Assert.AreEqual("S", basicInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.ElementAt(0).CategoryCode);
            Assert.AreEqual(10.00, basicInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.ElementAt(0).RateApplicablePercent);

            Assert.AreEqual(0.00, basicInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.ElementAt(1).CalculatedAmount.Value);
            Assert.AreEqual("VAT", basicInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.ElementAt(1).TypeCode);
            Assert.AreEqual(9.90, basicInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.ElementAt(1).BasisAmount.Value);
            Assert.AreEqual("Z", basicInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.ElementAt(1).CategoryCode);
            Assert.AreEqual(0.00, basicInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.ElementAt(1).RateApplicablePercent);

            Assert.AreEqual("20231019", basicInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradePaymentTerms.DueDateDateTime.DateTimeString.Value);
            Assert.AreEqual("102", basicInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradePaymentTerms.DueDateDateTime.DateTimeString.Format);
            Assert.AreEqual("30 jours net", basicInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradePaymentTerms.Description);

            Assert.AreEqual(1245.30m, basicInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.LineTotalAmount.Value);
            Assert.AreEqual(1245.30m, basicInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxBasisTotalAmount.Value);

            Assert.AreEqual(1, basicInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount.Count());
            Assert.AreEqual("EUR", basicInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount.ElementAt(0).CurrencyID);
            Assert.AreEqual(123.54m, basicInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount.ElementAt(0).Value);

            Assert.AreEqual(1368.84m, basicInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.GrandTotalAmount.Value);
            Assert.AreEqual(915.86m, basicInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.DuePayableAmount.Value);
            Assert.AreEqual(452.98m, basicInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TotalPrepaidAmount.Value);
        }
    }
}