using System;
using System.Collections.Generic;
using System.Linq;

namespace SKBKontur.TaskManagerClient.Youtrack.BusinessObjects
{
    public class YouTrackIssue
    {
        private Dictionary<string, object> fields;
        private static readonly DateTime Date1970 = new DateTime(1970, 1, 1);

        public string Id { get; set; }
        public YouTrackIssueField[] Field { get; set; }
        public YouTrackComment[] Comment { get; set; }

        public T SafeGet<T>(string key)
        {
            fields = fields ?? Field.ToDictionary(x => x.Name, x => x.Value);

            object result;
            if (fields.TryGetValue(key, out result) && result is T)
            {
                return (T) result;
            }

            return default(T);
        }

        public DateTime? SafeGetDateFromMilleseconds(string key)
        {
            var keyValue = SafeGet<string>(key);
            long pastMilleseconds;
            if (string.IsNullOrEmpty(keyValue) || !long.TryParse(keyValue, out pastMilleseconds))
            {
                return null;
            }
            return Date1970.AddMilliseconds(pastMilleseconds);
        }
    }
}