using System.ComponentModel;

namespace SKBKontur.Treller.WebApplication.Implementation.VirtualMachines.BusinessObjects
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

        [Description("Cassanrda + Elastic 3 узлов")]
        ThreeNodeJava,
    }
}