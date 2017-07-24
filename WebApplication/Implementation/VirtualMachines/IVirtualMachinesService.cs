using System;
using WebApplication.Implementation.VirtualMachines.BusinessObjects;

namespace WebApplication.Implementation.VirtualMachines
{
    public interface IVirtualMachinesService
    {
        Guid ExecuteCommandAsync(string command, string login, string[] virtualMachineNames);
        CommandExecuteResult GetExecuteCommandProgress(Guid executeId);
        void RefreshExecuteCommandProgress(Guid executeId);
    }
}