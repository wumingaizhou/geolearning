using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaxiDrive;

namespace TaxiDrive
{
    public partial class Form1 : Form
    {
        private SessionList Data;
        public Form1()
        {
            InitializeComponent();
        }
        private void toolOpen_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var epoches = FileHelper.Read("T2", openFileDialog1.FileName);//读取每一行的数据储存起来
                Data = new SessionList(epoches);//计算过程，计算已经在打开文件的时候进行完了
                richTextBox1.Text = "读取成功";
            }
        }

        private void toolCal_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = Data.ToString();
        }

        private void toolSave_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "计算结果|*.txt";
            saveFileDialog1.FileName = "计算结果.txt";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileHelper.Write(Data, saveFileDialog1.FileName);
            }
        }

        private void toolHelp_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "测绘程序设计（上册）第一次练习";
        }
    }
}
