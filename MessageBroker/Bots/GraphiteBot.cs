using System;
using System.Threading;
using Metrics;
using Metrics.Graphite;
using Metric = MessageBroker.Messages.Metric;

namespace MessageBroker.Bots
{
    public class GraphiteBot : IMetricsBot
    {
        private readonly Member me;

        public GraphiteBot(
            IMessenger messenger)
        {
            messenger.ChatRegistred += OnChatRegistred;
            me = messenger.RegisterBotMember(GetType());
            Metrics.Metric.Config.WithReporting(r => r.WithReport(new FilteredGraphiteReport(new TcpGraphiteSender("graphite-relay.skbkontur.ru", 2003)), TimeSpan.FromSeconds(5)));
        }

        private void OnChatRegistred(object sender, NewChatEventArgs eventArgs)
        {
            eventArgs.Chat.NewMessagePosted += FilterMetricAndHandle;
        }

        private void FilterMetricAndHandle(object sender, MessageEventArgs eventArgs)
        {
            if (eventArgs.Message is Metric metric)
            {
                Publish(metric);
                if (sender is IChat chat)
                {
                    chat.Post(me, $"Опубликовано: {metric}.");
                }
            }
        }

        public void Publish(Metric metric)
        {
            Metrics.Metric.Context("Billy").Meter(metric.Name, Unit.None).Mark(metric.Value);
        }

        public void Dispose()
        {
            Thread.Sleep(7000);
            Metrics.Metric.Config.WithReporting(r => r.StopAndClearAllReports());
            Metrics.Metric.ShutdownContext("Billy");
        }
    }
}