using System;
using System.Collections.Generic;
using System.Web.Mvc;
using SKBKontur.Treller.Logger;
using SKBKontur.Treller.WebApplication.Implementation.Services.Releases;

namespace SKBKontur.Treller.WebApplication.Controllers
{
    public class ReleasesController : ExceptionHandledController
    {

        public ReleasesController(
            ILoggerFactory loggerFactory) : base(loggerFactory)
        {
        }

        [HttpGet]
        public ActionResult Index()
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
            return View("Index", new ReleasesPageViewModel
            {
                Releases = releases,
                Urls = new Dictionary<string, string>
                {
                    {
                        "saveComment", Url.Action("SaveComment")
                    }
                }
            });
        }

        [HttpPost]
        public ActionResult SaveComment(Guid releaseId, string name, string text)
        {
            var savedComment = new Comment
            {
                CommentId = Guid.NewGuid(),
                CreateDate = DateTime.Now,
                Name = name,
                Text = text
            };

            return Json(savedComment);
        }
    }
}