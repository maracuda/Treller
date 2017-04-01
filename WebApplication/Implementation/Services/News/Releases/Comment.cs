using System;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Releases
{
    public class Comment
    {
        public Guid CommentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Text { get; set; }
        public string AvatarUrl { get; set; }
        public DateTime CreateDate { get; set; }
    }
}