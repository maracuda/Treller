using System;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Operationals.OperationsLog
{
    public interface IOperationsLog
    {
        void Append(string operationName, DateTime beginTime, DateTime endTime, bool isLaunchSuccessfully);
        object GetLastOperations(int count);
        int CountLanches(string operationName, DateTime fromTime, DateTime to);
        void CleanUp();
    }
}