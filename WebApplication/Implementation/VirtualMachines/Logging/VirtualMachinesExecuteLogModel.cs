using System;
using SKBKontur.Treller.WebApplication.Implementation.VirtualMachines.BusinessObjects;

namespace SKBKontur.Treller.WebApplication.Implementation.VirtualMachines.Logging
{
    public class VirtualMachinesExecuteLogModel
    {
        public Guid ExecuteId { get { return Execute.Id; } }
        public DateTime CreateTime { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public CommandExecuteResult Execute { get; set; }
    }
}