using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace XmlFileTransferHandle
{
    public class XmlSerialize
    {
        /// <summary>
        /// 根据文件的相对路径得到序列化好的实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">文件的相对路径</param>
        /// <returns></returns>
        public static T GetSerializeResult<T>(string path)
        {
            try
            {
                var filePath = string.Format(@"{0}{1}", AppDomain.CurrentDomain.BaseDirectory, path);
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    StreamReader sr = new StreamReader(fs, Encoding.Default);
                    String xmlInfo = sr.ReadToEnd();
                    sr.Close();
                    return DeserializeXml<T>(xmlInfo);
                }
            }
            catch (Exception)
            {
                //todo 写logo
                throw;
            }
          
        }

        /// <summary>
        /// 反序列化XML为类实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlObj"></param>
        /// <returns></returns>
        private static T DeserializeXml<T>(string xmlObj)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StringReader reader = new StringReader(xmlObj))
            {
                return (T)serializer.Deserialize(reader);
            }
        }

        /// <summary>
        /// 序列化类实例为XML
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static string SerializeXml<T>(T obj)
        {
            using (StringWriter writer = new StringWriter())
            {
                new XmlSerializer(obj.GetType()).Serialize(writer, obj);
                return writer.ToString();
            }
        }
    }
}
