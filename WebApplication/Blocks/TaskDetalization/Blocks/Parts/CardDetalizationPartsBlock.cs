namespace SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.Blocks.Parts
{
    public class CardDetalizationPartsBlock : BaseTaskDetalizationBlock
    {
        public CardBeforeDevelopPartBlock BeforeDevelop { get; set; }
        public CardDevelopPartBlock Develop { get; set; }
        public CardReviewPartBlock Review { get; set; }
        public CardPresentationPartBlock Presentation { get; set; }
        public CardTestingPartBlock Testing { get; set; }
        public CardReleaseWaitingPartBlock ReleaseWaiting { get; set; }
        public CardArchivePartBlock Archive { get; set; }
    }
}