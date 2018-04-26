using Metric = MessageBroker.Messages.Metric;

namespace MessageBroker.Bots
{
    public interface IMetricsBot : IBot
    {
        void Publish(Metric metric);
    }
}