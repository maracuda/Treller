using TaskManagerClient.BusinessObjects.ContinuousIntegration;

namespace TaskManagerClient.TeamCity
{
    public class TeamCityClient : IContinuousIntegrationClient
    {
        public BuildModel GetLastBuildInfo(string projectName, string buildConfigurationName)
        {
            throw new System.NotImplementedException();
        }
    }
}