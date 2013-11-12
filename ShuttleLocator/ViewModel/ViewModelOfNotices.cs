using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO.IsolatedStorage;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ShuttleLocator.Common;
using ShuttleLocator.Model;
using ShuttleLocator.Services;
using ShuttleLocator.Utils;

namespace ShuttleLocator.ViewModel
{
    public class ViewModelOfNotices:ViewModel<Notice>
    {
        /// <summary>
        /// 获取并解析通知公告数据集
        /// </summary>
        public ObservableCollection<Notice> Notices;
        /// <summary>
        /// 获取站点信息数据对象
        /// </summary>
        public override async Task<ObservableCollection<Notice>> GetDataModel()
        {
            ObservableCollection<Notice> notices;
            if (CheckNetworkState.IsNetworkAvailable())
            {
                notices = await GetUpdatedData();
            }
            else
            {
                notices = GetDefaultData();
            }
            Notices = notices;
            return notices;
        }
        /// <summary>
        /// 更新通知公告数据
        /// </summary>
        protected override async Task<ObservableCollection<Notice>> GetUpdatedData()
        {
            ObservableCollection<Notice> notices;
            try
            {
                var response = await GetResult();
                var result = response.Content.ReadAsStringAsync().Result;
                Debug.WriteLine("==============================");
                Debug.WriteLine("GetUpdatedData.Result: \n{0}", result);
                Debug.WriteLine("==============================");
                if (response.Headers.Date != null)
                {
                    var date = response.Headers.Date.GetValueOrDefault().   //时间格式：RFC1123
                        ToUniversalTime().ToString("R");
                    Debug.WriteLine("==============================");
                    Debug.WriteLine("GetUpdatedData.Header['Date'] from VMNotices: \n{0}", date);
                    Debug.WriteLine("==============================");
                    IsolatedStorageService<string>.SaveData(    //缓存响应头Date字段
                        new Dictionary<string, string>
                        {
                            { "NoticesHeaderDate", date }
                        });
                }
                if (result != "")
                {
                    notices = ParseData(result);
                    IsolatedStorageService<ObservableCollection<Notice>>.SaveData( //缓存站点信息数据
                        new Dictionary<string, ObservableCollection<Notice>>
                        {
                            { "Notices", notices }
                        });
                }
                else
                {
                    notices = IsolatedStorageService<ObservableCollection<Notice>>.GetSavedData("Notices");
                }
                return notices;
            }
            catch (Exception e)
            {
                var settings = IsolatedStorageSettings.ApplicationSettings;
                settings.Remove("Notices");
                settings.Remove("NoticesHeaderDate");
                notices = GetDefaultData();
                Debug.WriteLine("==============================");
                Debug.WriteLine("Error ocurred in GetUpdatedData from VMNotices: \n{0}", e.Message);
                Debug.WriteLine("==============================");
            }
            return notices;
        }
        /// <summary>
        /// 获取通知公告数据初值
        /// </summary>
        protected override ObservableCollection<Notice> GetDefaultData()
        {
            var notices = new ObservableCollection<Notice>();
            var version = new System.Reflection.AssemblyName(System.Reflection.Assembly.GetExecutingAssembly().FullName).Version;
            notices.Add(new Notice
            {
                Type = "Announcement",
                Title = "欢迎使用波板糖 for Windows Phone 8",
                Time = DateTime.Now.ToString("M", CultureInfo.CurrentCulture),
                Text = "波板糖已经升级，不兼容Windows Phone 7.x系统，硬件设备更新，定位更加准确。感谢各位童鞋对波板糖的支持，妈妈再也不用担心我错过校巴啦！"
            });
            notices.Add(new Notice
            {
                Type = "UpdateInfo",
                Title = String.Format("此版本【{0}】简述内容如下：", version),
                Time = DateTime.Now.ToString("M", CultureInfo.CurrentCulture),
                Text = "波板糖是一款由百步梯研发中心开发的校园生活手机应用。百步梯致力于开发更多服务于华工学子的产品，创新只为与你分享！",
                Appid = "ef9dae52-9fac-419d-9ef7-97bc145698d0"
                //请使用Windows Phone 启动器代替浏览器转跳
            });
            return notices;
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
                var date = IsolatedStorageService<string>.GetSavedData("StationsHeaderDate");
                if (date != null)
                {
                    var hearders = new WebHeaderCollection();
                    Debug.WriteLine("==============================");
                    Debug.WriteLine("Date header in GetResult from VMNotices: \n{0}", date);
                    Debug.WriteLine("==============================");
                    hearders["If-Modified-Since"] = date;
                    HttpWebRequestService.Headers = hearders;
                }
                Debug.WriteLine("==============================");
                Debug.WriteLine("Sending request in GetResult from VMNotices: \n{0}", DataUrls.StationDataUrl);
                Debug.WriteLine("==============================");
                response = await HttpWebRequestService.SendRequestTask(DataUrls.NoticeDataUrl);
            }
            catch (Exception e)
            {
                Debug.WriteLine("==============================");
                Debug.WriteLine("Error occurred in GetResult from VMNotices: \n{0}", e.Message);
                Debug.WriteLine("==============================");
            }
            return response;
        }
        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="result">原始数据字符串</param>
        /// <returns>重构后的数据集</returns>
        protected override ObservableCollection<Notice> ParseData(string result)
        {
            var notices = new ObservableCollection<Notice>();
            var version = new System.Reflection.AssemblyName(System.Reflection.Assembly.GetExecutingAssembly().FullName).Version;
            try
            {
                var objs = JObject.Parse(result);
                foreach (var obj in objs)
                {
                    switch (obj.Key)
                    {
                        case "Announce":
                        {
                            notices.Add(new Notice
                            {
                                Type = "Announcement",
                                Title = (string)objs[obj.Key]["Caption"],
                                Time = new DateTime(1970, 1, 1).AddSeconds((long)objs[obj.Key]["CreatedAt"]).ToString("M", CultureInfo.CurrentCulture),
                                Text = (string)objs[obj.Key]["Text"]
                            });
                            break;
                        }
                        case "Version":
                        {
                            notices.Add(new Notice
                            {
                                Type = "UpdateInfo",
                                Title = String.Format("此版本【{0}】简述内容如下：", version),
                                Time = new DateTime(1970, 1, 1).AddSeconds((long)objs[obj.Key]["Time"]).ToString("M", CultureInfo.CurrentCulture),
                                Text = (string)objs[obj.Key]["Desc"],
                                Appid = "ef9dae52-9fac-419d-9ef7-97bc145698d0"
                                //请使用Windows Phone 启动器代替浏览器转跳
                            });
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                notices = GetDefaultData();
                Debug.WriteLine("==============================");
                Debug.WriteLine("Error occurred in ParseData from VMNotices: \n{0}", e.Message);
                Debug.WriteLine("==============================");
            }
            return notices;
        }
    }
}
