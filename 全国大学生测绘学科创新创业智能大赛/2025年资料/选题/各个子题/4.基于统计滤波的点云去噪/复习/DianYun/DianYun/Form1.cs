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

namespace DianYun
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
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog
                {
                    FileName = "输入数据",
                    Filter = "|*.txt"
                };
                if(openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    var reader = new StreamReader(openFileDialog1.FileName);
                    points.Clear();
                    reader.ReadLine();

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if(line.Length > 0)
                        {
                            var point = new Point(line);
                            points.Add(point);
                        }
                    }

                    reader.Close();

                    foreach(var p in points)
                    {
                        dataGridView1.Rows.Add(p.X, p.Y, p.Z);
                    }

                    toolStripStatusLabel1.Text = "状态：导入成功";
                    tabControl1.SelectedIndex = 0;
                    MessageBox.Show("导入成功");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (points.Count == 0) return;
                Algo algo = new Algo(points);
                richTextBox1.Text = algo.GetResult();

                toolStripStatusLabel1.Text = "状态：计算成功";
                tabControl1.SelectedIndex = 1;
                MessageBox.Show("计算成功");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            try
            {
                if (points.Count == 0) return;
                SaveFileDialog saveFileDialog1 = new SaveFileDialog
                {
                    FileName = "输出报告",
                    Filter = "|*.txt"
                };
                if(saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    var writer = new StreamWriter(saveFileDialog1.FileName);

                    writer.Write(richTextBox1.Text);

                    writer.Close();

                    toolStripStatusLabel1.Text = "状态：保存成功";
                    tabControl1.SelectedIndex = 1;
                    MessageBox.Show("保存成功");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
