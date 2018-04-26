using MessageBroker.Bots;
using Xunit;

namespace Tests.Tests.IntegrationTests.MessageBroker
{
    public class GraphiteBotTest : IntegrationTest
    {
        [Fact]
        public void AbleToPublishTestMetric()
        {
            using (var metricBot = container.Get<IMetricsBot>())
            {
                metricBot.Publish(new global::MessageBroker.Messages.Metric
                {
                    Name = "Test.Ping",
                    Value = 1
                });
            }
        }

    }
}
