using System;
using SKBKontur.Treller.WebApplication.Implementation.VirtualMachines.BusinessObjects;

namespace SKBKontur.Treller.WebApplication.Implementation.VirtualMachines.Logging
{
    public interface IVirtualMachinesExecuteLog
    {
        void WriteLog(CommandExecuteResult result);
        VirtualMachinesExecuteLogModel FindLog(Guid executeId);
        VirtualMachinesExecuteLogModel[] SelectLastLogs(int count);
        void DeleteAll();
    }
}