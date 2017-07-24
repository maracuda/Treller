using System;

namespace WebApplication.Implementation.Services.Releases
{
    public class PresentationModel
    {
        public Guid PresentationId { get; set; }
        public DateTime CreateDate { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public Comment[] Comments { get; set; }
    }
}