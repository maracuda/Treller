using System;
using System.Collections.Generic;

namespace SKBKontur.Treller.WebApplication.Services.VirtualMachines.BusinessObjects
{
    public class CommandExecute
    {
        public Guid Id { get; set; }
        public string Command { get; set; }
        public string Login { get; set; }
        public Dictionary<string, AsyncPowershell> ProgressInfo { get; set; }
    }
}