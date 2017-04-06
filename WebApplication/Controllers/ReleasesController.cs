﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using SKBKontur.Treller.Logger;
using SKBKontur.Treller.WebApplication.Implementation.Services.Releases;

namespace SKBKontur.Treller.WebApplication.Controllers
{
    [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
    public class ReleasesController : ExceptionHandledController
    {
        private readonly IDemoPresentationsService demoPresentationsService;

        public ReleasesController(
            IDemoPresentationsService demoPresentationsService,
            ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            this.demoPresentationsService = demoPresentationsService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var presentations = demoPresentationsService.FetchPresentations(10);
            foreach (var presentationModel in presentations)
            {
                presentationModel.ImageUrl = Url.Action("DownloadContent", new RouteValueDictionary {{"presentationId", presentationModel.PresentationId}});
            }

            return View("Index", new DemoPresentationPageViewModel
            {
                Releases = presentations,
                Urls = new Dictionary<string, string>
                {
                    {
                        "saveComment", Url.Action("SaveComment")
                    }
                }
            });
        }

        [HttpGet]
        public ActionResult DownloadContent(Guid presentationId)
        {
            var content = demoPresentationsService.DownloadPresentationContent(presentationId);
            return new FileContentResult(content.Bytes, content.Type);
        }

        [HttpGet]
        public ActionResult FetchComments(Guid presentationId)
        {
            var comments = demoPresentationsService.FetchComments(presentationId);
            return Json(comments, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveComment(Guid presentationId, string name, string text)
        {
            var comment = demoPresentationsService.AppendComment(presentationId, name, text);
            return Json(comment);
        }
    }
}