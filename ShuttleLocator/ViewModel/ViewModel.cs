using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using ShuttleLocator.Model;

namespace ShuttleLocator.ViewModel
{
    public abstract class ViewModel<T>
    {
        /// <summary>
        /// 获取数据对象
        /// </summary>
        public abstract Task<ObservableCollection<T>> GetDataModel();
        /// <summary>
        /// 更新数据（异步）
        /// </summary>
        /// <returns>返回更新的数据对象</returns>
        protected abstract Task<ObservableCollection<T>> GetUpdatedData();
        /// <summary>
        /// 数据初值
        /// </summary>
        /// <returns>返回默认的数据对象</returns>
        protected abstract ObservableCollection<T> GetDefaultData();
        /// <summary>
        /// 获取数据（异步）
        /// </summary>
        /// <returns>原始数据字符串</returns>
        protected abstract Task<HttpResponseMessage> GetResult();
        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="result">原始数据字符串</param>
        /// <returns>重构后的指定对象集合</returns>
        protected abstract ObservableCollection<T> ParseData(string result);
    }
}
