using System.IO;
using System.Text;
using System.Runtime.Serialization.Json;

namespace ShuttleLocator.Utils
{
    /// <summary>
    /// 指定对象与json字符串相互转换
    /// 由于功能过分限制，现决定使用Json.Net( ▼-▼ )
    /// 但会保留此工具以便日后完善
    /// </summary>
    public class JsonConverter
    {
        /// <summary>
        /// 将json字符串转换成指定的目标对象
        /// </summary>
        /// <typeparam name="T">指定对象的类型</typeparam>
        /// <param name="json">json字符串源</param>
        /// <returns>返回指定的目标对象</returns>
        public static T ReadToObject<T>(string json) where T : class, new()
        {
            var specifiedObject = new T();
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            var serializer = new DataContractJsonSerializer(specifiedObject.GetType());
            specifiedObject = serializer.ReadObject(ms) as T;
            ms.Close();
            return specifiedObject;
        }
        /// <summary>
        /// 将指定的目标对象转换成json字符串
        /// </summary>
        /// <typeparam name="T">指定对象的类型</typeparam>
        /// <param name="specifiedObject">指定的目标随想</param>
        /// <returns>返回转换后的json字符串</returns>
        public static string WriteFromObject<T>(T specifiedObject) where T : class, new()
        {
            var ms = new MemoryStream();
            var serializer = new DataContractJsonSerializer(specifiedObject.GetType());
            serializer.WriteObject(ms, specifiedObject);
            var jsonBytes = ms.ToArray();
            ms.Close();
            return Encoding.UTF8.GetString(jsonBytes, 0, jsonBytes.Length);
        }
    }
}
