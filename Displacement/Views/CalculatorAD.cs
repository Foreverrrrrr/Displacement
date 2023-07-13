using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Displacement.Views
{
    public partial class CalculatorAD : Form
    {
        public CalculatorAD()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var t = Convert.ToDouble(textBox2.Text) - Convert.ToDouble(textBox1.Text);
                var k = Convert.ToDouble(textBox3.Text) / t;
                textBox4.Text = k.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("输入文本检测到有非数字字符");
            }
        }
    }
}
