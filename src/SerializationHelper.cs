using System;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace Utilities {
    public class SerializationHelper
    {
        public static string SerializeToXml<T>(T obj)
        {
            StringBuilder strBuilder = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(T));

            TextWriter writer = new StringWriter(strBuilder);
            serializer.Serialize(writer, obj);

            return strBuilder.ToString();
        }

        public static bool TrySerializeToXml<T>(T obj, out string xml) {
            try {
                xml = SerializeToXml(obj);
                return true;
            } catch (Exception e) {
                xml = e.Message;
                return false;
            }
        }

        public static T DeserializeXmlToObject<T>(string source)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            TextReader reader = new StringReader(source);
            return (T)serializer.Deserialize(reader);
        }

        public static bool TryDeserializeXmlToObject<T>(string source, out T obj) {
            try {
                obj = DeserializeXmlToObject<T>(source);
                return true;
            } catch {
                obj = default(T);
                return false;
            }
        }
    }
}
