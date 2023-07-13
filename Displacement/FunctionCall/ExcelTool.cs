using Displacement.ViewModels;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Displacement.FunctionCall
{
    public class ExcelTool
    {
        public static string Password { get; set; } = "Forever";

        public static string Path { get; set; } = @"TestDate.xlsx";

        public static string SavePath { get; set; }

        public ExcelTool(string path)
        {
            Path = path;
        }
        public struct Address
        {
            public string TestName { get; set; }

            public double Value { get; set; }
        }

        public static List<Address> Arguments { get; set; }

        public static List<string> TestTablName { get; set; } = new List<string>();

        public static string[] TableName2 = new string[] { "时间", "一号载具结果", "二号载具结果", "一号轴位移结果", "一号轴位移值", "一号蜗杆位移结果", "一号蜗杆位移值", "一号空载电流结果", "一号空载电流", "二号轴位移结果", "二号轴位移值", "二号蜗杆位移结果", "二号蜗杆位移值", "二号空载电流结果", "二号空载电流" };
        public static string[] TableName1 = new string[] { "时间", "一号载具结果", "二号载具结果", "一号串动量", "一号推动位移结果", "一号推动位移值", "一号拉动位移结果", "一号拉动位移值", "一号推动压力结果", "一号推动压力值", "一号拉动压力结果", "一号拉动压力值", "二号串动量", "二号推动位移结果", "二号推动位移值", "二号拉动位移结果", "二号拉动位移值", "二号推动压力结果", "二号推动压力值", "二号拉动压力结果", "二号拉动压力值" };


        public static string[] TableNamess;

        /// <summary>
        /// 更改指定表中的全部数据
        /// </summary>
        /// <param name="line">Value集合</param>
        /// <param name="table">型号名称</param>
        /// <param name="flow">流程</param>
        public static void Write(ObservableCollection<Parameter> line, string table)
        {
            using (ExcelPackage package = new ExcelPackage(new System.IO.FileInfo(Path)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Where(x => x.Name == table).FirstOrDefault();
                int rowCount = worksheet.Dimension != null ? worksheet.Dimension.Rows : 0;//列
                int colCount = worksheet.Dimension != null ? worksheet.Dimension.Columns : 0;//行
                for (int i = 1; i <= rowCount; i++)
                {
                    worksheet.Cells[i, 1].Value = line[i - 1].Name;
                    worksheet.Cells[i, 2].Value = line[i - 1].Value;
                }
                try
                {
                    package.SaveAs(new FileInfo(Path));
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message, ex);
                    MessageBox.Show($"{Path}文件被外部程序打开，程序即将自动退出,请关闭该文件后重新运行程序！", "Error");
                    //MainWindow.thiswindow.Close_Click(null, null);
                }
            }
        }

        /// <summary>
        /// 获取当前所有型号列表
        /// </summary>
        /// <param name="name"></param>
        public static List<string> GetTest()
        {
            FileInfo file = new FileInfo(Path);
            using (ExcelPackage package = new ExcelPackage(file))
            {
                TestTablName = new List<string>();
                foreach (var item in package.Workbook.Worksheets)
                {
                    if (item.Name != "Null")
                    {
                        TestTablName.Add(item.Name);
                    }
                }
                try
                {
                    package.Save();
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message, ex);
                    MessageBox.Show($"{Path}文件被外部程序打开，程序即将自动退出,请关闭该文件后重新运行程序！", "Error");
                    //MainWindow.thiswindow.Close_Click(null, null);
                }
            }
            return TestTablName;
        }

        /// <summary>
        /// 删除表
        /// </summary>
        /// <param name="name">表名</param>
        public static void DeleteTable(string name)
        {
            FileInfo file = new FileInfo(Path);
            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Where(x => x.Name == name).FirstOrDefault();
                if (worksheet != null)
                {
                    package.Workbook.Worksheets.Delete(worksheet);
                }
                try
                {
                    package.Save();
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message, ex);
                    MessageBox.Show($"{Path}文件被外部程序打开，程序即将自动退出,请关闭该文件后重新运行程序！", "Error");
                    //MainWindow.thiswindow.Close_Click(null, null);
                }
            }
            GetTest();
        }

        /// <summary>
        /// 更改型号名称
        /// </summary>
        /// <param name="atname">当前型号名称</param>
        /// <param name="name">目标型号名称</param>
        public static void ChangeName(string atname, string name)
        {
            FileInfo file = new FileInfo(Path);
            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Where(x => x.Name == atname).FirstOrDefault();
                if (worksheet != null)
                {
                    if (worksheet.Name == atname)
                    {
                        worksheet.Name = name;
                    }
                    try
                    {
                        package.Save();
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex.Message, ex);
                        MessageBox.Show($"{Path}文件被外部程序打开，程序即将自动退出,请关闭该文件后重新运行程序！", "Error");
                        //MainWindow.thiswindow.Close_Click(null, null);
                    }
                    TestTablName = new List<string>();
                    foreach (var item in package.Workbook.Worksheets)
                    {
                        TestTablName.Add(item.Name);
                    }
                    try
                    {
                        package.Save();
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex.Message, ex);
                        MessageBox.Show($"{Path}文件被外部程序打开，程序即将自动退出,请关闭该文件后重新运行程序！", "Error");
                        //MainWindow.thiswindow.Close_Click(null, null);
                    }
                }
            }
        }

        /// <summary>
        /// 新建型号
        /// </summary>
        /// <param name="name">型号名称</param>
        public static void NewTable(string name)
        {
            if (name == "Null")
                return;
            FileInfo file = new FileInfo(Path);
            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Where(x => x.Name == name).FirstOrDefault();
                if (worksheet == null)
                {
                    worksheet = package.Workbook.Worksheets.Add(name); package.Save();
                    ExcelWorksheet nullworksheet = package.Workbook.Worksheets.Where(x => x.Name == "Null").FirstOrDefault();
                    int rowCount = nullworksheet.Dimension != null ? nullworksheet.Dimension.Rows : 0;//列
                    int colCount = nullworksheet.Dimension != null ? nullworksheet.Dimension.Columns : 0;//行
                    for (int i = 1; i < rowCount; i++)
                    {
                        worksheet.Cells[i, 1].Value = nullworksheet.Cells[i + 1, 1].Value.ToString();
                        worksheet.Cells[i, 2].Value = nullworksheet.Cells[i + 1, 2].Value.ToString();
                    }
                    try
                    {
                        package.Save();
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex.Message, ex);
                        MessageBox.Show($"{Path}文件被外部程序打开，程序即将自动退出,请关闭该文件后重新运行程序！", "Error");
                        // MainWindow.thiswindow.Close_Click(null, null);
                    }
                }
            }
        }

        /// <summary>
        /// 读取表中参数
        /// </summary>
        /// <param name="tablename">表名</param>
        public static void Read(string tablename)
        {
            if (tablename == "Null")
                return;
            FileInfo file = new FileInfo(Path);
            using (ExcelPackage excelPackage = new ExcelPackage(file))
            {
                //指定需要读入的sheet名
                ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets[tablename];
                if (excelWorksheet != null)
                {
                    int rowCount = excelWorksheet.Dimension != null ? excelWorksheet.Dimension.Rows : 0;//列
                    int colCount = excelWorksheet.Dimension != null ? excelWorksheet.Dimension.Columns : 0;//行
                    List<Address> addresses = new List<Address>();
                    for (int i = 1; i <= rowCount; i++)
                    {
                        Address address = new Address();
                        address.TestName = excelWorksheet.Cells[i, 1].Value.ToString();
                        address.Value = Convert.ToDouble(excelWorksheet.Cells[i, 2].Value);
                        addresses.Add(address);
                    }
                    Arguments = addresses;
                }
            }
        }

        public static void SaveDataN4(No4 testone)
        {
            NewSaveData(SavePath);
            try
            {
                using (ExcelPackage package = new ExcelPackage(new System.IO.FileInfo(SavePath + "\\" + "本地数据" + "\\" + "四工位数据" + "\\" + DateTime.Now.ToString("yyyy-MM") + "\\" + DateTime.Now.ToString("dd") + ".xlsx")))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Count != 0 ? package.Workbook.Worksheets[1] : package.Workbook.Worksheets.Add("Form1");//是否存在工作表，不存在创建工作表
                    int rowCount = worksheet.Dimension != null ? worksheet.Dimension.End.Row : 0;//获取数据最后行
                    if (rowCount == 0)
                    {
                        for (int i = 1; i < TableName1.Length + 1; i++)
                        {
                            worksheet.Cells[1, i].Value = TableName1[i - 1];
                            worksheet.Cells[1, i].Style.Font.Name = "微软雅黑";
                        }
                        rowCount++;
                    }
                    List<string> strings = new List<string>();
                    worksheet.Cells[rowCount + 1, 1].Value = testone.Time;
                    worksheet.Cells[rowCount + 1, 2].Value = testone.OneResult;
                    worksheet.Cells[rowCount + 1, 3].Value = testone.TwoResult;
                    worksheet.Cells[rowCount + 1, 4].Value = testone.One_bunch_movement;

                    worksheet.Cells[rowCount + 1, 5].Value = testone.PushOneMoveResult;
                    worksheet.Cells[rowCount + 1, 6].Value = testone.PushOneMove;
                    worksheet.Cells[rowCount + 1, 7].Value = testone.PullOneMoveResult;
                    worksheet.Cells[rowCount + 1, 8].Value = testone.PullOneMove;
                    worksheet.Cells[rowCount + 1, 9].Value = testone.PushOneForceResult;
                    worksheet.Cells[rowCount + 1, 10].Value = testone.PushOneForce;
                    worksheet.Cells[rowCount + 1, 11].Value = testone.PullOneForceResult;
                    worksheet.Cells[rowCount + 1, 12].Value = testone.PullOneForce;
                    worksheet.Cells[rowCount + 1, 13].Value = testone.Two_bunch_movement;

                    worksheet.Cells[rowCount + 1, 14].Value = testone.PushTwoMoveResult;
                    worksheet.Cells[rowCount + 1, 15].Value = testone.PushTwoMove;
                    worksheet.Cells[rowCount + 1, 16].Value = testone.PullTwoMoveResult;
                    worksheet.Cells[rowCount + 1, 17].Value = testone.PullTwoMove;
                    worksheet.Cells[rowCount + 1, 18].Value = testone.PushTwoForceResult;
                    worksheet.Cells[rowCount + 1, 19].Value = testone.PushTwoForce;
                    worksheet.Cells[rowCount + 1, 20].Value = testone.PullTwoForceResult;
                    worksheet.Cells[rowCount + 1, 21].Value = testone.PullTwoForce;
                    package.SaveAs(new FileInfo(SavePath + "\\" + "本地数据" + "\\" + "四工位数据" + "\\" + DateTime.Now.ToString("yyyy-MM") + "\\" + DateTime.Now.ToString("dd") + ".xlsx"));
                };
                CleanFile(SavePath + "\\" + "本地数据" + "\\" + "四工位数据");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                MainWindowViewModel.thisModel.MessageLog(SavePath + "\\" + "本地数据" + "\\" + "四工位数据" + "\\" + DateTime.Now.ToString("yyyy-MM") + "\\" + DateTime.Now.ToString("dd") + ".xlsx" + "该Excel被外部程序打开！", MainWindowViewModel.thisModel.Abnormal);
            }

        }

        public static void SaveDataN5(No5 testone)
        {
            NewSaveData(SavePath);
            try
            {
                using (ExcelPackage package = new ExcelPackage(new System.IO.FileInfo(SavePath + "\\" + "本地数据" + "\\" + "五工位数据" + "\\" + DateTime.Now.ToString("yyyy-MM") + "\\" + DateTime.Now.ToString("dd") + ".xlsx")))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Count != 0 ? package.Workbook.Worksheets[1] : package.Workbook.Worksheets.Add("Form1");//是否存在工作表，不存在创建工作表
                    int rowCount = worksheet.Dimension != null ? worksheet.Dimension.End.Row : 0;//获取数据最后行
                    if (rowCount == 0)
                    {
                        for (int i = 1; i < TableName2.Length + 1; i++)
                        {
                            worksheet.Cells[1, i].Value = TableName2[i - 1];
                            worksheet.Cells[1, i].Style.Font.Name = "微软雅黑";
                        }
                        rowCount++;
                    }
                    List<string> strings = new List<string>();

                    worksheet.Cells[rowCount + 1, 1].Value = testone.Time;
                    worksheet.Cells[rowCount + 1, 2].Value = testone.OneResult;
                    worksheet.Cells[rowCount + 1, 3].Value = testone.TwoResult;
                    worksheet.Cells[rowCount + 1, 4].Value = testone.OneValueResult;

                    worksheet.Cells[rowCount + 1, 5].Value = testone.OneValue;
                    worksheet.Cells[rowCount + 1, 6].Value = testone.TwoValueResult;
                    worksheet.Cells[rowCount + 1, 7].Value = testone.TwoValue;
                    worksheet.Cells[rowCount + 1, 8].Value = testone.OneElEResult;

                    worksheet.Cells[rowCount + 1, 9].Value = Math.Round(testone.OneElE, 3);
                    worksheet.Cells[rowCount + 1, 10].Value = testone.ThreeValueResult;
                    worksheet.Cells[rowCount + 1, 11].Value = testone.ThreeValue;
                    worksheet.Cells[rowCount + 1, 12].Value = testone.FourValueResult;
                    worksheet.Cells[rowCount + 1, 13].Value = testone.FourValue;
                    worksheet.Cells[rowCount + 1, 14].Value = testone.TwoElEResult;

                    worksheet.Cells[rowCount + 1, 15].Value = Math.Round(testone.TwoElE, 3);
                    package.SaveAs(new FileInfo(SavePath + "\\" + "本地数据" + "\\" + "五工位数据" + "\\" + DateTime.Now.ToString("yyyy-MM") + "\\" + DateTime.Now.ToString("dd") + ".xlsx"));
                };
                CleanFile(SavePath + "\\" + "本地数据" + "\\" + "五工位数据");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                MainWindowViewModel.thisModel.MessageLog(SavePath + "\\" + "本地数据" + "\\" + "五工位数据" + "\\" + DateTime.Now.ToString("yyyy-MM") + "\\" + DateTime.Now.ToString("dd") + ".xlsx" + "该Excel被外部程序打开！", MainWindowViewModel.thisModel.Abnormal);
            }

        }


        public static void SaveData(float[] value)
        {
            try
            {


                if (TableNamess == null)
                {
                    var cy = File.ReadAllLines(@"Save_Table.txt");
                    TableNamess = cy[0].Split(',');
                }
                NewSaveData(SavePath);
                using (ExcelPackage package = new ExcelPackage(new System.IO.FileInfo(SavePath + "\\" + "本地数据" + "\\" + "自动运行数据" + "\\" + DateTime.Now.ToString("yyyy-MM") + "\\" + DateTime.Now.ToString("dd") + ".xlsx")))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Count != 0 ? package.Workbook.Worksheets[1] : package.Workbook.Worksheets.Add("Form1");//是否存在工作表，不存在创建工作表
                    int rowCount = worksheet.Dimension != null ? worksheet.Dimension.End.Row : 0;//获取数据最后行
                    if (rowCount == 0)
                    {
                        for (int i = 1; i < TableNamess.Length + 1; i++)
                        {
                            worksheet.Cells[1, i].Value = TableNamess[i - 1];
                            worksheet.Cells[1, i].Style.Font.Name = "微软雅黑";
                        }
                        rowCount++;
                    }
                    for (int i = 0; i < 2; i++)
                    {
                        worksheet.Cells[i + rowCount + 1, 1].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        worksheet.Cells[i + rowCount + 1, 2].Value = Math.Round(value[40]);
                        worksheet.Cells[i + rowCount + 1, 3].Value = Math.Round(value[41], 2);
                        worksheet.Cells[i + rowCount + 1, 4].Value = Math.Round(value[42], 2);
                        worksheet.Cells[i + rowCount + 1, 5].Value = Math.Round(value[43], 2);
                        worksheet.Cells[i + rowCount + 1, 6].Value = Math.Round(value[44], 2);
                        worksheet.Cells[i + rowCount + 1, 7].Value = Math.Round(value[45], 2);
                        worksheet.Cells[i + rowCount + 1, 8].Value = Math.Round(value[46], 2);
                        worksheet.Cells[i + rowCount + 1, 9].Value = Math.Round(value[47], 2);
                        worksheet.Cells[i + rowCount + 1, 10].Value = i + 1;
                        worksheet.Cells[i + rowCount + 1, 11].Value = value[38 + i] == 1 ? "Pass" : "NG";
                        worksheet.Cells[i + rowCount + 1, 12].Value = value[0 + i] == 1 ? "Pass" : "NG";
                        worksheet.Cells[i + rowCount + 1, 13].Value = value[2 + i] == 1 ? "Pass" : "NG";
                        worksheet.Cells[i + rowCount + 1, 14].Value = value[4 + i] == 1 ? "Pass" : "NG";
                        worksheet.Cells[i + rowCount + 1, 15].Value = value[6 + i] == 1 ? "Pass" : "NG";
                        worksheet.Cells[i + rowCount + 1, 16].Value = Math.Round(value[8 + i], 3);
                        worksheet.Cells[i + rowCount + 1, 17].Value = Math.Round(value[14 + i], 3);
                        worksheet.Cells[i + rowCount + 1, 18].Value = Math.Round(MainWindowViewModel.thisModel.ArgumentsData[11 + i].Value, 3);
                        worksheet.Cells[i + rowCount + 1, 19].Value = Math.Round(value[10 + i], 3);
                        worksheet.Cells[i + rowCount + 1, 20].Value = Math.Round(MainWindowViewModel.thisModel.ArgumentsData[13 + i].Value, 3);
                        worksheet.Cells[i + rowCount + 1, 21].Value = Math.Round(MainWindowViewModel.thisModel.ArgumentsData[15 + i].Value, 3);
                        worksheet.Cells[i + rowCount + 1, 22].Value = Math.Round(value[12 + i], 3);
                        worksheet.Cells[i + rowCount + 1, 23].Value = Math.Round(MainWindowViewModel.thisModel.ArgumentsData[17 + i].Value, 3);
                        worksheet.Cells[i + rowCount + 1, 24].Value = Math.Round(MainWindowViewModel.thisModel.ArgumentsData[19 + (i * 4)].Value, 3);
                        worksheet.Cells[i + rowCount + 1, 25].Value = Math.Round(value[16 + i], 3);
                        worksheet.Cells[i + rowCount + 1, 26].Value = Math.Round(MainWindowViewModel.thisModel.ArgumentsData[20 + (i * 4)].Value, 3);
                        worksheet.Cells[i + rowCount + 1, 27].Value = Math.Round(MainWindowViewModel.thisModel.ArgumentsData[21 + (i * 4)].Value, 3);
                        worksheet.Cells[i + rowCount + 1, 28].Value = Math.Round(value[18 + i], 3);
                        worksheet.Cells[i + rowCount + 1, 29].Value = Math.Round(MainWindowViewModel.thisModel.ArgumentsData[22 + (i * 4)].Value, 3);
                        worksheet.Cells[i + rowCount + 1, 30].Value = value[20 + i] == 1 ? "Pass" : "NG";
                        worksheet.Cells[i + rowCount + 1, 31].Value = Math.Round(MainWindowViewModel.thisModel.ArgumentsData[39].Value, 3);
                        worksheet.Cells[i + rowCount + 1, 32].Value = Math.Round(value[26 + i], 3);
                        worksheet.Cells[i + rowCount + 1, 33].Value = Math.Round(MainWindowViewModel.thisModel.ArgumentsData[40].Value, 3);
                        worksheet.Cells[i + rowCount + 1, 34].Value = Math.Round(MainWindowViewModel.thisModel.ArgumentsData[41 + (i * 2)].Value, 3);
                        worksheet.Cells[i + rowCount + 1, 35].Value = Math.Round(value[22 + i], 3);
                        worksheet.Cells[i + rowCount + 1, 36].Value = Math.Round(MainWindowViewModel.thisModel.ArgumentsData[42 + (i * 2)].Value, 3);
                        worksheet.Cells[i + rowCount + 1, 37].Value = Math.Round(value[24 + i], 3);
                        worksheet.Cells[i + rowCount + 1, 38].Value = Math.Round(value[28 + i], 3);
                        worksheet.Cells[i + rowCount + 1, 39].Value = value[20 + i] == 1 ? "Pass" : "NG";
                        worksheet.Cells[i + rowCount + 1, 40].Value = Math.Round(value[32 + i], 4);
                        worksheet.Cells[i + rowCount + 1, 41].Value = Math.Round(value[34 + i], 4);
                        worksheet.Cells[i + rowCount + 1, 42].Value = value[36 + i] == 1 ? "Pass" : "NG";
                    }
                    package.SaveAs(new FileInfo(SavePath + "\\" + "本地数据" + "\\" + "自动运行数据" + "\\" + DateTime.Now.ToString("yyyy-MM") + "\\" + DateTime.Now.ToString("dd") + ".xlsx"));
                };
                CleanFile(SavePath + "\\" + "本地数据" + "\\" + "自动运行数据");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                MessageBox.Show("外部程序打开Excel文件！,无法保存数据");
            }
        }

        public static void SaveDataT(string path, DataSet data)
        {
            if (TableNamess == null)
            {
                var cy = File.ReadAllLines(@"Save_Table.txt");
                TableNamess = cy[0].Split(',');
            }
            FileInfo file = new System.IO.FileInfo(path);
            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Count != 0 ? package.Workbook.Worksheets[1] : package.Workbook.Worksheets.Add("Database");//是否存在工作表，不存在创建工作表
                int rowCount = worksheet.Dimension != null ? worksheet.Dimension.End.Row : 0;//获取数据最后行
                if (rowCount == 0)
                {
                    for (int i = 1; i < TableNamess.Length + 1; i++)
                    {
                        worksheet.Cells[1, i].Value = TableNamess[i - 1];
                        worksheet.Cells[1, i].Style.Font.Name = "微软雅黑";
                    }
                    rowCount++;
                }
                for (int i = 0; i < data.Tables[0].Rows.Count; i++)
                {
                    for (int j = 0; j < data.Tables[0].Columns.Count; j++)
                    {
                        worksheet.Cells[i + 2, j + 1].Value = data.Tables[0].Rows[i][j];
                    }
                }
                package.SaveAs(file);
            };
        }

        /// <summary>
        /// 文件夹初始化
        /// </summary>
        /// <param name="path">路径</param>
        public static void NewSaveData(string path)
        {
            if (!Directory.Exists(path))
            {
                //创建文件夹
                try
                {
                    Directory.CreateDirectory(path + "\\" + "本地数据");
                    Directory.CreateDirectory(path + "\\" + "本地数据" + "\\" + "四工位数据");
                    Directory.CreateDirectory(path + "\\" + "本地数据" + "\\" + "五工位数据");
                    Directory.CreateDirectory(path + "\\" + "本地数据" + "\\" + "自动运行数据");
                    Log.Info($"创建存储文件夹:{path + "\\" + "本地数据"}");
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message, ex);
                }
            }
            if (!Directory.Exists(path + "\\" + "本地数据" + "\\" + "四工位数据" + "\\" + DateTime.Now.ToString("yyyy-MM")))
            {
                //创建文件夹
                try
                {
                    Directory.CreateDirectory(path + "\\" + "本地数据" + "\\" + "四工位数据" + "\\" + DateTime.Now.ToString("yyyy-MM"));
                    Log.Info($"创建存储文件夹:{path + "\\" + "本地数据" + "\\" + "四工位数据" + "\\" + DateTime.Now.ToString("yyyy-MM")}");
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message, ex);
                }
            }
            if (!Directory.Exists(path + "\\" + "本地数据" + "\\" + "五工位数据" + "\\" + DateTime.Now.ToString("yyyy-MM")))
            {
                //创建文件夹
                try
                {
                    Directory.CreateDirectory(path + "\\" + "本地数据" + "\\" + "五工位数据" + "\\" + DateTime.Now.ToString("yyyy-MM"));
                    Log.Info($"创建存储文件夹:{path + "\\" + "本地数据" + "\\" + "五工位数据" + "\\" + DateTime.Now.ToString("yyyy-MM")}");
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message, ex);
                }
            }
            if (!Directory.Exists(path + "\\" + "本地数据" + "\\" + "自动运行数据" + "\\" + DateTime.Now.ToString("yyyy-MM")))
            {
                //创建文件夹
                try
                {
                    Directory.CreateDirectory(path + "\\" + "本地数据" + "\\" + "自动运行数据" + "\\" + DateTime.Now.ToString("yyyy-MM"));
                    Log.Info($"创建存储文件夹:{path + "\\" + "本地数据" + "\\" + "自动运行数据" + "\\" + DateTime.Now.ToString("yyyy-MM")}");
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message, ex);
                }
            }

        }

        /// <summary>
        /// 文件定时删除
        /// </summary>
        /// <param name="path"></param>
        public static void CleanFile(string path, bool boolmes = false)
        {
            string pathcl = path;
            DirectoryInfo dir = new DirectoryInfo(pathcl);
            if (boolmes)
            {
                var files = dir.GetFiles();
                foreach (var file in files)
                {
                    if (file.CreationTime < DateTime.Now.AddDays(-120))
                    {
                        file.Delete();
                    }
                }
            }
            else
            {
                var files = dir.GetDirectories();
                foreach (var file in files)
                {
                    if (file.CreationTime < DateTime.Now.AddDays(-120))
                    {
                        file.Delete();
                    }
                }
            }
        }
    }
}

