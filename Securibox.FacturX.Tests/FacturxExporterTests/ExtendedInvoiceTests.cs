using NUnit.Framework;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Securibox.FacturX.Tests.FacturxExporterTests
{
    internal class ExtendedInvoiceTests
    {
        private readonly string _mainDir = $"{System.IO.Directory.GetCurrentDirectory()?.Split("\\bin")?.ElementAtOrDefault(0)}\\Invoices\\Custom\\";

        [SetUp]
        public void Setup()
        {
        }

        public static SpecificationModels.Extended.CrossIndustryInvoice GetInvoice_SpecificationModels()
        {
            Securibox.FacturX.SpecificationModels.Extended.CrossIndustryInvoice invoice = new Securibox.FacturX.SpecificationModels.Extended.CrossIndustryInvoice()
            {
                ExchangedDocument = new SpecificationModels.Extended.ExchangedDocument()
                {
                    ID = new SpecificationModels.Minimum.ID { Value = "F20220025" },
                    IssueDateTime = new SpecificationModels.Minimum.IssueDateTime() { DateTimeString = new SpecificationModels.Minimum.DateTimeString() { Value = "20220131", Format = "102" } },
                    TypeCode = "380",
                    IncludedNote = new SpecificationModels.Extended.NoteExtended[]
                    {
                        new SpecificationModels.Extended.NoteExtended()
                        {
                            Content = "FOURNISSEUR F SARL au capital de 50 000 EUR",
                            SubjectCode = "REG",

                        },
                        new SpecificationModels.Extended.NoteExtended()
                        {
                            Content = "RCS MAVILLE 123 456 782",
                            SubjectCode = "ABL",

                        },
                        new SpecificationModels.Extended.NoteExtended()
                        {
                            Content = "35 ma rue a moi, code postal Ville Pays – contact@masociete.fr - www.masociete.fr  – N° TVA : FR32 123 456 789",
                            SubjectCode = "AAI",

                        },
                        new SpecificationModels.Extended.NoteExtended()
                        {
                            Content = "Tout retard de paiement engendre une pénalité exigible à compter de la date d'échéance, calculée sur la base de trois fois le taux d'intérêt légal.",
                            SubjectCode = "PMD",

                        },
                        new SpecificationModels.Extended.NoteExtended()
                        {
                            Content = "Indemnité forfaitaire pour frais de recouvrement en cas de retard de paiement : 40 €.",
                            SubjectCode = "PMT",

                        },
                        new SpecificationModels.Extended.NoteExtended()
                        {
                            Content = "Les réglements reçus avant la date d'échéance ne donneront pas lieu à escompte.",
                            SubjectCode = "AAB",
                        },
                    },
                },
                ExchangedDocumentContext = new SpecificationModels.Extended.ExchangedDocumentContext()
                {
                    TestIndicator = new SpecificationModels.BasicWL.IndicatorType() { Indicator = false },
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
                                LineID = new SpecificationModels.Minimum.ID() {  Value = "1" },
                            },
                            SpecifiedTradeProduct = new SpecificationModels.Extended.TradeProduct()
                            {
                                GlobalID = new SpecificationModels.Minimum.ID() { Value = "598785412598745", SchemeID = "0088" },
                                Name = "PRESTATION SUPPORT",
                                Description = "Description",
                            },
                            SpecifiedLineTradeAgreement = new SpecificationModels.Extended.LineTradeAgreement()
                            {
                                BuyerOrderReferencedDocument = new SpecificationModels.EN16931.ReferencedDocumentEN16931()
                                {
                                    LineID = new SpecificationModels.Minimum.ID { Value = "1" },
                                },
                                NetPriceProductTradePrice = new SpecificationModels.Extended.TradePrice()
                                {
                                    ChargeAmount = new SpecificationModels.Minimum.Amount {Value = 60.0000m },
                                    BasisQuantity = new SpecificationModels.Basic.Quantity()
                                    {
                                        UnitCode = "C62",
                                        Value = 1.0000m,
                                    }
                                }
                            },
                            SpecifiedLineTradeDelivery = new SpecificationModels.Extended.LineTradeDelivery()
                            {
                                BilledQuantity = new SpecificationModels.Basic.Quantity()
                                {
                                    Value = 1.0000m,
                                    UnitCode = "C62",
                                }
                            },
                            SpecifiedLineTradeSettlement = new SpecificationModels.Extended.LineTradeSettlement()
                            {
                                ApplicableTradeTax = new SpecificationModels.Extended.TradeTaxExtended()
                                {
                                    TypeCode = "VAT",
                                    CategoryCode = "S",
                                    RateApplicablePercent = 20.00m,
                                },
                                SpecifiedTradeSettlementLineMonetarySummation = new SpecificationModels.Extended.TradeSettlementLineMonetarySummation()
                                {
                                    LineTotalAmount = new SpecificationModels.Minimum.Amount { Value = 60.00m },
                                }
                            }
                        },
                        new SpecificationModels.Extended.SupplyChainTradeLineItem()
                        {
                            AssociatedDocumentLineDocument = new SpecificationModels.Extended.DocumentLineDocument()
                            {
                                LineID = new SpecificationModels.Minimum.ID { Value = "2" },
                            },
                            SpecifiedTradeProduct = new SpecificationModels.Extended.TradeProduct()
                            {
                                Name = "FOURNITURES DIVERSES",
                                Description = "Description",
                            },
                            SpecifiedLineTradeAgreement = new SpecificationModels.Extended.LineTradeAgreement()
                            {
                                BuyerOrderReferencedDocument = new SpecificationModels.EN16931.ReferencedDocumentEN16931()
                                {
                                    LineID = new SpecificationModels.Minimum.ID { Value = "3" },
                                },
                                NetPriceProductTradePrice = new SpecificationModels.Extended.TradePrice()
                                {
                                    ChargeAmount = new SpecificationModels.Minimum.Amount { Value = 10.0000m },
                                }
                            },
                            SpecifiedLineTradeDelivery = new SpecificationModels.Extended.LineTradeDelivery()
                            {
                                BilledQuantity = new SpecificationModels.Basic.Quantity()
                                {
                                    Value = 3.0000m,
                                    UnitCode = "C62",
                                }
                            },
                            SpecifiedLineTradeSettlement = new SpecificationModels.Extended.LineTradeSettlement()
                            {
                                ApplicableTradeTax = new SpecificationModels.Extended.TradeTaxExtended()
                                {
                                    TypeCode = "VAT",
                                    CategoryCode = "S",
                                    RateApplicablePercent = 20.00m,
                                },
                                SpecifiedTradeSettlementLineMonetarySummation = new SpecificationModels.Extended.TradeSettlementLineMonetarySummation()
                                {
                                    LineTotalAmount = new SpecificationModels.Minimum.Amount { Value = 30.00m },
                                }
                            }
                        },
                        new SpecificationModels.Extended.SupplyChainTradeLineItem()
                        {
                            AssociatedDocumentLineDocument = new SpecificationModels.Extended.DocumentLineDocument()
                            {
                                LineID = new SpecificationModels.Minimum.ID() {  Value = "3" },
                            },
                            SpecifiedTradeProduct = new SpecificationModels.Extended.TradeProduct()
                            {
                                Name = "APPEL",
                                Description = "Description"
                            },
                            SpecifiedLineTradeAgreement = new SpecificationModels.Extended.LineTradeAgreement()
                            {
                                BuyerOrderReferencedDocument = new SpecificationModels.EN16931.ReferencedDocumentEN16931()
                                {
                                    LineID = new SpecificationModels.Minimum.ID { Value = "2" },
                                },
                                NetPriceProductTradePrice = new SpecificationModels.Extended.TradePrice()
                                {
                                    ChargeAmount = new SpecificationModels.Minimum.Amount { Value = 5.0000m },
                                }
                            },
                            SpecifiedLineTradeDelivery = new SpecificationModels.Extended.LineTradeDelivery()
                            {
                                BilledQuantity = new SpecificationModels.Basic.Quantity()
                                {
                                    Value = 1.0000m,
                                    UnitCode = "C62",
                                }
                            },
                            SpecifiedLineTradeSettlement = new SpecificationModels.Extended.LineTradeSettlement()
                            {
                                ApplicableTradeTax = new SpecificationModels.Extended.TradeTaxExtended()
                                {
                                    TypeCode = "VAT",
                                    CategoryCode = "S",
                                    RateApplicablePercent = 20.00m,
                                },
                                SpecifiedTradeSettlementLineMonetarySummation = new SpecificationModels.Extended.TradeSettlementLineMonetarySummation()
                                {
                                    LineTotalAmount = new SpecificationModels.Minimum.Amount { Value  = 5.00m },
                                }
                            }
                        },
                    },
                    ApplicableHeaderTradeAgreement = new SpecificationModels.Extended.HeaderTradeAgreement()
                    {
                        BuyerReference = "SERVEXEC",
                        SellerTradeParty = new SpecificationModels.Extended.TradeParty()
                        {
                            GlobalID = new SpecificationModels.Minimum.ID[]
                            {
                                new SpecificationModels.Minimum.ID()
                                {
                                    Value = "587451236587",
                                    SchemeID = "0088"
                                },
                            },
                            Name = "LE FOURNISSEUR",
                            SpecifiedLegalOrganization = new SpecificationModels.Extended.LegalOrganization()
                            {
                                ID = new SpecificationModels.Minimum.ID()
                                {
                                    Value = "123456782",
                                    SchemeID = "0002"
                                },
                                TradingBusinessName = "SELLER TRADE NAME",
                            },
                            PostalTradeAddress = new SpecificationModels.Extended.TradeAddressExtended()
                            {
                                CountryID = "FR",
                                PostcodeCode = "75018",
                                LineOne = "35 rue d'ici",
                                LineTwo = "Seller line 2",
                                CityName = "PARIS",
                            },
                            URIUniversalCommunication = new SpecificationModels.BasicWL.UniversalCommunication()
                            {
                                URIID = new SpecificationModels.Minimum.ID()
                                {
                                    Value = "moi@seller.com",
                                    SchemeID = "EM"
                                }
                            },
                            SpecifiedTaxRegistration = new SpecificationModels.Minimum.TaxRegistration[]
                            {
                                new SpecificationModels.Minimum.TaxRegistration()
                                {
                                    ID = new SpecificationModels.Minimum.ID()
                                    {
                                        Value = "FR11123456782",
                                        SchemeID = "VA"
                                    }
                                }
                            },
                        },
                        BuyerTradeParty = new SpecificationModels.Extended.TradeParty()
                        {
                            GlobalID = new SpecificationModels.Minimum.ID[]
                            {
                                new SpecificationModels.Minimum.ID()
                                {
                                    Value = "3654789851",
                                    SchemeID = "0088"
                                },
                            },
                            Name = "LE CLIENT",
                            SpecifiedLegalOrganization = new SpecificationModels.Extended.LegalOrganization()
                            {
                                ID = new SpecificationModels.Minimum.ID() { Value = "987654321", SchemeID = "0002" },
                            },
                            PostalTradeAddress = new SpecificationModels.Extended.TradeAddressExtended()
                            {
                                CountryID = "FR",
                                PostcodeCode = "06000",
                                LineOne = "58 rue de la mer",
                                LineTwo = "Buyer line 2",
                                CityName = "NICE",
                            },
                            URIUniversalCommunication = new SpecificationModels.BasicWL.UniversalCommunication()
                            {
                                URIID = new SpecificationModels.Minimum.ID()
                                {
                                    Value = "me@buyer.com",
                                    SchemeID = "EM"
                                }
                            },
                            SpecifiedTaxRegistration = new SpecificationModels.Minimum.TaxRegistration[]
                            {
                                new SpecificationModels.Minimum.TaxRegistration()
                                {
                                    ID = new SpecificationModels.Minimum.ID()
                                    {
                                        Value = "FR 05 987 654 321",
                                        SchemeID = "VA"
                                    }
                                }
                            },
                        },
                        BuyerOrderReferencedDocument = new SpecificationModels.EN16931.ReferencedDocumentEN16931()
                        {
                            IssuerAssignedID = new SpecificationModels.Minimum.ID { Value = "PO201925478" },
                        },
                        ContractReferencedDocument = new SpecificationModels.EN16931.ReferencedDocumentEN16931()
                        {
                            IssuerAssignedID = new SpecificationModels.Minimum.ID
                            {
                                Value = "CT2018120802"
                            },
                        }
                    },
                    ApplicableHeaderTradeDelivery = new SpecificationModels.Extended.TradeDelivery() 
                    {
                        ShipToTradeParty = new SpecificationModels.Extended.TradeParty()
                        {
                            Name = "DEL Name",
                            PostalTradeAddress = new SpecificationModels.Extended.TradeAddressExtended()
                            {
                                CountryID = "FR",
                                PostcodeCode = "06000",
                                LineOne = "DEL ADRESSE LIGNE 1",
                                LineTwo = "DEL line 2",
                                CityName = "NICE",
                            },
                        },
                        ActualDeliverySupplyChainEvent = new SpecificationModels.BasicWL.SupplyChainEvent()
                        {
                            OccurrenceDateTime = new SpecificationModels.Minimum.IssueDateTime()
                            {
                                DateTimeString = new SpecificationModels.Minimum.DateTimeString()
                                {
                                    Value = "20220128",
                                    Format = "102",
                                }
                            }
                        },
                        ReceivingAdviceReferencedDocument = new SpecificationModels.EN16931.ReferencedDocumentEN16931()
                        {
                            IssuerAssignedID = new SpecificationModels.Minimum.ID { Value = "RECEIV-ADV002" },
                        },
                        DespatchAdviceReferencedDocument = new SpecificationModels.EN16931.ReferencedDocumentEN16931()
                        {
                            IssuerAssignedID = new SpecificationModels.Minimum.ID { Value = "DESPADV002" },
                        },
                    },    
                    ApplicableHeaderTradeSettlement = new SpecificationModels.Extended.HeaderTradeSettlement() 
                    {
                        PaymentReference = "F20180023BUYER",
                        InvoiceCurrencyCode = "EUR",
                        PayeeTradeParty = new SpecificationModels.Extended.TradeParty()
                        {
                            Name = "PAYEE NAME",
                            SpecifiedLegalOrganization = new SpecificationModels.Extended.LegalOrganization()
                            {
                                ID = new SpecificationModels.Minimum.ID()
                                {
                                    Value = "123456782",
                                    SchemeID = "0002",
                                }
                            }
                        },
                        SpecifiedTradeSettlementPaymentMeans = new SpecificationModels.EN16931.TradeSettlementPaymentMeans()
                        {
                            TypeCode = "30",
                            PayeePartyCreditorFinancialAccount = new SpecificationModels.EN16931.CreditorFinancialAccount[]
                            {
                                new SpecificationModels.EN16931.CreditorFinancialAccount()
                                {
                                    IBANID = new SpecificationModels.Minimum.ID { Value = "FR76 1254 2547 2569 8542 5874 698" },
                                }
                            },
                        },
                        ApplicableTradeTax = new SpecificationModels.Extended.TradeTaxExtended[]
                        {
                            new SpecificationModels.Extended.TradeTaxExtended()
                            {
                                CalculatedAmount = new SpecificationModels.Minimum.Amount
                                { Value = 20.00m },
                                TypeCode = "VAT",
                                BasisAmount = new SpecificationModels.Minimum.Amount
                                { Value = 100.00m } ,
                                CategoryCode = "S",
                                DueDateTypeCode = "72",
                                RateApplicablePercent = 20.00m,
                            }
                        },
                        SpecifiedTradeAllowanceCharge = new SpecificationModels.Extended.TradeAllowanceChargeExtended[]
                        {
                            new SpecificationModels.Extended.TradeAllowanceChargeExtended()
                            {
                                ChargeIndicator = new SpecificationModels.BasicWL.IndicatorType()
                                {
                                    Indicator = false,
                                },
                                ActualAmount = new SpecificationModels.Minimum.Amount
                                { Value = 5.00m },
                                Reason = "REMISE COMMERCIALE",
                                CategoryTradeTax = new SpecificationModels.Extended.TradeTaxExtended()
                                {
                                    TypeCode = "VAT",
                                    CategoryCode = "S",
                                    RateApplicablePercent = 20.00m,
                                }
                            },
                            new SpecificationModels.Extended.TradeAllowanceChargeExtended()
                            {
                                ChargeIndicator = new SpecificationModels.BasicWL.IndicatorType()
                                {
                                    Indicator = true,
                                },
                                ActualAmount = new SpecificationModels.Minimum.Amount
                                { Value = 10.00m },
                                Reason = "FRAIS DEPLACEMENT",
                                CategoryTradeTax = new SpecificationModels.Extended.TradeTaxExtended()
                                {
                                    TypeCode = "VAT",
                                    CategoryCode = "S",
                                    RateApplicablePercent = 20.00m,
                                }
                            },
                        },
                        SpecifiedTradePaymentTerms = new SpecificationModels.Extended.TradePaymentTerms()
                        {
                            DueDateDateTime = new SpecificationModels.Minimum.IssueDateTime()
                            {
                                DateTimeString = new SpecificationModels.Minimum.DateTimeString()
                                {
                                    Value = "20220302",
                                    Format = "102"
                                }
                            }
                        },
                        SpecifiedTradeSettlementHeaderMonetarySummation = new SpecificationModels.EN16931.TradeSettlementHeaderMonetarySummation()
                        {
                            LineTotalAmount = new SpecificationModels.Minimum.Amount()
                            {
                                Value = 95.00m,
                            },
                            ChargeTotalAmount = new SpecificationModels.Minimum.Amount()
                            {
                                Value = 10.00m,
                            },
                            AllowanceTotalAmount = new SpecificationModels.Minimum.Amount()
                            {
                                Value = 5.00m,
                            },
                            TaxBasisTotalAmount = new SpecificationModels.Minimum.Amount()
                            {
                                Value = 100.80m,
                            },
                            TaxTotalAmount = new SpecificationModels.Minimum.Amount[] { new SpecificationModels.Minimum.Amount() { Value = 20.00m, CurrencyID = "EUR" } },
                            GrandTotalAmount = new SpecificationModels.Minimum.Amount()
                            {
                                Value = 120.00m,
                            },
                            TotalPrepaidAmount = new SpecificationModels.Minimum.Amount()
                            {
                                Value = 20.00m,
                            },
                            DuePayableAmount = new SpecificationModels.Minimum.Amount()
                            {
                                Value = 100.00m,
                            },
                        },
                        ReceivableSpecifiedTradeAccountingAccount = new SpecificationModels.Extended.TradeAccountingAccount()
                        {
                            ID = new SpecificationModels.Minimum.ID() { Value = "BUYER ACCOUNT REF" },
                        }
                    },
                }
            };

            return invoice;

        }

        [Test]
        public async Task WriteData_Extended_SUCCESS()
        {
            Securibox.FacturX.SpecificationModels.Extended.CrossIndustryInvoice invoice = GetInvoice_SpecificationModels();
            FacturxExporter exporter = new FacturxExporter();

            using (var stream = exporter.CreateFacturXStream(
                Path.Combine(_mainDir, "2023-6013_facture.pdf"),
                invoice,
                $"SEPEM: Invoice ",
                $"Invoice "))
            {
                await stream.CopyToAsync(new FileStream(Path.Combine(_mainDir, "2023-6013_facture_facturx_extended.pdf"), FileMode.Create));
            }
        }

        [Test]
        public async Task AssertWrittenData_Basic_SUCCESS()
        {
            var invoicePath = string.Format("{0}\\{1}", _mainDir, "2023-6013 - Basic - FacturX.pdf");

            var importer = new FacturxImporter(invoicePath);
            var basicInvoice = importer.ImportDataWithDeserialization() as Securibox.FacturX.SpecificationModels.Basic.CrossIndustryInvoice;

            Assert.NotNull(basicInvoice);

            Assert.AreEqual("F20220025", basicInvoice.ExchangedDocument.ID.Value);
            Assert.AreEqual("20220131", basicInvoice.ExchangedDocument.IssueDateTime.DateTimeString.Value);
            Assert.AreEqual("102", basicInvoice.ExchangedDocument.IssueDateTime.DateTimeString.Format);
            Assert.AreEqual("380", basicInvoice.ExchangedDocument.TypeCode);

            Assert.AreEqual(6, basicInvoice.ExchangedDocument.IncludedNote.Count());

            Assert.AreEqual("FOURNISSEUR F SARL au capital de 50 000 EUR", basicInvoice.ExchangedDocument.IncludedNote.ElementAt(0).Content);
            Assert.AreEqual("REG", basicInvoice.ExchangedDocument.IncludedNote.ElementAt(0).SubjectCode);

            Assert.AreEqual("RCS MAVILLE 123 456 782", basicInvoice.ExchangedDocument.IncludedNote.ElementAt(1).Content);
            Assert.AreEqual("ABL", basicInvoice.ExchangedDocument.IncludedNote.ElementAt(1).SubjectCode);

            Assert.AreEqual("35 ma rue a moi, code postal Ville Pays – contact@masociete.fr - www.masociete.fr  – N° TVA : FR32 123 456 789", basicInvoice.ExchangedDocument.IncludedNote.ElementAt(2).Content);
            Assert.AreEqual("AAI", basicInvoice.ExchangedDocument.IncludedNote.ElementAt(2).SubjectCode);

            Assert.AreEqual("Tout retard de paiement engendre une pénalité exigible à compter de la date d'échéance, calculée sur la base de trois fois le taux d'intérêt légal.", basicInvoice.ExchangedDocument.IncludedNote.ElementAt(3).Content);
            Assert.AreEqual("PMD", basicInvoice.ExchangedDocument.IncludedNote.ElementAt(3).SubjectCode);

            Assert.AreEqual("Indemnité forfaitaire pour frais de recouvrement en cas de retard de paiement : 40 €.", basicInvoice.ExchangedDocument.IncludedNote.ElementAt(4).Content);
            Assert.AreEqual("PMT", basicInvoice.ExchangedDocument.IncludedNote.ElementAt(4).SubjectCode);

            Assert.AreEqual("Les réglements reçus avant la date d'échéance ne donneront pas lieu à escompte.", basicInvoice.ExchangedDocument.IncludedNote.ElementAt(5).Content);
            Assert.AreEqual("AAB", basicInvoice.ExchangedDocument.IncludedNote.ElementAt(5).SubjectCode);

            Assert.AreEqual("A1", basicInvoice.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameter.ID.Value);
            Assert.AreEqual("urn:cen.eu:en16931:2017#compliant#urn:factur-x.eu:1p0:basic", basicInvoice.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameter.ID.Value);

            Assert.AreEqual("SERVEXEC", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerReference);

            Assert.AreEqual("PO201925478", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerOrderReferencedDocument.IssuerAssignedID.Value);
            Assert.AreEqual("CT2018120802", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.ContractReferencedDocument.IssuerAssignedID.Value);

            Assert.AreEqual("LE CLIENT", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.Name);
            Assert.AreEqual("987654321", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.SchemeID);

            Assert.AreEqual("LE FOURNISSEUR", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.Name);
            Assert.AreEqual("587451236587", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.GlobalID.ElementAt(0).Value);
            Assert.AreEqual("0088", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.GlobalID.ElementAt(0).SchemeID);
            Assert.AreEqual("123456782", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.SchemeID);
            Assert.AreEqual("SELLER TRADE NAME", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.TradingBusinessName);
            Assert.AreEqual("FR", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CountryID);
            Assert.AreEqual("75018", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.PostcodeCode);
            Assert.AreEqual("35 rue d'ici", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.LineOne);
            Assert.AreEqual("Seller line 2", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.LineTwo);
            Assert.AreEqual("FR", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CountryID);
            Assert.AreEqual("PARIS", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CityName);
            Assert.AreEqual("moi@seller.com", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.URIUniversalCommunication.URIID.Value);
            Assert.AreEqual("FR11123456782", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.Value);
            Assert.AreEqual("VA", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.SchemeID);

            Assert.AreEqual("DEL Name", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeDelivery.ShipToTradeParty.Name);
            Assert.AreEqual("FR", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.CountryID);
            Assert.AreEqual("06000", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.PostcodeCode);
            Assert.AreEqual("DEL ADRESSE LIGNE 1", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.LineOne);
            Assert.AreEqual("DEL line 2", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.LineTwo);
            Assert.AreEqual("NICE", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.CityName);

            Assert.AreEqual("20220128", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeDelivery.ActualDeliverySupplyChainEvent.OccurrenceDateTime.DateTimeString.Value);
            Assert.AreEqual("102", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeDelivery.ActualDeliverySupplyChainEvent.OccurrenceDateTime.DateTimeString.Format);

            Assert.AreEqual("DESPADV002", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeDelivery.DespatchAdviceReferencedDocument.IssuerAssignedID.Value);

            Assert.AreEqual("F20180023BUYER", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.PaymentReference);
            Assert.AreEqual("EUR", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.InvoiceCurrencyCode);

            Assert.AreEqual("PAYEE NAME", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.PayeeTradeParty.Name);
            Assert.AreEqual("123456782", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.PayeeTradeParty.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.PayeeTradeParty.SpecifiedLegalOrganization.ID.SchemeID);

            Assert.AreEqual("30", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementPaymentMeans.TypeCode);

            Assert.AreEqual(1, basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementPaymentMeans.PayeePartyCreditorFinancialAccount.Count());
            Assert.AreEqual("FR76 1254 2547 2569 8542 5874 698", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementPaymentMeans.PayeePartyCreditorFinancialAccount.ElementAt(0).IBANID.Value);

            Assert.AreEqual(1, basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.Count());
            Assert.AreEqual(20.00, basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.ElementAt(0).CalculatedAmount.Value);
            Assert.AreEqual("VAT", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.ElementAt(0).TypeCode);
            Assert.AreEqual(100.00, basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.ElementAt(0).BasisAmount.Value);
            Assert.AreEqual("S", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.ElementAt(0).CategoryCode);
            Assert.AreEqual("72", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.ElementAt(0).DueDateTypeCode);
            Assert.AreEqual(20.00, basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.ElementAt(0).RateApplicablePercent);

            Assert.AreEqual(2, basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeAllowanceCharge.Count());

            Assert.AreEqual(false, basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0).ChargeIndicator.Indicator);
            Assert.AreEqual(5.00, basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0).ActualAmount.Value);
            Assert.AreEqual("REMISE COMMERCIALE", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0).Reason);
            Assert.AreEqual("VAT", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0).CategoryTradeTax.TypeCode);
            Assert.AreEqual("S", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0).CategoryTradeTax.CategoryCode);
            Assert.AreEqual(20.00, basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0).CategoryTradeTax.RateApplicablePercent);

            Assert.AreEqual(true, basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1).ChargeIndicator.Indicator);
            Assert.AreEqual(10.00, basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1).ActualAmount.Value);
            Assert.AreEqual("FRAIS DEPLACEMENT", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1).Reason);
            Assert.AreEqual("VAT", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1).CategoryTradeTax.TypeCode);
            Assert.AreEqual("S", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1).CategoryTradeTax.CategoryCode);
            Assert.AreEqual(20.00, basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1).CategoryTradeTax.RateApplicablePercent);

            Assert.AreEqual("20220302", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradePaymentTerms.DueDateDateTime.DateTimeString.Value);
            Assert.AreEqual("102", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradePaymentTerms.DueDateDateTime.DateTimeString.Format);

            Assert.AreEqual(95.00, basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.LineTotalAmount.Value);
            Assert.AreEqual(10.00, basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.ChargeTotalAmount.Value);
            Assert.AreEqual(5.00, basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.AllowanceTotalAmount.Value);
            Assert.AreEqual(100.00, basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxBasisTotalAmount.Value);

            Assert.AreEqual(1, basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount.Count());
            Assert.AreEqual("EUR", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount.ElementAt(0).CurrencyID);
            Assert.AreEqual(20.00, basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount.ElementAt(0).Value);

            Assert.AreEqual(120.00, basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.GrandTotalAmount.Value);
            Assert.AreEqual(100.00, basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.DuePayableAmount.Value);
            Assert.AreEqual(20.00, basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TotalPrepaidAmount.Value);

            Assert.AreEqual("BUYER ACCOUNT REF", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ReceivableSpecifiedTradeAccountingAccount.ID.Value);
        }
    }
}
