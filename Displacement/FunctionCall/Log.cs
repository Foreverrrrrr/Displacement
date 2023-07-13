#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 * 版权所有 (c) 2022  NJRN 保留所有权利。
 * CLR版本：4.0.30319.42000
 * 机器名称：DESKTOP-FMADD4L
 * 命名空间：PrismFrame.FunctionCall
 * 唯一标识：4e996364-ec73-4595-94b5-08276dec8c8b
 * 文件名：Log
 * 当前用户域：DESKTOP-FMADD4L
 * 创建者：Forever
 * 创建时间：2022/7/13 14:10:57
 *----------------------------------------------------------------*/
#endregion << 版 本 注 释 >>
using Displacement.ViewModels;
using log4net;
using log4net.Config;
using Microsoft.SqlServer.Server;
using System;
using System.Windows;

[assembly: XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]

namespace Displacement.FunctionCall
{
    /// <summary>
    /// 运行日志
    /// </summary>
    public class Log
    {
        public enum State
        {
            Error,
            Normal,
            Run
        }

        /// <summary>
        /// 普通日志
        /// </summary>
        /// <param name="message">日志内容</param>
        public static void Info(string message)
        {
            ILog log = LogManager.GetLogger("Info");
            if (log.IsInfoEnabled)
            {
                log.Info(message);
            }
        }

        /// <summary>
        /// 错误日志带异常
        /// </summary>
        /// <param name="message">错误日志</param>
        public static void Error(string message, Exception ex)
        {
            ILog log = LogManager.GetLogger("Error");
            if (log.IsErrorEnabled)
            {
                log.Error(message, ex);
            }
        }

        /// <summary>
        /// 错误日志不带异常
        /// </summary>
        /// <param name="message">错误日志</param>
        public static void Error(string message)
        {
            ILog log = LogManager.GetLogger("Error");
            if (log.IsErrorEnabled)
            {
                log.Error(message);
            }
        }

        /// <summary>
        /// 日志页面传递
        /// </summary>
        /// <param name="log">日志消息</param>
        /// <param name="state">状态</param>
        public static void LogVision(string log, State state)
        {
            string stastring = "";
            switch (state)
            {
                case State.Error:
                    stastring = State.Error.ToString();
                    break;
                case State.Normal:
                    stastring = State.Normal.ToString();
                    break;
                case State.Run:
                    stastring = State.Run.ToString();
                    break;
                default:
                    break;
            }
            Application.Current.Dispatcher.BeginInvoke((Action)delegate ()
            {
                try
                {
                    MainWindowViewModel model = MainWindowViewModel.thisModel;
                    if (model.ObservableObj.Count < 50)
                    {
                        model.ObservableObj.Insert(0, new { 时间 = DateTime.Now.ToString(), 事件 = log, 状态 = state });
                        //model.ObservableObj.Add(new { 时间 = DateTime.Now.ToString(), 事件 = log, 状态 = state });
                    }
                    else
                    {
                        model.ObservableObj.Clear();
                        model.ObservableObj.Insert(0, new { 时间 = DateTime.Now.ToString(), 事件 = log, 状态 = state });
                    }
                }
                catch (Exception ex)
                {
                    Error(ex.Message, ex);
                }
            });
        }

        public static void LogVision(string log, string color="#FF0CFF00")
        {
            string stastring = "";
            State state= State.Normal;
            switch (color)
            {
                case "#FFFF0000":
                    stastring = State.Error.ToString();
                    state = State.Error;
                    break;
                case "#FFFFB900":
                    stastring = State.Normal.ToString();
                    state = State.Normal;
                    break;
                case "#FF0CFF00":
                    stastring = State.Run.ToString();
                    state = State.Run;
                    Log.Info(log);
                    break;
                default:
                    break;
            }
            Application.Current.Dispatcher.BeginInvoke((Action)delegate ()
            {
                try
                {
                    MainWindowViewModel model = MainWindowViewModel.thisModel;
                    if (model.ObservableObj.Count < 100)
                    {
                        model.ObservableObj.Insert(0, new { 时间 = DateTime.Now.ToString(), 事件 = log, 状态 = state });
                        //model.ObservableObj.Add(new { 时间 = DateTime.Now.ToString(), 事件 = log, 状态 = state });
                    }
                    else
                    {
                        model.ObservableObj.Clear();
                        model.ObservableObj.Insert(0, new { 时间 = DateTime.Now.ToString(), 事件 = log, 状态 = state });
                    }
                }
                catch (Exception ex)
                {
                    Error(ex.Message, ex);
                }
            });
        }

    }
}
