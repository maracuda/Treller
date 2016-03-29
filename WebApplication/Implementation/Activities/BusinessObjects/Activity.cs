using System.Linq;
using Microsoft.Ajax.Utilities;

namespace SKBKontur.Treller.WebApplication.Implementation.Activities.BusinessObjects
{
    public class Activity
    {
        private readonly string name;
        private readonly string description;

        public Activity(string name = null, string description = null, params ActivityItem[] items)
        {
            this.name = name;
            this.description = description;
            Items = items;
        }

        public string Name { get { return name ?? (Items ?? new ActivityItem[0]).FirstOrDefault().IfNotNull(x => x.Name); } }
        public string Description { get { return description ?? (Items ?? new ActivityItem[0]).FirstOrDefault().IfNotNull(x => x.Description); } }

        public ActivityItem[] Items { get; set; }
        public ActivityItem FirstItem { get { return Items[0]; } }
        public bool IsSimpleActivity { get { return Items == null || Items.Length == 1; } }
    }
}