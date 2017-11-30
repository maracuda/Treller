using System;
using System.Collections.Generic;
using System.Linq;
using Google.Apis.Sheets.v4.Data;

namespace MessageBroker
{
    public static class RequestFactory
    {
        public static Request CreateCleanSheetRequest(int sheetId)
        {
            return new Request
            {
                UpdateCells = new UpdateCellsRequest
                {
                    Fields = "userEnteredValue",
                    Range = new GridRange
                    {
                        SheetId = sheetId
                    }
                }
            };
        }

        public static Request RequestCreateCreateSheetRequest(string sheetName)
        {
            return new Request
            {
                AddSheet = new AddSheetRequest
                {
                    Properties = new SheetProperties
                    {
                        Title = sheetName
                    }
                }
            };
        }

        public static Request CreateAppendRowRequest(int sheetId, IEnumerable<object> rowData)
        {
            return new Request
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
