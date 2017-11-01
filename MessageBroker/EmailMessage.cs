namespace MessageBroker
{
    public class EmailMessage
    {
        public EmailMessage()
        {
            EmailAttachments = new EmailAttachment[0];
        }

        public string Title { get; set; }
        public string Body { get; set; }
        public string[] Recipients { get; set; }
        public string ReplyTo { get; set; }
        public string CopyTo { get; set; }
        public EmailAttachment[] EmailAttachments { get; set; }
    }

    public class EmailAttachment
    {
        public string Name { get; set; }
        public byte[] Content { get; set; }
    }
}