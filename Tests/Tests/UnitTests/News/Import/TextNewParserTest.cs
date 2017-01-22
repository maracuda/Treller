using System;
using System.IO;
using SKBKontur.Infrastructure.Common;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Content.Parsing;
using Xunit;
using Assert = SKBKontur.Treller.Tests.UnitWrappers.Assert;

namespace SKBKontur.Treller.Tests.Tests.UnitTests.News.Import
{
    public class TextNewParserTest : UnitTest
    {
        private const string textWithMotivationAndBothNews = "**Мотивация**: в сценарии распределенной продажи клиент закрепляется в соответствие с действующими правилами за S-агентом. По услугам УЦ сценарий РП работает на договоренности, что продлять клиента должен D-агент. Необходимо описать правила и реализовать массовый перевод по всем счетам РП по услугам УЦ за период с октября 2015 по июнь 2016 года.\nДо тех пор, пока не будет реализована схема автоматической передачи на продление клиентов в схеме РП по УЦ, такие действия по массовому переводу могут быть произведены еще несколько раз с периодичностью раз в квартал.\n\n**Аналитика**: [ссылка](https://wiki.skbkontur.ru/pages/viewpage.action?pageId=145690959)\n\n**UI**: [интерфейс](https://wiki.skbkontur.ru/pages/viewpage.action?pageId=112592641)\n\n**Release**: \n\n\n**Ветка**: \n\n**Технические новости**: Произвели перевод клиентов по продуктам УЦ от обслуживающего сервисного центра к доставляющему. Перевели клиентов, у которых был выставлен счет с 01.10.2015 по 30.06.2016. Цель перевода: поддержание процесса продления клиентов по продуктам УЦ.\n\n**Новости**: Произвели перевод клиентов по продуктам УЦ от обслуживающего сервисного центра к доставляющему. Перевели клиентов, у которых был выставлен счет с 01.10.2015 по 30.06.2016. Цель перевода: поддержание процесса продления клиентов по продуктам УЦ.\n\n**Обратная связь**:\n\n- Обратную связь от пользователей можно оперативно собрать через Таня Зонову (спросить решилась ли проблема)\n- Объективная обратная связь это процент продлений по УЦ, статистика будет только в ноябре 2016 ";
        private const string textWithMotivationAndLastTechNew = "**Мотивация**: в сценарии распределенной продажи клиент закрепляется в соответствие с действующими правилами за S-агентом. По услугам УЦ сценарий РП работает на договоренности, что продлять клиента должен D-агент. Необходимо описать правила и реализовать массовый перевод по всем счетам РП по услугам УЦ за период с октября 2015 по июнь 2016 года.\nДо тех пор, пока не будет реализована схема автоматической передачи на продление клиентов в схеме РП по УЦ, такие действия по массовому переводу могут быть произведены еще несколько раз с периодичностью раз в квартал.\n\n**Аналитика**: [ссылка](https://wiki.skbkontur.ru/pages/viewpage.action?pageId=145690959)\n\n**UI**: [интерфейс](https://wiki.skbkontur.ru/pages/viewpage.action?pageId=112592641)\n\n**Release**: \n\n\n**Ветка**: \n\n**Технические новости**: Произвели перевод клиентов по продуктам УЦ от обслуживающего сервисного центра к доставляющему. Перевели клиентов, у которых был выставлен счет с 01.10.2015 по 30.06.2016. Цель перевода: поддержание процесса продления клиентов по продуктам УЦ.";
        private const string textWithNewLinesAtTechNew = "**Технические новости**: Произвели перевод клиентов по продуктам УЦ от обслуживающего сервисного центра к доставляющему.\nПеревели клиентов, у которых был выставлен счет с 01.10.2015 по 30.06.2016. Цель перевода: поддержание процесса продления клиентов по продуктам УЦ.";

        private static readonly ContentParser contentParser = new ContentParser(new TokenParserFactory(),
            new DateTimeFactory());

        [Theory]
        [InlineData("Просто какой-то текст", "")]
        [InlineData(textWithMotivationAndBothNews, "Произвели перевод клиентов по продуктам УЦ от обслуживающего сервисного центра к доставляющему. Перевели клиентов, у которых был выставлен счет с 01.10.2015 по 30.06.2016. Цель перевода: поддержание процесса продления клиентов по продуктам УЦ.")]
        [InlineData(textWithMotivationAndLastTechNew, "Произвели перевод клиентов по продуктам УЦ от обслуживающего сервисного центра к доставляющему. Перевели клиентов, у которых был выставлен счет с 01.10.2015 по 30.06.2016. Цель перевода: поддержание процесса продления клиентов по продуктам УЦ.")]
        [InlineData(textWithNewLinesAtTechNew, "Произвели перевод клиентов по продуктам УЦ от обслуживающего сервисного центра к доставляющему.\nПеревели клиентов, у которых был выставлен счет с 01.10.2015 по 30.06.2016. Цель перевода: поддержание процесса продления клиентов по продуктам УЦ.")]
        public void TestParseTechInfo(string desc, string expectedTechInfo)
        {
            var actual = contentParser.Parse(Guid.Empty, string.Empty, desc, null).TechInfo;
            Assert.AreEqual(expectedTechInfo, actual);
        }

        [Theory]
        [InlineData("Просто какой-то текст", "")]
        [InlineData(textWithMotivationAndBothNews, "Произвели перевод клиентов по продуктам УЦ от обслуживающего сервисного центра к доставляющему. Перевели клиентов, у которых был выставлен счет с 01.10.2015 по 30.06.2016. Цель перевода: поддержание процесса продления клиентов по продуктам УЦ.")]
        public void TestParsePublicInfo(string desc, string expectedPublicInfo)
        {
            var actual = contentParser.Parse(Guid.Empty, string.Empty, desc, null).PubicInfo;
            Assert.AreEqual(expectedPublicInfo, actual);
        }

        [Fact]
        public void TestParsePublicInfoForAlternativeFormat()
        {
            var desc = GetNewsData("BigNewWithComplexSeparator.txt");
            var expected = GetNewsData("BigNewWithComplexSeparatorExpected.txt");
            var actual = contentParser.Parse(Guid.Empty, string.Empty, desc, null).PubicInfo;
            Assert.AreEqual(expected, actual);
        }

        private static string GetNewsData(string fileName)
        {
            var pathToTests = AppDomain.CurrentDomain.BaseDirectory;
            var rootPath = Path.GetPathRoot(pathToTests);
            while (!pathToTests.EndsWith("Tests\\") && !string.Equals(rootPath, pathToTests))
            {
                pathToTests = Path.GetFullPath(Path.Combine(pathToTests, "..\\"));
            }

            var pathToFile = Path.Combine(pathToTests, "Tests", "UnitTests", "TestData", "NewsExamples", fileName);
            return File.ReadAllText(pathToFile);
        }
    }
}