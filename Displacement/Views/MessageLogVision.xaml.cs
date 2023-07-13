using Displacement.FunctionCall;
using Displacement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Displacement.Views
{
    /// <summary>
    /// MessageLogVision.xaml 的交互逻辑
    /// </summary>
    public partial class MessageLogVision : Window
    {
        public double X { get; set; }

        public double Y { get; set; }

        public int Time { get; set; }

        public MessageLogVision()
        {
            InitializeComponent();
            this.Loaded += NotificationWindow_Loaded;
        }

        private void NotificationWindow_Loaded(object sender, RoutedEventArgs e)
        {
            MessageLogVision self = sender as MessageLogVision;
            if (self != null)
            {
                self.UpdateLayout();
                //SystemSounds.Exclamation.Play();//播放提示声
                self.Left = X - 335;//工作区最右边的值
                self.Top = self.Y - 155;
                DoubleAnimation animation = new DoubleAnimation();
                animation.Duration = new Duration(TimeSpan.FromMilliseconds(500));//NotifyTimeSpan是自己定义的一个int型变量，用来设置动画的持续时间
                animation.From = 0;
                animation.To = this.Width;
                Show.BeginAnimation(WidthProperty, animation);//设定动画应用于窗体的width属性
                Task.Factory.StartNew(delegate
                {
                    int seconds = Time;//通知时长
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(seconds));
                    Dispatcher.BeginInvoke(new Action(delegate
                    {
                        animation = new DoubleAnimation();
                        animation.Duration = new Duration(TimeSpan.FromMilliseconds(500));
                        animation.Completed += (s, a) =>
                        {
                            Dispatcher.BeginInvoke(new Action(delegate
                            {
                                self.Close();
                            }));
                        };
                        animation.From = this.Width;
                        animation.To = 0;
                        Show.BeginAnimation(Window.WidthProperty, animation);
                    }));
                });
            }
        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.Duration = new Duration(TimeSpan.FromMilliseconds(200));
            animation.Completed += (s, a) =>
            {
                Dispatcher.BeginInvoke(new Action(delegate
                {
                    this.Close();
                }));
            };
            animation.From = this.Width;
            animation.To = 0;
            Show.BeginAnimation(Window.WidthProperty, animation);
        }
    }
}
