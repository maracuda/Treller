using System;

namespace SKBKontur.Treller.WebApplication.Services.RoundDance
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