using TaskManagerClient.BusinessObjects.TaskManager;

namespace ProcessStats.Dev
{
    public static class KnownBoards
    {
        public static readonly Board BillingDelivery = new Board("58d22275df59d0815216e1f0", "Биллинг доставляет");
        public static readonly Board Motocycle = new Board("58874dd11d04205448b0435d", "Цикл продаж");
        public static readonly Board PortalAuth = new Board("5976f7264ba9e718190cc5d0", "Портальная аутентификация");
        public static readonly Board Market = new Board("595b48916ed422e278c6c9f8", "Маркет + ОФД");
        public static readonly Board Discounts = new Board("59ad172066c11f4cccf3e894", "Скидки");
        public static readonly Board ServiceTeam = new Board("5927c6dbb2e1640ccd2282fb", "Сервис Тим");
    }

    public static class KnownLists
    {
        public static readonly BoardList BillingDeliveryDone = new BoardList("58d2296edeed2d3c3e25acdd", KnownBoards.BillingDelivery.Id, "Готово");
        public static readonly BoardList BillingDeliveryFeedback = new BoardList("58d2295834d4e5619d71d008", KnownBoards.BillingDelivery.Id, "Обратная связь");
        public static readonly BoardList MotocycleDone = new BoardList("58875668c6677244cf4ccd03", KnownBoards.Motocycle.Id, "Готово");
        public static readonly BoardList PortalAuthDone = new BoardList("5976f74a5e8b7544b1fc0738", KnownBoards.PortalAuth.Id, "Готово");
        public static readonly BoardList MarketDone = new BoardList("595b48ae0cf6ee288bcba874", KnownBoards.Market.Id, "Готово");
        public static readonly BoardList DiscountsDone = new BoardList("59ad1c5344878b519796fa37", KnownBoards.Discounts.Id, "Готово");
        public static readonly BoardList ServiceTeamDone = new BoardList("5927c7194bb22deb57e43925", KnownBoards.ServiceTeam.Id, "То что сделали");
        public static readonly BoardList ServiceTeamDoneLongAgo = new BoardList("5927c6dbb2e1640ccd228304", KnownBoards.ServiceTeam.Id, "То что сделали давно");
    }
}