using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Linq;

namespace Common
{
    /// <summary>
    /// 将类型实例序列化和反序列化为 XML 流、字节数组或字符串
    /// </summary>
    public class XmlSerializer
    {
        public static string Serialize(Dictionary<string, string> data)
        {
            var xdoc = new XDocument(new XElement("data"));
            data.AsParallel().ForAll(a =>
            {
                xdoc.Root.Add(new XAttribute(a.Key, a.Value));
            });
            return xdoc.ToString();
        }

        public static bool Serialize(Dictionary<string, string> data, out string result)
        {
            var xdoc = new XDocument(new XElement("data"));
            data.AsParallel().ForAll(a =>
            {
                xdoc.Root.Add(new XAttribute(a.Key, a.Value));
            });
            result = xdoc.ToString();
            return true;
        }

        public static bool Serialize(object managedObject, out string value, params Type[] knownTypes)
        {
            return Serialize(managedObject, out value, knownTypes.AsEnumerable());
        }

        public static bool Serialize(object managedObject, out string value, IEnumerable<Type> knownTypes)
        {
            byte[] buffer = null;
            var result = Serialize(managedObject, out buffer, knownTypes);
            if (result)
                value = Encoding.UTF8.GetString(buffer);
            else
                value = null;
            return result;
        }

        public static bool Serialize(object managedObject, out byte[] buffer, params Type[] knownTypes)
        {
            return Serialize(managedObject, out buffer, knownTypes.AsEnumerable());
        }

        public static bool Serialize(object managedObject, out byte[] buffer, IEnumerable<Type> knownTypes)
        {
            Stream bufferStream = new MemoryStream();
            var result = Serialize(managedObject, ref bufferStream, knownTypes);
            if (result)
                buffer = ((MemoryStream)bufferStream).ToArray();
            else
                buffer = null;
            bufferStream.Close();
            return result;
        }

        public static bool Serialize(object managedObject, ref Stream bufferStream, params Type[] knownTypes)
        {
            return Serialize(managedObject, ref bufferStream, knownTypes.AsEnumerable());
        }

        public static bool Serialize(object managedObject, ref Stream bufferStream, IEnumerable<Type> knownTypes)
        {
            bool result;
            try
            {
                if (managedObject != null)
                {
                    var serializer = new DataContractSerializer(managedObject.GetType(), knownTypes);
                    serializer.WriteObject(bufferStream, managedObject);
                }
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public static Dictionary<string, string> Deserialize(string data)
        {
            var xdoc = XDocument.Parse(data);
            if (xdoc.Root == null)
                return new Dictionary<string, string>();
            else
                return xdoc.Root.Attributes().ToDictionary(k => k.Name.LocalName, e => e.Value);
        }

        public static bool Deserialize(string data, out Dictionary<string, string> result)
        {
            var xdoc = XDocument.Parse(data);
            result = xdoc.Root.Attributes().ToDictionary(k => k.Name.LocalName, e => e.Value);
            return true;
        }

        public static bool Deserialize<T>(string xmlObject, out T value)
        {
            return Deserialize<T>(Encoding.UTF8.GetBytes(xmlObject), out value);
        }

        public static bool Deserialize<T>(byte[] xmlObject, out T value)
        {
            using (var bufferStream = new MemoryStream(xmlObject))
            {
                return Deserialize<T>(bufferStream, out value);
            }
        }

        public static bool Deserialize<T>(Stream xmlObject, out T value)
        {
            bool result;
            try
            {
                var serializer = new DataContractSerializer(typeof(T));
                value = (T)serializer.ReadObject(xmlObject);
                result = true;
            }
            catch
            {
                value = default(T);
                result = false;
            }
            return result;
        }
    }
}