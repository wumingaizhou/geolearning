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

namespace MapSheetCode
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
                //读取数据
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.FileName = "输入数据";
                openFileDialog1.Filter = "数据|*.txt";
                if(openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    points.Clear();
                    var reader = new StreamReader(openFileDialog1.FileName);
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


                    //显示数据
                    //先清除原先的数据
                    dataGridView1.Rows.Clear();
                    foreach (var p in points)
                    {
                        dataGridView1.Rows.Add(p.ID, p.L, p.B);
                    }
                    toolStripStatusLabel1.Text = "状态：导入数据成功";
                    MessageBox.Show("导入成功");
                    tabControl1.SelectedIndex = 0;
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
                var scale = GetScaleSelected(groupBox1); //获得用户选择的比例尺
                if(scale == null)
                {
                    //如果未选择比例尺
                    MessageBox.Show("请选择比例尺！");
                    return;
                }
                if(points.Count > 0)
                {
                    Algo algo = new Algo(points,scale);
                    algo.Go(); //执行主算法

                    //保存结果
                    var pointsResult = "点名，经度，纬度，比例尺，图幅编号\n";
                    foreach(var p in points)
                    {
                        var templine = $"{p.ID}，{p.L}，{p.B}，{scale}，{p.newMapCode}\n";
                        pointsResult += templine;
                    }
                    var result = pointsResult;
                    richTextBox1.Text = result;

                    toolStripStatusLabel1.Text = "状态：计算成功";
                    MessageBox.Show("计算成功");
                    tabControl1.SelectedIndex = 1;
                }
                else
                {
                    MessageBox.Show("还未导入数据!");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        //获取用户选择的比例尺
        private string GetScaleSelected(GroupBox groupBox)
        {
            foreach(Control control in groupBox.Controls)
            {
                if(control is RadioButton && control.Focused)
                {
                    return control.Text;
                }
            }
            return null;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.FileName = "保存结果";
                saveFileDialog1.Filter = "结果|*.txt";
                if(saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    var writer = new StreamWriter(saveFileDialog1.FileName);
                    writer.Write(richTextBox1.Text);
                    writer.Close();
                    toolStripStatusLabel1.Text = "状态：保存成功";
                    MessageBox.Show("保存成功");
                    tabControl1.SelectedIndex = 1;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
