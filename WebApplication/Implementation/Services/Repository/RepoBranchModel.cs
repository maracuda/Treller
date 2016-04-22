using System;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Repository
{
    public class RepoBranchModel
    {
        public string Name { get; set; }
        public bool IsReleased { get; set; }
        public DateTime LastCommitTime { get; set; }
        public string CommitTime => LastCommitTime.ToString("dd.MM HH:mm");
    }
}