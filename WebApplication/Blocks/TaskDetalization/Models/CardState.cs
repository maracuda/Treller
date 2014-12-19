using System.ComponentModel;

namespace SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.Models
{
    public enum CardState
    {
        [Description("Неизвестно")]
        Unknown,

        [Description("Аналитика")]
        BeforeDevelop,

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

        [Description("В архиве")]
        Archived,
    }
}