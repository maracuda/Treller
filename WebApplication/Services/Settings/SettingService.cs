using System.Linq;

namespace SKBKontur.Treller.WebApplication.Services.Settings
{
    public class SettingService : ISettingService
    {
        private static readonly BoardSettings[] Settings = new[]
        {
            new BoardSettings
                {
                    Id = "4f4e2e4a0141dade72f808ef",
                    Name = "Биллинг",
                    DevelopListName = "Developing",
                    AnalyticListName = "Analitics & Design",
                    ReviewListName = "Review",
                    DevelopPresentationListName = "Team Presentation",
                    TestingListName = "Testing"
                },
            new BoardSettings
                {
                    Id = "50a218636f423ac72500927f",
                    Name = "CRM",
                    DevelopListName = "Developing",
                    AnalyticListName = "Analitics & Design",
                    ReviewListName = "Review",
                    DevelopPresentationListName = "Team Presentation",
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