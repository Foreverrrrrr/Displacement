using Displacement.FunctionCall;
using Displacement.Views;
using ImTools;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Timers;
using System.Windows;

namespace Displacement.ViewModels
{
    public partial class MainWindowViewModel : BindableBase, INotifyPropertyChanged
    {

        public readonly IRegionManager regionManager;
        public readonly IDialogService dialog;
        public DelegateCommand<string> OpenPage { get; set; }
        private Thread thread;


        public MainWindowViewModel(IRegionManager regionManager, IDialogService showdialog)
        {
            _thisModel = this;
            OpenPage = new DelegateCommand<string>(Open);
            this.regionManager = regionManager;
            this.dialog = showdialog;
            this.messageShow = new MessageShow(MessageShowLog);
            var t1 = new System.Timers.Timer(1000);
            t1.Elapsed += T1_Elapsed;
            t1.Enabled = true;
            t1.Start();
            Viewinitial();
            Log.Info("程序开始运行！");
            thread = new Thread(MessageShowDlog);
            thread.IsBackground = true;
            thread.Start();
        }
        /// <summary>
        /// 页面切换
        /// </summary>
        /// <param name="obj"></param>
        private void Open(string obj)
        {
            //首先通过IRegionManager接口获取全局定义可用区域
            //往这个区域动态设置内容
            //设置内容的方式是通过依赖注入的形式
            try
            {
                regionManager.Regions["ContentRegion"].RequestNavigate(obj);
                //Log.Info("切换" + obj + "页面");
               // Log.LogVision("切换" + obj + "页面", Log.State.Normal);
            }
            catch (System.Exception ex)
            {
                Log.Error(ex.Message, ex);
            }
        }

        /// <summary>
        /// 页面初始化加载
        /// </summary>
        public void Viewinitial()
        {
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(HomeVision));
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(SetPage));
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(LogVision));
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(SqlServerVision));
        }

        private void MessageShowDlog()
        {
            while (true)
            {
                if (Infoqueue.Count > 0)
                {
                    MessageColor = Infoqueue[0].Split('*')[1];
                    MessageText = Infoqueue[0].Split('*')[0];
                    Message = true;
                    Thread.Sleep(int.Parse(Infoqueue[0].Split('*')[2]));
                    Message = false;
                    Infoqueue.RemoveAt(0);
                    Thread.Sleep(100);
                }
                else
                {
                    Thread.Sleep(10);
                }
            }
        }

        private void T1_Elapsed(object sender, ElapsedEventArgs e)
        {
            DataTime = "";
            OperationTime = "";
        }

        /// <summary>
        /// 下滑消息滑窗
        /// </summary>
        /// <param name="str">消息</param>
        /// <param name="color">颜色</param>
        /// <param name="time">持续时间</param>
        private void MessageShowLog(string str, string color, int time)
        {
            if (Infoqueue.Count < 1000)
            {
                Infoqueue.Add(str + "*" + color + "*" + time);
            }

        }
    }
}
