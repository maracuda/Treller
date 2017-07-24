using TaskManagerClient.BusinessObjects.ContinuousIntegration;

namespace TaskManagerClient
{
    public interface IContinuousIntegrationClient
    {
        BuildModel GetLastBuildInfo(string projectName, string buildConfigurationName);
    }
}