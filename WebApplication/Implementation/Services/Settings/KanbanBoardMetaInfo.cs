namespace SKBKontur.Treller.WebApplication.Implementation.Services.Settings
{
    public class KanbanBoardMetaInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string DevelopListName { get; set; }
        public string AnalyticListName { get; set; }
        public string ReviewListName { get; set; }
        public string DevelopPresentationListName { get; set; }
        public string TestingListName { get; set; }
        public string WaitForReleaseListName { get; set; }
        public bool IsServiceTeamBoard { get; set; }
    }
}