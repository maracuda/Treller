using System;

namespace SKBKontur.Treller.WebApplication.Fan.Activities
{
    public class ActivityItem
    {
        public ActivityItem(TimeSpan startTime, string leadingName, ActivityFormat format, string name = null, string description = null)
        {
            Name = name;
            Description = description;
            LeadingName = leadingName;
            Format = format;
            StartTime = startTime;
        }

        public TimeSpan StartTime { get; private set; }
        public TimeSpan FinishTime { get; set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string LeadingName { get; private set; }
        public ActivityFormat Format { get; private set; }

        public string ScheduleTime
        {
            get
            {
                return string.Format("{0:D2}:{1:D2} - {2:D2}:{3:D2}", StartTime.Hours, StartTime.Minutes, FinishTime.Hours, FinishTime.Minutes);
            }
        }
    }
}