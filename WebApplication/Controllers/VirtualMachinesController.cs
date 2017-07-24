using System;
using System.Linq;
using System.Web.Mvc;
using Logger;
using WebApplication.Implementation.VirtualMachines;
using WebApplication.Implementation.VirtualMachines.BusinessObjects;
using WebApplication.Implementation.VirtualMachines.Logging;

namespace WebApplication.Controllers
{
    public class VirtualMachinesController : ExceptionHandledController
    {
        private readonly IVirtualMachinesStorage virtualMachinesStorage;
        private readonly IVirtualMachinesService virtualMachinesService;
        private readonly IVirtualMachinesExecuteLog virtualMachinesExecuteLog;

        public VirtualMachinesController(
            IVirtualMachinesStorage virtualMachinesStorage, 
            IVirtualMachinesService virtualMachinesService,
            IVirtualMachinesExecuteLog virtualMachinesExecuteLog,
            ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            this.virtualMachinesStorage = virtualMachinesStorage;
            this.virtualMachinesService = virtualMachinesService;
            this.virtualMachinesExecuteLog = virtualMachinesExecuteLog;
        }

        [HttpGet]
        public ActionResult Index(bool isLoginFailed = false, string command = null)
        {
            var machines = virtualMachinesStorage.GetAllVirtualMachines();
            ViewData["isLoginFailed"] = isLoginFailed;
            ViewData["command"] = command;
            return View("Index", machines);
        }

        [HttpPost]
        public ActionResult ExecuteCommand(string command, string login, string[] virtualMachineNames)
        {
            if (login != "mayloe" && login != "yord" && login != "chicha")
            {
                return RedirectToAction("Index", new {isLoginFailed = true});
            }

            virtualMachineNames = virtualMachineNames.Where(x => x != "false").ToArray();
            var executeId = virtualMachinesService.ExecuteCommandAsync(command, login, virtualMachineNames);

            return RedirectToAction("ShowLogs", new { ExecuteId = executeId });
        }

        [HttpGet]
        public ActionResult ShowLogs(Guid? executeId, Guid? refreshExecuteId = null)
        {
            if (refreshExecuteId.HasValue)
            {
                virtualMachinesService.RefreshExecuteCommandProgress(refreshExecuteId.Value);
            }
            CommandExecuteResult commandResult = null;
            if (executeId.HasValue)
            {
                commandResult = virtualMachinesService.GetExecuteCommandProgress(executeId.Value) ??
                                virtualMachinesExecuteLog.FindLog(executeId.Value).Execute;
            }
            

            var lastLogs = virtualMachinesExecuteLog.SelectLastLogs(1000);

            var resultModel = new ShowLogsViewModel
            {
                CurrentResult = commandResult,
                Logs = lastLogs
            };

            return View("ShowLogs", resultModel);
        }

        public ActionResult DeleteLogs()
        {
            virtualMachinesExecuteLog.DeleteAll();

            return RedirectToAction("Index");
        }
    }
}