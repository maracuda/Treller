using System;
using SKBKontur.Treller.WebApplication.Implementation.VirtualMachines.BusinessObjects;

namespace SKBKontur.Treller.WebApplication.Implementation.VirtualMachines.Services
{
    public interface IVirtualMachinesService
    {
        Guid ExecuteCommandAsync(string command, string login, string[] virtualMachineNames);
        CommandExecuteResult GetExecuteCommandProgress(Guid executeId);
        void RefreshExecuteCommandProgress(Guid executeId);
    }
}