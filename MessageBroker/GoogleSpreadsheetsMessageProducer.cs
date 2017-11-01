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

        public void Publish(string spreadsheetId, int sheetId, IEnumerable<object> rowData)
        {
            SheetsService sheetsService;
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(clientSecret)))
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

            AppendRow(sheetsService, spreadsheetId, sheetId, rowData);
        }

        private static void AppendRow(SheetsService service, string spreadSheetId, int sheetId, IEnumerable<object> rowData)
        {
            var updateRequest = new Request
            {
                AppendCells = new AppendCellsRequest
                {
                    Fields = "*",
                    Rows = new List<RowData>
                    {
                        new RowData
                        {
                            Values = rowData.Select(ConvertToCellData).ToList()
                        }
                    },
                    SheetId = sheetId
                }
            };
            service.Spreadsheets.BatchUpdate(new BatchUpdateSpreadsheetRequest { Requests = new List<Request> {updateRequest} }, spreadSheetId).Execute();
        }

        private static CellData ConvertToCellData(object value)
        {
            if (value is int intValue)
            {
                return new CellData
                {
                    UserEnteredValue = new ExtendedValue
                    {
                        NumberValue = intValue
                    }
                };
            }

            //TODO: try to find another way to post dates (without magic dates)
            if (value is DateTime date)
            {
                var magicDate = new DateTime(1899, 12, 30);
                var numberOfDaysSinceMagicDate = date.Subtract(magicDate).Days;
                return new CellData
                {

                    UserEnteredValue = new ExtendedValue
                    {
                        NumberValue = numberOfDaysSinceMagicDate
                    },
                    UserEnteredFormat = new CellFormat
                    {
                        NumberFormat = new NumberFormat
                        {
                            Type = "DATE"
                        }
                    }
                };
            }

            return new CellData
            {
                UserEnteredValue = new ExtendedValue
                {
                    StringValue = value.ToString()
                }
            };
        }
    }
}