using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using MessageBroker.Messages;

namespace MessageBroker.Bots
{
    public class GoogleSpreadsheetsBot : ISpreadsheetsBot
    {
        private readonly string clientSecret;
        private readonly Member me;

        public GoogleSpreadsheetsBot(
            IMessenger messenger,
            string clientSecret)
        {
            this.clientSecret = clientSecret;
            messenger.ChatRegistred += OnChatRegistred;
            me = messenger.RegisterBotMember(GetType());
        }

        private void OnChatRegistred(object sender, NewChatEventArgs eventArgs)
        {
            eventArgs.Chat.NewMessagePosted += FilterEmailAndHandle;
        }

        private void FilterEmailAndHandle(object sender, MessageEventArgs eventArgs)
        {
            if (eventArgs.Message is Report report)
            {
                Publish(report);
                if (sender is IChat chat)
                {
                    chat.Post(me, $"Опубликовано: {report}.");
                }
            }
        }

        private void Append(string spreadsheetId, string sheetName, DataRow dataRow)
        {
            ExecuteCommands(spreadsheetId, sheetsService =>
            {
                var sheetId = ReadSheetId(sheetsService, spreadsheetId, sheetName);
                return new List<Request>
                {
                    RequestFactory.CreateAppendRowRequest(sheetId, dataRow.Values)
                };
            });
        }

        private void Rewrite(string spreadsheetId, string sheetName, DataRow[] dataRows)
        {
            ExecuteCommands(spreadsheetId, sheetsService =>
            {
                var result = new List<Request>();

                var sheet = sheetsService.Spreadsheets.Get(spreadsheetId).Execute().Sheets.FirstOrDefault(s => s.Properties.Title.Equals(sheetName));
                if (sheet != null)
                {
                    result.Add(RequestFactory.CreateCleanSheetRequest(sheet.Properties.SheetId.Value));
                }
                else
                {
                    sheetsService.Spreadsheets.BatchUpdate(new BatchUpdateSpreadsheetRequest
                    {
                        Requests = new List<Request> { RequestFactory.RequestCreateCreateSheetRequest(sheetName) }

                    }, spreadsheetId).Execute();
                    sheet = sheetsService.Spreadsheets.Get(spreadsheetId).Execute().Sheets.First(s => s.Properties.Title.Equals(sheetName));
                }

                foreach (var dataRow in dataRows)
                {
                    result.Add(RequestFactory.CreateAppendRowRequest(sheet.Properties.SheetId.Value, dataRow.Values));
                }

                return result;
            });
        }

        public void Publish(Report report)
        {
            switch (report.Type)
            {
                case ReportType.Diff:
                {
                    if (report.DataRows.Length > 0)
                    {
                        Append(report.SpreadsheetId, report.SheetName, report.DataRows[0]);
                    }
                    break;
                }
                case ReportType.Full:
                {
                    Rewrite(report.SpreadsheetId, report.SheetName, report.DataRows);
                    break;
                }
            }
        }

        private void ExecuteCommands(string spreadsheetId, Func<SheetsService, IList<Request>> command)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes((string) clientSecret)))
            {
                var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new[] { SheetsService.Scope.Spreadsheets },
                    "user",
                    CancellationToken.None).Result;

                using (var sheetsService = new SheetsService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Treller"
                }))
                {
                    var requests = command(sheetsService);
                    sheetsService.Spreadsheets.BatchUpdate(new BatchUpdateSpreadsheetRequest { Requests = requests }, spreadsheetId).Execute();
                }
            }
        }

        private static IList<Sheet> ListSheets(SheetsService service, string spreadsheetId)
        {
            return service.Spreadsheets.Get(spreadsheetId).Execute().Sheets;
        }

        private int ReadSheetId(SheetsService service, string spreadsheetId, string sheetName)
        {
            var sheet = ListSheets(service, spreadsheetId).FirstOrDefault(s => s.Properties.Title.Equals(sheetName, StringComparison.OrdinalIgnoreCase));
            if (sheet?.Properties.SheetId == null)
                throw new Exception($"Fail to find sheet with name {sheetName} at spreadsheet with id {spreadsheetId}");
            return sheet.Properties.SheetId.Value;
        }

        public void Dispose()
        {
        }
    }
}