using System;
using System.Collections.Generic;
using System.IO;
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
                        new DirectionPeriod { Direction = Direction.ProductBilling, BeginDate = new DateTime(2015, 04, 6), EndDate = new DateTime(2015, 06, 29) },
                        new DirectionPeriod { Direction = Direction.Crm, BeginDate = new DateTime(2015, 06, 30), EndDate = new DateTime(2015, 8, 25)},
                        new DirectionPeriod { Direction = Direction.SpeedyFeatures, BeginDate = new DateTime(2015, 08, 25), EndDate = new DateTime(2015, 09, 10)},
                        new DirectionPeriod { Direction = Direction.Leave, BeginDate = new DateTime(2015, 09, 11), EndDate = new DateTime(2015, 09, 27)},
                        new DirectionPeriod { Direction = Direction.Duty, BeginDate = new DateTime(2015, 09, 28)},
                    }},
                new RoundDancePeople { Name = "Кун Андрей",
                    WorkPeriods = new []
                    {
                        new DirectionPeriod { Direction = Direction.Infrastructure, BeginDate = new DateTime(2015, 07, 27), EndDate = new DateTime(2015, 08, 16) },
                        new DirectionPeriod { Direction = Direction.Leave, BeginDate = new DateTime(2015, 08, 17) }
                    }},
                new RoundDancePeople { Name = "Павел Александров",
                    WorkPeriods = new []
                    {
                        new DirectionPeriod { Direction = Direction.ProductBilling, BeginDate = new DateTime(2015, 04, 6), EndDate = new DateTime(2015, 05, 10)},
                        new DirectionPeriod { Direction = Direction.ProductBilling, BeginDate = new DateTime(2015, 05, 25), EndDate = new DateTime(2015, 07, 12) },
                        new DirectionPeriod { Direction = Direction.Crm, BeginDate = new DateTime(2015, 07, 12), EndDate = new DateTime(2015, 07, 19)},
                        new DirectionPeriod { Direction = Direction.Support, BeginDate = new DateTime(2015, 07, 20), EndDate = new DateTime(2015, 07, 26)},
                        new DirectionPeriod { Direction = Direction.Infrastructure, BeginDate = new DateTime(2015, 07, 27), EndDate = new DateTime(2015, 08, 09)},
                        new DirectionPeriod { Direction = Direction.Duty, BeginDate = new DateTime(2015, 08, 10), EndDate = new DateTime(2015, 08, 16)},
                        new DirectionPeriod { Direction = Direction.Infrastructure, BeginDate = new DateTime(2015, 08, 17), EndDate = new DateTime(2015, 08, 23)},
                        new DirectionPeriod { Direction = Direction.SpeedyFeatures, BeginDate = new DateTime(2015, 08, 24), EndDate = new DateTime(2015, 09, 13)},
                        new DirectionPeriod { Direction = Direction.Duty, BeginDate = new DateTime(2015, 09, 14), EndDate = new DateTime(2015, 09, 20)},
                        new DirectionPeriod { Direction = Direction.SpeedyFeatures, BeginDate = new DateTime(2015, 09, 21)},
                    }},
                new RoundDancePeople { Name = "Никита Бурлаков",
                    WorkPeriods = new []
                    {
                        new DirectionPeriod { Direction = Direction.ProductBilling, BeginDate = new DateTime(2015, 4, 4), EndDate = new DateTime(2015, 4, 19) },
                        new DirectionPeriod { Direction = Direction.Support, BeginDate = new DateTime(2015, 4, 26), EndDate = new DateTime(2015, 5, 04) },
                        new DirectionPeriod { Direction = Direction.ProductBilling, BeginDate = new DateTime(2015, 4, 04), EndDate = new DateTime(2015, 5, 10) },
                        new DirectionPeriod { Direction = Direction.Support, BeginDate = new DateTime(2015, 5, 11), EndDate = new DateTime(2015, 5, 17) },
                        new DirectionPeriod { Direction = Direction.ProductBilling, BeginDate = new DateTime(2015, 5, 18), EndDate = new DateTime(2015, 08, 09)},
                        new DirectionPeriod { Direction = Direction.Infrastructure, BeginDate = new DateTime(2015, 8, 10), EndDate = new DateTime(2015, 8, 16)},
                        new DirectionPeriod { Direction = Direction.Duty, BeginDate = new DateTime(2015, 8, 17), EndDate = new DateTime(2015, 8, 23)},
                        new DirectionPeriod { Direction = Direction.ProlongationScenario, BeginDate = new DateTime(2015, 8, 24), EndDate = new DateTime(2015, 9, 13)},
                        new DirectionPeriod { Direction = Direction.Leave, BeginDate = new DateTime(2015, 9, 14), EndDate = new DateTime(2015, 9, 27)},
                        new DirectionPeriod { Direction = Direction.Duty, BeginDate = new DateTime(2015, 9, 28), EndDate = new DateTime(2015, 10, 04)},
                    }},
                new RoundDancePeople { Name = "Игорь Мамай",
                    WorkPeriods = new []
                    {
                        new DirectionPeriod { Direction = Direction.Infrastructure, BeginDate = new DateTime(2015, 8, 10), EndDate = new DateTime(2015, 8, 16)},
                        new DirectionPeriod { Direction = Direction.Duty, BeginDate = new DateTime(2015, 8, 17), EndDate = new DateTime(2015, 8, 23)},
                        new DirectionPeriod { Direction = Direction.SpeedyFeatures, BeginDate = new DateTime(2015, 8, 24), EndDate = new DateTime(2015, 8, 30)},
                        new DirectionPeriod { Direction = Direction.LinksDeliveryAgent, BeginDate = new DateTime(2015, 8, 31)},
                    }},
                new RoundDancePeople { Name = "Леша Романовский",
                    WorkPeriods = new []
                    {
                        new DirectionPeriod { Direction = Direction.СaServices, BeginDate = new DateTime(2015, 04, 6), EndDate = new DateTime(2015, 05, 17)},
                        new DirectionPeriod { Direction = Direction.ProductBilling, BeginDate = new DateTime(2015, 05, 18), EndDate = new DateTime(2015, 06, 19) },
                        new DirectionPeriod { Direction = Direction.Infrastructure, BeginDate = new DateTime(2015, 06, 20), EndDate = new DateTime(2015, 07, 01) },
                        new DirectionPeriod { Direction = Direction.Support, BeginDate = new DateTime(2015, 07, 6), EndDate = new DateTime(2015, 8, 19)},
                        new DirectionPeriod { Direction = Direction.Vendors, BeginDate = new DateTime(2015, 08, 20), EndDate = new DateTime(2015, 09, 07)},
                        new DirectionPeriod { Direction = Direction.RomingDiadoc, BeginDate = new DateTime(2015, 09, 08)}
                    }},
                new RoundDancePeople { Name = "Сережа Рожин",
                    WorkPeriods = new []
                    {
                        new DirectionPeriod { Direction = Direction.СaServices, BeginDate = new DateTime(2015, 04, 6), EndDate = new DateTime(2015, 07, 12) },
                        new DirectionPeriod { Direction = Direction.Support, BeginDate = new DateTime(2015, 07, 13), EndDate = new DateTime(2015, 08, 11)},
                        new DirectionPeriod { Direction = Direction.Leave, BeginDate = new DateTime(2015, 08, 12), EndDate = new DateTime(2015, 08, 28)},
                        new DirectionPeriod { Direction = Direction.Leave, BeginDate = new DateTime(2015, 08, 12), EndDate = new DateTime(2015, 08, 30)},
                        new DirectionPeriod { Direction = Direction.Duty, BeginDate = new DateTime(2015, 08, 31), EndDate = new DateTime(2015, 09, 06)},
                        new DirectionPeriod { Direction = Direction.SpeedyFeatures, BeginDate = new DateTime(2015, 09, 07)},
                    }},
                new RoundDancePeople { Name = "Леша Иванов",
                    WorkPeriods = new []
                    {
                        new DirectionPeriod { Direction = Direction.ProductBilling, BeginDate = new DateTime(2015, 04, 6), EndDate = new DateTime(2015, 05, 01)},
                        new DirectionPeriod { Direction = Direction.ProductBilling, BeginDate = new DateTime(2015, 05, 25), EndDate = new DateTime(2015, 06, 1)},
                        new DirectionPeriod { Direction = Direction.Support, BeginDate = new DateTime(2015, 06, 2), EndDate = new DateTime(2015, 07, 05) },
                        new DirectionPeriod { Direction = Direction.ProductBilling, BeginDate = new DateTime(2015, 07, 06), EndDate = new DateTime(2015, 08, 23)},
                        new DirectionPeriod { Direction = Direction.Duty, BeginDate = new DateTime(2015, 08, 24), EndDate = new DateTime(2015, 09, 01)},
                        new DirectionPeriod { Direction = Direction.Infrastructure, BeginDate = new DateTime(2015, 09, 02), EndDate = new DateTime(2015, 09, 15)},
                        new DirectionPeriod { Direction = Direction.SpeedyFeatures, BeginDate = new DateTime(2015, 09, 16)},
                    }},
                new RoundDancePeople { Name = "Оксана Запорожец",
                    WorkPeriods = new []
                    {
                        new DirectionPeriod { Direction = Direction.ProlongationScenario, BeginDate = new DateTime(2015, 08, 24)},
                    }},
                // Direction = Support
                new RoundDancePeople { Name = "Женя Клюкин",
                    WorkPeriods = new []
                    {
                        new DirectionPeriod { Direction = Direction.Support, BeginDate = new DateTime(2015, 04, 6), EndDate = new DateTime(2015, 6, 01)},
                        new DirectionPeriod { Direction = Direction.Crm, BeginDate = new DateTime(2015, 06, 2), EndDate = new DateTime(2015, 06, 10) },
                        new DirectionPeriod { Direction = Direction.Support, BeginDate = new DateTime(2015, 06, 10), EndDate = new DateTime(2015, 07, 12) },
                        new DirectionPeriod { Direction = Direction.СaServices, BeginDate = new DateTime(2015, 07, 13), EndDate = new DateTime(2015, 08, 16)},
                        new DirectionPeriod { Direction = Direction.Leave, BeginDate = new DateTime(2015, 08, 17), EndDate = new DateTime(2015, 09, 06)},
                        new DirectionPeriod { Direction = Direction.Duty, BeginDate = new DateTime(2015, 09, 07), EndDate = new DateTime(2015, 09, 13)},
                        new DirectionPeriod { Direction = Direction.SpeedyFeatures, BeginDate = new DateTime(2015, 09, 14)},
                    }},
                new RoundDancePeople { Name = "Антон Ежов",
                    WorkPeriods = new []
                    {
                        new DirectionPeriod { Direction = Direction.Support, BeginDate = new DateTime(2015, 04, 6), EndDate = new DateTime(2015, 07, 05) },
                        new DirectionPeriod { Direction = Direction.СaServices, BeginDate = new DateTime(2015, 07, 06), EndDate = new DateTime(2015, 08, 15)},
                        new DirectionPeriod { Direction = Direction.CaTariffsAndDiscounts, BeginDate = new DateTime(2015, 08, 16), EndDate = new DateTime(2015, 08, 31)},
                        new DirectionPeriod { Direction = Direction.Leave, BeginDate = new DateTime(2015, 09, 01), EndDate = new DateTime(2015, 09, 17)},
                        new DirectionPeriod { Direction = Direction.CaMigration, BeginDate = new DateTime(2015, 09, 18)}
                    }},
                new RoundDancePeople { Name = "Андрей Шалин",
                    WorkPeriods = new []
                    {
                        new DirectionPeriod { Direction = Direction.ProductBilling, BeginDate = new DateTime(2015, 04, 6), EndDate = new DateTime(2015, 04, 26)},
                        new DirectionPeriod { Direction = Direction.Support, BeginDate = new DateTime(2015, 04, 27), EndDate = new DateTime(2015, 6, 1)},
                        new DirectionPeriod { Direction = Direction.Crm, BeginDate = new DateTime(2015, 06, 2), EndDate = new DateTime(2015, 06, 20) },
                        new DirectionPeriod { Direction = Direction.Infrastructure, BeginDate = new DateTime(2015, 06, 20), EndDate = new DateTime(2015, 07, 01) },
                        new DirectionPeriod { Direction = Direction.Support, BeginDate = new DateTime(2015, 07, 01), EndDate = new DateTime(2015, 07, 19)},
                        new DirectionPeriod { Direction = Direction.Leave, BeginDate = new DateTime(2015, 07, 20), EndDate = new DateTime(2015, 08, 02)},
                        new DirectionPeriod { Direction = Direction.Duty, BeginDate = new DateTime(2015, 08, 03), EndDate = new DateTime(2015, 08, 09)},
                        new DirectionPeriod { Direction = Direction.Support, BeginDate = new DateTime(2015, 08, 10), EndDate = new DateTime(2015, 08, 19)},
                        new DirectionPeriod { Direction = Direction.Vendors, BeginDate = new DateTime(2015, 08, 20), EndDate = new DateTime(2015, 09, 07)},
                        new DirectionPeriod { Direction = Direction.RomingDiadoc, BeginDate = new DateTime(2015, 09, 08)}
                    }},
                new RoundDancePeople { Name = "Оля Соболева",
                    WorkPeriods = new []
                    {
                        new DirectionPeriod { Direction = Direction.ProductBilling, BeginDate = new DateTime(2015, 04, 6), EndDate = new DateTime(2015, 05, 18)},
                        new DirectionPeriod { Direction = Direction.Support, BeginDate = new DateTime(2015, 05, 19), EndDate = new DateTime(2015, 07, 19) },
                        new DirectionPeriod { Direction = Direction.СaServices, BeginDate = new DateTime(2015, 07, 20), EndDate = new DateTime(2015, 08, 15)},
                        new DirectionPeriod { Direction = Direction.CaTariffsAndDiscounts, BeginDate = new DateTime(2015, 08, 16), EndDate = new DateTime(2015, 08, 30)},
                        new DirectionPeriod { Direction = Direction.CaMigration, BeginDate = new DateTime(2015, 08, 31), EndDate = new DateTime(2015, 09, 19)},
                        new DirectionPeriod { Direction = Direction.Leave, BeginDate = new DateTime(2015, 09, 20)}
                    }},
                // Direction = CaServices
                new RoundDancePeople { Name = "Саша Чичерский",
                    WorkPeriods = new []
                    {
                        new DirectionPeriod { Direction = Direction.СaServices, BeginDate = new DateTime(2015, 04, 6), EndDate = new DateTime(2015, 07, 24)},
                        new DirectionPeriod { Direction = Direction.Leave, BeginDate = new DateTime(2015, 07, 13), EndDate = new DateTime(2015, 07, 26)},
                        new DirectionPeriod { Direction = Direction.Duty, BeginDate = new DateTime(2015, 07, 27), EndDate = new DateTime(2015, 08, 09)},
                        new DirectionPeriod { Direction = Direction.ProductBilling, BeginDate = new DateTime(2015, 08, 10), EndDate = new DateTime(2015, 08, 23)},
                        new DirectionPeriod { Direction = Direction.SpeedyFeatures, BeginDate = new DateTime(2015, 08, 24), EndDate = new DateTime(2015, 08, 30)},
                        new DirectionPeriod { Direction = Direction.LinksDeliveryAgent, BeginDate = new DateTime(2015, 08, 31)}
                    }},
                new RoundDancePeople { Name = "Юра Плинер",
                    WorkPeriods = new []
                    {
                        new DirectionPeriod { Direction = Direction.СaServices, BeginDate = new DateTime(2015, 04, 27), EndDate = new DateTime(2015, 08, 09)},
                        new DirectionPeriod { Direction = Direction.Leave, BeginDate = new DateTime(2015, 08, 10), EndDate = new DateTime(2015, 08, 23)},
                        new DirectionPeriod { Direction = Direction.Duty, BeginDate = new DateTime(2015, 08, 24), EndDate = new DateTime(2015, 09, 01)},
                        new DirectionPeriod { Direction = Direction.Infrastructure, BeginDate = new DateTime(2015, 09, 02), EndDate = new DateTime(2015, 09, 15)},
                        new DirectionPeriod { Direction = Direction.SpeedyFeatures, BeginDate = new DateTime(2015, 09, 16)},
                    }},
                new RoundDancePeople { Name = "Юра Суслов",
                    WorkPeriods = new []
                    {
                        new DirectionPeriod { Direction = Direction.СaServices, BeginDate = new DateTime(2015, 04, 6), EndDate = new DateTime(2015, 06, 29)},
                        new DirectionPeriod { Direction = Direction.Crm, BeginDate = new DateTime(2015, 06, 30), EndDate = new DateTime(2015, 08, 23)},
                        new DirectionPeriod { Direction = Direction.ProlongationScenario, BeginDate = new DateTime(2015, 08, 24), EndDate = new DateTime(2015, 09, 20)},
                        new DirectionPeriod { Direction = Direction.Duty, BeginDate = new DateTime(2015, 09, 21), EndDate = new DateTime(2015, 09, 27)},
                        new DirectionPeriod { Direction = Direction.SpeedyFeatures, BeginDate = new DateTime(2015, 09, 28)},
                    }},
                new RoundDancePeople { Name = "Саша Куликов",
                    WorkPeriods = new []
                    {
                        new DirectionPeriod { Direction = Direction.СaServices, BeginDate = new DateTime(2015, 07, 1), EndDate = new DateTime(2015, 08, 18)},
                        new DirectionPeriod { Direction = Direction.SpeedyFeatures, BeginDate = new DateTime(2015, 08, 19)},
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

        public RoundDancePeople[] GetAll()
        {
            peoples = peoples ?? GetCachedPeoples() ?? startVariant;
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