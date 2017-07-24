using System;

namespace TaskManagerClient.Youtrack.BusinessObjects
{
    public class Issue
    {
        public string Id { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public DateTime? Resolved { get; set; }
        public string CreatorLogin { get; set; }
        public string CreatorFullName { get; set; }
        public int CommentsCount { get; set; }
        public string LastComment { get; set; }
    }
}