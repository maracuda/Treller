using System;
using NUnit.Framework;
using SKBKontur.TaskManagerClient.BusinessObjects.TaskManager;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Import;

namespace SKBKontur.Treller.Tests.Tests.IntegrationTests.News
{
    public class TaskNewConverterTest : IntegrationTest
    {
        private const string realCardDescription = "**Мотивация**: в сценарии распределенной продажи клиент закрепляется в соответствие с действующими правилами за S-агентом. По услугам УЦ сценарий РП работает на договоренности, что продлять клиента должен D-агент. Необходимо описать правила и реализовать массовый перевод по всем счетам РП по услугам УЦ за период с октября 2015 по июнь 2016 года.\nДо тех пор, пока не будет реализована схема автоматической передачи на продление клиентов в схеме РП по УЦ, такие действия по массовому переводу могут быть произведены еще несколько раз с периодичностью раз в квартал.\n\n**Аналитика**: [ссылка](https://wiki.skbkontur.ru/pages/viewpage.action?pageId=145690959)\n\n**UI**: [интерфейс](https://wiki.skbkontur.ru/pages/viewpage.action?pageId=112592641)\n\n**Release**: \n\n\n**Ветка**: \n\n**Технические новости**: Произвели перевод клиентов по продуктам УЦ от обслуживающего сервисного центра к доставляющему. Перевели клиентов, у которых был выставлен счет с 01.10.2015 по 30.06.2016. Цель перевода: поддержание процесса продления клиентов по продуктам УЦ.\n\n**Новости**: Произвели перевод клиентов по продуктам УЦ от обслуживающего сервисного центра к доставляющему. Перевели клиентов, у которых был выставлен счет с 01.10.2015 по 30.06.2016. Цель перевода: поддержание процесса продления клиентов по продуктам УЦ.\n\n**Обратная связь**:\n\n- Обратную связь от пользователей можно оперативно собрать через Таня Зонову (спросить решилась ли проблема)\n- Объективная обратная связь это процент продлений по УЦ, статистика будет только в ноябре 2016 ";
        private const string cardDesciptionWithEmptyNewText = "**Мотивация**: в сценарии распределенной продажи клиент закрепляется в соответствие с действующими правилами за S-агентом. По услугам УЦ сценарий РП работает на договоренности, что продлять клиента должен D-агент. Необходимо описать правила и реализовать массовый перевод по всем счетам РП по услугам УЦ за период с октября 2015 по июнь 2016 года.\nДо тех пор, пока не будет реализована схема автоматической передачи на продление клиентов в схеме РП по УЦ, такие действия по массовому переводу могут быть произведены еще несколько раз с периодичностью раз в квартал.\n\n**Аналитика**: [ссылка](https://wiki.skbkontur.ru/pages/viewpage.action?pageId=145690959)\n\n**UI**: [интерфейс](https://wiki.skbkontur.ru/pages/viewpage.action?pageId=112592641)\n\n**Release**: \n\n\n**Ветка**: \n\n**Технические новости**: \n\n**Новости**: \n\n**Обратная связь**:\n\n- Обратную связь от пользователей можно оперативно собрать через Таня Зонову (спросить решилась ли проблема)\n- Объективная обратная связь это процент продлений по УЦ, статистика будет только в ноябре 2016 ";

        private ITaskNewConverter taskNewConverter;

        public override void SetUp()
        {
            base.SetUp();

            taskNewConverter = container.Get<ITaskNewConverter>();
        }

        [Test]
        [TestCase(realCardDescription, 3)]
        [TestCase(cardDesciptionWithEmptyNewText, 1)]
        public void TestConvert(string description, int expectedNewsCount)
        {
            var cardInfo = new BoardListCardInfo
            {
                Id = "cardId",
                Name = "Some task",
                Desc = description,
                Due = DateTime.Now
            };
            var actuals = taskNewConverter.Convert("someboard", cardInfo);
            Assert.AreEqual(expectedNewsCount, actuals.Count);
        }
    }
}