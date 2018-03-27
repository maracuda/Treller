namespace MessageBroker.Messages
{
    public class TextMessage : Message
    {
        public string Text { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}