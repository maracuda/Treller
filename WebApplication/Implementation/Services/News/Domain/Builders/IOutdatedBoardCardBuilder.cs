using SKBKontur.Infrastructure.Sugar;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Domain.Models;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Domain.Builders
{
    public interface IOutdatedBoardCardBuilder
    {
        Maybe<OutdatedBoardCardModel> TryBuildModel(string cardId);
    }
}