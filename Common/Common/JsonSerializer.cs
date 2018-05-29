using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Common
{
    /// <summary>
    /// 将类型实例序列化和反序列化为 JSON 流、字节数组或字符串
    /// </summary>
    public class JsonSerializer
    {
        public static bool Serialize(Dictionary<string, string> data, out string result)
        {
            var buffer = new StringBuilder();
            buffer.Append("{");
            foreach (var item in data)
                buffer.AppendFormat("\"{0}\":\"{1}\",", item.Key, item.Value);
            if (buffer.Length > 1)
                buffer.Remove(buffer.Length - 1, 1);
            buffer.Append("}");
            result = buffer.ToString();
            return true;
        }

        public static bool Serialize(Dictionary<string, object> data, out string result)
        {
            var buffer = new StringBuilder();
            buffer.Append("{");
            foreach (var item in data)
            {
                string value;
                if (JsonSerializer.Serialize(item.Value, out value))
                    buffer.AppendFormat("\"{0}\":{1},", item.Key, value);
                else
                    buffer.AppendFormat("\"{0}\":null,", item.Key, value);
            }
            if (buffer.Length > 1)
                buffer.Remove(buffer.Length - 1, 1);
            buffer.Append("}");
            result = buffer.ToString();
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
                if (managedObject == null)
                {
                    var buffer = Encoding.UTF8.GetBytes("null");
                    bufferStream.Write(buffer, 0, buffer.Length);
                }
                else
                {
                    var serializer = new DataContractJsonSerializer(managedObject.GetType(), knownTypes);
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

        public static bool Deserialize(string data, out Dictionary<string, string> result)
        {
            result = new Dictionary<string, string>();
            var r = new StringReader(data);
            var reader = System.Xml.XmlReader.Create(r);
            if (!reader.ReadToFollowing("data") || !reader.HasAttributes)
                return true;

            while (reader.MoveToNextAttribute())
                result.Add(reader.Name, reader.Value);
            return false;
        }

        public static bool Deserialize<T>(string jsonObject, out T value)
        {
            return Deserialize<T>(Encoding.UTF8.GetBytes(jsonObject), out value);
        }

        public static bool Deserialize<T>(byte[] jsonObject, out T value)
        {
            using (var bufferStream = new MemoryStream(jsonObject))
            {
                return Deserialize<T>(bufferStream, out value);
            }
        }

        public static bool Deserialize<T>(Stream jsonObject, out T value)
        {
            bool result;
            try
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                value = (T)serializer.ReadObject(jsonObject);
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