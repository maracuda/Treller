using SKBKontur.Treller.WebApplication.Implementation.RoundDance.BusinessObjects;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Settings
{
    public class BoardSettings
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string DevelopListName { get; set; }
        public string AnalyticListName { get; set; }
        public string ReviewListName { get; set; }
        public string DevelopPresentationListName { get; set; }
        public string TestingListName { get; set; }
        public string WaitForReleaseListName { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsServiceTeamBoard { get; set; }
        public Direction? DefaultDirection { get; set; }
    }
}