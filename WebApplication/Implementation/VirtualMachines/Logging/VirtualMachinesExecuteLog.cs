using System;
using System.Collections.Generic;
using System.Linq;
using Storage;
using WebApplication.Implementation.VirtualMachines.BusinessObjects;

namespace WebApplication.Implementation.VirtualMachines.Logging
{
    public class VirtualMachinesExecuteLog : IVirtualMachinesExecuteLog
    {
        private readonly IKeyValueStorage keyValueStorage;
        private const string ExecuteLogFileName = "virtualMachinesExecuteLog";

        public VirtualMachinesExecuteLog(IKeyValueStorage keyValueStorage)
        {
            this.keyValueStorage = keyValueStorage;
        }

        public void WriteLog(CommandExecuteResult executeResult)
        {
            var logs = keyValueStorage.Find<Dictionary<Guid, VirtualMachinesExecuteLogModel>>(ExecuteLogFileName) ?? new Dictionary<Guid, VirtualMachinesExecuteLogModel>();
            if (logs.ContainsKey(executeResult.Id))
            {
                var log = logs[executeResult.Id];
                log.LastUpdateTime = DateTime.Now;
                log.Execute = executeResult;
            }
            else
            {
                logs[executeResult.Id] = new VirtualMachinesExecuteLogModel
                {
                    Execute = executeResult,
                    CreateTime = DateTime.Now,
                    LastUpdateTime = DateTime.Now
                };
            }

            keyValueStorage.Write(ExecuteLogFileName, logs);
        }

        public VirtualMachinesExecuteLogModel FindLog(Guid executeId)
        {
            var result = keyValueStorage.Find<Dictionary<Guid, VirtualMachinesExecuteLogModel>>(ExecuteLogFileName) ?? new Dictionary<Guid, VirtualMachinesExecuteLogModel>();
            return result.ContainsKey(executeId) ? result[executeId] : null;
        }

        public VirtualMachinesExecuteLogModel[] SelectLastLogs(int count)
        {
            var result = keyValueStorage.Find<Dictionary<Guid, VirtualMachinesExecuteLogModel>>(ExecuteLogFileName) ?? new Dictionary<Guid, VirtualMachinesExecuteLogModel>();
            return result.Select(x => x.Value).OrderByDescending(x => x.CreateTime).Take(count).ToArray();
        }

        public void DeleteAll()
        {
            keyValueStorage.Write(ExecuteLogFileName, new Dictionary<Guid, VirtualMachinesExecuteLogModel>());
        }
    }
}