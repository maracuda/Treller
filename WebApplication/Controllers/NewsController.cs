using System;
using System.Web.Mvc;
using SKBKontur.Treller.Logger;
using SKBKontur.Treller.WebApplication.Implementation.Services.News;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.NewsFeed;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Releases;

namespace SKBKontur.Treller.WebApplication.Controllers
{
    public class NewsController : ExceptionHandledController
    {
        private readonly IBillingTimes billingTimes;

        public NewsController(
            IBillingTimes billingTimes,
            ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            this.billingTimes = billingTimes;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var news = new NewsViewModel
            {
                TaskNews = billingTimes.SelectAll()
            };
            return View("Index", news);
        }

        [HttpGet]
        public ActionResult Releases()
        {
            var releases = new[]
            {
                new Release
                {
                    ReleaseId = Guid.NewGuid(),
                    CreateDate = DateTime.Now,
                    Title = "Заголовок 1",
                    Content = "Описание релиза",
                    ImageUrl = null,
                    Comments = new []
                    {
                        new Comment
                        {
                            CommentId = Guid.NewGuid(),
                            Name = "Айбелив Айкенфлаев",
                            CreateDate = DateTime.Now,
                            Text = "The path of a cosmonaut is not an easy, triumphant march to glory. You have to get to know the meaning not just of joy but also of grief, before being allowed in the spacecraft cabin."
                        },
                        new Comment
                        {
                            CommentId = Guid.NewGuid(),
                            Name = "Иван Диван",
                            CreateDate = DateTime.Now,
                            Text = "The path of a cosmonaut is not an easy, triumphant march to glory. You have to get to know the meaning not just of joy but also of grief, before being allowed in the spacecraft cabin."
                        }
                    }
                },
                new Release
                {
                    ReleaseId = Guid.NewGuid(),
                    CreateDate = DateTime.Now,
                    Title = "Очень длинный заголовок очень длинный заголовок Очень длинный заголовок очень длинный заголовок Очень длинный заголовок очень длинный заголовок",
                    Content = "Очень длинное описание релиза Очень длинное описание релиза Очень длинное описание релиза Очень длинное описание релиза Очень длинное описание релиза Очень длинное описание релиза",
                    ImageUrl = "https://media.giphy.com/media/jd6TVgsph6w7e/giphy.gif"
                }
            };
            return View("Releases", new ReleasesPageViewModel
            {
                Releases = releases
            });
        }

        [HttpGet]
        public ActionResult GoToDetails(string taskId)
        {
            var taskNewModel = billingTimes.Read(taskId);
            return View("New", taskNewModel);
        }

        public ActionResult Publish(string taskId, PublishStrategy publishStrategy)
        {
            billingTimes.Publish(taskId, publishStrategy);
            return RedirectToAction("Index");
        }

        public ActionResult TryToRequestNew(string aboutCardId)
        {
            billingTimes.TryToRequestNew(aboutCardId);
            return RedirectToAction("Index");
        }
    }

    public class NewsViewModel
    {
        public TaskNewModel[] TaskNews { get; set; }
    }
}