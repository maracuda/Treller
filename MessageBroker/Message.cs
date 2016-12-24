namespace SKBKontur.Treller.MessageBroker
{
    public class Message
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string Recipient { get; set; }
        public string ReplyTo { get; set; }
        public string CopyTo { get; set; }
    }
}