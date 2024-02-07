using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MusicXml.Tools;
using MusicXml.Tools.Converter;
using OpenXml.Tools;

namespace Sheet2Jianpu
{
    public partial class Main : Form
    {
        public static Options options;

        public Main()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            InitializeListView();
            SetToolTips();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            Qromodyn.EmbeddedDllClass.ExtractEmbeddedDlls("DocumentFormat.OpenXml.dll");
        }

        private void InitializeListView()
        {
            listView_parts.Columns.Add("部別順序", 60, HorizontalAlignment.Left);
            listView_parts.Columns.Add("樂器", 109, HorizontalAlignment.Left);
        }

        private void SetToolTips()
        {
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(button_remove, "移除");
            toolTip.SetToolTip(button_up, "上移");
            toolTip.SetToolTip(button_down, "下移");
            toolTip.SetToolTip(pictureBox_about, "關於");
        }

        private void button_chromatic_Click(object sender, EventArgs e)
        {
            AddInstrument("半音階口琴");
        }

        private void button_tremolo_Click(object sender, EventArgs e)
        {
            AddInstrument("複音口琴");
        }
        
        private void button_blues_Click(object sender, EventArgs e)
        {
            AddInstrument("十孔口琴");
        }

        private void button_bariton_Click(object sender, EventArgs e)
        {
            AddInstrument("中音口琴");
        }

        private void button_soprano_horn_Click(object sender, EventArgs e)
        {
            AddInstrument("高音銅角口琴");
        }

        private void button_alto_horn_Click(object sender, EventArgs e)
        {
            AddInstrument("中音銅角口琴");
        }

        private void button_chord_Click(object sender, EventArgs e)
        {
            AddInstrument("和弦口琴");
        }

        private void button_bass_Click(object sender, EventArgs e)
        {
            AddInstrument("低音口琴");
        }

        private void button_double_bass_Click(object sender, EventArgs e)
        {
            AddInstrument("倍低音口琴");
        }

        private void AddInstrument(string instrument)
        {
            ListViewItem item = new ListViewItem();
            item.SubItems.Clear();
            item.SubItems[0].Text = (listView_parts.Items.Count + 1).ToString();
            item.SubItems.Add(instrument);
            listView_parts.Items.Add(item);
        }

        private void button_remove_Click(object sender, EventArgs e)
        {
            if (listView_parts.SelectedIndices.Count != 0)
            {
                int index = listView_parts.SelectedIndices[0];
                listView_parts.Items.RemoveAt(index);
                UpdateOrder();
                if (index > 0)
                    listView_parts.Items[index - 1].Selected = true;
            }
        }
        
        private void button_up_Click(object sender, EventArgs e)
        {
            if (listView_parts.SelectedIndices.Count != 0 &&
                listView_parts.SelectedIndices[0] != 0)
            {
                int index = listView_parts.SelectedIndices[0];
                var item = listView_parts.Items[index];
                listView_parts.Items.RemoveAt(index);
                listView_parts.Items.Insert(index - 1, item);
                UpdateOrder();
                listView_parts.Items[index - 1].Selected = true;
            }
        }

        private void button_down_Click(object sender, EventArgs e)
        {
            if (listView_parts.SelectedIndices.Count != 0 &&
                listView_parts.SelectedIndices[0] != listView_parts.Items.Count - 1)
            {
                int index = listView_parts.SelectedIndices[0];
                var item = listView_parts.Items[index];
                listView_parts.Items.RemoveAt(index);
                listView_parts.Items.Insert(index + 1, item);
                UpdateOrder();
                listView_parts.Items[index + 1].Selected = true;
            }
        }

        private void UpdateOrder()
        {
            for (int i = 0; i < listView_parts.Items.Count; i++)
                listView_parts.Items[i].SubItems[0].Text = (i + 1).ToString();
        }

        private void button_options_Click(object sender, EventArgs e)
        {
            OptionsForm optionsForm = new OptionsForm();
            optionsForm.ShowDialog();
        }

        private void pictureBox_about_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
            aboutForm.ShowDialog();
        }

        private void button_convert_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Title = "選擇樂譜";
            openFileDialog.Filter = "MusicXml 檔案 (*.xml,*.musicxml)|*.xml;*musicxml";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Word 文件 (*.docx)|*.docx";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var instruments = GetInstruments();
                    List<List<List<char>>> score;
                    if (MusicXmlConverter.Convert(openFileDialog.FileName, instruments, out score))
                    {
                        ScoreWriter.Write(saveFileDialog.FileName, score);
                        MessageBox.Show("轉換成功", "Sheet2Jianpu", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    }
                    else
                        MessageBox.Show("中止程序", "Sheet2Jianpu", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1); ;
                }
            }
        }

        private List<Instruments> GetInstruments()
        {
            List<Instruments> instruments = new List<Instruments>();
            foreach (ListViewItem item in listView_parts.Items)
            {
                string instrment = item.SubItems[1].Text;
                if (instrment == "半音階口琴")
                    instruments.Add(Instruments.ChromaticHarmonica);
                else if (instrment == "複音口琴")
                    instruments.Add(Instruments.TremoloHarmonica);
                else if (instrment == "十孔口琴")
                    instruments.Add(Instruments.BluesHarmonica);
                else if (instrment == "中音口琴")
                    instruments.Add(Instruments.BaritonHarmonica);
                else if (instrment == "高音銅角口琴")
                    instruments.Add(Instruments.SopranoHornHarmonica);
                else if (instrment == "中音銅角口琴")
                    instruments.Add(Instruments.AltoHornHarmonica);
                else if (instrment == "和弦口琴")
                    instruments.Add(Instruments.ChordHarmonica);
                else if (instrment == "低音口琴")
                    instruments.Add(Instruments.ContraBassHarmonica);
                else if (instrment == "倍低音口琴")
                    instruments.Add(Instruments.DoubleBassHarmonica);
            }
            return instruments;
        }        
    }
}
