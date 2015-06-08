using System.Collections.Generic;
using System.Linq;
using SKBKontur.BlocksMapping.Attributes;
using SKBKontur.TaskManagerClient.BusinessObjects;
using SKBKontur.Treller.WebApplication.Blocks.Builders;
using SKBKontur.Treller.WebApplication.Blocks.PeopleLoadPool.Models;
using SKBKontur.Treller.WebApplication.Blocks.PeopleLoadPool.ViewModels;
using SKBKontur.Treller.WebApplication.Services.Settings;

namespace SKBKontur.Treller.WebApplication.Blocks.PeopleLoadPool.Builders
{
    public class PeopleLoadPoolBuilders
    {
        private readonly ISettingService settingService;
        private readonly IUserAvatarViewModelBuilder userAvatarViewModelBuilder;

        public PeopleLoadPoolBuilders(ISettingService settingService, IUserAvatarViewModelBuilder userAvatarViewModelBuilder)
        {
            this.settingService = settingService;
            this.userAvatarViewModelBuilder = userAvatarViewModelBuilder;
        }

        [BlockModel(ContextKeys.PeopleLoadPoolKey)]
        public Dictionary<string, BoardSettings> BuildSettings(PeopleLoadPoolEnterModel enterModel)
        {
            if (enterModel == null || enterModel.BoardIds == null || enterModel.BoardIds.Length == 0)
            {
                return settingService.GetDevelopingBoards().ToDictionary(x => x.Id);
            }

            return settingService.GetDevelopingBoards().Where(x => enterModel.BoardIds.Contains(x.Id)).ToDictionary(x => x.Id);
        }

//        [BlockModel(ContextKeys.PeopleLoadPoolKey)]
//        public PeopleSimpleViewModel[] BuildPeoples(Dictionary<string, User> taskManagerUsers)
//        {
//            return taskManagerUsers
//                    .Select(x => new PeopleSimpleViewModel
//                                     {
//                                         UserAvatar = userAvatarViewModelBuilder.Build(x.Value)
//                                     })
//                    .ToArray();
//        }
    }
}