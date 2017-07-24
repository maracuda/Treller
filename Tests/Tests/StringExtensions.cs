using Serialization;

namespace Tests.Tests
{
    public static class StringExtensions
    {
        private static readonly IJsonSerializer jsonSerializer = new JsonSerializer();

        public static string Stringify(this object obj)
        {
            return jsonSerializer.Serialize(obj);
        }
    }
}