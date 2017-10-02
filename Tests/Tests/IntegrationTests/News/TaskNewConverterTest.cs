using System;
using System.Linq;
using TaskManagerClient.BusinessObjects.TaskManager;
using WebApplication.Implementation.Services.News.Reporters;
using Xunit;

namespace Tests.Tests.IntegrationTests.News
{
    public class TaskNewConverterTest : IntegrationTest
    {
        private const string realCardDescription = "**Мотивация**: в сценарии распределенной продажи клиент закрепляется в соответствие с действующими правилами за S-агентом. По услугам УЦ сценарий РП работает на договоренности, что продлять клиента должен D-агент. Необходимо описать правила и реализовать массовый перевод по всем счетам РП по услугам УЦ за период с октября 2015 по июнь 2016 года.\nДо тех пор, пока не будет реализована схема автоматической передачи на продление клиентов в схеме РП по УЦ, такие действия по массовому переводу могут быть произведены еще несколько раз с периодичностью раз в квартал.\n\n**Аналитика**: [ссылка](https://wiki.skbkontur.ru/pages/viewpage.action?pageId=145690959)\n\n**UI**: [интерфейс](https://wiki.skbkontur.ru/pages/viewpage.action?pageId=112592641)\n\n**Release**: \n\n\n**Ветка**: \n\n**Технические новости**: Произвели перевод клиентов по продуктам УЦ от обслуживающего сервисного центра к доставляющему. Перевели клиентов, у которых был выставлен счет с 01.10.2015 по 30.06.2016. Цель перевода: поддержание процесса продления клиентов по продуктам УЦ.\n\n**Новости**: Произвели перевод клиентов по продуктам УЦ от обслуживающего сервисного центра к доставляющему. Перевели клиентов, у которых был выставлен счет с 01.10.2015 по 30.06.2016. Цель перевода: поддержание процесса продления клиентов по продуктам УЦ.\n\n**Обратная связь**:\n\n- Обратную связь от пользователей можно оперативно собрать через Таня Зонову (спросить решилась ли проблема)\n- Объективная обратная связь это процент продлений по УЦ, статистика будет только в ноябре 2016 ";
        private const string cardDesciptionWithEmptyNewText = "**Мотивация**: в сценарии распределенной продажи клиент закрепляется в соответствие с действующими правилами за S-агентом. По услугам УЦ сценарий РП работает на договоренности, что продлять клиента должен D-агент. Необходимо описать правила и реализовать массовый перевод по всем счетам РП по услугам УЦ за период с октября 2015 по июнь 2016 года.\nДо тех пор, пока не будет реализована схема автоматической передачи на продление клиентов в схеме РП по УЦ, такие действия по массовому переводу могут быть произведены еще несколько раз с периодичностью раз в квартал.\n\n**Аналитика**: [ссылка](https://wiki.skbkontur.ru/pages/viewpage.action?pageId=145690959)\n\n**UI**: [интерфейс](https://wiki.skbkontur.ru/pages/viewpage.action?pageId=112592641)\n\n**Release**: \n\n\n**Ветка**: \n\n**Технические новости**: \n\n**Новости**: \n\n**Обратная связь**:\n\n- Обратную связь от пользователей можно оперативно собрать через Таня Зонову (спросить решилась ли проблема)\n- Объективная обратная связь это процент продлений по УЦ, статистика будет только в ноябре 2016 ";

        private readonly ITaskNewConverter taskNewConverter;

        public TaskNewConverterTest()
        {
            taskNewConverter = container.Get<ITaskNewConverter>();
        }

        [Theory]
        [InlineData(realCardDescription, 3)]
        [InlineData(cardDesciptionWithEmptyNewText, 1)]
        public void TestConvert(string description, int expectedReportsCount)
        {
            var boardList = new BoardList(null, "someboard", null, new[]
            {
                new BoardListCardInfo
                {
                    Id = "cardId",
                    Name = "Some task",
                    Desc = description,
                    Due = DateTime.Now
                }
            });
            var actuals = taskNewConverter.Convert(boardList);
            Assert.Equal(1, actuals.Count);
            Assert.Equal(expectedReportsCount, actuals.First().Reports.Length);
        }
    }
}