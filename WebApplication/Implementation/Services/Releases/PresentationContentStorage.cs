using System;
using Storage;

namespace WebApplication.Implementation.Services.Releases
{
    public class PresentationContentStorage : IPresentationContentStorage
    {
        private readonly IKeyValueStorage keyValueStorage;

        public PresentationContentStorage(IKeyValueStorage keyValueStorage)
        {
            this.keyValueStorage = keyValueStorage;
        }

        public PresentationContent Fetch(Guid presentationId)
        {
            var content = keyValueStorage.Find<PresentationContent>(BuildKey(presentationId));
            return content ?? PresentationContent.CreateEmpty(presentationId);
        }

        public void Create(Guid presentationId, string mimeType, byte[] bytes)
        {
            var content = new PresentationContent
            {
                PresentationId = presentationId,
                Type = mimeType,
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