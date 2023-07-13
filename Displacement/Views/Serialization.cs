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
    public partial class Serialization : Form
    {
        MainWindowViewModel model;
        public Serialization()
        {
            InitializeComponent();
            model = MainWindowViewModel.thisModel;
            Standard1.Text = model.Standard1;
            Standard2.Text = model.Standard2;
            Standard3.Text = model.Standard3;
            Standard4.Text = model.Standard4;

            Tone_up1.Text = model.Tone_up1;
            Tone_up2.Text = model.Tone_up2;
            Tone_up3.Text = model.Tone_up3;
            Tone_up4.Text = model.Tone_up4;

            Time_set.Text = model.StandardTime_set;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ConfigurationFiles.thisfiles.AmendPath(model, "Standard1", Standard1.Text);
            ConfigurationFiles.thisfiles.AmendPath(model, "Standard2", Standard2.Text);
            ConfigurationFiles.thisfiles.AmendPath(model, "Standard3", Standard3.Text);
            ConfigurationFiles.thisfiles.AmendPath(model, "Standard4", Standard4.Text);
            ConfigurationFiles.thisfiles.AmendPath(model, "Tone_up1", Tone_up1.Text);
            ConfigurationFiles.thisfiles.AmendPath(model, "Tone_up2", Tone_up2.Text);
            ConfigurationFiles.thisfiles.AmendPath(model, "Tone_up3", Tone_up3.Text);
            ConfigurationFiles.thisfiles.AmendPath(model, "Tone_up4", Tone_up4.Text);

            ConfigurationFiles.thisfiles.AmendPath(model, "StandardTime_set", Time_set.Text);
            this.Close();
        }

        private void Serialization_Load(object sender, EventArgs e)
        {

        }

        private void Serialization_FormClosing(object sender, FormClosingEventArgs e)
        {
           // button1_Click(sender, null);
        }
    }
}
