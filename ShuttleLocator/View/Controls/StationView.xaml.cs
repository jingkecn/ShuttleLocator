using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace ShuttleLocator.View.Controls
{
    public partial class StationView : UserControl
    {
        public StationView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var thisBuntton = (Button) sender;
            Debug.WriteLine("==============================");
            Debug.WriteLine("Station {0} is clicked!", thisBuntton.Content);
            Debug.WriteLine("==============================");
        }
    }
}
