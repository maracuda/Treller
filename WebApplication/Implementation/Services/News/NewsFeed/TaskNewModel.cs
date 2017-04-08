using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Extensions;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.NewsFeed
{
    public class TaskNewModel
    {
        public string TaskId { get; set; }
        public ContentModel Content { get; set; }
        public ReportModel[] Reports { get; set; }
        public string DeadlineStr => Content.DeadLine.Stringify("не указано");
    }
}