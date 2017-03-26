namespace SkbKontur.Treller.ProcessStats.Dev
{
    public interface IDevelopingProcessStageParser
    {
        DevelopingProcessStage TryParse(string stageName);
    }
}