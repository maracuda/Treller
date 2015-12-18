using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace SKBKontur.Treller.WebApplication.Controllers.RoundDance
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

                // Direction = ProductBilling
                new RoundDancePeople { Name = "Катя Зеленина",
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
                    }},
                new RoundDancePeople { Name = "Кун Андрей",
                    WorkPeriods = new []
                    {
                        new DirectionPeriod { BeginDate = new DateTime(2015, 07, 27), Direction = Direction.Infrastructure },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 08, 17), Direction = Direction.Leave },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 08, 31), Direction = Direction.Infrastructure },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 09, 07), Direction = Direction.LinksDeliveryAgent },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 10, 05), Direction = Direction.SpeedyFeatures },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 12, 15), Direction = Direction.Sickness },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 12, 17), Direction = Direction.SpeedyFeatures },
                    }},
                new RoundDancePeople { Name = "Павел Александров",
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
                    }},
                new RoundDancePeople { Name = "Никита Бурлаков",
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

                    }},
                new RoundDancePeople { Name = "Степан Мурашов",
                    WorkPeriods = new []
                    {
                        new DirectionPeriod { BeginDate = new DateTime(2015, 10, 20), Direction = Direction.Infrastructure,},
                        new DirectionPeriod { BeginDate = new DateTime(2015, 11, 06), Direction = Direction.SpeedyFeatures,},
                        new DirectionPeriod { BeginDate = new DateTime(2015, 11, 30), Direction = Direction.Duty,},
                        new DirectionPeriod { BeginDate = new DateTime(2015, 12, 14), Direction = Direction.SpeedyFeatures,},
                    }},
                new RoundDancePeople { Name = "Игорь Мамай",
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
                    }},
                new RoundDancePeople { Name = "Леша Романовский",
                    WorkPeriods = new []
                    {
                        new DirectionPeriod { BeginDate = new DateTime(2015, 04, 06), Direction = Direction.СaServices, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 05, 18), Direction = Direction.ProductBilling,  },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 06, 20), Direction = Direction.Infrastructure,  },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 07, 06), Direction = Direction.Support, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 08, 20), Direction = Direction.Vendors, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 09, 08), Direction = Direction.RomingDiadoc, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 09, 21), Direction = Direction.CaMigration, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 10, 05), Direction = Direction.SpeedyFeatures, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 10, 12), Direction = Direction.RomingDiadoc, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 11, 19), Direction = Direction.Leave, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 11, 24), Direction = Direction.RomingDiadoc, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 12, 08), Direction = Direction.SpeedyFeatures, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 12, 14), Direction = Direction.Duty },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 12, 21), Direction = Direction.SpeedyFeatures },
                    }},
                new RoundDancePeople { Name = "Сережа Рожин",
                    WorkPeriods = new []
                    {
                        new DirectionPeriod { BeginDate = new DateTime(2015, 04, 06), Direction = Direction.СaServices,  },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 07, 13), Direction = Direction.Support, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 08, 12), Direction = Direction.Leave, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 08, 31), Direction = Direction.Duty, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 09, 07), Direction = Direction.SpeedyFeatures, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 10, 26), Direction = Direction.Duty, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 11, 03), Direction = Direction.SpeedyFeatures},
                        new DirectionPeriod { BeginDate = new DateTime(2015, 11, 16), Direction = Direction.Infrastructure},
                        new DirectionPeriod { BeginDate = new DateTime(2015, 11, 30), Direction = Direction.Certificates},
                    }},
                new RoundDancePeople { Name = "Леша Иванов",
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
                    }},
                new RoundDancePeople { Name = "Оксана Запорожец",
                    WorkPeriods = new []
                    {
                        new DirectionPeriod { BeginDate = new DateTime(2015, 08, 24), Direction = Direction.ProlongationScenario, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 10, 09), Direction = Direction.Infrastructure, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 11, 05), Direction = Direction.SpeedyFeatures},
                        new DirectionPeriod { BeginDate = new DateTime(2015, 11, 25), Direction = Direction.Fisics},
                        new DirectionPeriod { BeginDate = new DateTime(2015, 11, 30), Direction = Direction.Leave},
                        new DirectionPeriod { BeginDate = new DateTime(2015, 12, 03), Direction = Direction.Fisics},
                        new DirectionPeriod { BeginDate = new DateTime(2015, 12, 07), Direction = Direction.Infrastructure},
                        new DirectionPeriod { BeginDate = new DateTime(2015, 12, 10), Direction = Direction.SpeedyFeatures},
                    }},
                // Direction = Support
                new RoundDancePeople { Name = "Женя Клюкин",
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
                    }},
                new RoundDancePeople { Name = "Антон Ежов",
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
                        new DirectionPeriod { BeginDate = new DateTime(2015, 12, 29), Direction = Direction.Leave},
                    }},
                new RoundDancePeople { Name = "Андрей Шалин",
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
                    }},
                // Direction = CaServices
                new RoundDancePeople { Name = "Саша Чичерский",
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
                    }},
                new RoundDancePeople { Name = "Юра Суслов",
                    WorkPeriods = new []
                    {
                        new DirectionPeriod { BeginDate = new DateTime(2015, 04, 06), Direction = Direction.СaServices, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 06, 30), Direction = Direction.Crm, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 08, 24), Direction = Direction.ProlongationScenario, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 10, 12), Direction = Direction.Duty, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 10, 26), Direction = Direction.SpeedyFeatures, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 11, 30), Direction = Direction.Duty, },
                        new DirectionPeriod { BeginDate = new DateTime(2015, 12, 07), Direction = Direction.SpeedyFeatures, },
                    }},
                new RoundDancePeople { Name = "Саша Куликов",
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