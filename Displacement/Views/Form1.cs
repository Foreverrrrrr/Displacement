using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Displacement.Views
{
    public partial class Form1 : Form
    {
        private Queue<byte[]> qu = new Queue<byte[]>();
        public Form1()
        {
            InitializeComponent();
        }
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        [DllImport("User32.dll ", EntryPoint = "SetParent")]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll ", EntryPoint = "ShowWindow")]
        public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpszClass, string lpszWindow);      //按照窗体类名或窗体标题查找窗体

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        private static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern uint SetWindowLong(IntPtr hwnd, int nIndex, uint newLong);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern uint GetWindowLong(IntPtr hwnd, int nIndex);



        const int GWL_STYLE = -16;
        const uint WS_BORDER = (uint)0x00800000L;
        const uint WS_THICKFRAME = (uint)0x00040000L;
        const uint SWP_NOMOVE = 0x0002;
        const uint SWP_NOSIZE = 0x0001;
        const uint SWP_NOZORDER = 0x0004;
        const uint SW_MAXIMIZE = 3;

        private const int HWND_TOP = 0x0;
        private const int WM_COMMAND = 0x0112;
        private const int WM_QT_PAINT = 0xC2DC;
        private const int WM_PAINT = 0x000F;
        private const int WM_SIZE = 0x0005;
        private const int SWP_FRAMECHANGED = 0x0020;
        public enum ShowWindowStyles : short
        {
            SW_HIDE = 0,
            SW_SHOWNORMAL = 1,
            SW_NORMAL = 1,
            SW_SHOWMINIMIZED = 2,
            SW_SHOWMAXIMIZED = 3,
            SW_MAXIMIZE = 3,
            SW_SHOWNOACTIVATE = 4,
            SW_SHOW = 5,
            SW_MINIMIZE = 6,
            SW_SHOWMINNOACTIVE = 7,
            SW_SHOWNA = 8,
            SW_RESTORE = 9,
            SW_SHOWDEFAULT = 10,
            SW_FORCEMINIMIZE = 11,
            SW_MAX = 11
        }



        // 控制嵌入程序的位置和尺寸
        private void ResizeControl(Process p)
        {
            SendMessage(p.MainWindowHandle, WM_COMMAND, WM_PAINT, 0);
            PostMessage(p.MainWindowHandle, WM_QT_PAINT, 0, 0);

            SetWindowPos(
            p.MainWindowHandle,
            HWND_TOP,
            0, // 设置偏移量,把原来窗口的菜单遮住
            0,
            (int)this.Width,
            (int)this.Height,
            SWP_FRAMECHANGED);

            SendMessage(p.MainWindowHandle, WM_COMMAND, WM_SIZE, 0);
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

            EmbedProcess();
            this.btnOpen.Enabled = true;
            this.btnClose.Enabled = false;
            this.btnStart.Enabled = false;
            this.btnEnd.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
        private void EmbedProcess()
        {
            Process p = new Process();
            p.StartInfo.FileName = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase+"应用目录\\BIW.Monitor.W500.Main.exe";
            p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Minimized;//加上这句效果更好
            p.Start();
            System.Threading.Thread.Sleep(1000);//加上，100如果效果没有就继续加大
            IntPtr P = new IntPtr();
            while (true)
            {
                P = FindWindow(null, "W500 - [2]");//通过标题查找窗口句柄,当然也可以按class查找，如果需要查找子窗口需要FindWindowEx函数
                Thread.Sleep(100);
                if (P == IntPtr.Zero)
                    continue;
                else
                    break;
            }

            // 除去窗体边框. 
            uint wndStyle = GetWindowLong(P, GWL_STYLE);
            wndStyle &= ~WS_BORDER;
            wndStyle &= ~WS_THICKFRAME;


            SetWindowLong(P, GWL_STYLE, wndStyle);
            // SetParent(P, this.Handle); //为要显示外部程序的容器，这里需要注意一些
            SetParent(P, this.tabPage1.Handle); //为要显示外部程序的容器，这里需要注意一些
            ShowWindow(P, 3);

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
             KillProcess("W500");
        }
        public static void KillProcess(string strProcessesByName)//关闭线程
        {
            foreach (Process p in Process.GetProcesses())//GetProcessesByName(strProcessesByName))
            {
                if (p.ProcessName.ToUpper().Contains(strProcessesByName))
                {
                    try
                    {
                        p.Kill();
                        p.WaitForExit(); // possibly with a timeout
                    }
                    catch (Win32Exception e)
                    {
                        MessageBox.Show(e.Message.ToString());   // process was terminating or can't be terminated - deal with it
                    }
                    catch (InvalidOperationException e)
                    {
                        MessageBox.Show(e.Message.ToString()); // process has already exited - might be able to let this one go
                    }
                }
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            this.btnOpen.Enabled = false;
            this.btnClose.Enabled = true;
            this.btnStart.Enabled = true;
            this.btnEnd.Enabled = false;

            if (!this.spPLC.IsOpen) //如果串口未打开,打开串口
            {
                this.spPLC.Open();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.btnOpen.Enabled = true;
            this.btnClose.Enabled = false;
            this.btnStart.Enabled = false;
            this.btnEnd.Enabled = false;

            if (this.spPLC.IsOpen)  //如果串口已打开，关闭串口
            {
                this.spPLC.Close();
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            this.btnStart.Enabled = false;
            this.btnEnd.Enabled = true;
            byte[] waitebyte = new byte[] { 0xAA ,0x00 ,0x08 ,0x00 ,0x00 ,0x00 ,0x00 ,0x31 ,0x32 ,0x33 ,0x00 ,0x05 ,0x06 ,0x07 ,0x08 ,0x09 ,0x10 ,0x11 ,0x12 ,0x13 ,0x14 ,0x15 ,0x16 ,0x17 ,0x18 ,0x19 ,0x20 ,0x00 ,0x00 ,0x00 ,0x14 ,0x15 ,0x16 ,0x17 ,0x18,0x19
,0x20 ,0x00 ,0x00 ,0x00 ,0x14 ,0x15 ,0x16 ,0x17 ,0x18 ,0x19 ,0x20 ,0x00 ,0x00 ,0x00 ,0x14 ,0x15 ,0x16 ,0x17 ,0x18 ,0x19 ,0x20 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00,0x4D };
            this.spPLC.Write(waitebyte, 0, waitebyte.Length);
        }

        private void btnEnd_Click(object sender, EventArgs e)
        {
            this.btnStart.Enabled = true;
            this.btnEnd.Enabled = false;
            spPLC.DiscardInBuffer();
            byte[] waitebyte = new byte[] { 0xAA ,0x00 ,0x04 ,0x00 ,0x00 ,0x00 ,0x00 ,0x31 ,0x32 ,0x33 ,0x00 ,0x05 ,0x06 ,0x07 ,0x08 ,0x09 ,0x10 ,0x11 ,0x12 ,0x13 ,0x14 ,0x15 ,0x16 ,0x17 ,0x18 ,0x19 ,0x20 ,0x00 ,0x00 ,0x00 ,0x14 ,0x15 ,0x16 ,0x17 ,0x18,0x19
,0x20 ,0x00 ,0x00 ,0x00 ,0x14 ,0x15 ,0x16 ,0x17 ,0x18 ,0x19 ,0x20 ,0x00 ,0x00 ,0x00 ,0x14 ,0x15 ,0x16 ,0x17 ,0x18 ,0x19 ,0x20 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00,0x49 };
            this.spPLC.Write(waitebyte, 0, waitebyte.Length);
            int milliseconds = 300;
            Thread.Sleep(milliseconds);
            Datereceive(sender, e);
        }

        private void Datereceive(object sender, EventArgs e)
        {
            //获取串口缓冲区中接收到的数据字节数
            int mByte = this.spPLC.BytesToRead;
            //开辟一个byte类型的数组，大小为缓冲区中的数据大小
            byte[] mData = new byte[mByte];
            //从串口缓冲区中读取数据
            this.spPLC.Read(mData, 0, mByte);
            //将byte类型数组数据转换成字符串

            string msg = "";

            //  txtReceive.Text = Convert.ToString( mData[5]);
            if (mData.Length == 66)
            {
                qu.Enqueue(mData);
                string to = "";
                byte[] readbyte = qu.Dequeue();
                to = (readbyte[4]).ToString("X2");

                msg = to;

            }

            this.BeginInvoke(new MethodInvoker(delegate
            {
                this.txtReceive.Clear();
                //  int j = Int32.Parse(msg);
                int i = Convert.ToInt32(msg);
                if (i % 10 == 6)
                {
                    this.txtReceive.Text = "NG" + "\r\n";
                    this.txtReceive.BackColor = System.Drawing.Color.Red;
                }
                else if (i % 10 == 5)
                {
                    this.txtReceive.Text = "OK" + "\r\n";
                    this.txtReceive.BackColor = System.Drawing.Color.Green;
                }
                else
                { MessageBox.Show("再试一次"); }

            }));

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
    }

}
