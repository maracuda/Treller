using System;

namespace SKBKontur.Treller.WebApplication.Implementation.RoundDance.BusinessObjects
{
    public class DirectionTransferViewModel
    {
        public string Name { get; set; }
        public string OldDirection { get; set; }
        public string NewDirection { get; set; }
        public DateTime TransferDate { get; set; }
        public DateTime? TransferEndDate { get; set; }

        public string TransferDates
        {
            get
            {
                return TransferEndDate.HasValue
                    ? string.Format("{0:dd.MM.yyyy} - {1:dd.MM.yyyy}", TransferDate, TransferEndDate.Value)
                    : TransferDate.ToString("dd.MM.yyyy");
            }
        }
    }
}