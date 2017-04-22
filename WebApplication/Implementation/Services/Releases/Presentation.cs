using System;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Releases
{
    public class Presentation
    {
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}