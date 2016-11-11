using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using NUnit.Framework.Constraints;

namespace SKBKontur.Treller.Tests.UnitWrappers
{
    public class DataContractConstraint<T> : Constraint
    {
        private readonly string expectedStr;
        private static readonly XmlWriterSettings xmlSettings;
        private static readonly XmlSerializerNamespaces xmlNamespace;

        public DataContractConstraint(T expected)
        {
            expectedStr = ToXmlString(expected);
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

        public override ConstraintResult ApplyTo<TActual>(TActual actual)
        {
            var actualStr = ToXmlString(actual);
            var isMatches = string.Equals(expectedStr, actualStr);
            return new EqualConstraintResult(new EqualConstraint(expectedStr), actualStr, isMatches);
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