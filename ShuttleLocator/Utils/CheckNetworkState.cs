using Microsoft.Phone.Net.NetworkInformation;

namespace ShuttleLocator.Utils
{
   /// <summary>
    /// 检测系统网络信息
    /// </summary>
    public class CheckNetworkState
    {
        public static bool IsNetworkAvailable()
        {
            return System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
        }

        public static string GetNetworkType()
        {
            return NetworkInterface.NetworkInterfaceType.ToString();
        }
    }
}
