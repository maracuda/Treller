using System;
using System.Linq;
using SKBKontur.TaskManagerClient.BusinessObjects.TaskManager;
using SKBKontur.Treller.WebApplication.Implementation.Services.Settings;
using SKBKontur.Treller.WebApplication.Implementation.TaskDetalization.BusinessObjects.Models;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.TaskManager
{
    public class CardStateBuilder : ICardStateBuilder
    {
        public CardState GetState(string boardListId, KanbanBoardMetaInfo metaInfo, BoardList[] boardLists)
        {
            var developList = boardLists.FirstOrDefault(list => IsInState(list.Name, metaInfo.DevelopListName));
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

            if (IsInState(boardList.Name, metaInfo.DevelopPresentationListName))
            {
                return boardList.Position > developList.Position 
                            ? CardState.Presentation
                            : CardState.AnalitycPresentation;
            }

            if (IsInState(boardList.Name, metaInfo.ReviewListName))
            {
                return CardState.Review;
            }

            if (IsInState(boardList.Name, metaInfo.TestingListName))
            {
                return CardState.Testing;
            }

            if (IsInState(boardList.Name, metaInfo.WaitForReleaseListName))
            {
                return CardState.ReleaseWaiting;
            }

            if (IsInState(boardList.Name, metaInfo.AnalyticListName))
            {
                return CardState.Analityc;
            }

            if (boardList.Position < developList.Position)
            {
                return CardState.BeforeDevelop;
            }

            if (boardList.Position > developList.Position)
            {
                return CardState.Released;
            }

            return CardState.Archived;
        }

        private static bool IsInState(string listName, string stateName)
        {
            return !string.IsNullOrEmpty(stateName) && listName.StartsWith(stateName, StringComparison.OrdinalIgnoreCase);
        }
    }
}