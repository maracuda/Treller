using System;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Releases
{
    public interface ICommentsService
    {
        Comment Append(Guid presentationId, string name, string text);
        Comment[] Fetch(Guid presentationId, int page = 1, int count = 20);
    }
}