using System;

namespace SKBKontur.Treller.WebApplication.Services.VirtualMachines.BusinessObjects
{
    public class CommandExecuteResult
    {
        public Guid Id { get; set; }
        public string Command { get; set; }
        public CommandExecuteVirtualMachineResult[] MachineResults { get; set; }
        public string Login { get; set; }
    }
}