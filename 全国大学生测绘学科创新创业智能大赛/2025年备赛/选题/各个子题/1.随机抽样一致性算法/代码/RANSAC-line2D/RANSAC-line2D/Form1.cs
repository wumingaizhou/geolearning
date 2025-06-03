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

namespace RANSAC_line2D
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private List<Point> points = new List<Point>(); //存储所有点的数据

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.FileName = "数据-2D";
                openFileDialog.Filter = "文本数据|*.txt";
                if(openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    points.Clear(); //重复添加数据时，需要清除原有数据
                    var reader = new StreamReader(openFileDialog.FileName);
                    reader.ReadLine(); //第一行不需要
                    while(!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if(line.Length > 0)
                        {
                            var tempPoint = new Point(line);
                            points.Add(tempPoint);
                        }
                    }
                    reader.Close();
                }
                toolStripStatusLabel1.Text = "状态：读取数据成功";
                MessageBox.Show("读取成功");
                tabControl1.SelectedIndex = 0;

                // dataGrid 更新数据
                foreach (var p in points)
                {
                    dataGridView1.Rows.Add(p.ID, p.X, p.Y);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if(points.Count > 0)
            {
                var sample = Convert.ToInt32(textBox1.Text); // 最小样本数
                var threshold = Convert.ToDouble(textBox3.Text); //距离阈值
                var iterations = Convert.ToInt32(textBox2.Text); // 最大迭代次数
                Algo algo = new Algo(sample, threshold, iterations);
                var lineGood = algo.go(points); // 最佳直线

                toolStripStatusLabel1.Text = "状态：计算完成";
                MessageBox.Show("计算完成");
                tabControl1.SelectedIndex = 1;

                richTextBox1.Text = $@"
输出结果：
直线参数：Ax + By + C = 0;
A = {lineGood.LineA}
B = {lineGood.LineB}
C = {lineGood.LineC}
内点数量：
N = {lineGood.CountPoint}

";
            }
            else
            {
                MessageBox.Show("未导入数据");
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "输出结果";
            saveFileDialog.Filter = "文本文档|*.txt";
            if(saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                var writer = new StreamWriter(saveFileDialog.FileName);
                writer.Write(richTextBox1.Text);
                writer.Close();
                toolStripStatusLabel1.Text = "状态：保存成功";
                MessageBox.Show("保存成功");
            }
        }
    }
}
