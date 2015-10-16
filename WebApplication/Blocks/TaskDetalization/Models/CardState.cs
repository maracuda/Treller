using System.ComponentModel;

namespace SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.Models
{
    public enum CardState
    {
        [Description("Неизвестно")]
        Unknown,

        [Description("Пул задач")]
        BeforeDevelop,

        [Description("Аналитика")]
        Analityc,

        [Description("Презентация аналитики")]
        AnalitycPresentation,

        [Description("Разработка")]
        Develop,

        [Description("Ревью")]
        Review,

        [Description("Презентация")]
        Presentation,

        [Description("Тестирование")]
        Testing,

        [Description("Ждёт релиз")]
        ReleaseWaiting,

        [Description("В релизе")]
        Released,

        [Description("В архиве")]
        Archived,
    }
}