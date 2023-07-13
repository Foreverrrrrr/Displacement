using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MotorAssembly.Views
{
    public partial class ReName : Form
    {
        public event Action<string> NewNameEv;
        public ReName()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != null && textBox1.Text != "")
            {
                if (NewNameEv != null)
                {
                    NewNameEv.Invoke(textBox1.Text);
                }
            }
            else
            {
                MessageBox.Show("未输入正确名称", "提示");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
