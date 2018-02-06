using Tolltech.TollEnnobler;

namespace SKBKontur.Tolltech.ProjectMap
{
    class Program
    {
        static void Main(string[] args)
        {
            var fixerRunner = new FixerRunner();

            fixerRunner.Run(new Settings
            {
                Log4NetFileName = "",
                ProjectNameFilter = x => x.Contains("BillingService"),
                RootNamespaceForNinjectConfiguring = "SKBKontur.Tolltech",
                SolutionPath = "D:/billy/Billing/Services/BillingServices.sln"
            });
        }
    }
}
