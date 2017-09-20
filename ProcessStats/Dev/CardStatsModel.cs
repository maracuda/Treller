using System;
using System.Collections.Generic;
using System.Linq;
using TaskManagerClient.BusinessObjects.TaskManager;

namespace ProcessStats.Dev
{
    public class CardStatsModel
    {
        private HashSet<CardLabel> labelsSet;

        public CardStatsModel()
        {
            ListStats = new Dictionary<string, TimeSpan>();
            labelsSet = new HashSet<CardLabel>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public CardSize Size { get; set; }

        public Dictionary<string, TimeSpan> ListStats { get; }

        public TimeSpan CycleTime => ListStats.Values.Aggregate(TimeSpan.Zero, (current, val) => current.Add(val));

        public void SetLabels(CardLabel[] labels)
        {
            labelsSet = new HashSet<CardLabel>(labels);
            Size = CardSizeParser.TryParse(labelsSet);
        }

        public bool HasLabel(CardLabel label)
        {
            return labelsSet.Contains(label);
        }
    }
}