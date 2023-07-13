using Displacement.ChartsModel.MVVM;
using Displacement.FunctionCall;
using Displacement.Views;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace Displacement.ViewModels
{
    public class SyncfuSionChartsModel : BindableBase
    {
        #region 产量饼图


        private ObservableCollection<DoughnutChartModel> _tax = new ObservableCollection<DoughnutChartModel>();

        public ObservableCollection<DoughnutChartModel> Tax
        {
            get { return _tax; }
            set { _tax = value; RaisePropertyChanged(); }
        }


        private double startAngle;
        public double StartAngle
        {
            get { return startAngle; }
            set { startAngle = value; RaisePropertyChanged(); }
        }

        private double endAngle;
        public double EndAngle
        {
            get { return endAngle; }
            set { endAngle = value; RaisePropertyChanged(); }
        }

        private string _selectedItemName;
        public string SelectedItemName
        {
            get { return _selectedItemName; }
            set { _selectedItemName = value; RaisePropertyChanged(); }
        }

        private double _selectedItemsPercentage;
        public double SelectedItemsPercentage
        {
            get { return _selectedItemsPercentage; }
            set
            {
                _selectedItemsPercentage = value; RaisePropertyChanged();
            }
        }

        private int selectedIndex;
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                if (selectedIndex != value)
                {
                    selectedIndex = value;
                    RaisePropertyChanged(nameof(this.SelectedIndex));
                    if (Tax != null && selectedIndex > -1 && selectedIndex < Tax.Count)
                    {
                        SelectedItemName = Tax[selectedIndex].Category;
                        SelectedItemsPercentage = Tax[selectedIndex].Percentage;
                    }
                }
            }
        }

        public SyncfuSionChartsModel()
        {
            DynamicData = new ObservableCollection<RealTimeChartModel>();
            Data = new ObservableCollection<RealTimeChartModel>();
            StartAngle = 180;
            EndAngle = 360;
            SelectedIndex = -1;
            //Rtime();
        }
        #endregion

        #region 曲线实时图
        private ObservableCollection<RealTimeChartModel> _dynam;
        public ObservableCollection<RealTimeChartModel> DynamicData
        {
            get { return _dynam; }
            set { _dynam = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<RealTimeChartModel> Data;
        private DispatcherTimer timer;
        private Random randomNumber;
        private int i = 0;

        public void Rtime()
        {
            timer = new DispatcherTimer();
            randomNumber = new Random();
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            timer.Start();

            return;
            Task.Run(() =>
            {
                List<double> intsy = new List<double>();
                List<double> intsx = new List<double>();
                while (false)
                {
                    RealTimeChartModel realTime = new RealTimeChartModel(randomNumber.Next(0, 10), i);
                    intsy.Add(realTime.ValueY);
                    intsx.Add(realTime.ValueX);
                    Data.Add(realTime);
                    if (Data.Count > 100)
                    {
                        break;
                    }
                    i++;
                }
                Application.Current.Dispatcher.BeginInvoke((Action)delegate ()
                {
                    DynamicData.AddRange(Data);
                });
            });
        }

        /// <summary>
        /// 寻峰（查找波峰或者波谷）
        /// </summary>
        /// <param name="data">数据源</param>
        /// <param name="IsTrough">0:波峰 1:波谷 2:波峰和波谷</param>
        /// <returns></returns>
        public static int[] FindPeaks(double[] data, int PeakStyle)
        {
            double[] diff = new double[data.Length - 1];
            for (int i = 0; i < diff.Length; i++)
            {
                diff[i] = data[i + 1] - data[i];
            }
            int[] sign = new int[diff.Length];
            for (int i = 0; i < sign.Length; i++)//波峰波谷区分
            {
                //if (diff[i] > 3) sign[i] = 1;
                //else if (diff[i] == 0) sign[i] = 0;
                //else sign[i] = -1;

                if (diff[i] > 3)
                {
                    sign[i] = 1;
                }
                else if (diff[i] < -1)
                {
                    sign[i] = -1;
                }
                else
                {
                    sign[i] = 0;
                }
            }
            for (int i = sign.Length - 1; i >= 0; i--)
            {
                if (sign[i] == 0 && i == sign.Length - 1)
                {
                    sign[i] = 1;
                }
                else if (sign[i] == 0)
                {
                    if (sign[i + 1] >= 0)
                    {
                        sign[i] = 1;
                    }
                    else
                    {
                        sign[i] = -1;
                    }
                }
            }
            List<int> result = new List<int>();
            for (int i = 0; i != sign.Length - 1; i++)
            {
                if (PeakStyle == 0)
                {
                    if (sign[i + 1] - sign[i] == -2)
                    {
                        result.Add(i + 1);
                    }
                }
                else if (PeakStyle == 1)
                {
                    if (sign[i + 1] - sign[i] == 2)
                    {
                        result.Add(i + 1);
                    }
                }
                else if (PeakStyle == 2)
                {
                    if (Math.Abs(sign[i + 1] - sign[i]) == 2)
                    {
                        result.Add(i + 1);
                    }
                }
            }
            return result.ToArray();//相当于原数组的下标
        }


        private void Timer_Tick(object sender, EventArgs e)
        {
            Random r = new Random();
            RealTimeChartModel realTime = new RealTimeChartModel(r.Next(0, 100), r.Next(0, 100));
            DynamicData.Add(realTime);
            if (DynamicData.Count > 800)
            {
                DynamicData.RemoveAt(0);
            }

        }
        #endregion
    }

    public class DoughnutChartModel : BindableBase
    {
        public DoughnutChartModel(string category, double percentage)
        {
            Category = category;
            Percentage = percentage;
        }

        private string _category;

        public string Category
        {
            get { return _category; }
            set { _category = value; RaisePropertyChanged(); }
        }

        private double _percentage;

        public double Percentage
        {
            get { return _percentage; }
            set { _percentage = value; RaisePropertyChanged(); }
        }

    }

    public class RealTimeChartModel : BindableBase
    {
        public RealTimeChartModel(double value, double value1)
        {
            ValueY = value;
            ValueX = value1;
        }

        private double _valuey;
        public double ValueY
        {
            get { return _valuey; }
            set { _valuey = value; RaisePropertyChanged(); }
        }

        private double _valuex;
        public double ValueX
        {
            get { return _valuex; }
            set { _valuex = value; RaisePropertyChanged(); }
        }
    }
}
