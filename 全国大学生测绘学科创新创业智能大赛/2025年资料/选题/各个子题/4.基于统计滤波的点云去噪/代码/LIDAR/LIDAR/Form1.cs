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

namespace LIDAR
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        List<Point> points = new List<Point>();

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //读取数据
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.FileName = "输入数据";
                openFileDialog1.Filter = "|*.txt";
                if(openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    points.Clear(); // 把之前的清除掉

                    var reader = new StreamReader(openFileDialog1.FileName);
                    reader.ReadLine(); //第一行不用读
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if(line.Length > 0)
                        {
                            var tempPoint = new Point(line);
                            points.Add(tempPoint);
                        }
                    }

                    reader.Close();
                    int number = 0; // 点号ID
                    foreach(var p in points)
                    {
                        number++;
                        dataGridView1.Rows.Add(number, p.X, p.Y, p.Z);
                    }
                    tabControl1.SelectedIndex = 0;
                    toolStripStatusLabel1.Text = "状态：导入数据成功";
                    MessageBox.Show("导入成功");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if(points.Count > 0)
                {
                    var cellSize = Convert.ToDouble(textBox1.Text);
                    var n_neighbors = Convert.ToInt32(textBox2.Text);
                    var k_std = Convert.ToDouble(textBox3.Text);
                    Algo algo = new Algo(points, cellSize, n_neighbors, k_std);

                    var result = algo.Go();

                    var Resulttext = 
$@"结果：原点云数量：{points.Count}。    去除噪声点后的点云数量：{result.Count}";
                    richTextBox1.Text = Resulttext;
                    tabControl1.SelectedIndex = 1;
                    toolStripStatusLabel1.Text = "状态：计算成功";
                    MessageBox.Show("计算成功");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
