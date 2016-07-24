using System;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Operationals.OperationsLog
{
    public class OperationsLog : IOperationsLog
    {
        public void Append(string operationName, DateTime beginTime, DateTime endTime, bool isLaunchSuccessfully)
        {
            //TODO: impl it
        }

        public object GetLastOperations(int count)
        {
            throw new NotImplementedException();
        }

        public int CountLanches(string operationName, DateTime fromTime, DateTime to)
        {
            return 0;
        }

        public void CleanUp()
        {
            throw new NotImplementedException();
        }
    }
}