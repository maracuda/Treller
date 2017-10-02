using TaskManagerClient.BusinessObjects.TaskManager;

namespace ProcessStats.Dev
{
    public partial class KnownTaskQueuesLabels
    {
        public static readonly CardLabel InfrastructureQueueLabel = new CardLabel {Color = CardLabelColor.Blue, Name = "Инфраструктура"};
        public static readonly CardLabel ProductQueueLabel = new CardLabel {Color = CardLabelColor.Yellow, Name = "Продукт"};
        public static readonly CardLabel SupportQueueLabel = new CardLabel {Color = CardLabelColor.Purple, Name = "Эксплуатация"};
        public static readonly CardLabel CrmQueueLabel = new CardLabel {Color = CardLabelColor.Lime, Name = "CRM"};
    }

}