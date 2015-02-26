using SKBKontur.TaskManagerClient.BusinessObjects.ContinuousIntegration;

namespace SKBKontur.TaskManagerClient.TeamCity
{
    public class TeamCityClient : IContinuousIntegrationClient
    {
        public BuildModel GetLastBuildInfo(string projectName, string buildConfigurationName)
        {
            throw new System.NotImplementedException();
        }
    }
}