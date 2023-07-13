using Displacement.FunctionCall;
using Displacement.ViewModels;
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
    public partial class SetPower : Form
    {
        MainWindowViewModel model;
        public SetPower()
        {
            InitializeComponent();
            model = MainWindowViewModel.thisModel;
            textBox1_V.Text = model.Set_V;
            textBox2_A.Text = model.Set_A;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double V = 0;
            double A = 0;
            if (double.TryParse(textBox1_V.Text, out V) && double.TryParse(textBox2_A.Text, out A))
            {
                if (V > 0 && V < 30)
                {
                    if (A > 0 && A < 5)
                    {
                        ConfigurationFiles.thisfiles.AmendPath(model, "Set_A", A.ToString());
                        ConfigurationFiles.thisfiles.AmendPath(model, "Set_V", V.ToString());
                    }
                }
            }
            this.Close();
        }
    }
}
