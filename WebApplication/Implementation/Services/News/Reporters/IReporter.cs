using System.Collections.Generic;
using SKBKontur.Infrastructure.Sugar;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Reporters
{
    public interface IReporter
    {
        IEnumerable<TaskNew> MakeReport();
        Maybe<IEnumerable<TaskNew>> TryToMakeReport(string aboutCardId);
    }
}