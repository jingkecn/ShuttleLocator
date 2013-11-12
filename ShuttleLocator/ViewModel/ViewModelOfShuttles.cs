using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ShuttleLocator.Common;
using ShuttleLocator.Model;
using ShuttleLocator.Services;
using ShuttleLocator.Utils;

namespace ShuttleLocator.ViewModel
{
    /// <summary>
    /// 获取并解析校巴状态数据集
    /// </summary>
    public class ViewModelOfShuttles:ViewModel<Shuttle>
    {
        public ObservableCollection<Shuttle> Shuttles; 
        /// <summary>
        /// 获取校巴状态信息数据对象
        /// </summary>
        public override async Task<ObservableCollection<Shuttle>> GetDataModel()
        {
            ObservableCollection<Shuttle> shuttles;
            if (CheckNetworkState.IsNetworkAvailable())
            {
                shuttles = await GetUpdatedData();
            }
            else
            {
                shuttles = GetDefaultData();
            }
            Shuttles = shuttles;
            return shuttles;
        }
        /// <summary>
        /// 更新校巴状态数据
        /// </summary>
        protected override async Task<ObservableCollection<Shuttle>> GetUpdatedData()
        {
            ObservableCollection<Shuttle> shuttles;
            try
            {
                var response = await GetResult();
                var result = response.Content.ReadAsStringAsync().Result;
                Debug.WriteLine("==============================");
                Debug.WriteLine("GetUpdatedData.Result: {0}", result);
                Debug.WriteLine("==============================");
                shuttles = result == "" ? GetDefaultData() : ParseData(result);
            }
            catch (Exception e)
            {
                shuttles = GetDefaultData();
                Debug.WriteLine("==============================");
                Debug.WriteLine("Error ocurred in GetUpdatedData from VMShuttles: {0}", e.Message);
                Debug.WriteLine("==============================");
            }
            return shuttles;
        }
        /// <summary>
        /// 获取校巴状态初值
        /// </summary>
        protected override ObservableCollection<Shuttle> GetDefaultData()
        {
            return default (ObservableCollection<Shuttle>);
        }
        /// <summary>
        /// 获取原始数据字符串
        /// </summary>
        /// <returns>原始数据字符串</returns>
        protected override async Task<HttpResponseMessage> GetResult()
        {
            var response = new HttpResponseMessage();
            try
            {
                response = await HttpWebRequestService.SendRequestTask(DataUrls.ShuttleDataUrl);
            }
            catch (Exception e)
            {
                Debug.WriteLine("==============================");
                Debug.WriteLine("Error occurred in GetResult from VMShuttles: {0}", e.Message);
                Debug.WriteLine("==============================");
            }
            return response;
        }
        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="result">原始数据字符串</param>
        /// <returns>重构后的数据集</returns>
        protected override ObservableCollection<Shuttle> ParseData(string result)
        {
            var shuttles = new ObservableCollection<Shuttle>();
            try
            {
                var objs = JObject.Parse(result);
                foreach (var obj in objs)
                {
                    //剔除测试数据
                    if (!(new Regex("^BUS[0-9]$")).IsMatch(obj.Key)) continue;
                    shuttles.Add(new Shuttle
                    {
                        #region 设置Shuttle属性
                        Name = (string)objs[obj.Key]["Name"],
                        Latitude = (double)objs[obj.Key]["Latitude"],
                        Longitude = (double)objs[obj.Key]["Longitude"],
                        Direction = (bool)objs[obj.Key]["Direction"],
                        Time = (long)objs[obj.Key]["Time"],
                        Position = (string)objs[obj.Key]["Station"],
                        PositionIndex = (int)objs[obj.Key]["StationIndex"],
                        Progress = (double)objs[obj.Key]["Percent"],
                        IsStopping = (bool)objs[obj.Key]["Stop"],
                        IsFlying = (bool)objs[obj.Key]["Fly"]
                        #endregion
                    });
                }
            }
            catch (Exception e)
            {
                shuttles = GetDefaultData();
                Debug.WriteLine("==============================");
                Debug.WriteLine("Error occurred in ParseData from VMShuttles: {0}", e.Message);
                Debug.WriteLine("==============================");
            }
            return shuttles;
        }
    }
}
