using System;
using SKBKontur.TaskManagerClient.BusinessObjects;
using SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.Models;
using SKBKontur.Treller.WebApplication.Services.Settings;

namespace SKBKontur.Treller.WebApplication.Blocks.Builders
{
    public class CardStateBuilder : ICardStateBuilder
    {
        public CardState GetState(string boardListId, BoardSettings setting, BoardList[] boardLists)
        {
            double developPosition = 0;
            BoardList currentList = null;
            foreach (var boardList in boardLists)
            {
                var isDevelop = string.Equals(boardList.Name, setting.DevelopListName, StringComparison.OrdinalIgnoreCase);
                if (isDevelop)
                {
                    developPosition = boardList.Position;
                }

                if (string.Equals(boardList.Id, boardListId, StringComparison.OrdinalIgnoreCase))
                {
                    if (isDevelop)
                    {
                        return CardState.Develop;
                    }

                    if (string.Equals(boardList.Name, setting.DevelopPresentationListName, StringComparison.OrdinalIgnoreCase))
                    {
                        return boardList.Position > developPosition ? CardState.Presentation : CardState.BeforeDevelop;
                    }

                    if (string.Equals(boardList.Name, setting.ReviewListName, StringComparison.OrdinalIgnoreCase))
                    {
                        return CardState.Review;
                    }

                    if (string.Equals(boardList.Name, setting.TestingListName, StringComparison.OrdinalIgnoreCase))
                    {
                        return CardState.Testing;
                    }

                    currentList = boardList;
                }
            }

            if (currentList != null && currentList.Position < developPosition)
            {
                return CardState.BeforeDevelop;
            }

            if (currentList != null && currentList.Position > developPosition)
            {
                return CardState.ReleaseWaiting;
            }

            return CardState.Archived;
        }
    }
}