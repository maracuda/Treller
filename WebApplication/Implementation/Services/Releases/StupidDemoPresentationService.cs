using System;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Releases
{
    public class StupidDemoPresentationService : IDemoPresentationsService
    {
        private readonly ICommentsService commentsService;
        private static readonly Guid relatedOrgsPresentationId = Guid.Parse("8007d88c-8596-4573-8ef9-eaf16b9cd157");

        public StupidDemoPresentationService(
            ICommentsService commentsService)
        {
            this.commentsService = commentsService;
        }

        public PresentationModel[] FetchPresentations(int count)
        {
            return new[]
            {
                new PresentationModel
                {
                    PresentationId = relatedOrgsPresentationId,
                    CreateDate = new DateTime(2017, 4, 1),
                    Title = "Связанные организации",
                    Content = @"Менеджер узнал, что один человек принимает решение о покупке одного продукта по нескольким организациям, хочет зафиксировать эту информацию, чтобы в будущем продлить/допродать одним звонком.
Что необходимо следать, чтобы зафиксировать данную связку в АРМ Партнера:
1. Открыть карточку клиента
2. Инициировать создание связи
3. Найти организацию, с которой нужно связать.
4. Можно указать контакт для связи
5. Можно указать продукт или несколько
6. Можно заполнить комментарий.

Редактирование связки закрывает следующие сценарии:
1. Добавить клиента в связь
2. Удалить клиента из связи
3. Удалить/изменить ключевой контакт
4. Изменить/добавить/удалить продукт связи
5. Изменить комментарий к связи
6. Удалить связь",
                    Comments = FetchComments(relatedOrgsPresentationId)
                }
            };
        }

        public Comment AppendComment(Guid presnetationId, string name, string text)
        {
            return commentsService.Append(presnetationId, name, text);
        }

        public PresentationContent DownloadPresentationConent(Guid presentationId)
        {
            return new PresentationContent
            {
                Bytes = null,
                Type = System.Net.Mime.MediaTypeNames.Image.Gif
            };
        }

        private Comment[] FetchComments(Guid presentationId)
        {
            return commentsService.Fetch(presentationId);
        }
    }
}