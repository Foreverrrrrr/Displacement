using Displacement.FunctionCall;
using Displacement.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Displacement.Views
{
    /// <summary>
    /// SqlServerVision.xaml 的交互逻辑
    /// </summary>
    public partial class SqlServerVision : System.Windows.Controls.UserControl
    {
        MainWindowViewModel model;
        public SqlServerVision()
        {
            model = MainWindowViewModel.thisModel;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string foldPath = "";
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Excel 工作簿|*.xlsx";//保存的文件扩展名
            dialog.Title = "保存文件";//对话框标题
            dialog.DefaultExt = "Excel|*.xlsx";//设置文件默认扩展名 
            dialog.InitialDirectory = @"D:\";//设置保存的初始目录
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foldPath = dialog.FileName;
                ExcelTool.SaveDataT(foldPath, data);
                data.Clear();
            }
        }
        private DataSet data = new DataSet();
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            model.InquireResult.Clear();
            DateTime b = (DateTime)DefaultDatePicker.Value;
            data = SQL_Server.SpecialQuery(b.Date, b.AddDays(1).Date);
            for (int i = 0; i < data.Tables[0].Rows.Count; i++)
            {
                Result test = new Result();
                int at = 0;
                int ac = 0;
                for (int j = 0; j < data.Tables[0].Columns.Count; j++)
                {
                    if (j == 0 || (j >= 9 && j <= 14) || j == 29 || j == 38 || j == 41)
                    {
                        test.StringResult[at] = (string)data.Tables[0].Rows[i][j];
                        at++;
                    }
                    else
                    {
                        test.FloatResult[ac] =Convert.ToSingle( data.Tables[0].Rows[i][j]);
                        ac++;
                    }
                }
                model.InquireResult.Add(test);
            }
        }
    }
}
