using SKBKontur.TaskManagerClient.BusinessObjects.ContinuousIntegration;

namespace SKBKontur.TaskManagerClient
{
    public interface IContinuousIntegrationClient
    {
        BuildModel GetLastBuildInfo(string projectName, string buildConfigurationName);
    }
}