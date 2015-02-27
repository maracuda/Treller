using System;
using SKBKontur.TaskManagerClient.BusinessObjects;
using SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.Models;
using SKBKontur.Treller.WebApplication.Services.Settings;
using System.Linq;

namespace SKBKontur.Treller.WebApplication.Blocks.Builders
{
    public class CardStateBuilder : ICardStateBuilder
    {
        public CardState GetState(string boardListId, BoardSettings setting, BoardList[] boardLists)
        {
            var developList = boardLists.FirstOrDefault(list => string.Equals(list.Name, setting.DevelopListName, StringComparison.OrdinalIgnoreCase));
            if (developList == null)
            {
                return CardState.BeforeDevelop;
            }

            if (string.Equals(boardListId, developList.Id, StringComparison.OrdinalIgnoreCase))
            {
                return CardState.Develop;
            }

            var boardList = boardLists.FirstOrDefault(x => string.Equals(x.Id, boardListId, StringComparison.OrdinalIgnoreCase));
            if (boardList == null)
            {
                return CardState.Archived;
            }

            if (string.Equals(boardList.Name, setting.DevelopPresentationListName, StringComparison.OrdinalIgnoreCase))
            {
                return boardList.Position > developList.Position 
                            ? CardState.Presentation
                            : CardState.AnalitycPresentation;
            }

            if (string.Equals(boardList.Name, setting.ReviewListName, StringComparison.OrdinalIgnoreCase))
            {
                return CardState.Review;
            }

            if (string.Equals(boardList.Name, setting.TestingListName, StringComparison.OrdinalIgnoreCase))
            {
                return CardState.Testing;
            }

            if (string.Equals(boardList.Name, setting.AnalyticListName, StringComparison.OrdinalIgnoreCase))
            {
                return CardState.Analityc;
            }

            if (boardList.Position < developList.Position)
            {
                return CardState.BeforeDevelop;
            }

            if (boardList.Position > developList.Position)
            {
                return CardState.ReleaseWaiting;
            }

            return CardState.Archived;
        }
    }
}