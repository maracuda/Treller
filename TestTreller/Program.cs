using System;
using System.Reflection;
using RestSharp.Serializers;
using SKBKontur.Infrastructure.ContainerConfiguration;
using SKBKontur.TaskManagerClient;
using SKBKontur.TaskManagerClient.Trello;
using TrelloNet;

namespace SKBKontur.Treller.TestTreller
{
    public class TrelloClientCustomizer : IContainerCustomizer
    {
        public void Customize(IContainer container)
        {
            container.RegisterType<TrelloClient>();
        }
    }

    public static class Program
    {
        private static IContainer container;
        private static JsonSerializer jsonSerializer;

        static void Main(string[] args)
        {
            var configurator = new ContainerConfigurator();
            container = configurator.Configure();
            var trelloClient = container.Get<ITaskManagerClient>();
            jsonSerializer = new JsonSerializer();
            var trello = ((Lazy<Trello>)trelloClient.GetType().GetField("trello", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(trelloClient)).Value;

            Do("get authorization uri", CheckAuthorizationUrl, trello);
            Do("authorize", Authorize, trelloClient);
            Do("get my data", GetMyData, trello);
            Do("get my orgs", GetMyOrganizations, trello);
            Do("get my boards", GetMyBoards, trello);
            Do("get my cards", GetMyCards, trello);
            Do("get cards actions", GetCardActions, trello);

            Console.WriteLine("Press key to exit");
            Console.ReadLine();
        }

        private static void GetCardActions(ITrello trello)
        {
            foreach (var action in trello.Actions.ForCard(new CardId("546050b4523580335944186a")))
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("Id:{0};Initiator:{1};date:{2};type:{3}", action.Id, action.MemberCreator.FullName, action.Date, action.GetType().Name);
                Console.WriteLine();
                Console.WriteLine(jsonSerializer.Serialize(action));
            }
        }

        private static void GetMyCards(ITrello trello)
        {
            foreach (var card in trello.Cards.ForMe())
            {
                Console.WriteLine("Id:{0};Name:{1};position:{2};desc:{3};url:{4};memberIds:{5}", card.Id, card.Name, card.Pos, card.Desc, card.Url, string.Join(",", card.IdMembers));

                Console.WriteLine("card lists:");
                foreach (var checklist in card.Checklists)
                {
                    Console.WriteLine("Id:{0};Name:{1};position:{2};checkItems:{3}", checklist.Id, checklist.Name, checklist.Pos, jsonSerializer.Serialize(checklist.CheckItems));
                }
                Console.WriteLine();
            }
        }

        private static void Do<T>(string actionName, Action<T> action, T param1)
        {
            Console.WriteLine();
            Console.WriteLine("Press key to " + actionName);
            Console.ReadKey();

            action(param1);
        }

        private static void Authorize(ITaskManagerClient taskManagerClient)
        {
            taskManagerClient.GetBoards(new[] { "4f4e2e4a0141dade72f808ef" });
            Console.WriteLine("Authorization complete");
        }

        private static void CheckAuthorizationUrl(ITrello trello)
        {
            var authorizationUri = trello.GetAuthorizationUrl("Treller", Scope.ReadWrite);
            Console.WriteLine(authorizationUri);
        }

        private static void GetMyData(ITrello trello)
        {
            var member = trello.Members.Me();
            
            Console.WriteLine("My Info:{0}", jsonSerializer.Serialize(member));
        }

        private static void GetMyOrganizations(ITrello trello)
        {
            foreach (var org in trello.Organizations.ForMe())
            {
                Console.WriteLine("Id:{0};Name:{1};DisplayName:{2}", org.Id, org.Name, org.DisplayName);
            }
        }

        private static void GetMyBoards(ITrello trello)
        {
            foreach (var board in trello.Boards.ForMe())
            {
                if (!string.Equals(board.Name, "CRM", StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(board.Name, "Billing", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                Console.WriteLine("Id:{0};Name:{1};OrganizationId:{2}", board.Id, board.Name, board.IdOrganization);
                Console.WriteLine("Board lists:");
                var boardId = new BoardId(board.Id);
                foreach (var list in trello.Lists.ForBoard(boardId))
                {
                    Console.WriteLine("Id:{0};Name:{1};position:{2}", list.Id, list.Name, list.Pos);
                }
                Console.WriteLine();
            }
        }

        
    }
}
