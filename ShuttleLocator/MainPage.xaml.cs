using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using ShuttleLocator.Common;
using ShuttleLocator.View.Controls;
using ShuttleLocator.ViewModel;

namespace ShuttleLocator
{
    public partial class MainPage
    {
        // 构造函数
        public MainPage()
        {
            InitializeComponent();
            DrawStations();
            // 用于本地化 ApplicationBar 的示例代码
            //BuildLocalizedApplicationBar();
        }

        private async void DrawStations()
        {
            var vmStations = new ViewModelOfStations();
            var stations = await vmStations.GetDataModel();
            StationViewControl.DataContext = from station in stations where true select station;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var vmStations = new ViewModelOfStations();
            var stations = await vmStations.GetDataModel();
            Debug.WriteLine("==============================");
            foreach (var station in stations)
            {
                Debug.WriteLine("Button cliked!!!{0}", station.Name);
            }
            Debug.WriteLine("==============================");
            //StationViewControl.DataContext = from station in vmStations.Stations where true select station;
        }

        // 用于生成本地化 ApplicationBar 的示例代码
        //private void BuildLocalizedApplicationBar()
        //{
        //    // 将页面的 ApplicationBar 设置为 ApplicationBar 的新实例。
        //    ApplicationBar = new ApplicationBar();

        //    // 创建新按钮并将文本值设置为 AppResources 中的本地化字符串。
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // 使用 AppResources 中的本地化字符串创建新菜单项。
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}