using System;
using System.Management.Automation;

namespace SKBKontur.Treller.WebApplication.Services.VirtualMachines.BusinessObjects
{
    public class AsyncPowershell : IDisposable
    {
        private bool isCompleted;
        private bool isSuccess = false;

        private string successResult = null;
        private string errorResult = null;
        private bool isResulted = false;

        public AsyncPowershell(PowerShell powerShell)
        {
            PowerShell = powerShell;
            AsyncResult = powerShell.BeginInvoke();
            isCompleted = false;
        }

        public PowerShell PowerShell { get; private set; }
        public IAsyncResult AsyncResult { get; private set; }

        public string[] GetFormattedResult()
        {
            if (isResulted)
            {
                return new [] { successResult, errorResult };
            }

            var powerShellResult = PowerShell.EndInvoke(AsyncResult);

            successResult = string.Join(Environment.NewLine, powerShellResult);
            errorResult = string.Join(Environment.NewLine, PowerShell.Streams.Error);

            isSuccess = PowerShell.Streams.Error.Count == 0;
            isResulted = true;

            return new [] { successResult, errorResult };
        }

        public bool IsFinished()
        {
            return isCompleted || AsyncResult.IsCompleted;
        }

        public bool IsSuccess()
        {
            return isSuccess;
        }

        public void Dispose()
        {
            if (!isCompleted)
            {
                PowerShell.Dispose();
            }
            isCompleted = true;
        }
    }
}