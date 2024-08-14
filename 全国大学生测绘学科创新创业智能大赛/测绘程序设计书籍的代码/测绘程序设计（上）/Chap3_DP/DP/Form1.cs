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

namespace DP
{
    public partial class Form1 : Form
    {
        pointEntity oripoints = new pointEntity();
        public Form1()
        {
            InitializeComponent();
        }

        private void toolOpen_Click(object sender, EventArgs e)
        {
            var openfileDialog1 = new OpenFileDialog();
            if(openfileDialog1.ShowDialog() == DialogResult.OK)
            {
                oripoints = fileHelper.Read(openfileDialog1.FileName);//读取文件，储存点信息到pointEntity里
                for(int i = 0;i < oripoints.Count - 1;i++)//将所有点显示出来
                {
                    //dataGridView是新内容
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[i].Cells[0].Value = oripoints[i].ID;
                    dataGridView1.Rows[i].Cells[1].Value = oripoints[i].X;
                    dataGridView1.Rows[i].Cells[2].Value = oripoints[i].Y;
                }
                richTextBox1.Text = "读取成功，总共：" + Convert.ToString(oripoints.Count) + "个点\n";
                tabControl1.SelectedIndex = 0;
            }
        }

        private void toolSave_Click(object sender, EventArgs e)
        {
            //输出压缩后节点信息到TXT文档中
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "压缩计算结果|*.txt";//输出指定文件格式
            saveFileDialog1.FileName = "压缩计算结果输出";//输出默认文件名字
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string strpath = saveFileDialog1.FileName;
                StreamWriter SW = new StreamWriter(strpath);
                SW.Write(richTextBox1.Text);
                SW.Close();
            }
        }

        private void toolCal_Click(object sender, EventArgs e)
        {
            try
            {
                int setMax = Convert.ToInt32(toolStripComboBox1.Text);//获取阈值
                var go = new Algo(oripoints,setMax);
                var result = go.calRun();//result是结果堆栈
                richTextBox1.Text += "算法执行成功！压缩后的点有：" + Convert.ToString(result.Count) + "个\n" + "--------------------------------\n";
                string repts = "点号         X坐标         Y坐标\n";
                while(result.Count != 0)
                {
                    Point temp = result.Pop();
                    repts += $"{ Convert.ToString(temp.ID):S3}" + "      " + Convert.ToString(temp.X) + "     " + Convert.ToString(temp.Y) + "\n";
                }
                richTextBox1.Text += repts;
                tabControl1.SelectedIndex = 1;

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        private void toolData_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private void toolReport_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }

        private void toolHelp_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "测绘程序设计（上册）第三次练习";
            tabControl1.SelectedIndex = 1;
        }
    }
}
