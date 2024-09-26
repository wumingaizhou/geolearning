using System;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;

namespace Dadixian
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            dataGridView1.Rows.Add();
            dataGridView1.Rows[0].Cells[0].Value = "1";
            dataGridView1.Rows[1].Cells[0].Value = "2";
        }
        List<Data> datas = new List<Data>();
        double tuoqiu;//选择的椭球体参数
        double tuoqiu1;//选择的椭球体参数
        double b;//短半轴
        double s;//大地线长
        private void toolOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "|*.txt";
            try
            {
                if(openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    var reader = new StreamReader(openFileDialog1.FileName);
                    while(!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var data = FileHelper.Read(line);
                        datas.Add(data);
                    }
                    reader.Close();
                }
                for(int i = 0;i < 2;i++)
                {
                    dataGridView1.Rows[i].Cells[1].Value = datas[i].B;
                    dataGridView1.Rows[i].Cells[2].Value = datas[i].L;
                }
                toolStripStatusLabel1.Text = "状态：导入数据成功";
                MessageBox.Show("写入数据成功");
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        
        private void toolCal_Click(object sender, EventArgs e)
        {
            if(dataGridView1.Rows[0].Cells[1].Value == null)
            {
                MessageBox.Show("未输入数据！");
            }
            else if(tuoqiu == 0)

            {
                MessageBox.Show("未选择椭球参数！");
            }
            else
            {
                var go = new Algo(datas, tuoqiu,tuoqiu1,b);
                textBox2.Text = $"{go.Distance} (m)";
                s = go.Distance;
            }
        }
        private void toolSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "结果输出|*.txt";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var writer = new StreamWriter(saveFileDialog1.FileName);
                writer.WriteLine($"{datas[0].B}  {datas[1].B}\n {datas[0].L}  {datas[1].L}\n大地线长：{s}");
                writer.Close();
            }
        }
        private void toolExit_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }
        private void ball1_Click(object sender, EventArgs e)
        {
            tuoqiu = 6.69342162297 * 1e-3;
            tuoqiu1 = 6.73852541468 * 1e-3;
            b = 6356863.0187;
            toolStripStatusLabel2.Text = "椭球：克拉索夫斯基";
        }

        private void ball2_Click(object sender, EventArgs e)
        {
            tuoqiu = 6.69438499959 * 1e-3;
            tuoqiu1 = 6.73950181947 * 1e-3;
            b = 6356755.2881;
            toolStripStatusLabel2.Text = "椭球：IUGG1975";
        }

        private void ball3_Click(object sender, EventArgs e)
        {
            tuoqiu = 6.69438002290 * 1e-3;
            tuoqiu1 = 6.73949677548 * 1e-3;
            b = 6356752.3141;
            toolStripStatusLabel2.Text = "椭球：CGCS2000";
        }
        private void bottom(object sender, MouseEventArgs e)
        {
            Cursor.Current = Cursors.Hand;
        }
    }
}
