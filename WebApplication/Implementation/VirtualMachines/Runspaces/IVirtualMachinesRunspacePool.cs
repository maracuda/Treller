using System;
using System.Management.Automation.Runspaces;

namespace SKBKontur.Treller.WebApplication.Implementation.VirtualMachines.Runspaces
{
    public interface IVirtualMachinesRunspacePool : IDisposable
    {
        Runspace OpenRunspace(string virtualMachineName);
    }
}