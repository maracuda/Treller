using System.ComponentModel;

namespace SKBKontur.Treller.WebApplication.Fan.Activities
{
    public enum ActivityFormat
    {
        [Description("Игровой")]
        Game,
        
        [Description("Доклад-дискуссия")]
        PresentationWithDiscussion,

        [Description("Дискуссия")]
        Discussion,

        [Description("Доклад + вопросы")]
        Presentation,

        [Description("Определяется участниками")]
        NotSelectedYet,
        
        [Description("")]
        Empty
    }
}