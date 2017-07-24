using System;

namespace WebApplication.Implementation.Services.Releases
{
    public class PresentationContent
    {
        public Guid PresentationId { get; set; }
        public byte[] Bytes { get; set; }
        public string Type { get; set; }

        public static PresentationContent CreateEmpty(Guid presentationId)
        {
            return new PresentationContent
            {
                PresentationId = presentationId,
                Bytes = new byte[0],
                Type = System.Net.Mime.MediaTypeNames.Text.Plain
            };
        }
    }
}