namespace SKBKontur.Treller.WebApplication.Services.VirtualMachines.BusinessObjects
{
    public class CommandExecuteVirtualMachineResult
    {
        public CommandExecuteVirtualMachineResult()
        {
            IsSuccess = true;
        }

        public string MachineName { get; set; }
        public string ResultText { get; set; }
        public string ErrorText { get; set; }
        public bool IsSuccess { get; set; }
        public bool IsFinished { get; set; }
    }
}