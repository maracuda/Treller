using System.Collections.Generic;
using Infrastructure.Sugar;

namespace WebApplication.Implementation.Services.News.Reporters
{
    public interface IReporter
    {
        IEnumerable<TaskNew> MakeReport();
        Maybe<IEnumerable<TaskNew>> TryToMakeReport(string aboutCardId);
    }
}