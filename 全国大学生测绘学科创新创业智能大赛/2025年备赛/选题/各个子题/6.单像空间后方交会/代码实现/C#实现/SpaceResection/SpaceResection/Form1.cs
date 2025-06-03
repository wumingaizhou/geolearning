using System;
using System.Collections.Generic; // list 需要
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO; // StreamReader需要
using System.Text.RegularExpressions; // Regex需要


namespace SpaceResection
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        double m = 0; // 影像比例尺

        double x0, y0, f; // 影像的内方位元素

        List<Point> points = new List<Point>(); // 用于存储所有点

        

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            // 1.打开数据，存储数据
            OpenFileDialog openFileDialog = new OpenFileDialog(); // 用于打开数据
            openFileDialog.FileName = "请选择数据"; // 提示词
            openFileDialog.Filter = "文本文件|*.txt|所有文件|*.*"; // 文件筛选器

            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // 如果点击了确定
                try
                {
                    points.Clear(); // 清除 points

                    var reader = new StreamReader(openFileDialog.FileName); // 读取器
                    var line1 = Regex.Split(reader.ReadLine(),@"\,+"); // 截取字符串
                    m = Convert.ToDouble(line1[1]); // 第一行有比例尺信息
                    var line2 = Regex.Split(reader.ReadLine(), @"\,+"); // 第二行有内方位元素信息
                    x0 = Convert.ToDouble(line2[4]);
                    y0 = Convert.ToDouble(line2[5]);
                    f = Convert.ToDouble(line2[6]) / 1000.0;//米

                    reader.ReadLine(); // 第三行没有用
                    while (!reader.EndOfStream)
                    {
                        //一直读下去，每次读一行
                        var line = reader.ReadLine(); // 读一行
                        if(line.Length > 0)
                        {
                            Point point = new Point(line);
                            points.Add(point);
                        }
                    }
                    reader.Close();

                    dataGridView1.Rows.Clear(); // 清除现有数据
                    for(int i = 0;i < points.Count;i++)
                    {
                        dataGridView1.Rows.Add(); // 先添加行，才能添加数据
                        dataGridView1.Rows[i].Cells[0].Value = points[i].id;
                        dataGridView1.Rows[i].Cells[1].Value = points[i].x;
                        dataGridView1.Rows[i].Cells[2].Value = points[i].y;
                        dataGridView1.Rows[i].Cells[3].Value = points[i].X;
                        dataGridView1.Rows[i].Cells[4].Value = points[i].Y;
                        dataGridView1.Rows[i].Cells[5].Value = points[i].Z;
                    }
                    toolStripStatusLabel1.Text = "状态：导入数据成功"; // 更新底部的文本信息。
                    MessageBox.Show("导入成功");
                    tabControl1.SelectedIndex = 0; // 把 tabControl 转到 page1，也就是 dataGridView 所在的地方。
                }
                catch (Exception)
                {

                    MessageBox.Show("数据格式错误!");
                }
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (points.Count > 0)
            {
                try
                {
                    Algo go = new Algo();
                    go.GetOrigin(points, m, f,x0,y0); //计算初始值
                    var result = go.Go(); // 循环迭代，并且保存结果

                    richTextBox2.Text = $@"计算成功,结果如下：
Xs0:{result.XS0_new}，
Ys0:{result.YS0_new},
Zs0:{result.ZS0_new},
旋转矩阵：
a1:{result.a1_new},
a2:{result.a2_new},
a3:{result.a3_new},
b1:{result.b1_new},
b2:{result.b2_new},
b3:{result.b3_new},
c1:{result.c1_new},
c2:{result.c2_new},
c3:{result.c3_new},




";

                    tabControl1.SelectedIndex = 1;// 把 tabControl 转到 page2，也就是 richTextBox 所在的地方。
                    toolStripStatusLabel1.Text = "状态：计算成功"; // 更新底部的文本信息。
                    MessageBox.Show("计算成功");
                    

                }
                catch (Exception)
                {

                    throw;
                }
            }
        }


        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            try
            {
                if (points.Count > 0)
                {
                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                    saveFileDialog1.FileName = "结果输出";
                    saveFileDialog1.Filter = "|*.txt";
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        var writer = new StreamWriter(saveFileDialog1.FileName);
                        writer.Write(richTextBox2.Text);
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
