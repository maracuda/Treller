using System;
using SKBKontur.Treller.Storage;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Releases
{
    public class PresentationContentService : IPresentationContentService
    {
        private readonly IKeyValueStorage keyValueStorage;

        public PresentationContentService(IKeyValueStorage keyValueStorage)
        {
            this.keyValueStorage = keyValueStorage;
        }

        public PresentationContent Fetch(Guid presentationId)
        {
            var content = keyValueStorage.Find<PresentationContent>(BuildKey(presentationId));
            return content ?? PresentationContent.CreateEmpty(presentationId);
        }

        public void Create(Guid presentationId, string type, byte[] bytes)
        {
            var content = new PresentationContent
            {
                PresentationId = presentationId,
                Type = type,
                Bytes = bytes
            };
            keyValueStorage.Write(BuildKey(presentationId), content);
        }

        private static string BuildKey(Guid presentationId)
        {
            return $"PresentationContent_{presentationId}";
        }
    }
}