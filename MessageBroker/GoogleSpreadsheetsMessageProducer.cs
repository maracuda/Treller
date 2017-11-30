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

namespace MessageBroker
{
    public class GoogleSpreadsheetsMessageProducer : ISpreadsheetsMessageProducer
    {
        private readonly string clientSecret;

        public GoogleSpreadsheetsMessageProducer(string clientSecret)
        {
            this.clientSecret = clientSecret;
        }

        public void Append(string spreadsheetId, string sheetName, DataRow dataRow)
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

        public void Rewrite(string spreadsheetId, string sheetName, DataRow[] dataRows)
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

        private void ExecuteCommands(string spreadsheetId, Func<SheetsService, IList<Request>> command)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(clientSecret)))
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
    }
}