using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Security;

namespace WebApplication.Implementation.VirtualMachines.Runspaces
{
    public class VirtualMachinesRunspacePool : IVirtualMachinesRunspacePool
    {
        private static readonly Dictionary<string, Runspace> OpenedRunspaces = new Dictionary<string, Runspace>(50, StringComparer.OrdinalIgnoreCase);

        public Runspace OpenRunspace(string virtualMachineName)
        {
            if (OpenedRunspaces.ContainsKey(virtualMachineName))
            {
                return OpenedRunspaces[virtualMachineName];
            }

            var result = RunspaceFactory.CreateRunspace(CreateConnectionInfo(virtualMachineName));
            result.Open();
            OpenedRunspaces[virtualMachineName] = result;
            return result;
        }

        private static WSManConnectionInfo CreateConnectionInfo(string virtualMachineName)
        {
            var password = new SecureString();
            foreach (var passChar in "tc_123456".ToCharArray())
            {
                password.AppendChar(passChar);
            }
            password.MakeReadOnly();

            return new WSManConnectionInfo(new Uri(string.Format("http://{0}:5985/wsman", virtualMachineName)), "http://schemas.microsoft.com/powershell/Microsoft.PowerShell", new PSCredential(virtualMachineName + @"\tc", password))
            {
                AuthenticationMechanism = AuthenticationMechanism.Negotiate,
                MaximumConnectionRedirectionCount = 1,
                OperationTimeout = 4 * 60 * 1000,
                OpenTimeout = 1 * 60 * 1000
            };
        }

        public void Dispose()
        {
            foreach (var runspace in OpenedRunspaces.Values)
            {
                runspace.Dispose();
            }
        }
    }
}