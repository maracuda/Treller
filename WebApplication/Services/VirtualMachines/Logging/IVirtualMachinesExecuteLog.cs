using System;
using SKBKontur.Treller.WebApplication.Services.VirtualMachines.BusinessObjects;

namespace SKBKontur.Treller.WebApplication.Services.VirtualMachines.Logging
{
    public interface IVirtualMachinesExecuteLog
    {
        void WriteLog(CommandExecuteResult result);
        VirtualMachinesExecuteLogModel FindLog(Guid executeId);
        VirtualMachinesExecuteLogModel[] SelectLastLogs(int count);
        void DeleteAll();
    }
}