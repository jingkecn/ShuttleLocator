using System;
using System.Globalization;

namespace ShuttleLocator.Common
{
    public class DataUrls
    {
        const string BaseUrl = "http://bbt.100steps.net/go/";

        public static string ShuttleDataUrl = String.Format(
            "{0}data?screw={1}", 
            BaseUrl, 
            DateTime.Now.ToString(CultureInfo.InvariantCulture));

        public static string StationDataUrl = String.Format(
            "{0}data/stationinfo?screw={1}",
            BaseUrl,
            DateTime.Now.ToString(CultureInfo.InvariantCulture));

        public static string NoticeDataUrl = String.Format(
            "{0}update/checkUpdate?platform=wp&screw={1}",
            BaseUrl,
            DateTime.Now.ToString(CultureInfo.InvariantCulture));
    }
}
