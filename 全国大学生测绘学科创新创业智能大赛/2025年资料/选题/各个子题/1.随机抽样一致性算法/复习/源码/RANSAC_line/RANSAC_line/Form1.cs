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

namespace RANSAC_line
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        

        List<Point> points = new List<Point>();

        // 读取数据
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog
                {
                    FileName = "输入数据",
                    Filter = "|*.txt"
                };
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    points.Clear(); // 防止重复输入数据

                    var reader = new StreamReader(openFileDialog1.FileName); // 读取数据
                    reader.ReadLine();
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine().Trim();
                        if(line.Length > 0)
                        {
                            var read_point = new Point(line);
                            points.Add(read_point);
                        }
                    }
                    reader.Close();

                    // 更新 dataGridView
                    foreach(var p in points)
                    {
                        dataGridView1.Rows.Add(p.ID, p.X, p.Y, p.Z);
                    }
                    //提示成功
                    toolStripStatusLabel1.Text = "状态：导入成功";
                    tabControl1.SelectedIndex = 0;
                    MessageBox.Show("导入成功");
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        // 执行计算
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (points.Count == 0)
            {
                MessageBox.Show("请导入数据");
                return;
            }
            // 存储用户变量
            UserInput.min_samples = Convert.ToInt32(textBox1.Text.Trim());
            UserInput.threshold = Convert.ToDouble(textBox2.Text.Trim());
            UserInput.max_times = Convert.ToInt32(textBox3.Text.Trim());

            Algo algo = new Algo(points);
            richTextBox1.Text = algo.GetResult();

            //提示成功
            toolStripStatusLabel1.Text = "状态：计算成功";
            tabControl1.SelectedIndex = 1;
            MessageBox.Show("计算成功");
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (points.Count == 0)
            {
                MessageBox.Show("请导入数据");
                return;
            }

            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                FileName = "保存结果",
                Filter = "|*.txt"
            };
            if(saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var writer = new StreamWriter(saveFileDialog1.FileName);
                writer.Write(richTextBox1.Text);
                writer.Close();
                //提示成功
                toolStripStatusLabel1.Text = "状态：保存成功";
                tabControl1.SelectedIndex = 1;
                MessageBox.Show("保存成功");
            }
        }
    }

    // 用户输入的参数，静态类
    public static class UserInput
    {
        public static int min_samples;
        public static double threshold;
        public static int max_times;
    }
}
