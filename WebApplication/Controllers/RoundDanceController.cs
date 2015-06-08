using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Linq;

namespace SKBKontur.Treller.WebApplication.Controllers
{
    public class RoundDanceController : Controller
    {
        private readonly IRoundDanceViewModelBuilder roundDanceViewModelBuilder;

        public RoundDanceController(IRoundDanceViewModelBuilder roundDanceViewModelBuilder)
        {
            this.roundDanceViewModelBuilder = roundDanceViewModelBuilder;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var viewModel = roundDanceViewModelBuilder.Build();
            return View("Index", viewModel);
        }
    }

    public interface IRoundDanceViewModelBuilder
    {
        RoundDanceViewModel Build();
    }

    public class RoundDanceViewModelBuilder : IRoundDanceViewModelBuilder
    {
        private readonly IRoundDancePeopleStorage roundDancePeopleStorage;

        public RoundDanceViewModelBuilder(IRoundDancePeopleStorage roundDancePeopleStorage)
        {
            this.roundDancePeopleStorage = roundDancePeopleStorage;
        }

        public RoundDanceViewModel Build()
        {
            var peoples = roundDancePeopleStorage.GetAll();

            return new RoundDanceViewModel
                       {
                           DirectionPeoples = peoples
                                                .Select(InnerBuild)
                                                .GroupBy(x => x.CurrentDirection)
                                                .ToDictionary(x => x.Key, x => x.OrderByDescending(o => o.CurrentWeight.Weight).ToArray())
                       };
        }

        private static RoundDancePeopleViewModel InnerBuild(RoundDancePeople people)
        {
            var currentDirection = people.WorkPeriods.Last().Direction;
            return new RoundDancePeopleViewModel
                       {
                           People = people,
                           CurrentDirection = currentDirection,
                           DirectionWeights = BuildWeight(people).OrderBy(t => t.Direction).ToDictionary(x => x.Direction)
                       };
        }

        private static RoundDanceDirectionWeight[] BuildWeight(RoundDancePeople people)
        {
            var result = people
                .WorkPeriods
                .GroupBy(x => x.Direction)
                .Select(x => BuildDirection(x.Where(d => d.Direction == x.Key).ToArray()))
                .ToArray();

            return Enum.GetValues(typeof (Direction))
                .Cast<Direction>()
                .Where(x => result.All(r => r.Direction != x))
                .Select(x => new RoundDanceDirectionWeight
                                 {
                                     Direction = x,
                                     Weight = 0,
                                     RotationWeight = 100
                                 }).Union(result).ToArray();

        }

        private static RoundDanceDirectionWeight BuildDirection(DirectionPeriod[] periods)
        {
            var directionWeight = 0;
            foreach (var period in periods)
            {
                var endDate = period.EndDate ?? DateTime.Now.Date;
                var daysCount = (endDate - period.BeginDate).Days + 1;
                var daysDiff = 90 - (DateTime.Now.Date - endDate.Date).Days;

                directionWeight += daysCount*daysDiff;
            }

            var maxWeight = 90*91;

            return new RoundDanceDirectionWeight
                       {
                           Direction = periods.First().Direction,
                           Weight = ToPercent(directionWeight, maxWeight),
                           RotationWeight = ToPercent(maxWeight - directionWeight, maxWeight)
                       };
        }

        private static decimal ToPercent(int value, int maxValue)
        {
            return decimal.Round((decimal) value * 100 / maxValue, 2);
        }
    }

    public class RoundDanceViewModel
    {
        public Dictionary<Direction, RoundDancePeopleViewModel[]> DirectionPeoples { get; set; }
    }

    public class RoundDanceDirectionWeight
    {
        public Direction Direction { get; set; }
        public decimal RotationWeight { get; set; }
        public decimal Weight { get; set; }
    }

    public class RoundDancePeopleViewModel
    {
        public RoundDancePeople People { get; set; }
        public Dictionary<Direction, RoundDanceDirectionWeight> DirectionWeights { get; set; }
        public Direction CurrentDirection { get; set; }
        public RoundDanceDirectionWeight CurrentWeight { get { return DirectionWeights[CurrentDirection]; } }
    }

    public enum Direction
    {
        [Description("Биллинг")]
        ProductBilling = 0,

        [Description("ТП")]
        Support,

        [Description("УЦ")]
        СaServices,

        [Description("Партнерка")]
        Crm,

        [Description("Инфраструктура")]
        Infrastructure
    }

    public class DirectionPeriod
    {
        public Direction Direction { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class RoundDancePeople
    {
        public string Name { get; set; }
        public DirectionPeriod[] WorkPeriods { get; set; }
    }

    public interface IRoundDancePeopleStorage
    {
        RoundDancePeople[] GetAll();
    }

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
                                   new RoundDancePeople { Name = "Катя Филипова",
                                                          WorkPeriods = new []
                                                                            {
                                                                                new DirectionPeriod { Direction = Direction.ProductBilling, BeginDate = new DateTime(2015, 04, 6) }
                                                                            }},
                                   new RoundDancePeople { Name = "Павел Александров",
                                                          WorkPeriods = new []
                                                                            {
                                                                                new DirectionPeriod { Direction = Direction.ProductBilling, BeginDate = new DateTime(2015, 04, 6), EndDate = new DateTime(2015, 05, 10)},
                                                                                new DirectionPeriod { Direction = Direction.ProductBilling, BeginDate = new DateTime(2015, 05, 25)}
                                                                            }},
                                   new RoundDancePeople { Name = "Никита Бурлаков",
                                                          WorkPeriods = new []
                                                                            {
                                                                                new DirectionPeriod { Direction = Direction.ProductBilling, BeginDate = new DateTime(2015, 4, 4), EndDate = new DateTime(2015, 4, 19) },
                                                                                new DirectionPeriod { Direction = Direction.Support, BeginDate = new DateTime(2015, 4, 26), EndDate = new DateTime(2015, 5, 04) },
                                                                                new DirectionPeriod { Direction = Direction.ProductBilling, BeginDate = new DateTime(2015, 4, 04), EndDate = new DateTime(2015, 5, 10) },
                                                                                new DirectionPeriod { Direction = Direction.Support, BeginDate = new DateTime(2015, 5, 11), EndDate = new DateTime(2015, 5, 17) },
                                                                                new DirectionPeriod { Direction = Direction.ProductBilling, BeginDate = new DateTime(2015, 5, 18) }
                                                                            }},
                                   new RoundDancePeople { Name = "Леша Романовский",
                                                          WorkPeriods = new []
                                                                            {
                                                                                new DirectionPeriod { Direction = Direction.СaServices, BeginDate = new DateTime(2015, 04, 6), EndDate = new DateTime(2015, 05, 17)},
                                                                                new DirectionPeriod { Direction = Direction.ProductBilling, BeginDate = new DateTime(2015, 05, 18)}
                                                                            }},
                                   new RoundDancePeople { Name = "Леша Иванов",
                                                          WorkPeriods = new []
                                                                            {
                                                                                new DirectionPeriod { Direction = Direction.ProductBilling, BeginDate = new DateTime(2015, 04, 6), EndDate = new DateTime(2015, 05, 01)},
                                                                                new DirectionPeriod { Direction = Direction.ProductBilling, BeginDate = new DateTime(2015, 05, 25), EndDate = new DateTime(2015, 06, 1)},
                                                                                new DirectionPeriod { Direction = Direction.Support, BeginDate = new DateTime(2015, 06, 2)}
                                                                            }},
                                   // Direction = Support
                                   new RoundDancePeople { Name = "Женя Клюкин",
                                                          WorkPeriods = new []
                                                                            {
                                                                                new DirectionPeriod { Direction = Direction.Support, BeginDate = new DateTime(2015, 04, 6), EndDate = new DateTime(2015, 6, 01)},
                                                                                new DirectionPeriod { Direction = Direction.Crm, BeginDate = new DateTime(2015, 06, 2)}
                                                                            }},
                                   new RoundDancePeople { Name = "Антон Ежов",
                                                          WorkPeriods = new []
                                                                            {
                                                                                new DirectionPeriod { Direction = Direction.Support, BeginDate = new DateTime(2015, 04, 6) }
                                                                            }},
                                   new RoundDancePeople { Name = "Андрей Шалин",
                                                          WorkPeriods = new []
                                                                            {
                                                                                new DirectionPeriod { Direction = Direction.ProductBilling, BeginDate = new DateTime(2015, 04, 6), EndDate = new DateTime(2015, 04, 26)},
                                                                                new DirectionPeriod { Direction = Direction.Support, BeginDate = new DateTime(2015, 04, 27), EndDate = new DateTime(2015, 6, 1)},
                                                                                new DirectionPeriod { Direction = Direction.Crm, BeginDate = new DateTime(2015, 06, 2)}
                                                                            }},
                                   new RoundDancePeople { Name = "Оля Соболева",
                                                          WorkPeriods = new []
                                                                            {
                                                                                new DirectionPeriod { Direction = Direction.ProductBilling, BeginDate = new DateTime(2015, 04, 6), EndDate = new DateTime(2015, 05, 18)},
                                                                                new DirectionPeriod { Direction = Direction.Support, BeginDate = new DateTime(2015, 05, 19)}
                                                                            }},
                                   // Direction = CaServices
                                   new RoundDancePeople { Name = "Саша Чичерский",
                                                          WorkPeriods = new []
                                                                            {
                                                                                new DirectionPeriod { Direction = Direction.СaServices, BeginDate = new DateTime(2015, 04, 6)}
                                                                            }},
                                   new RoundDancePeople { Name = "Сережа Рожин",
                                                          WorkPeriods = new []
                                                                            {
                                                                                new DirectionPeriod { Direction = Direction.СaServices, BeginDate = new DateTime(2015, 04, 6)},
                                                                            }},
                                   new RoundDancePeople { Name = "Юра Плинер",
                                                          WorkPeriods = new []
                                                                            {
                                                                                new DirectionPeriod { Direction = Direction.СaServices, BeginDate = new DateTime(2015, 04, 27)}
                                                                            }},
                                   new RoundDancePeople { Name = "Юра Суслов",
                                                          WorkPeriods = new []
                                                                            {
                                                                                new DirectionPeriod { Direction = Direction.СaServices, BeginDate = new DateTime(2015, 04, 6)}
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
    }
}