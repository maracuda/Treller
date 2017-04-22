using System;
using System.IO;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Releases
{
    public class PresentationConverter
    {
        private readonly IPresentationStorage presentationStorage;
        private readonly IPresentationContentStorage presentationContentStorage;

        public PresentationConverter(
            IPresentationStorage presentationStorage,
            IPresentationContentStorage presentationContentStorage)
        {
            this.presentationStorage = presentationStorage;
            this.presentationContentStorage = presentationContentStorage;
        }

        public void Convert()
        {
            var presentationID = Guid.Parse("0634ce0d-2054-424a-868e-b1b4512aceab");

            presentationStorage.Append(
                presentationID,
                new DateTime(2017, 4, 24),
                "Трамвайчики: Конечный получатель (КП) в мастере выставления счета",
                @"<p><b>Мотивация</b> В КЭ около 3% счетов выставляется по схеме конечный получатель отличный отплательщика. Исследования показали, что счета равномерно распределены по дате выставления в течение года. Более того, юристы настаивают на реализации этой схемы для расширения пилота КЭ.</p>
<p><b>Сценарии</b></p>
<ol>
<li>Сценарий выставления счета в Партнерке. Появился опциональный шаг, на котором можно указать реквизиты КП. По умолчанию, счет выставляется на плательщика, так как это частотный сценарий.</li>
<li>Доставка сертификата. По тарифам с выбранным КП доставка осуществляется ао реквизитам КП (нельзя указать произвольные реквизиты).</li>
<li>Доставка подписки. По тарифам с выбранным КП доставка осуществляется на лицевой счет с реквизитами, совпадающими с реквизитами КП.</li>
<li>Сценарии бронирования и печати документов не были модифицированы.</li>
</ol>
<p><b>Технические подробности</b></p>
<ol>
<li>Возможность работать по схеме КП настраивается в тарифе.</li>
<li>В базовую инфраструктуру проекта добавлен сервис абонентов. Миссия сущности абонента: единая точка получения товаров и услуг. Сервис абонентов инкапсулирует различные системы хранения реквизитов КП.</li>
<li>В базовую инфраструктуру проекта добавлен сервис адресов. Сервис адресов инкапсулирует связь между абонентом и лицевыми счетами продуктов (эта возможность гарантирует доставку товаров). Сервис адресов инкапсулирует взаимодействие с порталом.</li>
</ol>
<p><b>Статистика</b> В 20 из 4875 счетов указан КП отличный от плтельщика. ООРВ боится массово рассказывать про эту возможность. </p>
");
            var bytes = File.ReadAllBytes("C:\\1.gif");
            presentationContentStorage.Create(presentationID, System.Net.Mime.MediaTypeNames.Image.Gif, bytes);
        }
    }
}