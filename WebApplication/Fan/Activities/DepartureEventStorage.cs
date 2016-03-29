using System;
using System.Collections.Generic;
using System.Linq;

namespace SKBKontur.Treller.WebApplication.Fan.Activities
{
    public class DepartureEventStorage : IDepartureEventStorage
    {
        private readonly EventViewModel[] eventStorage;

        public DepartureEventStorage()
        {
            eventStorage = new[]
            {
                CreateEvent("Выезд Контур.Биллинг 25-26 марта в Вилла Богема", new DateTime(2016, 3, 25),
                    new Activity("Приветственный кофе", "можно будет налить себе кофе/чай/воду, будет минимальный набор печенок", new ActivityItem(new TimeSpan(10, 0, 0), "", ActivityFormat.Empty)),
                    new Activity("Открытие выезда", "описание цели выезда, активностей, плана и погружение в первую активность – тренинг", new ActivityItem(new TimeSpan(10, 15, 0), "Толя", ActivityFormat.Game)),
                    new Activity("Тренинг по фасилитации", "примерно о чем: http://scrumtrek.ru/trainings/view/65/. Цель: научить каждого базовым принципам фасилитации. Будет много активностей на ногах в формате игры!",
                        new ActivityItem(new TimeSpan(10, 20, 0), "Толя", ActivityFormat.Empty, "Открытие тренинга", "План, цели и что будем делать"),
                        new ActivityItem(new TimeSpan(10, 30, 0), "Толя", ActivityFormat.Game, "Часть 1", "Вводная по фасилитации, понимание структуры митинга, практика по нескольким техникам"),
                        new ActivityItem(new TimeSpan(11, 20, 0), "", ActivityFormat.Empty, "Кофе-брейк", "нужно проветрить, дополнительно принесут какие-то плюшки пожевать"),
                        new ActivityItem(new TimeSpan(11, 30, 0), "Толя", ActivityFormat.Game, "Часть 2", "Закрепляем материал, каждый попробует себя в роли фасилитатора"),
                        new ActivityItem(new TimeSpan(12, 20, 0), "", ActivityFormat.Empty, "Перерыв", "желательно проветрить помещение, всем погулять, развеяться, можно налить чай/кофе"),
                        new ActivityItem(new TimeSpan(12, 30, 0), "Толя", ActivityFormat.Game, "Подводим итоги тренинга", "Разбор парковки и ретроспектива тренинга")
                        ),
                    new Activity("Обед", "В ресторане", new ActivityItem(new TimeSpan(12, 45, 0), "", ActivityFormat.Empty)),
                    new Activity("Ожидания от нашего проекта", "Ирина в формате доклада-дискуссии расскажет какие ожидания есть у наших заказчиков по нашему проекту", new ActivityItem(new TimeSpan(13, 20, 0), "Ирина", ActivityFormat.PresentationWithDiscussion)),
                    new Activity("Перерыв", "желательно проветрить помещение, всем погулять, развеяться, можно налить чай/кофе", new ActivityItem(new TimeSpan(14, 40, 0), "", ActivityFormat.Empty)),
                    new Activity("Круглый стол", "обсуждаем фундаментальные вопросы любого характера в безопасной среде (например: что делают менеджеры) Цель: Найти дальнейшие пути развития команды",
                        new ActivityItem(new TimeSpan(14, 50, 0), "Толя", ActivityFormat.Discussion, "Часть 1", ""),
                        new ActivityItem(new TimeSpan(15, 40, 0), "", ActivityFormat.Empty, "Кофе-брейк", "желательно проветрить, дополнительые плюшки и пирожки"),
                        new ActivityItem(new TimeSpan(15, 50, 0), "Толя", ActivityFormat.Discussion, "Часть 2", "")
                        ),
                    new Activity("Перерыв", "Неформальное обсуждение итогов круглого стола, проветриваем, отдыхаем", new ActivityItem(new TimeSpan(16, 40, 0), "", ActivityFormat.Empty)),
                    new Activity("Большой переезд", "по мотивам переезда Контур-Экстерна в новый биллинг, Аня расскажет нам какие есть ожидания по ту сторону: состояние заказчика, его пожелания, планы и т.д.",
                        new ActivityItem(new TimeSpan(16, 50, 0), "Аня Бороздина", ActivityFormat.Presentation)),
                    new Activity("Закрытие", "Окончание общей части, ретроспектива и секретная часть!", new ActivityItem(new TimeSpan(17, 50, 0), "Толя", ActivityFormat.Empty)),
                    new Activity("Ужин", "В ресторане", new ActivityItem(new TimeSpan(18, 00, 0), "", ActivityFormat.Empty)),
                    new Activity("Общение по секциям", "Время для встречи функциональными группами: бэк-разработчиками, тестировщиками и тд", new ActivityItem(new TimeSpan(18, 30, 0), "", ActivityFormat.NotSelectedYet)),
                    new Activity("Шашлыки", "вся вилла наша(!), там будет сауна, бассейн, баня (нужно взять купальники), беседка для мяса, полное описание, что можно заюзать тут: http://www.villabogema.ru/#!arendavilla/c1ggj ",
                        new ActivityItem(new TimeSpan(20, 0, 0), "", ActivityFormat.NotSelectedYet) { FinishTime = new TimeSpan(27, 0, 0) }),
                    // Суббота
                    new Activity("Завтрак", "В ресторане", new ActivityItem(new TimeSpan(33, 30, 0), "", ActivityFormat.Empty)),
                    new Activity("Лошади", "в 11:00 собираемся у главного входа (записываемся на вики, если хотим кататься). Нужно взять правильную одежду, т.к. лошадки не очень чистые", new ActivityItem(new TimeSpan(35, 00, 0), "", ActivityFormat.Empty))
                    )
            };
        }

        public EventViewModel GetNextEvent()
        {
            return eventStorage.OrderBy(x => x.EventStartDate).Last();
        }

        private static EventViewModel CreateEvent(string eventName, DateTime eventDate, params Activity[] activities)
        {
            ActivityItem lastItem = null;
            foreach (var item in activities.SelectMany(x => x.Items))
            {
                if (lastItem != null && lastItem.FinishTime == TimeSpan.Zero)
                {
                    lastItem.FinishTime = item.StartTime;
                }

                lastItem = item;
            }

            var activitiesByDate = activities
                .GroupBy(x => (int)(x.Items[0].StartTime.TotalHours / 24))
                .Select(x => new KeyValuePair<DateTime, Activity[]>(eventDate.AddDays(x.Key), x.ToArray()))
                .ToArray();

            return new EventViewModel
            {
                EventName = eventName,
                EventStartDate = eventDate,
                ActivitiesByDate = activitiesByDate
            };
        }
    }
}