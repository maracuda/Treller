using System;
using System.Linq;
using SKBKontur.Infrastructure.Sugar;
using SKBKontur.TaskManagerClient;
using SKBKontur.Treller.WebApplication.Implementation.Services.ErrorService;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Domain.Models;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Domain.Builders
{
    public class OutdatedBoardCardBuilder : IOutdatedBoardCardBuilder
    {
        private readonly ITaskManagerClient taskManagerClient;
        private readonly IErrorService errorService;

        private static readonly TimeSpan cardExpirationPeriod = TimeSpan.FromDays(3);

        public OutdatedBoardCardBuilder(
            ITaskManagerClient taskManagerClient,
            IErrorService errorService)
        {
            this.taskManagerClient = taskManagerClient;
            this.errorService = errorService;
        }

        public Maybe<OutdatedBoardCardModel> TryBuildModel(string cardId)
        {
            try
            {
                var boardCard = taskManagerClient.GetCard(cardId);
                var boardList = taskManagerClient.GetBoardLists(boardCard.BoardId)
                                                 .FirstOrDefault(x => x.Id == boardCard.BoardListId);
                if (boardList == null)
                {
                    errorService.SendError($"Fail to find board list with id {boardCard.BoardListId} at board {boardCard.BoardId} for card {cardId}.");
                    return null;
                }

                return new OutdatedBoardCardModel
                {
                    CardId = cardId,
                    IsArchived = boardCard.IsArchived,
                    BoardListName = boardList.Name,
                    LastActivity = boardCard.LastActivity,
                    ExpirationPeriod = cardExpirationPeriod
                };
            }
            catch (Exception e)
            {
                errorService.SendError("Fail to build model due to network or integration errors.", e);
                return null;
            }
        }
    }
}