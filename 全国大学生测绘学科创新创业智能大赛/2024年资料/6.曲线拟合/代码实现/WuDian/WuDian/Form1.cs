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

namespace WuDian
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        List<Point> Points = new List<Point>();
        int selectFlag = 1;

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
                    var resPoints = Algo.AddPoints(Points, selectFlag);
                    Algo.GetTidu(ref Points, resPoints);
                    Algo.GetCanshu(ref Points);
                    string resTxt = "起点名：    终点名：    Sin：    Cos：    E0：    E1：   E2：   E3：   F0：   F1：   F2：   F3：   \n";
                    foreach(Point d in Points)
                    {
                        resTxt += $"{d.name}     {Convert.ToInt16(d.name) + 1}     {d.Sin:F3}     {d.Cos:F3}     {d.E0:F3}    {d.E1:F3}    {d.E2:F3}    {d.E3:F3}    {d.F0:F3}    {d.F1:F3}    {d.F2:F3}    {d.F3:F3}\n";
                        richTextBox1.Text = resTxt;
                    }
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
                    saveFileDialog1.FileName = "result";
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
        private void 闭合ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = "状态：闭合";
            selectFlag = 1;
        }

        private void 不闭合ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = "状态：不闭合";
            selectFlag = 0;
        }
    }
}
