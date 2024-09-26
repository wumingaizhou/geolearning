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

namespace SpaceFowardIntersection
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private Data data;
        private void ToolOpen_Click(object sender, EventArgs e)
        {
            //dataGridView1.Rows.Clear();
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "|*.txt";
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var reader = new StreamReader(openFileDialog1.FileName);
                    string line = "";
                    while(!reader.EndOfStream)
                    {
                        line += reader.ReadLine() + "/s";
                    }
                    data = FileHelper.Read(line);
                    reader.Close();
                    dataGridView1Tool();//辅助数据填入
                    MessageBox.Show("读取数据成功");
                    toolStripStatusLabel1.Text = "数据导入状态：成功导入";
                }
                catch(Exception ex)
                {
                    throw ex;
                }

            }
        }
        private void dataGridView1Tool()
        {
            dataGridView1.Rows.Add();
            dataGridView1.Rows[0].Cells[0].Value = "1";
            dataGridView1.Rows[1].Cells[0].Value = "2";
            for(int i = 1;i < 10;i++)
            {
                dataGridView1.Rows[0].Cells[i].Value = data.totalList[0][i - 1];
            }
            for (int i = 1; i < 10; i++)
            {
                dataGridView1.Rows[1].Cells[i].Value = data.totalList[1][i - 1];
            }
        }

        private void ToolCal_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "u   v   w\n";
            richTextBox1.Text += $"{data.u1:F3} {data.v1:F3} {data.w1:F3}\n{data.u2:F3} {data.v2:F3} {data.w2:F3}\n";
            richTextBox1.Text += "N1   N2\n";
            richTextBox1.Text += $"{data.N1:F3}  {data.N2:F3}\n";
            richTextBox1.Text += "X   Y   Z\n";
            richTextBox1.Text += $"{data.X:F3} {data.Y:F3} {data.Z:F3}";
            Tab.SelectedIndex = 1;
            MessageBox.Show("计算成功");
            toolStripStatusLabel2.Text = "计算状态：计算成功";
        }


        private void toolSave_Click(object sender, EventArgs e)
        {
            if(richTextBox1.Text != "")
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "|*.txt";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    var writer = new StreamWriter(saveFileDialog1.FileName);
                    writer.Write(richTextBox1.Text);
                    writer.Close();
                }
                MessageBox.Show("保存成功");
            }
            else
            {
                MessageBox.Show("还未计算！");
            }
        }
        private void botton(object sender, MouseEventArgs e)
        {
            Cursor.Current = Cursors.Hand;//鼠标
        }
    }
}
