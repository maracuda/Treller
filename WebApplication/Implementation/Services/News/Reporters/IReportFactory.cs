namespace WebApplication.Implementation.Services.News.Reporters
{
    public interface IReportFactory
    {
        Report CreateCutomerReport(Content.Content content, string taskId);
        Report CreateSupportReport(Content.Content content, string taskId);
        Report CreateTeamReport(Content.Content content, string taskId);
    }
}