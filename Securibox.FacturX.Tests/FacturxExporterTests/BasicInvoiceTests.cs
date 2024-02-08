using NUnit.Framework;
using Securibox.FacturX.Models.Basic;
using Securibox.FacturX.Models.Minimum;
using Securibox.FacturX.Models.Minimum.Enum;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Securibox.FacturX.Tests.FacturxExporterTests
{
    internal class BasicInvoiceTests
    {
        private readonly string _mainDir = $"{System.IO.Directory.GetCurrentDirectory()?.Split("\\bin")?.ElementAtOrDefault(0)}\\Invoices\\Custom\\";

        [SetUp]
        public void Setup()
        {
        }

        public static Models.Basic.Invoice GetInvoice_Models()
        {
            Securibox.FacturX.Models.Basic.Invoice invoice = new Securibox.FacturX.Models.Basic.Invoice()
            {
                ProccessControl = new ProccessControl()
                {
                    SpecificationIdentification = "urn:cen.eu:en16931:2017#compliant#urn:factur-x.eu:1p0:basic"
                },
                Header = new Models.BasicWL.Header("2023-6013", "380", new System.DateTime(2023, 9, 20)),
                StakeHolders = new Models.BasicWL.StakeHolders()
                {
                    Seller = new Models.BasicWL.Seller()
                    {
                        LegalRegistration = new Models.BasicWL.LegalRegistration()
                        {
                            TradingName = "Société Hôtelière du Pacano",
                            Id = "12345682400016",
                            Scheme = "0002"
                        },
                        PostalAddress = new Models.BasicWL.PostalAddress(
                            new string[] { "2, rue de la Paix" },
                            "75000",
                            "Paris",
                            "FR"
                            ),
                        Name = "Société Hôtelière du Pacano",
                        VatRegistration = new Models.Minimum.TaxRegistration()
                        {
                            Id = "FR40123456824",
                            Scheme = TaxSchemeId.VA
                        }
                    },
                    Buyer = new Models.BasicWL.Buyer()
                    {
                        LegalRegistration = new Models.BasicWL.LegalRegistration()
                        {
                            TradingName = "Securibox SARL",
                            Id = "50000371000034",
                            Scheme = "0002"
                        },
                        PostalAddress = new Models.BasicWL.PostalAddress(
                            new string[] { "27, Rue de Bassano" },
                            "75008",
                            "Paris",
                            "FR"
                        ),
                        Name = "Securibox",
                        VatRegistration = new Models.Minimum.TaxRegistration()
                        {
                            Id = "FR38500003710",
                            Scheme = TaxSchemeId.VA
                        }
                    },
                },
                TaxDistributionList = new Models.BasicWL.TaxDistribution[]
                {
                    new Models.BasicWL.TaxDistribution()
                    {

                        CategoryAmount = (decimal)123.54,
                        CategoryBaseAmount = (decimal)1235.40,
                        CategoryCode = "S",
                        CategoryType = "VAT",
                        CategoryRate = (decimal)10.00,
                        AddedTaxPointDateCode = "5"
                    },
                    new Models.BasicWL.TaxDistribution()
                    {

                        CategoryAmount = (decimal)0,
                        CategoryBaseAmount = (decimal)9.90,
                        CategoryCode = "Z",
                        CategoryType = "VAT",
                        CategoryRate = (decimal)0.00,
                        AddedTaxPointDateCode = "5"
                    }
                },
                Totals = new Models.BasicWL.Totals()
                {

                    NetAmountSum = (decimal)1245.30,
                    PaidAmount = (decimal)452.98,
                    TotalAmountWithoutVat = new Models.Minimum.TotalAmount((decimal)1245.30),
                    TotalAmountWithVat = new Models.Minimum.TotalAmount((decimal)1368.84),
                    TotalVatAmount = new Models.Minimum.TotalAmountAndCurrency((decimal)123.54, "EUR"),
                    AmountToBePaid = (decimal)915.86
                },
                PaymentTerms = new Models.BasicWL.PaymentTerms()
                {
                    Conditions = "Paiement à réception de facture",
                    DueDate = new System.DateTime(2023, 10, 19)
                },
                PaymentInstructions = new Models.BasicWL.PaymentInstructions("30", "FR7430003000402964223654P78"),
                LineList = new InvoiceLine[] {
                    new InvoiceLine()
                    {
                        LineDetails = new LineDetails("1"),
                        ItemDetails = new LineItemDetails("Chambre du 09/08/2023 au 15/08/2023", new Models.BasicWL.GlobalIdentification("3518370400049", "1060")),
                        NetPriceDetails = new LineNetPriceDetails((decimal)205.90, new QuantityUnit(6)),
                        VatDetails = new LineVatDetails("VAT", "S", (decimal)10.00),
                        Totals = new LineTotals((decimal)1235.40),
                    },
                    new InvoiceLine()
                    {
                        LineDetails = new LineDetails("2"),
                        ItemDetails = new LineItemDetails("Forfait taxe de s\u00E9jour", new Models.BasicWL.GlobalIdentification("3518370400050", "1060")),
                        NetPriceDetails = new LineNetPriceDetails((decimal)1.65, new QuantityUnit(6)),
                        VatDetails = new LineVatDetails("VAT", "S", (decimal)0.00),
                        Totals = new LineTotals((decimal)9.90),
                    }
                }

            };
            return invoice;

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
                                LineID = "1",
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
                            SpecifiedLineTradeDelivery = new SpecificationModels.Basic.LineTradeDelivery()
                            {
                                BilledQuantity = new SpecificationModels.Basic.Quantity()
                                {
                                    Value = 1.0000m,
                                    UnitCode = "C62",
                                }
                            },
                            SpecifiedLineTradeSettlement = new SpecificationModels.Basic.LineTradeSettlement()
                            {
                                ApplicableTradeTax = new SpecificationModels.BasicWL.TradeTax()
                                {
                                    TypeCode = "VAT",
                                    CategoryCode = "S",
                                    RateApplicablePercent = 20.00m,
                                },
                                SpecifiedTradeSettlementLineMonetarySummation = new SpecificationModels.Basic.TradeSettlementLineMonetarySummation()
                                {
                                    LineTotalAmount = new SpecificationModels.Minimum.Amount { Value = 60.00m },
                                }
                            }
                        },
                        new SpecificationModels.Basic.SupplyChainTradeLineItem()
                        {
                            AssociatedDocumentLineDocument = new SpecificationModels.Basic.DocumentLineDocument()
                            {
                                LineID = "2",
                            },
                            SpecifiedTradeProduct = new SpecificationModels.Basic.TradeProduct()
                            {
                                Name = "FOURNITURES DIVERSES",
                            },
                            SpecifiedLineTradeAgreement = new SpecificationModels.Basic.LineTradeAgreement()
                            {
                                NetPriceProductTradePrice = new SpecificationModels.Basic.TradePrice()
                                {
                                    ChargeAmount = new SpecificationModels.Minimum.Amount { Value = 10.0000m },
                                }
                            },
                            SpecifiedLineTradeDelivery = new SpecificationModels.Basic.LineTradeDelivery()
                            {
                                BilledQuantity = new SpecificationModels.Basic.Quantity()
                                {
                                    Value = 3.0000m,
                                    UnitCode = "C62",
                                }
                            },
                            SpecifiedLineTradeSettlement = new SpecificationModels.Basic.LineTradeSettlement()
                            {
                                ApplicableTradeTax = new SpecificationModels.BasicWL.TradeTax()
                                {
                                    TypeCode = "VAT",
                                    CategoryCode = "S",
                                    RateApplicablePercent = 20.00m,
                                },
                                SpecifiedTradeSettlementLineMonetarySummation = new SpecificationModels.Basic.TradeSettlementLineMonetarySummation()
                                {
                                    LineTotalAmount = new SpecificationModels.Minimum.Amount { Value = 30.00m },
                                }
                            }
                        },
                        new SpecificationModels.Basic.SupplyChainTradeLineItem()
                        {
                            AssociatedDocumentLineDocument = new SpecificationModels.Basic.DocumentLineDocument()
                            {
                                LineID = "3",
                            },
                            SpecifiedTradeProduct = new SpecificationModels.Basic.TradeProduct()
                            {
                                Name = "APPEL",
                            },
                            SpecifiedLineTradeAgreement = new SpecificationModels.Basic.LineTradeAgreement()
                            {
                                NetPriceProductTradePrice = new SpecificationModels.Basic.TradePrice()
                                {
                                    ChargeAmount = new SpecificationModels.Minimum.Amount { Value  = 5.0000m },
                                }
                            },
                            SpecifiedLineTradeDelivery = new SpecificationModels.Basic.LineTradeDelivery()
                            {
                                BilledQuantity = new SpecificationModels.Basic.Quantity()
                                {
                                    Value = 1.0000m,
                                    UnitCode = "C62",
                                }
                            },
                            SpecifiedLineTradeSettlement = new SpecificationModels.Basic.LineTradeSettlement()
                            {
                                ApplicableTradeTax = new SpecificationModels.BasicWL.TradeTax()
                                {
                                    TypeCode = "VAT",
                                    CategoryCode = "S",
                                    RateApplicablePercent = 20.00m,
                                },
                                SpecifiedTradeSettlementLineMonetarySummation = new SpecificationModels.Basic.TradeSettlementLineMonetarySummation()
                                {
                                    LineTotalAmount = new SpecificationModels.Minimum.Amount { Value = 5.00m },
                                }
                            }
                        },
                    },
                    ApplicableHeaderTradeAgreement = new SpecificationModels.BasicWL.HeaderTradeAgreement()
                    {
                        BuyerReference = "SERVEXEC",
                        SellerTradeParty = new SpecificationModels.BasicWL.TradeParty()
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
                            SpecifiedLegalOrganization = new SpecificationModels.BasicWL.LegalOrganization()
                            {
                                ID = new SpecificationModels.Minimum.ID()
                                {
                                    Value = "123456782",
                                    SchemeID = "0002"
                                },
                                TradingBusinessName = "SELLER TRADE NAME",
                            },
                            PostalTradeAddress = new SpecificationModels.BasicWL.TradeAddress()
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
                            SpecifiedTaxRegistration = new SpecificationModels.Minimum.TaxRegistration()
                            {
                                ID = new SpecificationModels.Minimum.ID()
                                {
                                    Value = "FR11123456782",
                                    SchemeID = "VA"
                                }
                            }
                        },
                        BuyerTradeParty = new SpecificationModels.BasicWL.TradeParty()
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
                            SpecifiedLegalOrganization = new SpecificationModels.BasicWL.LegalOrganization()
                            {
                                ID = new SpecificationModels.Minimum.ID() { Value = "987654321", SchemeID = "0002" },
                            },
                            PostalTradeAddress = new SpecificationModels.BasicWL.TradeAddress()
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
                            SpecifiedTaxRegistration = new SpecificationModels.Minimum.TaxRegistration()
                            {
                                ID = new SpecificationModels.Minimum.ID()
                                {
                                    Value = "FR 05 987 654 321",
                                    SchemeID = "VA"
                                }
                            }
                        },
                        BuyerOrderReferencedDocument = new SpecificationModels.BasicWL.ReferencedDocument()
                        {
                            IssuerAssignedID = new SpecificationModels.Minimum.ID() { Value = "PO201925478" }
                        },
                        ContractReferencedDocument = new SpecificationModels.BasicWL.ReferencedDocument()
                        {
                            IssuerAssignedID = new SpecificationModels.Minimum.ID() { Value = "CT2018120802" },
                        },
                    },
                    ApplicableHeaderTradeDelivery = new SpecificationModels.BasicWL.HeaderTradeDelivery()
                    {
                        ShipToTradeParty = new SpecificationModels.BasicWL.TradeParty()
                        {
                            Name = "DEL Name",
                            PostalTradeAddress = new SpecificationModels.BasicWL.TradeAddress()
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
                        DespatchAdviceReferencedDocument = new SpecificationModels.BasicWL.ReferencedDocument()
                        {
                            IssuerAssignedID = new SpecificationModels.Minimum.ID { Value = "DESPADV002" },
                        },
                    },
                    ApplicableHeaderTradeSettlement = new SpecificationModels.BasicWL.HeaderTradeSettlement()
                    {
                        PaymentReference = "F20180023BUYER",
                        InvoiceCurrencyCode = "EUR",
                        PayeeTradeParty = new SpecificationModels.BasicWL.TradeParty()
                        {
                            Name = "PAYEE NAME",
                            SpecifiedLegalOrganization = new SpecificationModels.BasicWL.LegalOrganization()
                            {
                                ID = new SpecificationModels.Minimum.ID()
                                {
                                    Value = "123456782",
                                    SchemeID = "0002",
                                }
                            }
                        },
                        SpecifiedTradeSettlementPaymentMeans = new SpecificationModels.BasicWL.TradeSettlementPaymentMeans()
                        {
                            TypeCode = "30",
                            PayeePartyCreditorFinancialAccount = new SpecificationModels.BasicWL.CreditorFinancialAccount[]
                            {
                                new SpecificationModels.BasicWL.CreditorFinancialAccount()
                                {
                                    IBANID = new SpecificationModels.Minimum.ID() { Value = "FR76 1254 2547 2569 8542 5874 698" },
                                }
                            },
                        },
                        ApplicableTradeTax = new SpecificationModels.BasicWL.TradeTax[]
                        {
                            new SpecificationModels.BasicWL.TradeTax()
                            {
                                CalculatedAmount = new SpecificationModels.Minimum.Amount { Value = 20.00m },
                                TypeCode = "VAT",
                                BasisAmount = new SpecificationModels.Minimum.Amount { Value = 100.00m },
                                CategoryCode = "S",
                                DueDateTypeCode = "72",
                                RateApplicablePercent = 20.00m,
                            }
                        },
                        SpecifiedTradeAllowanceCharge = new SpecificationModels.BasicWL.TradeAllowanceCharge[]
                        {
                            new SpecificationModels.BasicWL.TradeAllowanceCharge()
                            {
                                ChargeIndicator = new SpecificationModels.BasicWL.IndicatorType()
                                {
                                    Indicator = false,
                                },
                                ActualAmount = new SpecificationModels.Minimum.Amount { Value = 5.00m },
                                Reason = "REMISE COMMERCIALE",
                                CategoryTradeTax = new SpecificationModels.BasicWL.TradeTax()
                                {
                                    TypeCode = "VAT",
                                    CategoryCode = "S",
                                    RateApplicablePercent = 20.00m,
                                }
                            },
                            new SpecificationModels.BasicWL.TradeAllowanceCharge()
                            {
                                ChargeIndicator = new SpecificationModels.BasicWL.IndicatorType()
                                {
                                    Indicator = true,
                                },
                                ActualAmount = new SpecificationModels.Minimum.Amount { Value = 10.00m },
                                Reason = "FRAIS DEPLACEMENT",
                                CategoryTradeTax = new SpecificationModels.BasicWL.TradeTax()
                                {
                                    TypeCode = "VAT",
                                    CategoryCode = "S",
                                    RateApplicablePercent = 20.00m,
                                }
                            },
                        },
                        SpecifiedTradePaymentTerms = new SpecificationModels.BasicWL.TradePaymentTerms()
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
                        SpecifiedTradeSettlementHeaderMonetarySummation = new SpecificationModels.BasicWL.TradeSettlementHeaderMonetarySummation()
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
                                Value = 100.00m,
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
                        ReceivableSpecifiedTradeAccountingAccount = new SpecificationModels.BasicWL.TradeAccountingAccount()
                        {
                            ID = new SpecificationModels.Minimum.ID() { Value = "BUYER ACCOUNT REF" },
                        }
                    },
                }
            };

            return invoice;

        }

        [Test]
        public async Task WriteData_Basic_Models_SUCCESS()
        {
            Securibox.FacturX.Models.Basic.Invoice invoice = GetInvoice_Models();
            FacturxExporter exporter = new FacturxExporter();

            using (var stream = exporter.CreateFacturXStream(
                Path.Combine(_mainDir, "2023-6013_facture.pdf"),
                invoice,
                $"SEPEM: Invoice {invoice.Header.InvoiceNumber}",
                $"Invoice {invoice.Header.InvoiceNumber} dated {invoice.Header.EmissionDate.ToString("yyyy-MM-dd")} issued by {invoice.StakeHolders.Seller.Name}"))
            {
                await stream.CopyToAsync(new FileStream(Path.Combine(_mainDir, "2023-6013_facture_facturx_basic.pdf"), FileMode.CreateNew));
            }
        }
        public static SpecificationModels.Basic.CrossIndustryInvoice GetInvoice_SpecificationModels()
        {
            Securibox.FacturX.SpecificationModels.Basic.CrossIndustryInvoice invoice = new Securibox.FacturX.SpecificationModels.Basic.CrossIndustryInvoice()
            {
                ExchangedDocument = new SpecificationModels.BasicWL.ExchangedDocument()
                {
                    ID = new SpecificationModels.Minimum.ID { Value = "F20220025" },
                    IssueDateTime = new SpecificationModels.Minimum.IssueDateTime() { DateTimeString = new SpecificationModels.Minimum.DateTimeString() { Value = "20220131", Format = "102" } },
                    TypeCode = "380",
                    IncludedNote = new SpecificationModels.BasicWL.Note[]
                    {
                        new SpecificationModels.BasicWL.Note()
                        {
                            Content = "FOURNISSEUR F SARL au capital de 50 000 EUR",
                            SubjectCode = "REG",

                        },
                        new SpecificationModels.BasicWL.Note()
                        {
                            Content = "RCS MAVILLE 123 456 782",
                            SubjectCode = "ABL",

                        },
                        new SpecificationModels.BasicWL.Note()
                        {
                            Content = "35 ma rue a moi, code postal Ville Pays – contact@masociete.fr - www.masociete.fr  – N° TVA : FR32 123 456 789",
                            SubjectCode = "AAI",

                        },
                        new SpecificationModels.BasicWL.Note()
                        {
                            Content = "Tout retard de paiement engendre une pénalité exigible à compter de la date d'échéance, calculée sur la base de trois fois le taux d'intérêt légal.",
                            SubjectCode = "PMD",

                        },
                        new SpecificationModels.BasicWL.Note()
                        {
                            Content = "Indemnité forfaitaire pour frais de recouvrement en cas de retard de paiement : 40 €.",
                            SubjectCode = "PMT",

                        },
                        new SpecificationModels.BasicWL.Note()
                        {
                            Content = "Les réglements reçus avant la date d'échéance ne donneront pas lieu à escompte.",
                            SubjectCode = "AAB",
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
                                LineID = "1",
                            },
                            SpecifiedTradeProduct = new SpecificationModels.Basic.TradeProduct()
                            {
                                GlobalID = new SpecificationModels.Minimum.ID() { Value = "598785412598745", SchemeID = "0088" },
                                Name = "PRESTATION SUPPORT",
                            },
                            SpecifiedLineTradeAgreement = new SpecificationModels.Basic.LineTradeAgreement()
                            {
                                NetPriceProductTradePrice = new SpecificationModels.Basic.TradePrice()
                                {
                                    ChargeAmount = new SpecificationModels.Minimum.Amount { Value = 60.0000m },
                                    BasisQuantity = new SpecificationModels.Basic.Quantity()
                                    {
                                        UnitCode = "C62",
                                        Value = 1.0000m,
                                    }
                                }
                            },
                            SpecifiedLineTradeDelivery = new SpecificationModels.Basic.LineTradeDelivery()
                            {
                                BilledQuantity = new SpecificationModels.Basic.Quantity()
                                {
                                    Value = 1.0000m,
                                    UnitCode = "C62",
                                }
                            },
                            SpecifiedLineTradeSettlement = new SpecificationModels.Basic.LineTradeSettlement()
                            {
                                ApplicableTradeTax = new SpecificationModels.BasicWL.TradeTax()
                                {
                                    TypeCode = "VAT",
                                    CategoryCode = "S",
                                    RateApplicablePercent = 20.00m,
                                },
                                SpecifiedTradeSettlementLineMonetarySummation = new SpecificationModels.Basic.TradeSettlementLineMonetarySummation()
                                {
                                    LineTotalAmount = new SpecificationModels.Minimum.Amount { Value = 60.00m },
                                }
                            }
                        },
                        new SpecificationModels.Basic.SupplyChainTradeLineItem()
                        {
                            AssociatedDocumentLineDocument = new SpecificationModels.Basic.DocumentLineDocument()
                            {
                                LineID = "2",
                            },
                            SpecifiedTradeProduct = new SpecificationModels.Basic.TradeProduct()
                            {
                                Name = "FOURNITURES DIVERSES",
                            },
                            SpecifiedLineTradeAgreement = new SpecificationModels.Basic.LineTradeAgreement()
                            {
                                NetPriceProductTradePrice = new SpecificationModels.Basic.TradePrice()
                                {
                                    ChargeAmount = new SpecificationModels.Minimum.Amount { Value = 10.0000m },
                                }
                            },
                            SpecifiedLineTradeDelivery = new SpecificationModels.Basic.LineTradeDelivery()
                            {
                                BilledQuantity = new SpecificationModels.Basic.Quantity()
                                {
                                    Value = 3.0000m,
                                    UnitCode = "C62",
                                }
                            },
                            SpecifiedLineTradeSettlement = new SpecificationModels.Basic.LineTradeSettlement()
                            {
                                ApplicableTradeTax = new SpecificationModels.BasicWL.TradeTax()
                                {
                                    TypeCode = "VAT",
                                    CategoryCode = "S",
                                    RateApplicablePercent = 20.00m,
                                },
                                SpecifiedTradeSettlementLineMonetarySummation = new SpecificationModels.Basic.TradeSettlementLineMonetarySummation()
                                {
                                    LineTotalAmount = new SpecificationModels.Minimum.Amount { Value = 30.00m },
                                }
                            }
                        },
                        new SpecificationModels.Basic.SupplyChainTradeLineItem()
                        {
                            AssociatedDocumentLineDocument = new SpecificationModels.Basic.DocumentLineDocument()
                            {
                                LineID = "3",
                            },
                            SpecifiedTradeProduct = new SpecificationModels.Basic.TradeProduct()
                            {
                                Name = "APPEL",
                            },
                            SpecifiedLineTradeAgreement = new SpecificationModels.Basic.LineTradeAgreement()
                            {
                                NetPriceProductTradePrice = new SpecificationModels.Basic.TradePrice()
                                {
                                    ChargeAmount = new SpecificationModels.Minimum.Amount { Value  = 5.0000m },
                                }
                            },
                            SpecifiedLineTradeDelivery = new SpecificationModels.Basic.LineTradeDelivery()
                            {
                                BilledQuantity = new SpecificationModels.Basic.Quantity()
                                {
                                    Value = 1.0000m,
                                    UnitCode = "C62",
                                }
                            },
                            SpecifiedLineTradeSettlement = new SpecificationModels.Basic.LineTradeSettlement()
                            {
                                ApplicableTradeTax = new SpecificationModels.BasicWL.TradeTax()
                                {
                                    TypeCode = "VAT",
                                    CategoryCode = "S",
                                    RateApplicablePercent = 20.00m,
                                },
                                SpecifiedTradeSettlementLineMonetarySummation = new SpecificationModels.Basic.TradeSettlementLineMonetarySummation()
                                {
                                    LineTotalAmount = new SpecificationModels.Minimum.Amount { Value = 5.00m },
                                }
                            }
                        },
                    },
                    ApplicableHeaderTradeAgreement = new SpecificationModels.BasicWL.HeaderTradeAgreement()
                    {
                        BuyerReference = "SERVEXEC",
                        SellerTradeParty = new SpecificationModels.BasicWL.TradeParty()
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
                            SpecifiedLegalOrganization = new SpecificationModels.BasicWL.LegalOrganization()
                            {
                                ID = new SpecificationModels.Minimum.ID()
                                {
                                    Value = "123456782",
                                    SchemeID = "0002"
                                },
                                TradingBusinessName = "SELLER TRADE NAME",
                            },
                            PostalTradeAddress = new SpecificationModels.BasicWL.TradeAddress()
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
                            SpecifiedTaxRegistration = new SpecificationModels.Minimum.TaxRegistration()
                            {
                                ID = new SpecificationModels.Minimum.ID()
                                {
                                    Value = "FR11123456782",
                                    SchemeID = "VA"
                                }
                            }
                        },
                        BuyerTradeParty = new SpecificationModels.BasicWL.TradeParty()
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
                            SpecifiedLegalOrganization = new SpecificationModels.BasicWL.LegalOrganization()
                            {
                                ID = new SpecificationModels.Minimum.ID() { Value = "987654321", SchemeID = "0002" },
                            },
                            PostalTradeAddress = new SpecificationModels.BasicWL.TradeAddress()
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
                            SpecifiedTaxRegistration = new SpecificationModels.Minimum.TaxRegistration()
                            {
                                ID = new SpecificationModels.Minimum.ID()
                                {
                                    Value = "FR 05 987 654 321",
                                    SchemeID = "VA"
                                }
                            }
                        },
                        BuyerOrderReferencedDocument = new SpecificationModels.BasicWL.ReferencedDocument()
                        {
                            IssuerAssignedID = new SpecificationModels.Minimum.ID() { Value = "PO201925478" }
                        },
                        ContractReferencedDocument = new SpecificationModels.BasicWL.ReferencedDocument()
                        {
                            IssuerAssignedID = new SpecificationModels.Minimum.ID() { Value = "CT2018120802" },
                        },
                    },
                    ApplicableHeaderTradeDelivery = new SpecificationModels.BasicWL.HeaderTradeDelivery()
                    {
                        ShipToTradeParty = new SpecificationModels.BasicWL.TradeParty()
                        {
                            Name = "DEL Name",
                            PostalTradeAddress = new SpecificationModels.BasicWL.TradeAddress()
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
                        DespatchAdviceReferencedDocument = new SpecificationModels.BasicWL.ReferencedDocument()
                        {
                            IssuerAssignedID = new SpecificationModels.Minimum.ID { Value = "DESPADV002" },
                        },
                    },
                    ApplicableHeaderTradeSettlement = new SpecificationModels.BasicWL.HeaderTradeSettlement()
                    {
                        PaymentReference = "F20180023BUYER",
                        InvoiceCurrencyCode = "EUR",
                        PayeeTradeParty = new SpecificationModels.BasicWL.TradeParty()
                        {
                            Name = "PAYEE NAME",
                            SpecifiedLegalOrganization = new SpecificationModels.BasicWL.LegalOrganization()
                            {
                                ID = new SpecificationModels.Minimum.ID()
                                {
                                    Value = "123456782",
                                    SchemeID = "0002",
                                }
                            }
                        },
                        SpecifiedTradeSettlementPaymentMeans = new SpecificationModels.BasicWL.TradeSettlementPaymentMeans()
                        {
                            TypeCode = "30",
                            PayeePartyCreditorFinancialAccount = new SpecificationModels.BasicWL.CreditorFinancialAccount[]
                            {
                                new SpecificationModels.BasicWL.CreditorFinancialAccount()
                                {
                                    IBANID = new SpecificationModels.Minimum.ID() { Value = "FR76 1254 2547 2569 8542 5874 698" },
                                }
                            },
                        },
                        ApplicableTradeTax = new SpecificationModels.BasicWL.TradeTax[]
                        {
                            new SpecificationModels.BasicWL.TradeTax()
                            {
                                CalculatedAmount = new SpecificationModels.Minimum.Amount { Value = 20.00m },
                                TypeCode = "VAT",
                                BasisAmount = new SpecificationModels.Minimum.Amount { Value = 100.00m },
                                CategoryCode = "S",
                                DueDateTypeCode = "72",
                                RateApplicablePercent = 20.00m,
                            }
                        },
                        SpecifiedTradeAllowanceCharge = new SpecificationModels.BasicWL.TradeAllowanceCharge[]
                        {
                            new SpecificationModels.BasicWL.TradeAllowanceCharge()
                            {
                                ChargeIndicator = new SpecificationModels.BasicWL.IndicatorType()
                                {
                                    Indicator = false,
                                },
                                ActualAmount = new SpecificationModels.Minimum.Amount { Value = 5.00m },
                                Reason = "REMISE COMMERCIALE",
                                CategoryTradeTax = new SpecificationModels.BasicWL.TradeTax()
                                {
                                    TypeCode = "VAT",
                                    CategoryCode = "S",
                                    RateApplicablePercent = 20.00m,
                                }
                            },
                            new SpecificationModels.BasicWL.TradeAllowanceCharge()
                            {
                                ChargeIndicator = new SpecificationModels.BasicWL.IndicatorType()
                                {
                                    Indicator = true,
                                },
                                ActualAmount = new SpecificationModels.Minimum.Amount { Value = 10.00m },
                                Reason = "FRAIS DEPLACEMENT",
                                CategoryTradeTax = new SpecificationModels.BasicWL.TradeTax()
                                {
                                    TypeCode = "VAT",
                                    CategoryCode = "S",
                                    RateApplicablePercent = 20.00m,
                                }
                            },
                        },
                        SpecifiedTradePaymentTerms = new SpecificationModels.BasicWL.TradePaymentTerms()
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
                        SpecifiedTradeSettlementHeaderMonetarySummation = new SpecificationModels.BasicWL.TradeSettlementHeaderMonetarySummation()
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
                                Value = 100.00m,
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
                        ReceivableSpecifiedTradeAccountingAccount = new SpecificationModels.BasicWL.TradeAccountingAccount()
                        {
                            ID = new SpecificationModels.Minimum.ID() { Value = "BUYER ACCOUNT REF" },
                        }
                    },
                }
            };

            return invoice;

        }

        [Test]
        public async Task WriteData_Basic_SUCCESS()
        {
            Securibox.FacturX.SpecificationModels.Basic.CrossIndustryInvoice invoice = GetInvoice_SpecificationModels();
            FacturxExporter exporter = new FacturxExporter();

            using (var stream = exporter.CreateFacturXStream(
                string.Format("{0}\\{1}", _mainDir, "2023-6013 - Jappera.pdf"),
                invoice,
                $"SEPEM: Invoice ",
                $"Invoice "))
            {
                await stream.CopyToAsync(new FileStream(string.Format("{0}\\{1}", _mainDir, "2023-6013 - Basic - FacturX.pdf"), FileMode.OpenOrCreate));
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
            Assert.AreEqual(20.00m, basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.ElementAt(0).CalculatedAmount.Value);
            Assert.AreEqual("VAT", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.ElementAt(0).TypeCode);
            Assert.AreEqual(100.00m, basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.ElementAt(0).BasisAmount.Value);
            Assert.AreEqual("S", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.ElementAt(0).CategoryCode);
            Assert.AreEqual("72", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.ElementAt(0).DueDateTypeCode);
            Assert.AreEqual(20.00m, basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.ElementAt(0).RateApplicablePercent);

            Assert.AreEqual(2, basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeAllowanceCharge.Count());

            Assert.AreEqual(false, basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0).ChargeIndicator.Indicator);
            Assert.AreEqual(5.00m, basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0).ActualAmount.Value);
            Assert.AreEqual("REMISE COMMERCIALE", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0).Reason);
            Assert.AreEqual("VAT", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0).CategoryTradeTax.TypeCode);
            Assert.AreEqual("S", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0).CategoryTradeTax.CategoryCode);
            Assert.AreEqual(20.00m, basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0).CategoryTradeTax.RateApplicablePercent);

            Assert.AreEqual(true, basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1).ChargeIndicator.Indicator);
            Assert.AreEqual(10.00m, basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1).ActualAmount.Value);
            Assert.AreEqual("FRAIS DEPLACEMENT", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1).Reason);
            Assert.AreEqual("VAT", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1).CategoryTradeTax.TypeCode);
            Assert.AreEqual("S", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1).CategoryTradeTax.CategoryCode);
            Assert.AreEqual(20.00m, basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1).CategoryTradeTax.RateApplicablePercent);

            Assert.AreEqual("20220302", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradePaymentTerms.DueDateDateTime.DateTimeString.Value);
            Assert.AreEqual("102", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradePaymentTerms.DueDateDateTime.DateTimeString.Format);

            Assert.AreEqual(95.00m, basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.LineTotalAmount.Value);
            Assert.AreEqual(10.00m, basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.ChargeTotalAmount.Value);
            Assert.AreEqual(5.00m, basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.AllowanceTotalAmount.Value);
            Assert.AreEqual(100.00m, basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxBasisTotalAmount.Value);

            Assert.AreEqual(1, basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount.Count());
            Assert.AreEqual("EUR", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount.ElementAt(0).CurrencyID);
            Assert.AreEqual(20.00m, basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount.ElementAt(0).Value);

            Assert.AreEqual(120.00m, basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.GrandTotalAmount.Value);
            Assert.AreEqual(100.00m, basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.DuePayableAmount.Value);
            Assert.AreEqual(20.00m, basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TotalPrepaidAmount.Value);

            Assert.AreEqual("BUYER ACCOUNT REF", basicInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ReceivableSpecifiedTradeAccountingAccount.ID.Value);
        }
    }
}
