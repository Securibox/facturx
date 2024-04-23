using NUnit.Framework;
using Securibox.FacturX.SpecificationModels.Minimum;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Securibox.FacturX.Tests.FacturxExporterTests
{
    internal class ExtendedInvoiceTests
    {
        private readonly string _mainDir = $"{System.IO.Directory.GetCurrentDirectory()?.Split("\\bin")?.ElementAtOrDefault(0)}\\Invoices\\Custom\\";
        private readonly string _invoiceName = "2023-6013_facture_facturx_extended.pdf";

        [SetUp]
        public void Setup()
        {
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

        public static SpecificationModels.Extended.CrossIndustryInvoice GetInvoice_SpecificationModels()
        {
            Securibox.FacturX.SpecificationModels.Extended.CrossIndustryInvoice invoice = new Securibox.FacturX.SpecificationModels.Extended.CrossIndustryInvoice()
            {
                ExchangedDocument = new SpecificationModels.Extended.ExchangedDocument()
                {
                    ID = new SpecificationModels.Minimum.ID { Value = "2023-6013" },
                    IssueDateTime = new SpecificationModels.Minimum.IssueDateTime() { DateTimeString = new SpecificationModels.Minimum.DateTimeString() { Value = "20230920", Format = "102" } },
                    TypeCode = "380",
                    IncludedNote = new SpecificationModels.Extended.NoteExtended[]
                       {
                        new SpecificationModels.Extended.NoteExtended()
                        {
                            Content = "SASU au capital de 200000€",
                            SubjectCode = "REG",

                        },
                        new SpecificationModels.Extended.NoteExtended()
                        {
                            Content = "R.C.S Paris 123 456 789",
                            SubjectCode = "ABL",

                        },
                        new SpecificationModels.Extended.NoteExtended()
                        {
                            Content = "2, rue de la Paix – 75000 Paris – Tel: +33 1 01 12 34 56",
                            SubjectCode = "AAI",

                        },
                        new SpecificationModels.Extended.NoteExtended()
                        {
                            Content = "APE 5510Z – TVA FR40123456824",
                            SubjectCode = "AAI",

                        },
                        new SpecificationModels.Extended.NoteExtended()
                        {
                            Content = "La loi n°92/1442 du 31 décembre 1992 nous fait l’obligation de vous indiquer que le non-respect des conditions de paiement entraine des intérêts de retard suivant modalités et taux défini par la loi.",
                            SubjectCode = "PMD",

                        },
                        new SpecificationModels.Extended.NoteExtended()
                        {
                            Content = "Une indemnité forfaitaire de 40€ sera due pour frais de recouvrement en cas de retard de paiement.",
                            SubjectCode = "PMT",

                        },
                       },
                },
                ExchangedDocumentContext = new SpecificationModels.Extended.ExchangedDocumentContext()
                {
                    BusinessProcessSpecifiedDocumentContextParameter = new SpecificationModels.Minimum.DocumentContextParameter()
                    {
                        ID = new SpecificationModels.Minimum.ID() { Value = "A1" },
                    },
                    GuidelineSpecifiedDocumentContextParameter = new SpecificationModels.Minimum.DocumentContextParameter()
                    {
                        ID = new SpecificationModels.Minimum.ID() { Value = "urn:cen.eu:en16931:2017#conformant#urn:factur-x.eu:1p0:extended" }
                    },
                },
                SupplyChainTradeTransaction = new SpecificationModels.Extended.SupplyChainTradeTransaction()
                {
                    IncludedSupplyChainTradeLineItem = new SpecificationModels.Extended.SupplyChainTradeLineItem[]
                       {
                        new SpecificationModels.Extended.SupplyChainTradeLineItem()
                        {
                            AssociatedDocumentLineDocument = new SpecificationModels.Extended.DocumentLineDocument()
                            {
                                LineID = new SpecificationModels.Minimum.ID { Value = "1" },
                            },
                            SpecifiedTradeProduct = new SpecificationModels.Extended.TradeProduct()
                            {
                                Name = "Chambre du 09/08/2023 au 15/08/2023",
                            },
                            SpecifiedLineTradeAgreement = new SpecificationModels.Extended.LineTradeAgreement()
                            {
                                NetPriceProductTradePrice = new SpecificationModels.Extended.TradePrice()
                                {
                                    ChargeAmount = new SpecificationModels.Minimum.Amount { Value =  205.9000m },
                                    BasisQuantity = new SpecificationModels.Basic.Quantity()
                                    {
                                        UnitCode = "C62",
                                        Value = 6.0000m,
                                    }
                                }
                            },
                            SpecifiedLineTradeSettlement = new SpecificationModels.Extended.LineTradeSettlement()
                            {
                                ApplicableTradeTax = new SpecificationModels.Extended.TradeTaxExtended()
                                {
                                    TypeCode = "VAT",
                                    CategoryCode = "S",
                                    RateApplicablePercent = 10.00m,
                                },
                                SpecifiedTradeSettlementLineMonetarySummation = new SpecificationModels.Extended.TradeSettlementLineMonetarySummation()
                                {
                                    LineTotalAmount = new SpecificationModels.Minimum.Amount { Value = 1235.40m },
                                }
                            },
                            SpecifiedLineTradeDelivery = new SpecificationModels.Extended.LineTradeDelivery()
                            {
                                BilledQuantity = new SpecificationModels.Basic.Quantity()
                                {
                                        UnitCode = "C62",
                                        Value = 6.0000m,
                                }
                            }

                        },
                        new SpecificationModels.Extended.SupplyChainTradeLineItem()
                        {
                            AssociatedDocumentLineDocument = new SpecificationModels.Extended.DocumentLineDocument()
                            {
                                LineID = new ID { Value = "2" },
                            },
                            SpecifiedTradeProduct = new SpecificationModels.Extended.TradeProduct()
                            {
                                Name = "Forfait taxe de séjour",
                            },
                            SpecifiedLineTradeAgreement = new SpecificationModels.Extended.LineTradeAgreement()
                            {
                                NetPriceProductTradePrice = new SpecificationModels.Extended.TradePrice()
                                {
                                    ChargeAmount = new SpecificationModels.Minimum.Amount { Value = 1.65m },
                                    BasisQuantity = new SpecificationModels.Basic.Quantity()
                                    {
                                        UnitCode = "C62",
                                        Value = 6.0000m,
                                    }
                                }
                            },
                            SpecifiedLineTradeSettlement = new SpecificationModels.Extended.LineTradeSettlement()
                            {
                                ApplicableTradeTax = new SpecificationModels.Extended.TradeTaxExtended()
                                {
                                    TypeCode = "VAT",
                                    CategoryCode = "Z",
                                    RateApplicablePercent = 0.00m,
                                },
                                SpecifiedTradeSettlementLineMonetarySummation = new SpecificationModels.Extended.TradeSettlementLineMonetarySummation()
                                {
                                    LineTotalAmount = new SpecificationModels.Minimum.Amount { Value = 9.90m },
                                }
                            },
                            SpecifiedLineTradeDelivery = new SpecificationModels.Extended.LineTradeDelivery()
                            {
                                BilledQuantity = new SpecificationModels.Basic.Quantity()
                                {
                                    UnitCode = "C62",
                                    Value = 6.0000m,
                                }
                            }
                        },
                       },
                    ApplicableHeaderTradeAgreement = new SpecificationModels.Extended.HeaderTradeAgreement()
                    {
                        SellerTradeParty = new SpecificationModels.Extended.TradeParty()
                        {
                            Name = "Société Hôtelière du Pacano",
                            SpecifiedLegalOrganization = new SpecificationModels.Extended.LegalOrganization()
                            {
                                ID = new SpecificationModels.Minimum.ID()
                                {
                                    Value = "12345682400016",
                                    SchemeID = "0002"
                                },
                            },
                            PostalTradeAddress = new SpecificationModels.Extended.TradeAddressExtended()
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
                            SpecifiedTaxRegistration = new SpecificationModels.Minimum.TaxRegistration[]
                              {
                                new SpecificationModels.Minimum.TaxRegistration()
                                {
                                    ID = new SpecificationModels.Minimum.ID()
                                    {
                                        Value = "FR40123456824",
                                        SchemeID = "VA"
                                    }
                                },
                              }
                        },
                        BuyerTradeParty = new SpecificationModels.Extended.TradeParty()
                        {
                            Name = "Securibox SARL",
                            SpecifiedLegalOrganization = new SpecificationModels.Extended.LegalOrganization()
                            {
                                ID = new SpecificationModels.Minimum.ID() { Value = "50000371000034", SchemeID = "0002" },
                            },
                            SpecifiedTaxRegistration = new SpecificationModels.Minimum.TaxRegistration[]
                              {
                                new SpecificationModels.Minimum.TaxRegistration
                                {
                                    ID = new SpecificationModels.Minimum.ID()
                                    {
                                        Value = "FR38500003710",
                                        SchemeID = "VA"
                                    }
                                },
                              },
                            PostalTradeAddress = new SpecificationModels.Extended.TradeAddressExtended()
                            {
                                CountryID = "FR",
                                PostcodeCode = "75008",
                                LineOne = "27, Rue de Bassano",
                                CityName = "Paris",
                            },
                        },
                    },
                    ApplicableHeaderTradeDelivery = new SpecificationModels.Extended.HeaderTradeDelivery()
                    {
                    },
                    ApplicableHeaderTradeSettlement = new SpecificationModels.Extended.HeaderTradeSettlement()
                    {
                        InvoiceCurrencyCode = "EUR",
                        SpecifiedTradeSettlementPaymentMeans = new SpecificationModels.EN16931.TradeSettlementPaymentMeans()
                        {
                            TypeCode = "30",
                            PayeePartyCreditorFinancialAccount = new SpecificationModels.EN16931.CreditorFinancialAccount[]
                               {
                                new SpecificationModels.EN16931.CreditorFinancialAccount()
                                {
                                    IBANID = new SpecificationModels.Minimum.ID { Value = "FR7430003000402964223654P78" },
                                }
                               },
                        },
                        ApplicableTradeTax = new SpecificationModels.Extended.TradeTaxExtended[]
                           {
                            new SpecificationModels.Extended.TradeTaxExtended()
                            {
                                CalculatedAmount = new SpecificationModels.Minimum.Amount { Value = 123.54m },
                                TypeCode = "VAT",
                                CategoryCode = "S",
                                BasisAmount = new SpecificationModels.Minimum.Amount { Value = 1235.40m },
                                RateApplicablePercent = 10.00m,
                            },
                            new SpecificationModels.Extended.TradeTaxExtended()
                            {
                                CalculatedAmount = new SpecificationModels.Minimum.Amount { Value = 0.00m },
                                TypeCode = "VAT",
                                CategoryCode = "Z",
                                BasisAmount = new SpecificationModels.Minimum.Amount { Value = 9.90m },
                                RateApplicablePercent = 0.00m,
                            }
                           },
                        SpecifiedTradePaymentTerms = new SpecificationModels.Extended.TradePaymentTerms()
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
                        SpecifiedTradeSettlementHeaderMonetarySummation = new SpecificationModels.EN16931.TradeSettlementHeaderMonetarySummation()
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
        public async Task WriteData_Extended_SUCCESS()
        {
            var outputPath = Path.Combine(_mainDir, "2023-6013_facture_facturx_extended.pdf");

            Securibox.FacturX.SpecificationModels.Extended.CrossIndustryInvoice invoice = GetInvoice_SpecificationModels();
            FacturxExporter exporter = new FacturxExporter();

            using (var stream = exporter.CreateFacturXStream(
                Path.Combine(_mainDir, "2023-6013_facture.pdf"),
                invoice,
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
        public void AssertWrittenData_Extended_SUCCESS()
        {
            var invoicePath = Path.Combine(_mainDir, "2023-6013_facture_facturx_extended.pdf");

            var importer = new FacturxImporter(invoicePath);
            var extendedInvoice = importer.ImportDataWithDeserialization() as Securibox.FacturX.SpecificationModels.Extended.CrossIndustryInvoice;

            Assert.NotNull(extendedInvoice);

            Assert.AreEqual("2023-6013", extendedInvoice!.ExchangedDocument.ID.Value);
            Assert.AreEqual("20230920", extendedInvoice!.ExchangedDocument.IssueDateTime.DateTimeString.Value);
            Assert.AreEqual("102", extendedInvoice!.ExchangedDocument.IssueDateTime.DateTimeString.Format);
            Assert.AreEqual("380", extendedInvoice!.ExchangedDocument.TypeCode);

            Assert.AreEqual(6, extendedInvoice!.ExchangedDocument.IncludedNote.Count());

            Assert.AreEqual("SASU au capital de 200000€", extendedInvoice!.ExchangedDocument.IncludedNote.ElementAt(0).Content);
            Assert.AreEqual("REG", extendedInvoice!.ExchangedDocument.IncludedNote.ElementAt(0).SubjectCode);

            Assert.AreEqual("R.C.S Paris 123 456 789", extendedInvoice!.ExchangedDocument.IncludedNote.ElementAt(1).Content);
            Assert.AreEqual("ABL", extendedInvoice!.ExchangedDocument.IncludedNote.ElementAt(1).SubjectCode);

            Assert.AreEqual("2, rue de la Paix – 75000 Paris – Tel: +33 1 01 12 34 56", extendedInvoice!.ExchangedDocument.IncludedNote.ElementAt(2).Content);
            Assert.AreEqual("AAI", extendedInvoice!.ExchangedDocument.IncludedNote.ElementAt(2).SubjectCode);

            Assert.AreEqual("APE 5510Z – TVA FR40123456824", extendedInvoice!.ExchangedDocument.IncludedNote.ElementAt(3).Content);
            Assert.AreEqual("AAI", extendedInvoice!.ExchangedDocument.IncludedNote.ElementAt(3).SubjectCode);

            Assert.AreEqual("La loi n°92/1442 du 31 décembre 1992 nous fait l’obligation de vous indiquer que le non-respect des conditions de paiement entraine des intérêts de retard suivant modalités et taux défini par la loi.", extendedInvoice!.ExchangedDocument.IncludedNote.ElementAt(4).Content);
            Assert.AreEqual("PMD", extendedInvoice!.ExchangedDocument.IncludedNote.ElementAt(4).SubjectCode);

            Assert.AreEqual("Une indemnité forfaitaire de 40€ sera due pour frais de recouvrement en cas de retard de paiement.", extendedInvoice!.ExchangedDocument.IncludedNote.ElementAt(5).Content);
            Assert.AreEqual("PMT", extendedInvoice!.ExchangedDocument.IncludedNote.ElementAt(5).SubjectCode);

            Assert.AreEqual("A1", extendedInvoice!.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameter.ID.Value);
            Assert.AreEqual("urn:cen.eu:en16931:2017#conformant#urn:factur-x.eu:1p0:extended", extendedInvoice!.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameter.ID.Value);

            Assert.AreEqual(2, extendedInvoice!.SupplyChainTradeTransaction.IncludedSupplyChainTradeLineItem.Count());

            var line1 = extendedInvoice!.SupplyChainTradeTransaction.IncludedSupplyChainTradeLineItem.ElementAt(0);
            Assert.AreEqual("1", line1.AssociatedDocumentLineDocument.LineID.Value);
            Assert.AreEqual("Chambre du 09/08/2023 au 15/08/2023", line1.SpecifiedTradeProduct.Name);
            Assert.AreEqual(205.9000m, line1.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value);
            Assert.AreEqual(6.0000m, line1.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.Value);
            Assert.AreEqual("C62", line1.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.UnitCode);
            Assert.AreEqual("VAT", line1.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode);
            Assert.AreEqual("S", line1.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode);
            Assert.AreEqual(10.00m, line1.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent);
            Assert.AreEqual(1235.40m, line1.SpecifiedLineTradeSettlement.SpecifiedTradeSettlementLineMonetarySummation.LineTotalAmount.Value);

            var line2 = extendedInvoice!.SupplyChainTradeTransaction.IncludedSupplyChainTradeLineItem.ElementAt(1);
            Assert.AreEqual("2", line2.AssociatedDocumentLineDocument.LineID.Value);
            Assert.AreEqual("Forfait taxe de séjour", line2.SpecifiedTradeProduct.Name);
            Assert.AreEqual(1.6500m, line2.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value);
            Assert.AreEqual(6.0000m, line2.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.Value);
            Assert.AreEqual("C62", line2.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.UnitCode);
            Assert.AreEqual("VAT", line2.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode);
            Assert.AreEqual("Z", line2.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode);
            Assert.AreEqual(0.00m, line2.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent);
            Assert.AreEqual(9.90m, line2.SpecifiedLineTradeSettlement.SpecifiedTradeSettlementLineMonetarySummation.LineTotalAmount.Value);

            Assert.AreEqual("Securibox SARL", extendedInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.Name);
            Assert.AreEqual("50000371000034", extendedInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", extendedInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.SchemeID);
            Assert.AreEqual("FR38500003710", extendedInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedTaxRegistration.ElementAt(0).ID.Value);
            Assert.AreEqual("VA", extendedInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedTaxRegistration.ElementAt(0).ID.SchemeID);
            Assert.AreEqual("FR", extendedInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.CountryID);
            Assert.AreEqual("75008", extendedInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.PostcodeCode);
            Assert.AreEqual("27, Rue de Bassano", extendedInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.LineOne);
            Assert.AreEqual("Paris", extendedInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.CityName);

            Assert.AreEqual("Société Hôtelière du Pacano", extendedInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.Name);
            Assert.AreEqual("12345682400016", extendedInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", extendedInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.SchemeID);
            Assert.AreEqual("FR", extendedInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CountryID);
            Assert.AreEqual("75000", extendedInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.PostcodeCode);
            Assert.AreEqual("2, rue de la Paix", extendedInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.LineOne);
            Assert.AreEqual("PARIS", extendedInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CityName);
            Assert.AreEqual("info@hotel-du-pacano.fr", extendedInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.URIUniversalCommunication.URIID.Value);
            Assert.AreEqual("EM", extendedInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.URIUniversalCommunication.URIID.SchemeID);
            Assert.AreEqual("FR40123456824", extendedInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ElementAt(0).ID.Value);
            Assert.AreEqual("VA", extendedInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ElementAt(0).ID.SchemeID);

            Assert.AreEqual("EUR", extendedInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.InvoiceCurrencyCode);

            Assert.AreEqual("30", extendedInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementPaymentMeans.TypeCode);

            Assert.AreEqual(1, extendedInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementPaymentMeans.PayeePartyCreditorFinancialAccount.Count());
            Assert.AreEqual("FR7430003000402964223654P78", extendedInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementPaymentMeans.PayeePartyCreditorFinancialAccount.ElementAt(0).IBANID.Value);

            Assert.AreEqual(2, extendedInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.Count());

            Assert.AreEqual(123.54m, extendedInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.ElementAt(0).CalculatedAmount.Value);
            Assert.AreEqual("VAT", extendedInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.ElementAt(0).TypeCode);
            Assert.AreEqual(1235.40m, extendedInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.ElementAt(0).BasisAmount.Value);
            Assert.AreEqual("S", extendedInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.ElementAt(0).CategoryCode);
            Assert.AreEqual(10.00, extendedInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.ElementAt(0).RateApplicablePercent);

            Assert.AreEqual(0.00, extendedInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.ElementAt(1).CalculatedAmount.Value);
            Assert.AreEqual("VAT", extendedInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.ElementAt(1).TypeCode);
            Assert.AreEqual(9.90, extendedInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.ElementAt(1).BasisAmount.Value);
            Assert.AreEqual("Z", extendedInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.ElementAt(1).CategoryCode);
            Assert.AreEqual(0.00, extendedInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.ElementAt(1).RateApplicablePercent);

            Assert.AreEqual("20231019", extendedInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradePaymentTerms.DueDateDateTime.DateTimeString.Value);
            Assert.AreEqual("102", extendedInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradePaymentTerms.DueDateDateTime.DateTimeString.Format);
            Assert.AreEqual("30 jours net", extendedInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradePaymentTerms.Description);

            Assert.AreEqual(1245.30m, extendedInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.LineTotalAmount.Value);
            Assert.AreEqual(1245.30m, extendedInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxBasisTotalAmount.Value);

            Assert.AreEqual(1, extendedInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount.Count());
            Assert.AreEqual("EUR", extendedInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount.ElementAt(0).CurrencyID);
            Assert.AreEqual(123.54m, extendedInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount.ElementAt(0).Value);

            Assert.AreEqual(1368.84m, extendedInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.GrandTotalAmount.Value);
            Assert.AreEqual(915.86m, extendedInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.DuePayableAmount.Value);
            Assert.AreEqual(452.98m, extendedInvoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TotalPrepaidAmount.Value);
        }
    }
}
