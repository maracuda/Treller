using System.ComponentModel;

namespace SKBKontur.Treller.WebApplication.Services.VirtualMachines.BusinessObjects
{
    public enum VirtualMachineProfile
    {
        [Description("Тестовые стенды")]
        Stand,

        [Description("Интеграционные и юнит тесты")]
        Ci,

        [Description("Функциональные тесты")]
        FunctionalCi,

        [Description("3 узла")]
        ThreeNode,

        [Description("Билд сервер 3 узлов")]
        BuildServer,
    }
}