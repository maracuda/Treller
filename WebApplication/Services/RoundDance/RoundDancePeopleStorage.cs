using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace SKBKontur.Treller.WebApplication.Services.RoundDance
{
    public class RoundDancePeopleStorage : IRoundDancePeopleStorage
    {
        private readonly RoundDancePeople[] startVariant;
        private static readonly string StorFilePattern = Path.Combine(HttpRuntime.AppDomainAppPath, "TrellerCache{0}.json");
        private RoundDancePeople[] peoples;

        public RoundDancePeopleStorage()
        {
            startVariant = new[]
            {
#region peoples left
                //                new RoundDancePeople { Name = "Кун Андрей", Email = "scalder@skbkontur.ru",
//                    WorkPeriods = new []
//                    {
//                        new DirectionPeriod { BeginDate = new DateTime(2015, 07, 27), Direction = Direction.Infrastructure },
//                        new DirectionPeriod { BeginDate = new DateTime(2015, 08, 17), Direction = Direction.Leave },
//                        new DirectionPeriod { BeginDate = new DateTime(2015, 08, 31), Direction = Direction.Infrastructure },
//                        new DirectionPeriod { BeginDate = new DateTime(2015, 09, 07), Direction = Direction.LinksDeliveryAgent },
//                        new DirectionPeriod { BeginDate = new DateTime(2015, 10, 05), Direction = Direction.SpeedyFeatures },
//                        new DirectionPeriod { BeginDate = new DateTime(2015, 12, 15), Direction = Direction.Sickness },
//                        new DirectionPeriod { BeginDate = new DateTime(2015, 12, 17), Direction = Direction.SpeedyFeatures },
//                    }},

//                new RoundDancePeople { Name = "Оксана Запорожец", Email = "oksanchike@skbkontur.ru",
//                    WorkPeriods = new []
//                    {
//                        new DirectionPeriod { BeginDate = new DateTime(2015, 08, 24), Direction = Direction.ProlongationScenario, },
//                        new DirectionPeriod { BeginDate = new DateTime(2015, 10, 09), Direction = Direction.Infrastructure, },
//                        new DirectionPeriod { BeginDate = new DateTime(2015, 11, 05), Direction = Direction.SpeedyFeatures},
//                        new DirectionPeriod { BeginDate = new DateTime(2015, 11, 25), Direction = Direction.Fisics},
//                        new DirectionPeriod { BeginDate = new DateTime(2015, 11, 30), Direction = Direction.Leave},
//                        new DirectionPeriod { BeginDate = new DateTime(2015, 12, 03), Direction = Direction.Fisics},
//                        new DirectionPeriod { BeginDate = new DateTime(2015, 12, 07), Direction = Direction.Infrastructure},
//                        new DirectionPeriod { BeginDate = new DateTime(2015, 12, 10), Direction = Direction.SpeedyFeatures},
//                    }},

//                new RoundDancePeople { Name = "Леша Романовский", Email = "logicman@skbkontur.ru",
//                    WorkPeriods = new []
//                    {
//                        new DirectionPeriod { BeginDate = new DateTime(2015, 04, 06), Direction = Direction.СaServices, },
//                        new DirectionPeriod { BeginDate = new DateTime(2015, 05, 18), Direction = Direction.ProductBilling,  },
//                        new DirectionPeriod { BeginDate = new DateTime(2015, 06, 20), Direction = Direction.Infrastructure,  },
//                        new DirectionPeriod { BeginDate = new DateTime(2015, 07, 06), Direction = Direction.Support, },
//                        new DirectionPeriod { BeginDate = new DateTime(2015, 08, 20), Direction = Direction.Vendors, },
//                        new DirectionPeriod { BeginDate = new DateTime(2015, 09, 08), Direction = Direction.RomingDiadoc, },
//                        new DirectionPeriod { BeginDate = new DateTime(2015, 09, 21), Direction = Direction.CaMigration, },
//                        new DirectionPeriod { BeginDate = new DateTime(2015, 10, 05), Direction = Direction.SpeedyFeatures, },
//                        new DirectionPeriod { BeginDate = new DateTime(2015, 10, 12), Direction = Direction.RomingDiadoc, },
//                        new DirectionPeriod { BeginDate = new DateTime(2015, 11, 19), Direction = Direction.Leave, },
//                        new DirectionPeriod { BeginDate = new DateTime(2015, 11, 24), Direction = Direction.RomingDiadoc, },
//                        new DirectionPeriod { BeginDate = new DateTime(2015, 12, 08), Direction = Direction.SpeedyFeatures, },
//                        new DirectionPeriod { BeginDate = new DateTime(2015, 12, 14), Direction = Direction.Duty },
//                        new DirectionPeriod { BeginDate = new DateTime(2015, 12, 21), Direction = Direction.SpeedyFeatures },
//                        new DirectionPeriod { BeginDate = new DateTime(2016, 01, 13), Direction = Direction.Infrastructure },
//                        new DirectionPeriod { BeginDate = new DateTime(2016, 01, 18), Direction = Direction.SpeedyFeatures },
//                        new DirectionPeriod { BeginDate = new DateTime(2016, 01, 25), Direction = Direction.Infrastructure, },
//                        new DirectionPeriod { BeginDate = new DateTime(2016, 02, 01), Direction = Direction.SpeedyFeatures, },
//                        new DirectionPeriod { BeginDate = new DateTime(2016, 02, 15), Direction = Direction.Leave },
//                    }},
//                new RoundDancePeople { Name = "Сережа Рожин", Email = "s.rozhin@skbkontur.ru",
//                    WorkPeriods = new []
//                    {
//                        new DirectionPeriod { BeginDate = new DateTime(2015, 04, 06), Direction = Direction.СaServices,  },
//                        new DirectionPeriod { BeginDate = new DateTime(2015, 07, 13), Direction = Direction.Support, },
//                        new DirectionPeriod { BeginDate = new DateTime(2015, 08, 12), Direction = Direction.Leave, },
//                        new DirectionPeriod { BeginDate = new DateTime(2015, 08, 31), Direction = Direction.Duty, },
//                        new DirectionPeriod { BeginDate = new DateTime(2015, 09, 07), Direction = Direction.SpeedyFeatures, },
//                        new DirectionPeriod { BeginDate = new DateTime(2015, 10, 26), Direction = Direction.Duty, },
//                        new DirectionPeriod { BeginDate = new DateTime(2015, 11, 03), Direction = Direction.SpeedyFeatures},
//                        new DirectionPeriod { BeginDate = new DateTime(2015, 11, 16), Direction = Direction.Infrastructure},
//                        new DirectionPeriod { BeginDate = new DateTime(2015, 11, 30), Direction = Direction.Certificates},
//                        new DirectionPeriod { BeginDate = new DateTime(2016, 02, 08), Direction = Direction.Leave},
//                    }},
                // Direction = Support
                // Direction = ProductBilling
#endregion
                new RoundDancePeople { Name = "Катя Зеленина", Email = "fea@skbkontur.ru",
                    WorkPeriods = new []
                    {
                        new DirectionPeriod { BeginDate = new DateTime(2015, 04, 06), Direction = Direction.ProductBilling },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 06, 30), Direction = Direction.Crm },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 08, 25), Direction = Direction.SpeedyFeatures },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 09, 11), Direction = Direction.Leave },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 09, 28), Direction = Direction.SpeedyFeatures },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 10, 29), Direction = Direction.Fisics },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 12, 14), Direction = Direction.Duty },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 12, 21), Direction = Direction.SpeedyFeatures },
                        new DirectionPeriod { BeginDate = new DateTime(2016, 01, 19), Direction = Direction.Infrastructure },
                        new DirectionPeriod { BeginDate = new DateTime(2016, 02, 08), Direction = Direction.Duty },
                        new DirectionPeriod { BeginDate = new DateTime(2016, 02, 15), Direction = Direction.SpeedyFeatures, PairName = "Мурашов"},
                        new DirectionPeriod { BeginDate = new DateTime(2016, 03, 09), Direction = Direction.Infrastructure},
                        new DirectionPeriod { BeginDate = new DateTime(2016, 03, 21), Direction = Direction.ObConnection},
                    }},
                new RoundDancePeople { Name = "Павел Александров", Email = "paul@skbkontur.ru",
                    WorkPeriods = new []
                    {
                        new DirectionPeriod { BeginDate = new DateTime(2015, 04, 06), Direction = Direction.ProductBilling},
                        new DirectionPeriod { BeginDate = new DateTime(2015, 05, 25), Direction = Direction.ProductBilling },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 07, 12), Direction = Direction.Crm, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 07, 20), Direction = Direction.Support, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 07, 27), Direction = Direction.Infrastructure, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 08, 10), Direction = Direction.Duty, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 08, 17), Direction = Direction.Infrastructure, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 08, 24), Direction = Direction.SpeedyFeatures, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 09, 14), Direction = Direction.SpeedyFeatures, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 09, 21), Direction = Direction.Duty, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 09, 28), Direction = Direction.SpeedyFeatures, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 10, 12), Direction = Direction.Infrastructure, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 10, 19), Direction = Direction.RomingDiadoc, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 12, 07), Direction = Direction.Duty, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 12, 14), Direction = Direction.SpeedyFeatures, },
                        new DirectionPeriod { BeginDate = new DateTime(2016, 01, 25), Direction = Direction.Duty, },
                        new DirectionPeriod { BeginDate = new DateTime(2016, 02, 01), Direction = Direction.SpeedyFeatures, PairName = "Бурлаков" },
                        new DirectionPeriod { BeginDate = new DateTime(2016, 03, 21), Direction = Direction.Infrastructure, PairName = "Димов" },
                    }},
                new RoundDancePeople { Name = "Лев Димов", Email = "dimov@skbkontur.ru",
                    WorkPeriods = new []
                    {
                        new DirectionPeriod { BeginDate = new DateTime(2016, 03, 21), Direction = Direction.Infrastructure },
                    }},
                new RoundDancePeople { Name = "Никита Бурлаков", Email = "burlakov.nick@skbkontur.ru",
                    WorkPeriods = new []
                    {
                        new DirectionPeriod { BeginDate = new DateTime(2015, 04, 04), Direction = Direction.ProductBilling,   },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 04, 26), Direction = Direction.Support,  },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 05, 04), Direction = Direction.ProductBilling,  },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 05, 11), Direction = Direction.Support,  },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 05, 18), Direction = Direction.ProductBilling, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 08, 10), Direction = Direction.Infrastructure, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 08, 17), Direction = Direction.Duty, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 08, 24), Direction = Direction.ProlongationScenario, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 09, 14), Direction = Direction.Leave, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 09, 28), Direction = Direction.Duty, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 10, 05), Direction = Direction.SpeedyFeatures, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 10, 12), Direction = Direction.Infrastructure, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 11, 02), Direction = Direction.Duty, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 11, 10), Direction = Direction.SpeedyFeatures, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 11, 23), Direction = Direction.Fisics, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 12, 10), Direction = Direction.Certificates, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 12, 18), Direction = Direction.Leave, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 12, 21), Direction = Direction.Certificates, },
                        new DirectionPeriod { BeginDate = new DateTime(2016, 01, 12), Direction = Direction.SpeedyFeatures, },
                        new DirectionPeriod { BeginDate = new DateTime(2016, 01, 13), Direction = Direction.Infrastructure, },
                        new DirectionPeriod { BeginDate = new DateTime(2016, 01, 25), Direction = Direction.Duty, },
                        new DirectionPeriod { BeginDate = new DateTime(2016, 02, 08), Direction = Direction.Leave, },
                        new DirectionPeriod { BeginDate = new DateTime(2016, 02, 15), Direction = Direction.SpeedyFeatures, },
                        new DirectionPeriod { BeginDate = new DateTime(2016, 03, 21), Direction = Direction.Duty, },
                    }},
                new RoundDancePeople { Name = "Степан Мурашов", Email = "murashov_sv@skbkontur.ru",
                    WorkPeriods = new []
                    {
                        new DirectionPeriod { BeginDate = new DateTime(2015, 10, 20), Direction = Direction.Infrastructure,},
                        new DirectionPeriod { BeginDate = new DateTime(2015, 11, 06), Direction = Direction.SpeedyFeatures,},
                        new DirectionPeriod { BeginDate = new DateTime(2015, 11, 30), Direction = Direction.Duty,},
                        new DirectionPeriod { BeginDate = new DateTime(2015, 12, 14), Direction = Direction.SpeedyFeatures,},
                        new DirectionPeriod { BeginDate = new DateTime(2016, 01, 19), Direction = Direction.Infrastructure },
                        new DirectionPeriod { BeginDate = new DateTime(2016, 02, 08), Direction = Direction.Duty },
                        new DirectionPeriod { BeginDate = new DateTime(2016, 02, 15), Direction = Direction.SpeedyFeatures },
                        new DirectionPeriod { BeginDate = new DateTime(2016, 03, 21), Direction = Direction.ModifierBuy},
                    }},
                new RoundDancePeople { Name = "Игорь Мамай", Email = "i.mamay@skbkontur.ru",
                    WorkPeriods = new []
                    {
                        new DirectionPeriod { BeginDate = new DateTime(2015, 08, 10), Direction = Direction.Infrastructure, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 08, 17), Direction = Direction.Duty, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 08, 24), Direction = Direction.SpeedyFeatures, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 08, 31), Direction = Direction.LinksDeliveryAgent, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 09, 28), Direction = Direction.Duty, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 10, 12), Direction = Direction.SpeedyFeatures, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 10, 29), Direction = Direction.Fisics, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 11, 24), Direction = Direction.Leave, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 12, 08), Direction = Direction.SpeedyFeatures, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 12, 14), Direction = Direction.Sickness, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 12, 21), Direction = Direction.Duty, },
                        new DirectionPeriod { BeginDate = new DateTime(2016, 01, 11), Direction = Direction.SpeedyFeatures, },
                        new DirectionPeriod { BeginDate = new DateTime(2016, 01, 12), Direction = Direction.Infrastructure, },
                        new DirectionPeriod { BeginDate = new DateTime(2016, 01, 14), Direction = Direction.SpeedyFeatures, PairName = "Иванов" },
                        new DirectionPeriod { BeginDate = new DateTime(2016, 02, 10), Direction = Direction.BillDetalization, PairName = "Иванов" },
                    }},
                new RoundDancePeople { Name = "Леша Иванов", Email = "a.ivanov@skbkontur.ru",
                    WorkPeriods = new []
                    {
                        new DirectionPeriod { BeginDate = new DateTime(2015, 04, 06), Direction = Direction.ProductBilling, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 05, 25), Direction = Direction.ProductBilling, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 06, 02), Direction = Direction.Support,  },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 07, 06), Direction = Direction.ProductBilling, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 08, 24), Direction = Direction.Duty,  },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 09, 02), Direction = Direction.Infrastructure, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 09, 16), Direction = Direction.SpeedyFeatures, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 10, 05), Direction = Direction.Duty, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 10, 19), Direction = Direction.SpeedyFeatures},
                        new DirectionPeriod { BeginDate = new DateTime(2015, 11, 17), Direction = Direction.Duty},
                        new DirectionPeriod { BeginDate = new DateTime(2015, 11, 23), Direction = Direction.SpeedyFeatures},
                        new DirectionPeriod { BeginDate = new DateTime(2015, 12, 21), Direction = Direction.Leave},
                        new DirectionPeriod { BeginDate = new DateTime(2016, 01, 11), Direction = Direction.Duty},
                        new DirectionPeriod { BeginDate = new DateTime(2016, 01, 18), Direction = Direction.SpeedyFeatures},
                        new DirectionPeriod { BeginDate = new DateTime(2016, 02, 10), Direction = Direction.BillDetalization },
                    }},
                new RoundDancePeople { Name = "Женя Клюкин", Email = "johneg@skbkontur.ru",
                    WorkPeriods = new []
                    {
                        new DirectionPeriod { BeginDate = new DateTime(2015, 04, 06), Direction = Direction.Support, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 06, 02), Direction = Direction.Crm,  },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 06, 10), Direction = Direction.Support,  },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 07, 13), Direction = Direction.СaServices, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 08, 17), Direction = Direction.Leave, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 09, 07), Direction = Direction.Duty, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 09, 14), Direction = Direction.SpeedyFeatures},
                        new DirectionPeriod { BeginDate = new DateTime(2015, 11, 10), Direction = Direction.Duty},
                        new DirectionPeriod { BeginDate = new DateTime(2015, 11, 18), Direction = Direction.SpeedyFeatures},
                        new DirectionPeriod { BeginDate = new DateTime(2015, 11, 30), Direction = Direction.Certificates},
                        new DirectionPeriod { BeginDate = new DateTime(2015, 12, 28), Direction = Direction.Leave},
                        new DirectionPeriod { BeginDate = new DateTime(2016, 01, 12), Direction = Direction.Duty},
                        new DirectionPeriod { BeginDate = new DateTime(2016, 01, 18), Direction = Direction.SpeedyFeatures},
                        new DirectionPeriod { BeginDate = new DateTime(2016, 01, 25), Direction = Direction.Infrastructure, PairName = "Шалин" },
                        new DirectionPeriod { BeginDate = new DateTime(2016, 02, 15), Direction = Direction.Duty },
                        new DirectionPeriod { BeginDate = new DateTime(2016, 02, 24), Direction = Direction.Infrastructure },
                        new DirectionPeriod { BeginDate = new DateTime(2016, 03, 09), Direction = Direction.SpeedyFeatures},
                        new DirectionPeriod { BeginDate = new DateTime(2016, 03, 21), Direction = Direction.Duty},
                        new DirectionPeriod { BeginDate = new DateTime(2016, 03, 23), Direction = Direction.SpeedyFeatures},
                    }},
                new RoundDancePeople { Name = "Антон Ежов", Email = "anton.ezhov@skbkontur.ru",
                    WorkPeriods = new []
                    {
                        new DirectionPeriod { BeginDate = new DateTime(2015, 04, 06), Direction = Direction.Support,  },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 07, 06), Direction = Direction.СaServices, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 08, 16), Direction = Direction.CaTariffsAndDiscounts, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 09, 01), Direction = Direction.Leave, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 09, 18), Direction = Direction.SpeedyFeatures, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 10, 28), Direction = Direction.Fop},
                        new DirectionPeriod { BeginDate = new DateTime(2015, 11, 10), Direction = Direction.Duty},
                        new DirectionPeriod { BeginDate = new DateTime(2015, 11, 23), Direction = Direction.SpeedyFeatures},
                        new DirectionPeriod { BeginDate = new DateTime(2016, 01, 18), Direction = Direction.Duty},
                        new DirectionPeriod { BeginDate = new DateTime(2016, 01, 25), Direction = Direction.SpeedyFeatures, },
                        new DirectionPeriod { BeginDate = new DateTime(2016, 02, 08), Direction = Direction.Infrastructure, PairName = "Чичерский" },
                        new DirectionPeriod { BeginDate = new DateTime(2016, 02, 24), Direction = Direction.Duty },
                        new DirectionPeriod { BeginDate = new DateTime(2016, 03, 09), Direction = Direction.SpeedyFeatures, PairName = "Клюкин"},
                        new DirectionPeriod { BeginDate = new DateTime(2016, 03, 21), Direction = Direction.Leave},
                        new DirectionPeriod { BeginDate = new DateTime(2016, 03, 28), Direction = Direction.SpeedyFeatures},
                    }},
                new RoundDancePeople { Name = "Андрей Шалин", Email = "shalin@skbkontur.ru",
                    WorkPeriods = new []
                    {
                        new DirectionPeriod { BeginDate = new DateTime(2015, 04, 06), Direction = Direction.ProductBilling, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 04, 27), Direction = Direction.Support, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 06, 02), Direction = Direction.Crm,  },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 06, 20), Direction = Direction.Infrastructure,  },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 07, 01), Direction = Direction.Support, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 07, 20), Direction = Direction.Leave, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 08, 03), Direction = Direction.Duty, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 08, 10), Direction = Direction.Support, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 08, 20), Direction = Direction.Vendors, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 09, 08), Direction = Direction.RomingDiadoc, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 09, 21), Direction = Direction.CaMigration, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 10, 05), Direction = Direction.Infrastructure, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 10, 19), Direction = Direction.SpeedyFeatures, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 10, 26), Direction = Direction.Duty,  },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 11, 03), Direction = Direction.SpeedyFeatures},
                        new DirectionPeriod { BeginDate = new DateTime(2015, 11, 23), Direction = Direction.Duty},
                        new DirectionPeriod { BeginDate = new DateTime(2015, 11, 30), Direction = Direction.SpeedyFeatures},
                        new DirectionPeriod { BeginDate = new DateTime(2016, 01, 18), Direction = Direction.Duty},
                        new DirectionPeriod { BeginDate = new DateTime(2016, 01, 25), Direction = Direction.SpeedyFeatures },
                        new DirectionPeriod { BeginDate = new DateTime(2016, 02, 01), Direction = Direction.Infrastructure, },
                        new DirectionPeriod { BeginDate = new DateTime(2016, 02, 16), Direction = Direction.SpeedyFeatures, },
                        new DirectionPeriod { BeginDate = new DateTime(2016, 03, 09), Direction = Direction.Duty, },
                        new DirectionPeriod { BeginDate = new DateTime(2016, 03, 21), Direction = Direction.ObConnection},
                    }},
                // Direction = CaServices
                new RoundDancePeople { Name = "Саша Чичерский", Email = "chicherskiy@skbkontur.ru",
                    WorkPeriods = new []
                    {
                        new DirectionPeriod { BeginDate = new DateTime(2015, 04, 06), Direction = Direction.СaServices, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 07, 13), Direction = Direction.Leave, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 07, 27), Direction = Direction.Duty, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 08, 10), Direction = Direction.ProductBilling, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 08, 24), Direction = Direction.SpeedyFeatures, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 08, 31), Direction = Direction.LinksDeliveryAgent, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 09, 28), Direction = Direction.Infrastructure, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 10, 10), Direction = Direction.Duty,  },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 10, 26), Direction = Direction.SpeedyFeatures, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 11, 03), Direction = Direction.Infrastructure},
                        new DirectionPeriod { BeginDate = new DateTime(2015, 12, 01), Direction = Direction.Leave},
                        new DirectionPeriod { BeginDate = new DateTime(2015, 12, 14), Direction = Direction.Certificates},
                        new DirectionPeriod { BeginDate = new DateTime(2016, 02, 08), Direction = Direction.Infrastructure},
                        new DirectionPeriod { BeginDate = new DateTime(2016, 02, 10), Direction = Direction.Leave},
                        new DirectionPeriod { BeginDate = new DateTime(2016, 02, 18), Direction = Direction.Infrastructure},
                        new DirectionPeriod { BeginDate = new DateTime(2016, 02, 19), Direction = Direction.SpeedyFeatures},
                        new DirectionPeriod { BeginDate = new DateTime(2016, 02, 24), Direction = Direction.Duty},
                        new DirectionPeriod { BeginDate = new DateTime(2016, 03, 09), Direction = Direction.SpeedyFeatures},
                    }},
                new RoundDancePeople { Name = "Юра Суслов", Email = "suslov_yura@skbkontur.ru",
                    WorkPeriods = new []
                    {
                        new DirectionPeriod { BeginDate = new DateTime(2015, 04, 06), Direction = Direction.СaServices, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 06, 30), Direction = Direction.Crm, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 08, 24), Direction = Direction.ProlongationScenario, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 10, 12), Direction = Direction.Duty, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 10, 26), Direction = Direction.SpeedyFeatures, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 11, 30), Direction = Direction.Duty, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 12, 07), Direction = Direction.SpeedyFeatures, },
                        new DirectionPeriod { BeginDate = new DateTime(2016, 02, 01), Direction = Direction.Duty, },
                        new DirectionPeriod { BeginDate = new DateTime(2016, 02, 08), Direction = Direction.SpeedyFeatures},
                        new DirectionPeriod { BeginDate = new DateTime(2016, 02, 16), Direction = Direction.Infrastructure},
                        new DirectionPeriod { BeginDate = new DateTime(2016, 02, 19), Direction = Direction.SpeedyFeatures},
                        new DirectionPeriod { BeginDate = new DateTime(2016, 03, 09), Direction = Direction.Duty, },
                        new DirectionPeriod { BeginDate = new DateTime(2016, 03, 21), Direction = Direction.ModifierBuy},
                    }},
                new RoundDancePeople { Name = "Саша Куликов", Email = "a.kulikov@skbkontur.ru",
                    WorkPeriods = new []
                    {
                        new DirectionPeriod { BeginDate = new DateTime(2015, 07, 01), Direction = Direction.СaServices, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 08, 19), Direction = Direction.SpeedyFeatures, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 09, 21), Direction = Direction.Duty, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 09, 28), Direction = Direction.Infrastructure, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 10, 05), Direction = Direction.SpeedyFeatures, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 10, 28), Direction = Direction.Fop},
                        new DirectionPeriod { BeginDate = new DateTime(2015, 11, 23), Direction = Direction.Duty},
                        new DirectionPeriod { BeginDate = new DateTime(2015, 11, 30), Direction = Direction.SpeedyFeatures},
                        new DirectionPeriod { BeginDate = new DateTime(2015, 12, 21), Direction = Direction.Duty},
                        new DirectionPeriod { BeginDate = new DateTime(2016, 01, 11), Direction = Direction.SpeedyFeatures},
                        new DirectionPeriod { BeginDate = new DateTime(2016, 01, 18), Direction = Direction.Infrastructure },
                        new DirectionPeriod { BeginDate = new DateTime(2016, 01, 25), Direction = Direction.Leave, },
                        new DirectionPeriod { BeginDate = new DateTime(2016, 02, 01), Direction = Direction.SpeedyFeatures, },
                        new DirectionPeriod { BeginDate = new DateTime(2016, 02, 15), Direction = Direction.Duty, },
                        new DirectionPeriod { BeginDate = new DateTime(2016, 02, 24), Direction = Direction.Infrastructure, },
                        new DirectionPeriod { BeginDate = new DateTime(2016, 03, 09), Direction = Direction.SpeedyFeatures},
                        new DirectionPeriod { BeginDate = new DateTime(2016, 03, 23), Direction = Direction.Duty},
                    }},
                new RoundDancePeople { Name = "Кирилл Иванов", Email = "ikp@skbkontur.ru",
                    WorkPeriods = new []
                    {
                        new DirectionPeriod { BeginDate = new DateTime(2016, 03, 09), Direction = Direction.Infrastructure},
                        new DirectionPeriod { BeginDate = new DateTime(2016, 03, 21), Direction = Direction.Duty},
                        new DirectionPeriod { BeginDate = new DateTime(2016, 03, 23), Direction = Direction.SpeedyFeatures},
                    }},
            };
        }

        private static RoundDancePeople[] GetCachedPeoples()
        {
            var fileName = string.Format(StorFilePattern, "RoundDance");
            if (File.Exists(fileName))
            {
                return JsonConvert.DeserializeObject<RoundDancePeople[]>(File.ReadAllText(fileName, Encoding.UTF8));
            }

            return null;
        }

        private RoundDancePeople[] GetStartVariant()
        {
            var dancePeoples = startVariant;

            foreach (var people in dancePeoples)
            {
                DirectionPeriod lastPeriod = null;
                foreach (var period in people.WorkPeriods)
                {
                    if (lastPeriod != null)
                    {
                        if (lastPeriod.BeginDate >= period.BeginDate)
                        {
                            throw new ArgumentOutOfRangeException(String.Format("period is bad for {0}", people.Name));
                        }

                        lastPeriod.SetNextPeriod(period);
                    }
                    
                    lastPeriod = period;
                }

                people.WorkPeriods.Last().SetAsCurrentPeriod();
            }

            return dancePeoples;
        }

        public RoundDancePeople[] GetAll()
        {
            peoples = peoples ?? GetCachedPeoples() ?? GetStartVariant();
            return peoples;
        }

        public Dictionary<Direction, int> GetDirectionResourceNeeds()
        {
            return new Dictionary<Direction, int>
            {
                {Direction.Crm, 2},
                {Direction.Infrastructure, 0},
                {Direction.ProductBilling, 2},
                {Direction.Support, 5},
                {Direction.СaServices, 4}
            };
        }
    }
}