using System;

namespace WebApplication.Implementation.Services.Releases
{
    public class Comment
    {
        public Guid PresentationId { get; set; }
        public Guid CommentId { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public string AvatarUrl { get; set; }
        public DateTime CreateDate { get; set; }
    }
}