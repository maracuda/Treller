using System;
using System.Collections.Generic;

namespace SkbKontur.Treller.ProcessStats.Dev
{
    public class DevelopingProcessStageParser : IDevelopingProcessStageParser
    {
        private readonly Dictionary<string, DevelopingProcessStage> knownStagesMap = new Dictionary<string, DevelopingProcessStage>
        {
            {"Analytics & Design", DevelopingProcessStage.Analyzing},
            {"Dev", DevelopingProcessStage.Developing },
            {"Review", DevelopingProcessStage.Reviewing },
            {"Testing", DevelopingProcessStage.Testing },
            {"Wait For Release", DevelopingProcessStage.Done },
            {"Released", DevelopingProcessStage.Done }
        };

        public DevelopingProcessStage TryParse(string stageName)
        {
            foreach (var key in knownStagesMap.Keys)
            {
                if (string.Equals(stageName, key, StringComparison.OrdinalIgnoreCase))
                    return knownStagesMap[key];
            }

            return DevelopingProcessStage.Unknown;
        }
    }
}