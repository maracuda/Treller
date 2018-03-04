using ViskeyTube.Common;
using Xunit;

namespace Tests.Tests.IntegrationTests.ViskeyTube
{
    public class HtmlHelpersest
    {
        [Theory]
        [InlineData("BlaBla", "BlaBla")]
        [InlineData(@"<ol><li>Артур рассказал про&nbsp;решения задачи &quot;"
                    +@"Проблемы с вебами на боевой&quot;</li><li>Никитос рассказал про то куда движется идея нагрузочного тестирования."
                    +@" Собрали рабочую группу.</li><li>Рома рассказал про фишки минорных версий шарпа. Решили закинуть реквест админам"
                    +@" по их вкручивание к нам.&nbsp;</li><li>Степа<ac:link><ri:attachment ri:filename=""За MustUseReturnValue замолвите"
        +@" слово.pptx"" /><ac:plain-text-link-body><![CDATA[ рассказал про атрибут аннотации MustUseReturnValue]]></ac:plain-text-link-"
        +@"body></ac:link>. Был холивар.&nbsp;</li><li>Пашка предложил делать записи вискарей и выкладывать их в публичный доступ по "
        +@"ссылке.</li></ol>", "BlaBla")]
        public void AbleToParseHtml(string source, string expected)
        {
            var actual = source.FromHtml();
            Assert.Equal(expected, actual);
        }
    }
}