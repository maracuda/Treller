using System;
using System.Management.Automation.Runspaces;

namespace SKBKontur.Treller.WebApplication.Services.VirtualMachines.Runspaces
{
    public interface IVirtualMachinesRunspacePool : IDisposable
    {
        Runspace OpenRunspace(string virtualMachineName);
    }
}