using System;
using MessageBroker.Messages;

namespace MessageBroker
{
    public interface IMessenger
    {
        IChat RegisterChat(string name, string description);
        event EventHandler<NewChatEventArgs> ChatRegistred;
        Member RegisterBotMember(Type typeOfBot);
    }

    public class SingleAppMessenger : IMessenger
    {
        public IChat RegisterChat(string name, string description)
        {
            var newChat = new NoHistoryChat(name, description);
            var newChatEventArgs = new NewChatEventArgs
            {
                Chat = newChat
            };
            ChatRegistred?.Invoke(this, newChatEventArgs);
            return newChat;
        }

        public event EventHandler<NewChatEventArgs> ChatRegistred;
        public Member RegisterBotMember(Type typeOfBot)
        {
            return new Bot(typeOfBot);
        }
    }

    public class NewChatEventArgs : EventArgs
    {
        public IChat Chat { get; set; }
    }

    public interface IChat
    {
        string Name { get; }
        string Description { get; }
        void Post(Member member, Message message);
        void Post(Member member, string text);
        event EventHandler<MessageEventArgs> NewMessagePosted;
    }

    public class NoHistoryChat : IChat
    {
        public NoHistoryChat(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public string Name { get; }
        public string Description { get; }
        public void Post(Member member, Message message)
        {
            var messageArgs = new MessageEventArgs
            {
                Member = member,
                Message = message
            };
            NewMessagePosted?.Invoke(this, messageArgs);
        }

        public void Post(Member member, string text)
        {
            Post(member, new TextMessage { Text = text });
        }

        public event EventHandler<MessageEventArgs> NewMessagePosted;
    }

    public class MessageEventArgs : EventArgs
    {
        public Message Message { get; set; }
        public Member Member { get; set; }
    }

    public abstract class Member
    {
        
    }

    public class Bot : Member
    {
        public string MachineName { get; }
        public string UserName { get; }
        public string BotName { get; }

        public Bot(Type typeOfBot)
        {
            MachineName = Environment.MachineName;
            UserName = Environment.UserName;
            BotName = typeOfBot.Name;
        }

        public override string ToString()
        {
            return $"{MachineName}_{UserName}_{BotName}";
        }
    }
}