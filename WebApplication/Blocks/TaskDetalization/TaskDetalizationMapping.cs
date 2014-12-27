using System;
using System.Collections.Generic;
using SKBKontur.Billy.Core.BlocksMapping.Abstrations;
using SKBKontur.Billy.Core.BlocksMapping.Mappings;
using SKBKontur.TaskManagerClient.BusinessObjects;
using SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.Blocks;
using SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.Blocks.Parts;
using SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.Models;
using SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.ViewModels;
using SKBKontur.Billy.Core.BlocksMapping.BlockExtenssions;
using System.Linq;

namespace SKBKontur.Treller.WebApplication.Blocks.TaskDetalization
{
    public class TaskDetalizationMapping : IContextBlocksMapping
    {
        // TODO: Move to builder
        private static readonly IBlockMapper[] Mappers =
        {
            BlockMapper.Declare<CardAvatarBlock, Dictionary<bool, UserAvatarViewModel[]>, UserAvatarViewModel[]>(x => x.SuggestedActiveAvatars, x => GetAvatars(x, true)),
            BlockMapper.Declare<CardAvatarBlock, Dictionary<bool, UserAvatarViewModel[]>, UserAvatarViewModel[]>(x => x.SuggestedNotActiveAvatars, x => GetAvatars(x, false)),
                    
            BlockMapper.Declare<CardLabelsBlock, BoardCard, CardLabel[]>(x => x.Labels, x => x.Labels),

            BlockMapper.Declare<CardNameBlock, BoardCard, string>(x => x.OriginalName, x => x.Name),
            BlockMapper.Declare<CardNameBlock, BoardCard, string>(x => x.ApplicationName, x => x.Name + "(В будущем монжо менять)"),
            BlockMapper.Declare<CardNameBlock, BoardCard, string>(x => x.CardUrl, x => x.Url),
            BlockMapper.Declare<CardNameBlock, BoardCard, string>(x => x.ControlVersionSystemBranchName, x => GetCardBrunchName(x)),
            BlockMapper.Declare<CardNameBlock, string>(x => x.ControlVersionSystemBranchUrl, value: "Can't retrieve branchUrl"),
                    
            BlockMapper.Declare<CardStateBlock, CardState>(x => x.State),
            BlockMapper.Declare<CardStateBlock, BoardCard, DateTime?>(x => x.DueDate, x => x.DueDate),
            BlockMapper.Declare<CardStateBlock, CardStateInfo, DateTime>(x => x.BeginDate, x => x.States[x.CurrentState].BeginDate),
            BlockMapper.Declare<CardStateBlock, CardStateInfo, int>(x => x.DueDays, x => (DateTime.Now.Date - x.States[x.CurrentState].BeginDate.Date).Days),

            BlockMapper.Declare<CardDetalizationPartsBlock, CardArchivePartBlock>(x => x.Archive),
            BlockMapper.Declare<CardDetalizationPartsBlock, CardBeforeDevelopPartBlock>(x => x.BeforeDevelop),
            BlockMapper.Declare<CardDetalizationPartsBlock, CardDevelopPartBlock>(x => x.Develop),
            BlockMapper.Declare<CardDetalizationPartsBlock, CardPresentationPartBlock>(x => x.Presentation),
            BlockMapper.Declare<CardDetalizationPartsBlock, CardReleaseWaitingPartBlock>(x => x.ReleaseWaiting),
            BlockMapper.Declare<CardDetalizationPartsBlock, CardReviewPartBlock>(x => x.Review),
            BlockMapper.Declare<CardDetalizationPartsBlock, CardTestingPartBlock>(x => x.Testing),
        };

        private static string GetCardBrunchName(BoardCard card)
        {
            int branchIndex;
            if (!string.IsNullOrEmpty(card.Description) && (branchIndex = card.Description.IndexOf("ветка", StringComparison.OrdinalIgnoreCase)) > 0)
            {
                return new string(card.Description.Skip(branchIndex).Take(15).ToArray());
            }

            return "Can't retrieve branchName";
        }

        private static UserAvatarViewModel[] GetAvatars(Dictionary<bool, UserAvatarViewModel[]> users, bool isActive)
        {
            return users.SafeGet(isActive, new UserAvatarViewModel[0]);
        }

        public IBlockMapper[] SelectAll()
        {
            return Mappers;
        }

        public string GetContextKey()
        {
            return ContextKeys.TaskDetalizationKey;
        }
    }
}