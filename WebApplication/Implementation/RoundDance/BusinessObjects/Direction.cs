using System.ComponentModel;

namespace SKBKontur.Treller.WebApplication.Implementation.RoundDance.BusinessObjects
{
    public enum Direction
    {
        [Description("Биллинг")]
        ProductBilling = 0,

        [Description("ТП")]
        Support,

        [Description("УЦ")]
        СaServices,

        [Description("Партнерка")]
        Crm,

        [Description("Инфраструктура")]
        Infrastructure,

        [Description("Отпуск")]
        Leave,

        [Description("Болезнь")]
        Sickness,

        [Description("Дежурство")]
        Duty,

        [Description("Шустрые задачи")]
        SpeedyFeatures,

        [Description("Тарифы и скидки УЦ")]
        CaTariffsAndDiscounts,

        [Description("Вендоры")]
        Vendors,

        [Description("Сценарий продления")]
        ProlongationScenario,

        [Description("Связи Д-агента")]
        LinksDeliveryAgent,

        [Description("Миграция УЦ")]
        CaMigration,

        [Description("Роуминг ДД")]
        RomingDiadoc,

        [Description("Физики")]
        Fisics,

        [Description("Fop")]
        Fop,

        [Description("Сертификаты")]
        Certificates,

        [Description("Детализация счета")]
        BillDetalization,

        [Description("Продажи ОБ")]
        ObConnection,

        [Description("Модификаторы")]
        ModifierBuy,
    }
}