using Displacement.ViewModels;
using Syncfusion.UI.Xaml.Charts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Displacement.Views
{
    /// <summary>
    /// LogVision.xaml 的交互逻辑
    /// </summary>
    public partial class LogVision : UserControl
    {
        private MainWindowViewModel model;
        private SyncfuSionChartsModel syncfumodel;
        public static SfChart Chartpie;

        public LogVision()
        {
            InitializeComponent();
            model = MainWindowViewModel.thisModel;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }


    public class ConVerter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value.ToString() == "Error")
            {
                return Brushes.Red;
            }
            else if (value.ToString() == "Normal")
            {
                return Brushes.LawnGreen;
            }
            else
            {
                return Brushes.White;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
