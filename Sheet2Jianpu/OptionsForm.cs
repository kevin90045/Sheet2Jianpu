using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sheet2Jianpu
{
    public partial class OptionsForm : Form
    {
        public OptionsForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            checkBox_align.Checked = MusicXml.Tools.Converter.Options.GetInstance().Align;
            checkBox_sharp_alter.Checked = MusicXml.Tools.Converter.Options.GetInstance().AllSharpAlter;
        }

        private void checkBox_align_CheckedChanged(object sender, EventArgs e)
        {
            var options = MusicXml.Tools.Converter.Options.GetInstance();
            options.Align = checkBox_align.Checked;
        }

        private void checkBox_sharp_alter_CheckedChanged(object sender, EventArgs e)
        {
            var options = MusicXml.Tools.Converter.Options.GetInstance();
            options.AllSharpAlter = checkBox_sharp_alter.Checked;
        }
    }
}
