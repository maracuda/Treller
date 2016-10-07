using System;
using System.IO;
using NUnit.Framework;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Import;

namespace SKBKontur.Treller.Tests.Tests.UnitTests.News.Import
{
    public class TextNewParserTest : UnitTest
    {
        private const string textWithMotivationAndBothNews = "**Мотивация**: в сценарии распределенной продажи клиент закрепляется в соответствие с действующими правилами за S-агентом. По услугам УЦ сценарий РП работает на договоренности, что продлять клиента должен D-агент. Необходимо описать правила и реализовать массовый перевод по всем счетам РП по услугам УЦ за период с октября 2015 по июнь 2016 года.\nДо тех пор, пока не будет реализована схема автоматической передачи на продление клиентов в схеме РП по УЦ, такие действия по массовому переводу могут быть произведены еще несколько раз с периодичностью раз в квартал.\n\n**Аналитика**: [ссылка](https://wiki.skbkontur.ru/pages/viewpage.action?pageId=145690959)\n\n**UI**: [интерфейс](https://wiki.skbkontur.ru/pages/viewpage.action?pageId=112592641)\n\n**Release**: \n\n\n**Ветка**: \n\n**Технические новости**: Произвели перевод клиентов по продуктам УЦ от обслуживающего сервисного центра к доставляющему. Перевели клиентов, у которых был выставлен счет с 01.10.2015 по 30.06.2016. Цель перевода: поддержание процесса продления клиентов по продуктам УЦ.\n\n**Новости**: Произвели перевод клиентов по продуктам УЦ от обслуживающего сервисного центра к доставляющему. Перевели клиентов, у которых был выставлен счет с 01.10.2015 по 30.06.2016. Цель перевода: поддержание процесса продления клиентов по продуктам УЦ.\n\n**Обратная связь**:\n\n- Обратную связь от пользователей можно оперативно собрать через Таня Зонову (спросить решилась ли проблема)\n- Объективная обратная связь это процент продлений по УЦ, статистика будет только в ноябре 2016 ";
        private const string textWithMotivationAndLastTechNew = "**Мотивация**: в сценарии распределенной продажи клиент закрепляется в соответствие с действующими правилами за S-агентом. По услугам УЦ сценарий РП работает на договоренности, что продлять клиента должен D-агент. Необходимо описать правила и реализовать массовый перевод по всем счетам РП по услугам УЦ за период с октября 2015 по июнь 2016 года.\nДо тех пор, пока не будет реализована схема автоматической передачи на продление клиентов в схеме РП по УЦ, такие действия по массовому переводу могут быть произведены еще несколько раз с периодичностью раз в квартал.\n\n**Аналитика**: [ссылка](https://wiki.skbkontur.ru/pages/viewpage.action?pageId=145690959)\n\n**UI**: [интерфейс](https://wiki.skbkontur.ru/pages/viewpage.action?pageId=112592641)\n\n**Release**: \n\n\n**Ветка**: \n\n**Технические новости**: Произвели перевод клиентов по продуктам УЦ от обслуживающего сервисного центра к доставляющему. Перевели клиентов, у которых был выставлен счет с 01.10.2015 по 30.06.2016. Цель перевода: поддержание процесса продления клиентов по продуктам УЦ.";
        private const string textWithNewLinesAtTechNew = "**Технические новости**: Произвели перевод клиентов по продуктам УЦ от обслуживающего сервисного центра к доставляющему.\nПеревели клиентов, у которых был выставлен счет с 01.10.2015 по 30.06.2016. Цель перевода: поддержание процесса продления клиентов по продуктам УЦ.";

        [Test]
        [TestCase("Просто какой-то текст", false, "")]
        [TestCase(textWithMotivationAndBothNews, true, "Произвели перевод клиентов по продуктам УЦ от обслуживающего сервисного центра к доставляющему. Перевели клиентов, у которых был выставлен счет с 01.10.2015 по 30.06.2016. Цель перевода: поддержание процесса продления клиентов по продуктам УЦ.")]
        [TestCase(textWithMotivationAndLastTechNew, true, "Произвели перевод клиентов по продуктам УЦ от обслуживающего сервисного центра к доставляющему. Перевели клиентов, у которых был выставлен счет с 01.10.2015 по 30.06.2016. Цель перевода: поддержание процесса продления клиентов по продуктам УЦ.")]
        [TestCase(textWithNewLinesAtTechNew, true, "Произвели перевод клиентов по продуктам УЦ от обслуживающего сервисного центра к доставляющему.\nПеревели клиентов, у которых был выставлен счет с 01.10.2015 по 30.06.2016. Цель перевода: поддержание процесса продления клиентов по продуктам УЦ.")]
        public void TestParseSupportNews(string text, bool isParsedSuccesfully, string expectedNewText)
        {
            var actual = new SupportNewsTextParser().TryParse(text);
            Assert.AreEqual(isParsedSuccesfully, actual.HasValue);
            if (isParsedSuccesfully)
            {
                Assert.AreEqual(expectedNewText, actual.Value);
            }
        }

        [Test]
        [TestCase("Просто какой-то текст", true, "Всем доброго времени суток.\r\nКомадна Биллинга только что доставила огненный релиз на боевые.\r\n")]
        [TestCase(textWithMotivationAndBothNews, true, "Всем доброго времени суток.\r\nКомадна Биллинга только что доставила огненный релиз на боевые.\r\nНемного о задаче: в сценарии распределенной продажи клиент закрепляется в соответствие с действующими правилами за S-агентом. По услугам УЦ сценарий РП работает на договоренности, что продлять клиента должен D-агент. Необходимо описать правила и реализовать массовый перевод по всем счетам РП по услугам УЦ за период с октября 2015 по июнь 2016 года.\nДо тех пор, пока не будет реализована схема автоматической передачи на продление клиентов в схеме РП по УЦ, такие действия по массовому переводу могут быть произведены еще несколько раз с периодичностью раз в квартал.\r\nПодробнее читайте здесь: [ссылка](https://wiki.skbkontur.ru/pages/viewpage.action?pageId=145690959)")]
        public void TestParseTeamNews(string text, bool isParsedSuccesfully, string expectedNewText)
        {
            var actual = new TeamNewsTextParser().TryParse(text);
            Assert.AreEqual(isParsedSuccesfully, actual.HasValue);
            if (isParsedSuccesfully)
            {
                Assert.AreEqual(expectedNewText, actual.Value);
            }
        }

        [Test]
        [TestCase("Просто какой-то текст", false, "")]
        [TestCase(textWithMotivationAndBothNews, true, "Произвели перевод клиентов по продуктам УЦ от обслуживающего сервисного центра к доставляющему. Перевели клиентов, у которых был выставлен счет с 01.10.2015 по 30.06.2016. Цель перевода: поддержание процесса продления клиентов по продуктам УЦ.")]
        public void TestParseCustomerNews(string text, bool isParsedSuccesfully, string expectedNewText)
        {
            var actual = new CustomerNewsTextParser().TryParse(text);
            Assert.AreEqual(isParsedSuccesfully, actual.HasValue);
            if (isParsedSuccesfully)
            {
                Assert.AreEqual(expectedNewText, actual.Value);
            }
        }

        [Test]
        public void TestParseCustomerNewsForAlternativeFormat()
        {
            var text = GetNewsData("BigNewWithComplexSeparator.txt");
            var expected = GetNewsData("BigNewWithComplexSeparatorExpected.txt");
            var actual = new CustomerNewsTextParser().TryParse(text);
            Assert.AreEqual(true, actual.HasValue);
            Assert.AreEqual(expected, actual.Value);
        }

        private static string GetNewsData(string fileName)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tests", "UnitTests", "TestData", "NewsExamples", fileName);
            return File.ReadAllText(path);
        }
    }
}