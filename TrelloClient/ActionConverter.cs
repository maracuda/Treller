using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TrelloNet;
using Action = TrelloNet.Action;

namespace SKBKontur.Treller.TrelloClient
{
    public class ActionConverter : JsonConverter
    {
        private static readonly Dictionary<string, Func<JObject, TrelloNet.Action>> TypeMap = new Dictionary<string, Func<JObject, TrelloNet.Action>>()
        {
          {
            "createCard",
            (Func<JObject, TrelloNet.Action>) (_ => (TrelloNet.Action) new CreateCardAction())
          },
          {
            "commentCard",
            (Func<JObject, TrelloNet.Action>) (_ => (TrelloNet.Action) new CommentCardAction())
          },
          {
            "addMemberToCard",
            (Func<JObject, TrelloNet.Action>) (_ => (TrelloNet.Action) new AddMemberToCardAction())
          },
          {
            "removeMemberFromCard",
            (Func<JObject, TrelloNet.Action>) (_ => (TrelloNet.Action) new RemoveMemberFromCardAction())
          },
          {
            "updateCheckItemStateOnCard",
            (Func<JObject, TrelloNet.Action>) (_ => (TrelloNet.Action) new UpdateCheckItemStateOnCardAction())
          },
          {
            "addAttachmentToCard",
            (Func<JObject, TrelloNet.Action>) (_ => (TrelloNet.Action) new AddAttachmentToCardAction())
          },
          {
            "deleteAttachmentFromCard",
            (Func<JObject, TrelloNet.Action>) (_ => (TrelloNet.Action) new DeleteAttachmentFromCardAction())
          },
          {
            "addChecklistToCard",
            (Func<JObject, TrelloNet.Action>) (_ => (TrelloNet.Action) new AddChecklistToCardAction())
          },
          {
            "removeChecklistFromCard",
            (Func<JObject, TrelloNet.Action>) (_ => (TrelloNet.Action) new RemoveChecklistFromCardAction())
          },
          {
            "createList",
            (Func<JObject, TrelloNet.Action>) (_ => (TrelloNet.Action) new CreateListAction())
          },
          {
            "createBoard",
            (Func<JObject, TrelloNet.Action>) (_ => (TrelloNet.Action) new CreateBoardAction())
          },
          {
            "addMemberToBoard",
            (Func<JObject, TrelloNet.Action>) (_ => (TrelloNet.Action) new AddMemberToBoardAction())
          },
          {
            "removeMemberFromBoard",
            (Func<JObject, TrelloNet.Action>) (_ => (TrelloNet.Action) new RemoveMemberFromBoardAction())
          },
          {
            "addToOrganizationBoard",
            (Func<JObject, TrelloNet.Action>) (_ => (TrelloNet.Action) new AddToOrganizationBoardAction())
          },
          {
            "removeFromOrganizationBoard",
            (Func<JObject, TrelloNet.Action>) (_ => (TrelloNet.Action) new RemoveFromOrganizationBoardAction())
          },
          {
            "createOrganization",
            (Func<JObject, TrelloNet.Action>) (_ => (TrelloNet.Action) new CreateOrganizationAction())
          },
          {
            "updateBoard",
            new Func<JObject, TrelloNet.Action>(ActionConverter.CreateUpdateBoardAction)
          },
          {
            "updateCard",
            new Func<JObject, TrelloNet.Action>(ActionConverter.CreateUpdateCardAction)
          },
          {
            "updateList",
            new Func<JObject, TrelloNet.Action>(ActionConverter.CreateUpdateListAction)
          },
          {
            "updateOrganization",
            new Func<JObject, TrelloNet.Action>(ActionConverter.CreateUpdateOrganizationAction)
          },
          {
            "moveCardToBoard",
            (Func<JObject, TrelloNet.Action>) (_ => (TrelloNet.Action) new MoveCardToBoardAction())
          },
          {
            "moveCardFromBoard",
            (Func<JObject, TrelloNet.Action>) (_ => (TrelloNet.Action) new MoveCardFromBoardAction())
          },
          {
            "convertToCardFromCheckItem",
            (Func<JObject, TrelloNet.Action>) (_ => (TrelloNet.Action) new ConvertToCardFromCheckItemAction())
          },
          {
            "deleteCard",
            (Func<JObject, TrelloNet.Action>) (_ => (TrelloNet.Action) new DeleteCardAction())
          },
          {
            "copyCard",
            (Func<JObject, TrelloNet.Action>) (_ => (TrelloNet.Action) new CopyCardAction())
          }
        };

        static ActionConverter()
        {
        }

        private static Action CreateUpdateOrganizationAction(JObject jObject)
        {
            var organizationAction = new UpdateOrganizationAction();
            ApplyUpdateData(organizationAction.Data, jObject);
            return organizationAction;
        }

        private static Action CreateUpdateListAction(JObject jObject)
        {
            var updateListAction = new UpdateListAction();
            ApplyUpdateData(updateListAction.Data, jObject);
            return updateListAction;
        }

        private static Action CreateUpdateBoardAction(JObject jObject)
        {
            var updateBoardAction = new UpdateBoardAction();
            ApplyUpdateData(updateBoardAction.Data, jObject);
            return updateBoardAction;
        }

        private static Action CreateUpdateCardAction(JObject jObject)
        {
            if (jObject["data"]["listBefore"] != null)
                return new UpdateCardMoveAction();
            if (jObject["data"]["old"]["closed"] != null)
                return new CloseCardAction();
            if (jObject["data"]["old"]["pos"] != null)
                return new UpdateCardPositionAction();
            var updateCardAction = new UpdateCardAction();
            ApplyUpdateData(updateCardAction.Data, jObject);
            return updateCardAction;
        }

        private static void ApplyUpdateData(IUpdateData updateData, JObject jObject)
        {
            var jtoken = jObject["data"]["old"];
            if (jtoken == null)
            {
                return;
            }
            var name = ((JProperty)jtoken.First).Name;
            updateData.Old = new Old
            {
                PropertyName = name,
                Value = jtoken[name]
            };
        }

        private static string ParseType(JObject jObject)
        {
            return jObject["type"].ToObject<string>();
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(Action).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            Func<JObject, Action> func;
            var obj = TypeMap.TryGetValue(ParseType(jObject), out func) ? func(jObject) : new Action();
            serializer.Populate(jObject.CreateReader(), obj);
            return obj;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}