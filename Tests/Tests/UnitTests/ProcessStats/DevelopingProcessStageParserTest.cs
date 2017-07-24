using ProcessStats.Dev;
using Xunit;

namespace Tests.Tests.UnitTests.ProcessStats
{
    public class DevelopingProcessStageParserTest : UnitTest
    {
        private readonly DevelopingProcessStageParser developingProcessStageParser;

        public DevelopingProcessStageParserTest()
        {
            developingProcessStageParser = new DevelopingProcessStageParser();
        }

        [Theory]
        [InlineData("Analytics & Design", DevelopingProcessStage.Analyzing)]
        [InlineData("Dev", DevelopingProcessStage.Developing)]
        [InlineData("Review", DevelopingProcessStage.Reviewing)]
        [InlineData("Testing", DevelopingProcessStage.Testing)]
        [InlineData("Wait For Release", DevelopingProcessStage.Done)]
        [InlineData("Released", DevelopingProcessStage.Done)]
        public void ParseKnownStageNames(string stageName, DevelopingProcessStage expectedStage)
        {
            var actual = developingProcessStageParser.TryParse(stageName);
            Assert.Equal(expectedStage, actual);
        }

        [Fact]
        public void UnknownIsDefaultStage()
        {
            Assert.Equal(DevelopingProcessStage.Unknown, developingProcessStageParser.TryParse("zzz"));
        }

        [Theory]
        [InlineData("Released")]
        public void ParseIsCaseInsensitive(string stageName)
        {
            var expected = developingProcessStageParser.TryParse(stageName);
            Assert.Equal(expected, developingProcessStageParser.TryParse(stageName.ToLower()));
            Assert.Equal(expected, developingProcessStageParser.TryParse(stageName.ToUpper()));
        }
    }
}