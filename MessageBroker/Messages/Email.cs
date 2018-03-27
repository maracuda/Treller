using System.Text;

namespace MessageBroker.Messages
{
    public class Email : Message
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string[] Recipients { get; set; }
        public string ReplyTo { get; set; }
        public string CopyTo { get; set; }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Получатели: {string.Join(",", Recipients)}");
            if (!string.IsNullOrEmpty(ReplyTo))
            {
                stringBuilder.AppendLine($"Куда отвечать: {ReplyTo}");
            }
            if (!string.IsNullOrEmpty(CopyTo))
            {
                stringBuilder.AppendLine($"Копия: {CopyTo}");
            }
            stringBuilder.AppendLine("Письмо");
            stringBuilder.AppendLine($"{Title}");
            stringBuilder.AppendLine($"{Body.Substring(0, Body.Length < 1000 ? Body.Length : 1000)}");
            return stringBuilder.ToString();
        }
    }
}