using System.Linq;

namespace SKBKontur.Treller.WebApplication.Services.Settings
{
    public class SettingService : ISettingService
    {
        private static readonly BoardSettings[] Settings = new[]
        {
            new BoardSettings
                {
                    Id = "552ab670e01e4af28afdc2c2",
                    Name = "Биллинг продуктов",
                    DevelopListName = "Dev",
                    AnalyticListName = "Analytics & Design",
                    ReviewListName = "Review",
                    DevelopPresentationListName = "",
                    TestingListName = "Testing"
                },
            new BoardSettings
                {
                    Id = "552aae2b8ca279a4eb485af7",
                    Name = "Услуги УЦ",
                    DevelopListName = "Dev",
                    AnalyticListName = "Analytics & Design",
                    ReviewListName = "Review",
                    DevelopPresentationListName = "",
                    TestingListName = "Testing"
                },
            new BoardSettings
                {
                    Id = "552ab5e211d6ae9b61dbaa77",
                    Name = "Оптимизация ТП",
                    DevelopListName = "Dev",
                    AnalyticListName = "Analytics & Design",
                    ReviewListName = "Review",
                    DevelopPresentationListName = "",
                    TestingListName = "Testing"
                },
            new BoardSettings
                {
                    Id = "552ab2abc66faf3bec7915f4",
                    Name = "Инфраструктура",
                    DevelopListName = "Dev",
                    AnalyticListName = "",
                    ReviewListName = "Review",
                    DevelopPresentationListName = "",
                    TestingListName = "Testing"
                }
        };

        public string[] GetDevelopingBoardIds()
        {
            return Settings.Select(x => x.Id).ToArray();
        }

        public BoardSettings[] GetDevelopingBoards()
        {
            return Settings;
        }
    }
}