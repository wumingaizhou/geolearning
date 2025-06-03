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

namespace TimeTransform
{
    public partial class Form1 : Form
    {
        private List<Time> timeEntity;
        public string Calendar;
        public Form1()
        {
            InitializeComponent();
        }

        private void toolOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "原始数据|*.txt";
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                timeEntity = fileHelper.Read(openFileDialog1.FileName);
                Calendar = "------公历（年 月 日 时 分 秒）------\n";
                for(int i = 0;i < timeEntity.Count;i++)
                {
                    Calendar += timeEntity[i].ToString();
                }
                richTextBox1.Text = Calendar;
            }
        }

        private void toolCal_Click(object sender, EventArgs e)
        {
            Algo go = new Algo(timeEntity);
            var JD = go.GetJD();//儒略日
            var Accday = go.GetAccday();//年积日
            var fish = go.GetFish();
            richTextBox1.Text = Calendar + JD + Accday + fish;

        }

        private void toolSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.FileName = "计算结果";
            saveFileDialog1.Filter = "计算结果*|.txt";
            if(saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamWriter streamWriter1 = new StreamWriter(saveFileDialog1.FileName);
                streamWriter1.Write(richTextBox1.Text);
                streamWriter1.Close();
            }
            

        }

        private void toolHelp_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "第五次练习";
        }
    }
}
