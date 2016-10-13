using Newtonsoft.Json;

namespace SKBKontur.Treller.Tests.Tests
{
    public static class StringExtensions
    {
        public static string Stringify(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}