namespace MessageBroker.Messages
{
    public class Metric : Message
    {
        public string Name { get; set; }
        public int Value { get; set; }

        public override string ToString()
        {
            return $"Metric {Name} value of {Value}";
        }
    }
}