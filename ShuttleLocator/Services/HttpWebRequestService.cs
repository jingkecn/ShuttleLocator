using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ShuttleLocator.Services
{
    public class HttpWebRequestService
    {
        public static WebHeaderCollection Headers;
        /// <summary>
        /// 远程请求数据服务
        /// </summary>
        /// <param name="url">数据接口</param>
        /// <returns>返回相应的响应对象</returns>
        public static async Task<HttpResponseMessage> SendRequestTask(string url)
        {
            var response = new HttpResponseMessage();
            try
            {
                var client = new HttpClient();
                if (Headers != null)
                {
                    foreach (var key in Headers.AllKeys)
                    {
                        client.DefaultRequestHeaders.Add(key, Headers[key]);
                        Debug.WriteLine("==============================");
                        Debug.WriteLine("Adding header: \nKey: {0}\nValue: {1}", key, Headers[key]);
                        Debug.WriteLine("==============================");
                    }
                }
                response = await client.GetAsync(url);
            }
            catch (WebException e)
            {
                Debug.WriteLine("==============================");
                Debug.WriteLine("Web Exception occured in HttpWebRequestService: \n{0}", e.Message);
                Debug.WriteLine("==============================");
            }
            catch (Exception e)
            {
                Debug.WriteLine("==============================");
                Debug.WriteLine("Error occured in HttpWebRequestService: \n{0}", e.Message);
                Debug.WriteLine("==============================");
            }
            return response;
        } 
    }
}
