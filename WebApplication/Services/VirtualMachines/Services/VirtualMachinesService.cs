using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using SKBKontur.Treller.WebApplication.Services.VirtualMachines.BusinessObjects;
using SKBKontur.Treller.WebApplication.Services.VirtualMachines.Logging;
using SKBKontur.Treller.WebApplication.Services.VirtualMachines.Runspaces;

namespace SKBKontur.Treller.WebApplication.Services.VirtualMachines.Services
{
    public class VirtualMachinesService : IVirtualMachinesService
    {
        private readonly IVirtualMachinesRunspacePool virtualMachinesRunspacePool;
        private readonly IVirtualMachinesExecuteLog virtualMachinesExecuteLog;
        private static readonly Dictionary<Guid, CommandExecute> Executes = new Dictionary<Guid, CommandExecute>();

        public VirtualMachinesService(IVirtualMachinesRunspacePool virtualMachinesRunspacePool, IVirtualMachinesExecuteLog virtualMachinesExecuteLog)
        {
            this.virtualMachinesRunspacePool = virtualMachinesRunspacePool;
            this.virtualMachinesExecuteLog = virtualMachinesExecuteLog;
        }

        public Guid ExecuteCommandAsync(string command, string login, string[] virtualMachineNames)
        {
            var executeId = Guid.NewGuid();
            var powerShellResults = new Dictionary<string, AsyncPowershell>();
            foreach (var virtualMachine in virtualMachineNames)
            {
                var powerShell = PowerShell.Create();
                powerShell.Runspace = virtualMachinesRunspacePool.OpenRunspace(virtualMachine);
                powerShell.AddScript(command);
                powerShellResults[virtualMachine] = new AsyncPowershell(powerShell);
            }
            Executes.Add(executeId, new CommandExecute { Id = executeId, ProgressInfo = powerShellResults, Command = command, Login = login });

            return executeId;
        }

        public CommandExecuteResult GetExecuteCommandProgress(Guid executeId)
        {
            if (!Executes.ContainsKey(executeId))
            {
                return null;
            }

            var commandExecute = Executes[executeId];

            var machineResults = new Dictionary<string, string[]>();
            var machines = commandExecute.ProgressInfo.Select(x => x.Key).ToArray();
            foreach (var virtualMachine in machines)
            {
                var myPowershell = commandExecute.ProgressInfo[virtualMachine];
                if (!myPowershell.IsFinished())
                {
                    machineResults[virtualMachine] = new [] { "Is running", "" };
                    continue;
                }

                try
                {
                    machineResults[virtualMachine] = myPowershell.GetFormattedResult();
                }
                catch (Exception ex)
                {
                    machineResults[virtualMachine] = new [] { "", ex.Message };
                }
                finally
                {
                    myPowershell.Dispose();
                }
            }

            var result = new CommandExecuteResult
            {
                Id = commandExecute.Id,
                Command = commandExecute.Command,
                Login = commandExecute.Login,
                MachineResults = machines.Select(x => new CommandExecuteVirtualMachineResult
                {
                    MachineName = x,
                    ResultText = machineResults[x][0],
                    ErrorText = machineResults[x][1],
                    IsFinished = commandExecute.ProgressInfo[x].IsFinished(),
                    IsSuccess = commandExecute.ProgressInfo[x].IsSuccess()
                }).ToArray()
            };

            virtualMachinesExecuteLog.WriteLog(result);

            return result;
        }

        public void RefreshExecuteCommandProgress(Guid executeId)
        {
            GetExecuteCommandProgress(executeId);
        }
    }
}