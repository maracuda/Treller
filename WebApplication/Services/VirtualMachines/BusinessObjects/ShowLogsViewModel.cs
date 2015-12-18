using SKBKontur.Treller.WebApplication.Services.VirtualMachines.Logging;

namespace SKBKontur.Treller.WebApplication.Services.VirtualMachines.BusinessObjects
{
    public class ShowLogsViewModel
    {
        public CommandExecuteResult CurrentResult { get; set; }
        public VirtualMachinesExecuteLogModel[] Logs { get; set; }
    }
}