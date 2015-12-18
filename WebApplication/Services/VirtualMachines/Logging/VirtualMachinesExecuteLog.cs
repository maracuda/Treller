using System;
using System.Collections.Generic;
using System.Linq;
using SKBKontur.BlocksMapping.BlockExtenssions;
using SKBKontur.Treller.WebApplication.Services.VirtualMachines.BusinessObjects;
using SKBKontur.Treller.WebApplication.Storages;

namespace SKBKontur.Treller.WebApplication.Services.VirtualMachines.Logging
{
    public class VirtualMachinesExecuteLog : IVirtualMachinesExecuteLog
    {
        private readonly ICachedFileStorage cachedFileStorage;
        private const string ExecuteLogFileName = "virtualMachinesExecuteLog";

        public VirtualMachinesExecuteLog(ICachedFileStorage cachedFileStorage)
        {
            this.cachedFileStorage = cachedFileStorage;
        }

        public void WriteLog(CommandExecuteResult executeResult)
        {
            var logs = cachedFileStorage.Find<Dictionary<Guid, VirtualMachinesExecuteLogModel>>(ExecuteLogFileName) ?? new Dictionary<Guid, VirtualMachinesExecuteLogModel>();
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

            cachedFileStorage.Write(ExecuteLogFileName, logs);
        }

        public VirtualMachinesExecuteLogModel FindLog(Guid executeId)
        {
            var result = cachedFileStorage.Find<Dictionary<Guid, VirtualMachinesExecuteLogModel>>(ExecuteLogFileName) ?? new Dictionary<Guid, VirtualMachinesExecuteLogModel>();
            return result.SafeGet(executeId);
        }

        public VirtualMachinesExecuteLogModel[] SelectLastLogs(int count)
        {
            var result = cachedFileStorage.Find<Dictionary<Guid, VirtualMachinesExecuteLogModel>>(ExecuteLogFileName) ?? new Dictionary<Guid, VirtualMachinesExecuteLogModel>();
            return result.Select(x => x.Value).OrderByDescending(x => x.CreateTime).Take(count).ToArray();
        }

        public void DeleteAll()
        {
            cachedFileStorage.Write(ExecuteLogFileName, new Dictionary<Guid, VirtualMachinesExecuteLogModel>());
        }
    }
}