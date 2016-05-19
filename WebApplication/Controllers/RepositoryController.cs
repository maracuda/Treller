using System;
using System.Collections.Generic;
using System.Web.Mvc;
using SKBKontur.Treller.WebApplication.Implementation.Repository;
using SKBKontur.Treller.WebApplication.Implementation.Services.Notifications;

namespace SKBKontur.Treller.WebApplication.Controllers
{
    public class RepositoryController : Controller
    {
        private readonly IOldBranchesModelBuilder oldBranchesModelBuilder;
        private readonly INotificationService notificationService;

        public RepositoryController(
            IOldBranchesModelBuilder oldBranchesModelBuilder,
            INotificationService notificationService)
        {
            this.oldBranchesModelBuilder = oldBranchesModelBuilder;
            this.notificationService = notificationService;
        }

        public ActionResult Index()
        {
            var oldBranchesModel = oldBranchesModelBuilder.Build(TimeSpan.FromDays(30), TimeSpan.FromDays(60));
            return View("Index", oldBranchesModel);
        }

        public ActionResult NotifyCommitersAboutOldBranches()
        {
            var commiterIndex = new Dictionary<string, List<string>>();
            var oldBranchesModel = oldBranchesModelBuilder.Build(TimeSpan.FromDays(30), TimeSpan.FromDays(60));
            foreach (var veryOldBranch in oldBranchesModel.VeryOldBracnhes)
            {
                if (!commiterIndex.ContainsKey(veryOldBranch.Commit.Committer_email))
                {
                    commiterIndex.Add(veryOldBranch.Commit.Committer_email, new List<string>());
                }
                commiterIndex[veryOldBranch.Commit.Committer_email].Add(veryOldBranch.Name);
            }

            foreach (var emailToBranchesIndex in commiterIndex)
            {
                var body = "Дорогой разработчик!\r\n\r\n" +
                           $"Спешу сообщить тебе, что у нас в репозитории есть очень старые ветки: {string.Join(",", emailToBranchesIndex.Value)}.\r\n" +
                           "По воле случая ты был последним, кто коммитил в эту ветку/и.\r\n" +
                           "Пожалуйста, посмотри нельзя ли закрыть эти ветки (репозиторию очень тяжело от большого количества веток).\r\n\r\n" +
                           "С любовью твой автоматический уведомлятор.";
                notificationService.SendMessage("hvorost@skbkontur.ru", "Уведомление о старых ветках", body, false, "hvorost@skbkontur.ru");
            }

            return RedirectToAction("Index");
        }
    }
}