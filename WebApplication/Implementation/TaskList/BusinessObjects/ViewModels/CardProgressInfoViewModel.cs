namespace SKBKontur.Treller.WebApplication.Implementation.TaskList.BusinessObjects.ViewModels
{
    public class CardProgressInfoViewModel
    {
        public decimal CurrentCount { get; set; }
        public decimal TotalCount { get; set; }

        public string Progress { get { return (TotalCount > 0 ? CurrentCount * 100M / TotalCount : 0).ToString("#0"); } }
    }
}