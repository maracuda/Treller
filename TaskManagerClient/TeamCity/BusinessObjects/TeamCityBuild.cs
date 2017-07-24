using System;

namespace TaskManagerClient.TeamCity.BusinessObjects
{
    public class TeamCityBuild
    {
        public long Id { get; set; }//": 1481441,
        public string Number { get; set; }//": "47",
        public string Status { get; set; }//": "FAILURE",
        public string State { get; set; }//": "finished",
        public string StatusText { get; set; }//": "Tests failed: 12 (3 new), passed: 1179, ignored: 27",
        public TeamCityBuildType Type { get; set; }
        public DateTime QueuedDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
    }

    public class TeamCityBuildType
    {
        public string Id  { get; set; }// "Billy_BillingIntegrationTestsNewDeployVmBillyTest1",
        public string Name  { get; set; }//"Billing, IntegrationTests (vm-billy-test1)",
        public string ProjectName { get; set; } //": "Billy",
        public string ProjectId { get; set; }//": "Billy",
    }
}