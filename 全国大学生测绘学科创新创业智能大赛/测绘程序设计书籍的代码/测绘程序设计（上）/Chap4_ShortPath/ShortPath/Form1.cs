using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ShortPath
{
    public partial class Form1 : Form
    {
        private SessionList dataList;
        public Form1()
        {
            InitializeComponent();
        }

        private void toolOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "计算数据|*.txt";
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                dataList = fileHelper.Read(openFileDialog1.FileName);
                richTextBox1.Text = dataList.ToString();
            }
        }

        private void toolCal_Click(object sender, EventArgs e)
        {
            Algo go = new Algo(dataList);
            go.Run();
            string res = go.ToString();
            richTextBox1.Text = res;

        }

        private void toolSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "计算结果|*.txt";
            save.FileName = "计算结果";
            if(save.ShowDialog() == DialogResult.OK)
            {
                var reader = new StreamWriter(save.FileName);
                reader.Write(richTextBox1.Text);
                reader.Close();
            }
        }

        private void toolHelp_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "第四次练习";
        }
    }
}
