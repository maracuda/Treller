using System;
using System.Text;
using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Extensions;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News
{
    public class TaskNew
    {
        public string BoardId { get; set; }
        public string TaskId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public NewDeliveryChannelType DeliveryChannel { get; set; }
        public DateTime? DoNotDeliverUntil { get; set; }
        public long TimeStamp { get; set; }

        public string PrimaryKey => $"{TaskId}{DeliveryChannel}";

        public bool HasSamePrimaryKey(TaskNew anotherTaskNew)
        {
            return string.Equals(TaskId, anotherTaskNew.TaskId, StringComparison.OrdinalIgnoreCase)
                   && DeliveryChannel == anotherTaskNew.DeliveryChannel;
        }

        public string BuildDiff(TaskNew anotherTaskNew)
        {
            if (!string.Equals(BoardId, anotherTaskNew.BoardId, StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException($"Fail to build diff for task news with different board ids. This: {BoardId}. Another: {anotherTaskNew.BoardId}.");
            if (!HasSamePrimaryKey(anotherTaskNew))
                throw new ArgumentException($"Fail to build diff for task news with different primary keys. " +
                                            $"This: {TaskId},{DeliveryChannel}. Another: {anotherTaskNew.TaskId},{anotherTaskNew.DeliveryChannel}.");

            var builder = new StringBuilder();
            if (!string.Equals(Title, anotherTaskNew.Title, StringComparison.Ordinal))
            {
                builder.Append($"Title {Title} changed to {anotherTaskNew.Title}");
            }
            if (!string.Equals(Text, anotherTaskNew.Text, StringComparison.Ordinal))
            {
                builder.Append($"Text {Text} changed to {anotherTaskNew.Text}");
            }
            if (DoNotDeliverUntil != anotherTaskNew.DoNotDeliverUntil)
            {
                builder.Append($"DoNotDeliverUntil {DoNotDeliverUntil.SafeDateTimeFormat()} changed to {anotherTaskNew.DoNotDeliverUntil.SafeDateTimeFormat()}");
            }
            return builder.ToString();
        }
    }

    public enum NewDeliveryChannelType
    {
        Team = 1,
        Support = 2,
        Customer = 3
    }
}