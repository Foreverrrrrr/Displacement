using Displacement.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Displacement.Views
{
    /// <summary>
    /// LoginSystem.xaml 的交互逻辑
    /// </summary>
    public partial class LoginSystem : Window, INotifyPropertyChanged
    {

        private MainWindowViewModel model;
        private MainWindow main;
        public LoginSystem()
        {
            InitializeComponent();
            main = MainWindow.thiswindow;
            model = MainWindowViewModel.thisModel;
            this.DataContext = this;
            System.Timers.Timer t = new System.Timers.Timer(100);
            t.Elapsed += T_Elapsed;
            t.Enabled = true;
            t.AutoReset = true;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _upload = false;
        public bool Upload
        {
            get { return _upload; }
            set { _upload = value; PropertyChanged?.DynamicInvoke(this, new PropertyChangedEventArgs("Upload")); }
        }
        private string _logtext;

        public string Logtext
        {
            get { return _logtext; }
            set { _logtext = value; PropertyChanged?.DynamicInvoke(this, new PropertyChangedEventArgs("Logtext")); }
        }

        private string pass;
        private string name;

        private void T_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(delegate
            {
                name = username.Text;
                pass = password.Password;

            }));
        }

        private void Min_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            if (model.Username == "未登入")
                Environment.Exit(0);
        }


        private void login_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                Upload = true;
                if (name == "操作员" && pass == "123")
                {
                    model.Boost = false;
                    Logtext = "正在加载......";
                    if (model.Username == "未登入")
                        Thread.Sleep(2000);
                    model.Username = name;
                    Dispatcher.BeginInvoke(new Action(delegate
                    {
                        main.ShowInTaskbar = true;
                        main.Visibility = Visibility.Visible;
                        this.Close();
                    }));
                }
                else if (name == "工程师" && pass == "1")
                {
                    model.Boost = true;
                    Logtext = "正在加载......";
                    if (model.Username == "未登入")
                        Thread.Sleep(1000);
                    model.Username = name;
                    Dispatcher.BeginInvoke(new Action(delegate
                    {
                        main.ShowInTaskbar = true;
                        main.Visibility = Visibility.Visible;
                        this.Close();
                    }));
                    //this.Close();
                }
                else
                {
                    Thread.Sleep(500);
                    Logtext = "请重新确认账号密码！";
                    Upload = false;
                }
            });
        }
    }
}
