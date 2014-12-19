using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using NUnit.Framework.Constraints;

namespace SKBKontur.TestInfrastructure.UnitWrappers
{
    public class DataContractConstraint<T> : Constraint
    {
        private readonly string expectedString;
        private static XmlWriterSettings xmlSettings;
        private static XmlSerializerNamespaces xmlNamespace;

        public DataContractConstraint(T expected)
        {
            expectedString = ToXmlString(expected, typeof(T));
        }

        static DataContractConstraint()
        {
            xmlSettings = new XmlWriterSettings
                              {
                                  Indent = true,
                                  Encoding = new UTF8Encoding(false),
                                  OmitXmlDeclaration = true
                              };
            xmlNamespace = new XmlSerializerNamespaces();
            xmlNamespace.Add("", "");
        }

        public override bool Matches(object actualValue)
        {
            actual = actualValue;
            if (!(actualValue is T))
                return false;
            return expectedString == ToXmlString((T)actualValue, typeof(T));
        }

        public override void WriteDescriptionTo(MessageWriter writer)
        {
            writer.WriteExpectedValue(expectedString);
        }

        public override void WriteActualValueTo(MessageWriter writer)
        {
            if (!(actual is T))
                base.WriteActualValueTo(writer);
            else
                writer.WriteActualValue(ToXmlString((T)actual, typeof(T)));
        }

        private static string ToXmlString(object value, Type type)
        {
            var serializer = new XmlSerializer(type);
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