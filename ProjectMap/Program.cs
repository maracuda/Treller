using Tolltech.TollEnnobler;

namespace ProjectMap
{
    public class Program
    {
        static void Main(string[] args)
        {
            var fixerRunner = new FixerRunner();

            fixerRunner.Run(new Settings
            {
                ProjectNameFilter = x => true,
                SolutionPath = "D:/billy/MegaWithoutCI.sln"
            }, new[] { new DocumentLister() });
        }
    }
}
