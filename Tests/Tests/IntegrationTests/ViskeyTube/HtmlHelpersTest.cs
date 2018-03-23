using ViskeyTube.DomainLayer.Common;
using Xunit;

namespace Tests.Tests.IntegrationTests.ViskeyTube
{
    public class HtmlHelpersTest
    {
        [Theory]
        [InlineData("BlaBla", "BlaBla")]
        [InlineData(@"<ol><li>Артур рассказал про решения задачи &quot;Проблемы с вебами на боевой&quot;</li><li>Никитос рассказал про то куда движется идея нагрузочного тестирования. "+
                    @"Собрали рабочую группу.</li><li>Рома рассказал про фишки минорных версий шарпа. Решили закинуть реквест админам по их вкручивание к нам. "+
                    @"</li><li>Степа<a href=""/download/attachments/195427133/%D0%97%D0%B0%20MustUseReturnValue%20%D0%B7%D0%B0%D0%BC%D0%BE%D0%BB%D0%B2%D0%B8%D1%82%D0%B5%"
                    +@"20%D1%81%D0%BB%D0%BE%D0%B2%D0%BE.pptx?version=1&amp;modificationDate=1511518542238&amp;api=v2"" data-linked-resource-id=""195427173"" data-linked-"+
                    @"resource-version=""1"" data-linked-resource-type=""attachment"" data-linked-resource-default-alias=""За MustUseReturnValue замолвите слово.pptx"" data-"+
                    @"nice-type=""PowerPoint Presentation"" data-linked-resource-content-type=""application/vnd.openxmlformats-officedocument.presentationml.presentation"" data-"+
                    @"linked-resource-container-id=""195427133"" data-linked-resource-container-version=""2""> рассказал про атрибут аннотации MustUseReturnValue</a>. Был холивар. "+
                    @"</li><li>Пашка предложил делать записи вискарей и выкладывать их в публичный доступ по ссылке.</li></ol>", @"
Артур рассказал про решения задачи ""Проблемы с вебами на боевой""
Никитос рассказал про то куда движется идея нагрузочного тестирования. Собрали рабочую группу.
Рома рассказал про фишки минорных версий шарпа. Решили закинуть реквест админам по их вкручивание к нам. 
Степа
 рассказал про атрибут аннотации MustUseReturnValue
. Был холивар. 
Пашка предложил делать записи вискарей и выкладывать их в публичный доступ по ссылке.
")]
        public void AbleToParseHtml(string source, string expected)
        {
            var actual = source.FromHtml();
            Assert.Equal(expected, actual);
        }
    }
}