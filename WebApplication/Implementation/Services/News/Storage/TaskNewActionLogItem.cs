using System;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Storage
{
    internal class TaskNewActionLogItem
    {
        private TaskNewActionLogItem() { }

        public string PrimaryKey { get; set; }
        public TaskNewActionTypes ActionType { get; set; }
        public string Diff { get; set; }
        public DateTime UtcActionDateTime { get; set; }

        public static TaskNewActionLogItem Create(string primaryKey, DateTime utcActionDateTime)
        {
            return new TaskNewActionLogItem
            {
                PrimaryKey = primaryKey,
                ActionType = TaskNewActionTypes.Create,
                Diff = string.Empty,
                UtcActionDateTime = utcActionDateTime
            };
        }

        public static TaskNewActionLogItem Update(string primaryKey, string diff, DateTime utcActionDateTime)
        {
            return new TaskNewActionLogItem
            {
                PrimaryKey = primaryKey,
                ActionType = TaskNewActionTypes.Update,
                Diff = diff,
                UtcActionDateTime = utcActionDateTime
            };
        }

        public static TaskNewActionLogItem Delete(string primaryKey, DateTime utcActionDateTime)
        {
            return new TaskNewActionLogItem
            {
                PrimaryKey = primaryKey,
                ActionType = TaskNewActionTypes.Delete,
                Diff = string.Empty,
                UtcActionDateTime = utcActionDateTime
            };
        }
    }
}