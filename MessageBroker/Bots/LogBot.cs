using Logger;

namespace MessageBroker.Bots
{
    public class LogBot : ILogBot
    {
        private readonly Member me;
        private readonly ILogger logger;

        public LogBot(
            IMessenger messenger,
            ILoggerFactory loggerFactory)
        {
            messenger.ChatRegistred += OnChatRegistred;
            me = messenger.RegisterBotMember(GetType());
            logger = loggerFactory.Get<LogBot>();
        }

        private void OnChatRegistred(object sender, NewChatEventArgs eventArgs)
        {
            eventArgs.Chat.NewMessagePosted += FilterMetricAndHandle;
        }

        private void FilterMetricAndHandle(object sender, MessageEventArgs eventArgs)
        {
            logger.LogInfo($"{eventArgs.Member} posted '{eventArgs.Message}'.");
        }

        public void Dispose()
        {
        }
    }
}