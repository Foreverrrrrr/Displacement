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
    public partial class NewParameter : Form
    {
        public event Action<string> ParameterChangedEv;
        public NewParameter()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != null && textBox1.Text != "")
            {
                if (ParameterChangedEv != null)
                {
                    ParameterChangedEv.Invoke(textBox1.Text);
                }
            }
            else
            {
                MessageBox.Show("未输入参数名称", "提示");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
