using System;
using System.IO;
using SKBKontur.Treller.WebApplication.Implementation.Services.Releases;
using Xunit;

namespace SKBKontur.Treller.Tests.Tests.IntegrationTests.Storages
{
    public class KeyValueStorageTest : IntegrationTest
    {
        private readonly IPresentationContentService presentationContentService;

        public KeyValueStorageTest()
        {
            presentationContentService = container.Create<IPresentationContentService>();
        }

        [Fact]
        public void CreatePresentationContentForFirstPresentation()
        {
            var bytes = File.ReadAllBytes("C:\\kontur\\zzz.gif");
            presentationContentService.Create(Guid.Parse("8007d88c-8596-4573-8ef9-eaf16b9cd157"), System.Net.Mime.MediaTypeNames.Image.Gif, bytes);
        }
    }
}