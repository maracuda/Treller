using System;
using WebApplication.Implementation.VirtualMachines.BusinessObjects;

namespace WebApplication.Implementation.VirtualMachines.Logging
{
    public class VirtualMachinesExecuteLogModel
    {
        public Guid ExecuteId { get { return Execute.Id; } }
        public DateTime CreateTime { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public CommandExecuteResult Execute { get; set; }
    }
}