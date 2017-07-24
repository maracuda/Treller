using System;
using System.Linq;
using Infrastructure.Sugar;
using Logger;
using TaskManagerClient;
using WebApplication.Implementation.Services.News.Domain.Models;

namespace WebApplication.Implementation.Services.News.Domain.Builders
{
    public class OutdatedBoardCardBuilder : IOutdatedBoardCardBuilder
    {
        private readonly ITaskManagerClient taskManagerClient;
        private readonly ILoggerFactory loggerFactory;

        private static readonly TimeSpan cardExpirationPeriod = TimeSpan.FromDays(3);

        public OutdatedBoardCardBuilder(
            ITaskManagerClient taskManagerClient,
            ILoggerFactory loggerFactory)
        {
            this.taskManagerClient = taskManagerClient;
            this.loggerFactory = loggerFactory;
        }

        public Maybe<OutdatedBoardCardModel> TryBuildModel(string cardId)
        {
            try
            {
                var boardCard = taskManagerClient.GetCard(cardId);
                if (boardCard == null)
                    return null;

                var boardList = taskManagerClient.GetBoardLists(boardCard.BoardId).FirstOrDefault(x => x.Id == boardCard.BoardListId);
                if (boardList == null)
                {
                    loggerFactory.Get<OutdatedBoardCardBuilder>().LogError($"Fail to find board list with id {boardCard.BoardListId} at board {boardCard.BoardId} for card {cardId}.");
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
                loggerFactory.Get<OutdatedBoardCardBuilder>().LogError("Fail to build model due to network or integration errors.", e);
                return null;
            }
        }
    }
}