using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    /// <summary>
    /// 获取并解析站点信息数据集
    /// </summary>
    public class ViewModelOfStations:ViewModel<Station>
    {
        public ObservableCollection<Station> Stations;
        /// <summary>
        /// 获取站点信息数据对象
        /// </summary>
        public override async Task<ObservableCollection<Station>> GetDataModel()
        {
            ObservableCollection<Station> stations;
            if (CheckNetworkState.IsNetworkAvailable())
            {
                stations = await GetUpdatedData();
            }
            else
            {
                stations = GetDefaultData();
            }
            Stations = stations;
            return stations;
        }
        /// <summary>
        /// 更新站点信息数据
        /// </summary>
        protected override async Task<ObservableCollection<Station>> GetUpdatedData()
        {
            ObservableCollection<Station> stations;
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
                    Debug.WriteLine("GetUpdatedData.Header['Date'] from VMStations: \n{0}", date);
                    Debug.WriteLine("==============================");
                    IsolatedStorageService<string>.SaveData(    //缓存响应头Date字段
                        new Dictionary<string, string>
                        {
                            { "StationsHeaderDate", date }
                        });
                }
                if (result != "")
                {
                    stations = ParseData(result);
                    IsolatedStorageService<ObservableCollection<Station>>.SaveData( //缓存站点信息数据
                        new Dictionary<string, ObservableCollection<Station>>
                        {
                            { "Stations", stations }
                        });
                }
                else
                {
                    stations = IsolatedStorageService<ObservableCollection<Station>>.GetSavedData("Stations");
                }
                return stations;
            }
            catch (Exception e)
            {
                var settings = IsolatedStorageSettings.ApplicationSettings;
                settings.Remove("Stations");
                settings.Remove("StationsHeaderDate");
                stations = GetDefaultData();
                Debug.WriteLine("==============================");
                Debug.WriteLine("Error ocurred in GetUpdatedData from VMStations: \n{0}", e.Message);
                Debug.WriteLine("==============================");
            }
            return stations;
        }
        /// <summary>
        /// 获取站点信息数据初值
        /// </summary>
        protected override ObservableCollection<Station> GetDefaultData()
        {
            var stations = new ObservableCollection<Station>
            {
                #region 设置默认的站点信息
                new Station
                {
                    Index = 1,
                    Name = "南门总站",
                    Info = "图书馆，单车维修，华工正门，正门腐败一条街，华工科技园，天一快印，麟鸿楼，汕头校友楼，逸夫工程馆（化工学院），13号楼（食品学院）"
                },
                new Station
                {
                    Index = 2,
                    Name = "中山像站",
                    Info = "1号楼，文体中心，电讯楼，东区体育馆，游泳池"
                },
                new Station
                {
                    Index = 3,
                    Name = "百步梯站",
                    Info = "百步梯，12号楼（工商管理学院），25号楼（物理实验）"
                },
                new Station
                {
                    Index = 4,
                    Name = "人文馆站",
                    Info = "27号楼，3号楼（自动化学院），4号楼（数学学院、外语学院），华工校医院，东区饭堂，清真饭堂，五山地铁站，逸夫人文馆，逸夫科学馆，励吾科技楼，计算机中心，9号楼（电力学院）"
                },
                new Station
                {
                    Index = 5,
                    Name = "西五站",
                    Info = "水电中心，校园价，饭堂服务点，西湖苑，工商银行，西区体育场，中区饭堂，学六饭堂，邮局"
                },
                new Station
                {
                    Index = 6,
                    Name = "西秀村站",
                    Info = "34号楼，轮滑场，排球场，西秀村小区（短租房，腐败一条街）"
                },
                new Station
                {
                    Index = 7,
                    Name = "附中站",
                    Info = "华工附小，XX实验基地"
                },
                new Station
                {
                    Index = 8,
                    Name = "修理厂站",
                    Info = "天桥，多品美超市，宵夜集中地"
                },
                new Station
                {
                    Index = 9,
                    Name = "北门站",
                    Info = "饭堂服务点，北一饭堂，继续教育学院"
                },
                new Station
                {
                    Index = 10,
                    Name = "北湖站",
                    Info = "北区图书馆，科技园一号楼、二号楼"
                },
                new Station
                {
                    Index = 11,
                    Name = "卫生所站",
                    Info = "26号楼，北区校园价，打印店，电脑维修点，眼镜店，网络教育学院"
                },
                new Station
                {
                    Index = 12,
                    Name = "北二总站",
                    Info = "北二饭堂，单车维修，35号楼，北湖便利店，打印店，眼镜店，北区体育场，天河客运站"
                }
                #endregion
            };
            return stations;
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
                    Debug.WriteLine("Date header in GetResult from VMStations: \n{0}", date);
                    Debug.WriteLine("==============================");
                    hearders["If-Modified-Since"] = date;
                    HttpWebRequestService.Headers = hearders;
                }
                Debug.WriteLine("==============================");
                Debug.WriteLine("Sending request in GetResult from VMStations: \n{0}", DataUrls.StationDataUrl);
                Debug.WriteLine("==============================");
                response = await HttpWebRequestService.SendRequestTask(DataUrls.StationDataUrl);
            }
            catch (Exception e)
            {
                Debug.WriteLine("==============================");
                Debug.WriteLine("Error occurred in GetResult from VMStations: \n{0}", e.Message);
                Debug.WriteLine("==============================");
            }
            return response;
        }
        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="result">原始数据字符串</param>
        /// <returns>重构后的数据集</returns>
        protected override ObservableCollection<Station> ParseData(string result)
        {
            var stations = new ObservableCollection<Station>();
            try
            {
                var arrs = JArray.Parse(result);
                foreach (var arr in arrs)
                {
                    stations.Add(new Station
                    {
                        Index = (int)arr["index"],
                        Name = (string)arr["station"],
                        Info = (string)arr["info"]
                    });
                }
            }
            catch (Exception e)
            {
                stations = GetDefaultData();
                Debug.WriteLine("==============================");
                Debug.WriteLine("Error occurred in ParseData from VMStations: \n{0}", e.Message);
                Debug.WriteLine("==============================");
            }
            return stations;
        }
    }
}
