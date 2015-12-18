using System;
using SKBKontur.Treller.WebApplication.Services.VirtualMachines.BusinessObjects;

namespace SKBKontur.Treller.WebApplication.Services.VirtualMachines.Services
{
    public interface IVirtualMachinesService
    {
        Guid ExecuteCommandAsync(string command, string login, string[] virtualMachineNames);
        CommandExecuteResult GetExecuteCommandProgress(Guid executeId);
        void RefreshExecuteCommandProgress(Guid executeId);
    }
}