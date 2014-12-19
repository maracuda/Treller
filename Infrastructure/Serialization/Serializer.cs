namespace SKBKontur.Infrastructure.Serialization
{
//    public class Serializer : ISerializer
//    {
//        public byte[] Serialize<T>(string serializeType, T data)
//        {
//            Validate(serializeType);
//
////            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
//            throw new I
//        }
//
//        public T Deserialize<T>(string serializeType, byte[] data)
//        {
//            Validate(serializeType);
//            return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(data));
//        }
//
//        public string Stringify<T>(string serializeType, T data)
//        {
//            Validate(serializeType);
//            return JsonConvert.SerializeObject(data, Formatting.Indented);
//        }
//
//        private static void Validate(string serializeType)
//        {
//            if (!string.Equals(serializeType, "json", StringComparison.OrdinalIgnoreCase))
//            {
//                throw new SerializationException("Can't serizalize by unknown type :" + serializeType);
//            }
//        }
//    }
}