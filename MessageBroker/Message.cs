namespace MessageBroker
{
    public class Message
    {
        public Message()
        {
            Attachments = new Attachment[0];
        }

        public string Title { get; set; }
        public string Body { get; set; }
        public string Recipient { get; set; }
        public string ReplyTo { get; set; }
        public string CopyTo { get; set; }
        public Attachment[] Attachments { get; set; }
    }

    public class Attachment
    {
        public string Name { get; set; }
        public byte[] Content { get; set; }
    }
}