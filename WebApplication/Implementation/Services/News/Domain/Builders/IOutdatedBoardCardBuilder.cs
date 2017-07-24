using Infrastructure.Sugar;
using WebApplication.Implementation.Services.News.Domain.Models;

namespace WebApplication.Implementation.Services.News.Domain.Builders
{
    public interface IOutdatedBoardCardBuilder
    {
        Maybe<OutdatedBoardCardModel> TryBuildModel(string cardId);
    }
}