using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using TaskManagerClient.CredentialServiceAbstractions;

namespace ProcessStats.SpreadsheetProducer
{
    public class SpreadsheetProducer : ISpreadsheetProducer
    {
        private readonly ISpreadsheetsCredentialService spreadsheetsCredentialService;

        public SpreadsheetProducer(ISpreadsheetsCredentialService spreadsheetsCredentialService)
        {
            this.spreadsheetsCredentialService = spreadsheetsCredentialService;
        }

        public void Publish(string spreadsheetId, int sheetId, string sheetName, string[] rowData)
        {
            SheetsService sheetsService;
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(spreadsheetsCredentialService.ClientSecret)))
            {
                var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new[] {SheetsService.Scope.Spreadsheets},
                    "user",
                    CancellationToken.None).Result;
                sheetsService = new SheetsService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Treller"
                });
            }

            AppendRow(sheetsService, spreadsheetId, sheetId, sheetName, rowData);
        }

        private static void AppendRow(SheetsService service, string spreadSheetId, int sheetId, string sheetName, IEnumerable<string> rowData)
        {
            var rowIndex = service.Spreadsheets.Values.Get(spreadSheetId, $"'{sheetName}'!A:A").Execute().Values?.Count ?? 0;
            var updateRequest = new Request
            {
                UpdateCells = new UpdateCellsRequest
                {
                    Start = new GridCoordinate
                    {
                        SheetId = sheetId,
                        RowIndex = rowIndex,
                        ColumnIndex = 0
                    },
                    Rows = new List<RowData>
                    {
                        new RowData
                        {
                            Values = rowData.Select(value => new CellData
                            {
                                UserEnteredValue = new ExtendedValue
                                {
                                    StringValue = value
                                }
                            }).ToList()
                        }
                    },
                    Fields = "userEnteredValue"
                }
            };
            service.Spreadsheets.BatchUpdate(new BatchUpdateSpreadsheetRequest { Requests = new List<Request> {updateRequest} }, spreadSheetId).Execute();
        }
    }
}