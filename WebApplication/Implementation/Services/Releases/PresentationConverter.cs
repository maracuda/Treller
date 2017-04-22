using System;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Releases
{
    public class PresentationConverter
    {
        private readonly IPresentationStorage presentationStorage;

        public PresentationConverter(
            IPresentationStorage presentationStorage)
        {
            this.presentationStorage = presentationStorage;
        }

        public void Convert()
        {
            presentationStorage.Append(
                Guid.Parse("8007d88c-8596-4573-8ef9-eaf16b9cd157"),
                new DateTime(2017, 4, 1),
                "Связанные организации",
                @"<p>Менеджер узнал, что один человек принимает решение о покупке одного продукта по нескольким организациям, хочет зафиксировать эту информацию, чтобы в будущем продлить/допродать одним звонком.</p>
<p>Что необходимо следать, чтобы зафиксировать данную связку в АРМ Партнера:</p>
<ol>
<li>Открыть карточку клиента</li>
<li>Инициировать создание связи</li>
<li>Найти организацию, с которой нужно связать</li>
<li>Можно указать контакт для связи</li>
<li>Можно указать продукт или несколько</li>
<li>Можно заполнить комментарий</li>
</ol>

<p>Редактирование связки закрывает следующие сценарии:</p>
<ol>
<li>Добавить клиента в связь</li>
<li>Удалить клиента из связи</li>
<li>Удалить/изменить ключевой контакт</li>
<li>Изменить/добавить/удалить продукт связи</li>
<li>Изменить комментарий к связи</li>
<li>Удалить связь</li>
</ol>");
        }
    }
}