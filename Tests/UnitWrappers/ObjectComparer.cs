using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SKBKontur.Treller.Tests.UnitWrappers
{
    public static class ObjectComparer
    {
        private static XmlWriterSettings xmlSettings;
        private static XmlSerializerNamespaces xmlNamespace;

        public static bool AreEqual<T>(T actual, T expected)
        {
            xmlSettings = new XmlWriterSettings
            {
                Indent = true,
                Encoding = new UTF8Encoding(false),
                OmitXmlDeclaration = true
            };
            XmlSerializerNamespaces xmlNamespace = new XmlSerializerNamespaces();
            xmlNamespace.Add("", "");

            var expectedStr = ToXmlString(expected);
            var actualStr = ToXmlString(actual);
            return actualStr.Equals(expectedStr);
        }

        private static string ToXmlString<T>(T value)
        {
            var serializer = new XmlSerializer(typeof(T));
            byte[] result;
            using (var stream = new MemoryStream())
            {
                using (var writer = XmlWriter.Create(stream, xmlSettings))
                {
                    serializer.Serialize(writer, value, xmlNamespace);
                }
                result = stream.ToArray();
            }
            return Encoding.UTF8.GetString(result);
        }
    }
}
