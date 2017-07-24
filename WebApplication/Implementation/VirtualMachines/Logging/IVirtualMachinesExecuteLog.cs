using System;
using WebApplication.Implementation.VirtualMachines.BusinessObjects;

namespace WebApplication.Implementation.VirtualMachines.Logging
{
    public interface IVirtualMachinesExecuteLog
    {
        void WriteLog(CommandExecuteResult result);
        VirtualMachinesExecuteLogModel FindLog(Guid executeId);
        VirtualMachinesExecuteLogModel[] SelectLastLogs(int count);
        void DeleteAll();
    }
}