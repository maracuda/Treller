using System;

namespace SKBKontur.Treller.WebApplication.Implementation.RoundDance.BusinessObjects
{
    public class DirectionTransferViewModel
    {
        public string Name { get; set; }
        public Direction OldDirection { get; set; }
        public Direction NewDirection { get; set; }
        public DateTime TransferDate { get; set; }
        public DateTime? TransferEndDate { get; set; }
    }
}