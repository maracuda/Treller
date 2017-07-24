using System;

namespace WebApplication.Implementation.Services.Releases
{
    public interface ICommentsStorage
    {
        Comment Append(Guid presentationId, string name, string text);
        Comment[] Fetch(Guid presentationId, int page = 1, int count = 20);
    }
}