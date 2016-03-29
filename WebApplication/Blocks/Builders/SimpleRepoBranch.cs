using System;

namespace SKBKontur.Treller.WebApplication.Blocks.Builders
{
    public class SimpleRepoBranch
    {
        public string Name { get; set; }
        public string CommitTime { get { return LastCommitTime.ToString("dd.MM HH:mm"); } }
        public bool IsReleased { get; set; }

        // todo: возможно убрать
        public DateTime LastCommitTime { get; set; }
    }
}