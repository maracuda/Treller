using System;
using SKBKontur.TaskManagerClient.BusinessObjects;
using SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.Models;

namespace SKBKontur.Treller.WebApplication.Services.News
{
    public class CardNewsModel
    {
        public string CardId { get; set; }
        public string CardName { get; set; }
        public string CardDescription { get; set; }
        public CardLabel[] Labels { get; set; }
        public CardState State { get; set; }
        public DateTime? DueDate { get; set; }
    }
}