using Displacement.AutomaticMain;
using Displacement.FunctionCall;
using Displacement.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static Displacement.FunctionCall.Geometry;

namespace Displacement.Views
{
    /// <summary>
    /// HomeVision.xaml 的交互逻辑
    /// </summary>
    public partial class HomeVision : UserControl
    {
        ObservableCollection<Point2D> _oneadsampling = new ObservableCollection<Point2D>();
        ObservableCollection<Point2D> _oneadsampling1 = new ObservableCollection<Point2D>();
        ObservableCollection<Point2D> _oneadsampling2 = new ObservableCollection<Point2D>();
        ObservableCollection<Point2D> _oneadsampling3 = new ObservableCollection<Point2D>();
        MainWindowViewModel model;
        public HomeVision()
        {
            InitializeComponent();
            model = MainWindowViewModel.thisModel;
            //Random random = new Random();
            //double[] t = new double[] { 2, 5, 6, 20, 25, 30, 40, 5, 15, 22, 10, 5, 50, 7, 20, 5, 60 ,56,57,58,60,63};

            //for (int i = 0; i < t.Length; i++)
            //{
            //    _oneadsampling.Add(new Point2D() { X = i, Y = t[i] });
            //    _oneadsampling1.Add(new Point2D() { X = i, Y = t[i] * 2 });
            //}
            //var t1 = Geometry.FindPeaks(t, Geometry.Wave.Wavecrest);
            //_oneadsampling2.Add(new Point2D() { X = 0, Y = t1[0] });
            //_oneadsampling2.Add(new Point2D() { X = t1[1], Y = t1[0] });
            //_oneadsampling2.Add(new Point2D() { X = t1[1], Y = 0 });
            //t1 = Geometry.FindPeaks(t, Geometry.Wave.Trough);
            //model.SerializationOneAdSampling = _oneadsampling;
            //model.SerializationTwoAdSampling = _oneadsampling1;
            //_oneadsampling3.Add(new Point2D() { X = 0, Y = t1[0] });
            //_oneadsampling3.Add(new Point2D() { X = t1[1], Y = t1[0] });
            //_oneadsampling3.Add(new Point2D() { X = t1[1], Y = 0 });
            //model.SerializationOnePeak = _oneadsampling2;
            //model.SerializationOneGrain = _oneadsampling3;


            //model.SerializationThreeAdSampling = _oneadsampling;
            //model.SerializationFourAdSampling = _oneadsampling1;
            //model.SerializationThreePeak = _oneadsampling2;
            //model.SerializationThreeGrain = _oneadsampling3;
        }

        private void ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            
        }
        private void ReTable(string ns)
        {
            ExcelTool.Read(ns);
            model.ArgumentsData.Clear();
            for (int i = 0; i < ExcelTool.Arguments.Count; i++)
            {
                model.ArgumentsData.Add(new Parameter { Name = ExcelTool.Arguments[i].TestName, Value = ExcelTool.Arguments[i].Value });
            }
        }

        private void standard_set(object sender, RoutedEventArgs e)
        {
            Serialization serialization = new Serialization();
            serialization.Show();
        }

        private void Set_AutoPower(object sender, RoutedEventArgs e)
        {
            SetPower set = new SetPower();
            set.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Task.Run(() =>
            //{
            //    //lock (this)
            //    //{
            //    //    for (int i = 0; i < 10; i++)
            //    //    {
            //    //        ModbusTCP_PLC.thisModbus.Write("208", 1);
            //    //        ModbusTCP_PLC.thisModbus.Write("218", 1);
            //    //        Thread.Sleep(7000);
            //    //    }
            //    //}
            //    while (true)
            //    {
            //        var cd = ControlPower.Get_Out();

            //    }
            //});
        }
    }
}
