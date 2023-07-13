using Displacement.FunctionCall;
using Displacement.ViewModels;
using ImTools;
using MotorAssembly.Views;
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
using System.Xml.Linq;

namespace Displacement.Views
{
    /// <summary>
    /// SetPage.xaml 的交互逻辑
    /// </summary>
    public partial class SetPage : UserControl
    {
        MainWindowViewModel model;
        public SetPage()
        {
            InitializeComponent();
            model = MainWindowViewModel.thisModel;
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

        private void ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            ConfigurationFiles.thisfiles.AmendPath(model, "Arguments", model.ParameterNameList.Find(x => x.ID == model.ParameterIndexes).Name);
            ReTable(ParametName.Text);
        }

        /// <summary>
        /// 新建
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NewParameter parameter = new NewParameter();
            parameter.ParameterChangedEv += (s) =>
            {
                model.ParameterNameList = new List<MainWindowViewModel.ComboxList>();
                model.MessageLog($"新建：“{s}”参数");
                ExcelTool.NewTable(s);
                var list = ExcelTool.GetTest();
                for (int i = 0; i < list.Count; i++)
                {
                    model.ParameterNameList.Add(new MainWindowViewModel.ComboxList { ID = i, Name = list[i] });
                }
                model.ParameterIndexes = model.ParameterNameList.Find(x => x.Name == s).ID;
                ReTable(ParametName.Text);
                parameter.Close();
            };
            parameter.ShowDialog();
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Parameter> comboxes = test.ItemsSource as ObservableCollection<Parameter>;
            ExcelTool.Write(comboxes, ParametName.Text);
            ReTable(ParametName.Text);

        }

        /// <summary>
        /// 修改名称
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ReName name = new ReName();
            name.NewNameEv += (s) =>
            {
                ExcelTool.ChangeName(ParametName.Text, s);
                var list = ExcelTool.GetTest();
                model.ParameterNameList = new List<MainWindowViewModel.ComboxList>();
                for (int i = 0; i < list.Count; i++)
                {
                    model.ParameterNameList.Add(new MainWindowViewModel.ComboxList { ID = i, Name = list[i] });
                }
                model.ParameterIndexes = model.ParameterNameList.Find(x => x.Name == s).ID;
                ReTable(s);
                name.Close();
            };
            name.ShowDialog();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var mess = MessageBox.Show("是否删除当前参数？", "提示", MessageBoxButton.YesNo);
            if (mess == MessageBoxResult.Yes)
            {
                ExcelTool.DeleteTable(ParametName.Text);
                model.MessageLog($"删除：“{ParametName.Text}”参数", model.Normal, 5);
                model.ParameterNameList.Clear();
                for (int i = 0; i < ExcelTool.TestTablName.Count; i++)
                {
                    model.ParameterNameList.Add(new MainWindowViewModel.ComboxList { ID = i, Name = ExcelTool.TestTablName[i] });
                }
                model.ParameterIndexes = 0;
                ReTable(model.ParameterNameList.Find(x => x.ID == 0).Name);
            }
        }

        /// <summary>
        /// 传感器器模电转换物理计算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            CalculatorAD calculatorAD = new CalculatorAD();
            calculatorAD.Show();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Parameter> comboxes = test.ItemsSource as ObservableCollection<Parameter>;
            for (int i = 0; i < 7; i++)
            {
                comboxes[i].Value += MainWindow.realtime[i];
            }
            comboxes[6].Value += MainWindow.realtime[7];
            comboxes[7].Value += MainWindow.realtime[6];
            ExcelTool.Write(comboxes, ParametName.Text);
            ReTable(ParametName.Text);
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Parameter> comboxes = test.ItemsSource as ObservableCollection<Parameter>;
            for (int i = 0; i < 8; i++)
            {
                comboxes[i].Value =0;
            }
            ExcelTool.Write(comboxes, ParametName.Text);
            ReTable(ParametName.Text);

        }
    }
}
