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

namespace MoLan
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        List<Point> Points = new List<Point>();
        Output Result = new Output();
        double[,] Matrix;

        private void toolOpen_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.FileName = "原始数据";
                openFileDialog1.Filter = "|*.txt";
                if(openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    Points.Clear();
                    var reader = new StreamReader(openFileDialog1.FileName);
                    while(!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if(line.Length > 0)
                        {
                            Point temp = new Point(line);
                            Points.Add(temp);
                        }
                    }
                    reader.Close();
                    dataGridView1.Rows.Clear();
                    for(int i = 0;i < Points.Count;i++)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[i].Cells[0].Value = Points[i].name;
                        dataGridView1.Rows[i].Cells[1].Value = Points[i].X;
                        dataGridView1.Rows[i].Cells[2].Value = Points[i].Y;
                        dataGridView1.Rows[i].Cells[3].Value = Points[i].interest;
                    }
                    toolStripStatusLabel1.Text = "状态：导入数据成功";
                    MessageBox.Show("导入成功");
                    tabControl1.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void toolCal_Click(object sender, EventArgs e)
        {
            try
            {
                if(Points.Count > 0)
                {
                    Algo.GetTuoYuan(ref Points, ref Result);
                    Matrix = Algo.GetMatrix(ref Points);
                    Algo.GetMolan(ref Points, ref Matrix,ref Result);
                    Algo.GetZ(ref Points,ref Result);
                    string resTxt = "长半轴：  短半轴：  cita：  全局莫兰指数：  \n";
                    resTxt += $"{Result.SDEx:F3}  {Result.SDEy:F3}  {Result.cita:F3}  {Result.GlobalMolan:F3}";
                    richTextBox1.Text = resTxt;
                    toolStripStatusLabel1.Text = "状态：计算成功";
                    MessageBox.Show("计算成功");
                    tabControl1.SelectedIndex = 1;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void toolSave_Click(object sender, EventArgs e)
        {
            try
            {
                if(Points.Count > 0)
                {
                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                    saveFileDialog1.FileName = "结果输出";
                    saveFileDialog1.Filter = "|*.txt";
                    if(saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        var writer = new StreamWriter(saveFileDialog1.FileName);
                        writer.Write(richTextBox1.Text);
                        writer.Close();
                        toolStripStatusLabel1.Text = "状态：保存成功";
                        MessageBox.Show("保存成功");
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
