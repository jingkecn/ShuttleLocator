using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.IsolatedStorage;

namespace ShuttleLocator.Services
{
    public class IsolatedStorageService<T>
    {
        /// <summary>
        /// 缓存数据
        /// </summary>
        /// <param name="dictionary">指定数据对象的键与值得映射字典</param>
        public static void SaveData(Dictionary<string, T> dictionary)
        {
            try
            {
                var settings = IsolatedStorageSettings.ApplicationSettings;
                foreach (var pair in dictionary)
                {
                    if (settings.Contains(pair.Key))
                    {
                        settings[pair.Key] = pair.Value;
                        Debug.WriteLine("==============================");
                        Debug.WriteLine("Settings have contained {0} Object!", pair.Key);
                        Debug.WriteLine("==============================");
                    }
                    else
                    {
                        settings.Add(pair.Key, pair.Value);
                        Debug.WriteLine("==============================");
                        Debug.WriteLine("Settings doesn't contain {0} Object!", pair.Key);
                        Debug.WriteLine("==============================");
                    }
                    settings.Save();
                    Debug.WriteLine("==============================");
                    Debug.WriteLine("Your '{0}' data has been successfully saved: \n{1}", pair.Key, pair.Value);
                    Debug.WriteLine("==============================");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("==============================");
                Debug.WriteLine("Fail to save data: \n{0}", e.Message);
                Debug.WriteLine("==============================");
            }
            
        }
        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="key">索引键</param>
        /// <returns>返回指定的数据对象</returns>
        public static T GetSavedData(string key)
        {
            T value;
            try
            {
                var settings = IsolatedStorageSettings.ApplicationSettings;
                value = (T)settings[key];
            }
            catch (Exception)
            {
                value = default(T);
            }
            return value;
        }
    }
}
