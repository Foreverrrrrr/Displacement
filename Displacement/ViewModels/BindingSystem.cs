using Displacement.FunctionCall;
using Displacement.Views;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace Displacement.ViewModels
{
    public partial class MainWindowViewModel : BindableBase, INotifyPropertyChanged
    {
        private static MainWindowViewModel _thisModel = null;

        /// <summary>
        /// 数据模型
        /// </summary>
        public static MainWindowViewModel thisModel
        {
            get { return _thisModel; }
        }

        private string _username = "未登入";
        /// <summary>
        /// 用户权限
        /// </summary>
        public string Username
        {
            get { return _username; }
            set
            {
                SetProperty(ref _username, value); Log.Info(Username + "权限");
            }
        }

        #region 时间计算
        private string _datatime;
        /// <summary>
        /// 本地时间
        /// </summary>
        public string DataTime
        {
            get { return _datatime; }
            set { _datatime = DateTime.Now.ToString(); RaisePropertyChanged(); }
        }

        private DateTime _runtime = DateTime.Now;
        /// <summary>
        /// 程序启动时间
        /// </summary>
        public DateTime Runtime
        {
            get { return _runtime; }
        }

        private string _operationtime;

        /// <summary>
        /// 程序运行时长计算
        /// </summary>
        public string OperationTime
        {
            get { return _operationtime; }
            set
            {
                TimeSpan ts = (DateTime.Now - Runtime);
                _operationtime = ts.Days.ToString() + "天" + ts.Hours.ToString() + "小时" + ts.Minutes.ToString() + "分" + ts.Seconds + "秒"; RaisePropertyChanged();
            }
        }
        #endregion

        #region 消息记录
        private ObservableCollection<string> _infoqueue = new ObservableCollection<string>();
        /// <summary>
        /// 下滑窗消息集合
        /// </summary>
        public ObservableCollection<string> Infoqueue
        {
            get { return _infoqueue; }
            set { _infoqueue = value; }
        }

        private ObservableCollection<object> observableObj = new ObservableCollection<object>();
        /// <summary>
        /// 日志页面消息集合
        /// </summary>
        public ObservableCollection<object> ObservableObj { get => observableObj; set => observableObj = value; }

        private bool _progressbar = false;
        /// <summary>
        /// 加载滑动条状态
        /// </summary>
        public bool Progbar
        {
            get { return _progressbar; }
            set { SetProperty(ref _progressbar, value); }
        }

        private int _progbarvlaue = 0;
        /// <summary>
        /// 滑动值
        /// </summary>
        public int ProgbarVlaue
        {
            get { return _progbarvlaue; }
            set { _progbarvlaue = value; RaisePropertyChanged(); }
        }
        #endregion

        #region 颜色String
        /// <summary>
        /// 正常UI颜色
        /// </summary>
        public string Normal { get { return "#FFFFB900"; } }
        /// <summary>
        /// 异常UI颜色
        /// </summary>
        public string Abnormal { get { return "#FFFF0000"; } }
        /// <summary>
        /// 结果UI颜色
        /// </summary>
        public string Consequence { get { return "#FF0CFF00"; } }
        #endregion

        #region 获取显示器分辨率
        private double _height = SystemParameters.PrimaryScreenHeight-40;
        public double Height
        {
            get { return _height; }
            set { _height = value; RaisePropertyChanged(); }
        }

        private double _width = SystemParameters.PrimaryScreenWidth;

        public double Width
        {
            get { return _width; }
            set { _width = value; RaisePropertyChanged(); }
        }

        private double _pagewithMax = SystemParameters.PrimaryScreenWidth - 110;

        public double PageWidthMax
        {
            get { return _pagewithMax; }
            set { _pagewithMax = value; RaisePropertyChanged(); }
        }


        private double _pagewithMin = SystemParameters.PrimaryScreenWidth - 210;
        public double PageWidthMin
        {
            get { return _pagewithMin; }
            set { _pagewithMin = value; RaisePropertyChanged(); }
        }
        #endregion

        #region 消息弹窗
        public delegate void MessageShow(string showstr, string color = "#FFFFB900", int time = 5000);
        public MessageShow messageShow;

        public delegate void MessageDialogs(string showstr, string color = "#FFFFB900", int time = 3);

        public MessageDialogs MessageLog;
        private static List<MessageLogVision> _dialogs = new List<MessageLogVision>();

        public static List<MessageLogVision> Dialogs
        {
            get { return _dialogs; }
            set { _dialogs = value; }
        }

        private string _dialogsstring;

        public string DialogsString
        {
            get { return _dialogsstring; }
            set { _dialogsstring = value; RaisePropertyChanged(); }
        }


        private bool _message;
        /// <summary>
        /// 下滑弹窗（True开）
        /// </summary>
        public bool Message
        {
            get { return _message; }
            set
            {
                SetProperty(ref _message, value);
            }
        }

        private string _messagecolor = "#FFFFB900";
        /// <summary>
        /// 下滑弹窗背景颜色
        /// </summary>
        public string MessageColor
        {
            get { return _messagecolor; }
            set
            {
                SetProperty(ref _messagecolor, value);
            }
        }

        private string _messagetexte;
        /// <summary>
        /// 下滑弹窗显示文本
        /// </summary>
        public string MessageText
        {
            get { return _messagetexte; }
            set
            {
                _messagetexte = value; RaisePropertyChanged();
            }
        }
        #endregion

        #region 程序权限

        private bool _boost;
        /// <summary>
        /// 等级权限
        /// </summary>
        public bool Boost
        {
            get { return _boost; }
            set { _boost = value; RaisePropertyChanged(); }
        }

        #endregion
    }

}
