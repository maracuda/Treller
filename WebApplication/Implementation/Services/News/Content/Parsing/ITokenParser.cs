namespace WebApplication.Implementation.Services.News.Content.Parsing
{
    public interface ITokenParser
    {
        string TryParse(string str, string defaultValue);
    }
}