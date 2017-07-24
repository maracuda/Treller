using System;
using System.Management.Automation.Runspaces;

namespace WebApplication.Implementation.VirtualMachines.Runspaces
{
    public interface IVirtualMachinesRunspacePool : IDisposable
    {
        Runspace OpenRunspace(string virtualMachineName);
    }
}