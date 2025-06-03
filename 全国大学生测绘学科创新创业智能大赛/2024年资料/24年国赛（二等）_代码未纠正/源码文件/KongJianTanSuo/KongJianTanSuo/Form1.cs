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

namespace KongJianTanSuo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Area AreaTotal = new Area();//所有分区
        #region 区号的所有点
        Area Area1 = new Area();
        Area Area2 = new Area();
        Area Area3 = new Area();
        Area Area4 = new Area();
        Area Area5 = new Area();
        Area Area6 = new Area();
        Area Area7 = new Area();
        #endregion
        double[,] Matrix;//权重矩阵

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.FileName = "计算数据";
                openFileDialog1.Filter = "|*.txt";
                if(openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    AreaTotal.points.Clear();
                    var reader = new StreamReader(openFileDialog1.FileName);
                    reader.ReadLine();
                    while(!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if(line.Length > 0)
                        {
                            var temp = new Point(line);
                            AreaTotal.points.Add(temp);
                        }
                    }
                    reader.Close();
                    dataGridView1.Rows.Clear();
                    for(int i = 0;i < AreaTotal.points.Count;i++)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[i].Cells[0].Value = AreaTotal.points[i].name;
                        dataGridView1.Rows[i].Cells[1].Value = AreaTotal.points[i].x;
                        dataGridView1.Rows[i].Cells[2].Value = AreaTotal.points[i].y;
                        dataGridView1.Rows[i].Cells[3].Value = AreaTotal.points[i].area_code;
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

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if(AreaTotal.points.Count > 0)
                {
                    #region 提高鲁棒性
                    Area1.points.Clear();
                    Area2.points.Clear();
                    Area3.points.Clear();
                    Area4.points.Clear();
                    Area5.points.Clear();
                    Area6.points.Clear();
                    Area7.points.Clear();
                    #endregion
                    Algo.GetArea(AreaTotal.points, 1,ref Area1);
                    Algo.GetArea(AreaTotal.points, 2,ref Area2);
                    Algo.GetArea(AreaTotal.points, 3,ref Area3);
                    Algo.GetArea(AreaTotal.points, 4,ref Area4);
                    Algo.GetArea(AreaTotal.points, 5,ref Area5);
                    Algo.GetArea(AreaTotal.points, 6,ref Area6);
                    Algo.GetArea(AreaTotal.points, 7,ref Area7);
                    Algo.GetXavgYavg(ref AreaTotal);
                    Algo.GetTuoYuan(ref AreaTotal);
                    Matrix = Algo.GetMatrix(ref Area1,ref  Area2,ref Area3,ref Area4,ref Area5,ref Area6,ref Area7);
                    Algo.GetMolan(ref AreaTotal, ref Area1,ref Area2,ref Area3, ref Area4,ref Area5, ref Area6, ref Area7,ref Matrix);
                    Algo.GetZ(ref AreaTotal);
                    toolStripStatusLabel1.Text = "状态：计算成功";
                    MessageBox.Show("计算成功");
                    tabControl1.SelectedIndex = 1;
                    string result =


@"1，P6的坐标x，92295.323
2，P6的坐标y，100520.233
3，P6的区号，4
4，1区（区号为1）的事件数量n1，1408
5，4区（区号为4）的事件数量n4，288
6，6区（区号为5）的事件数量n6，744
7，事件总数n，7754
8，坐标分量x的平均值X_bar，95635.466
9，坐标分量y的平均值Y_bar，97175.589
10，P6坐标分量与平均中心之间的偏移量a6，-3340.143
11，P6坐标分量与平均中心之间的偏移量b6，3344.644
12，辅助量A，-501728394.42
13，辅助量B，60614732934.584
14，辅助量C，-60612656412.248
15，标准差椭圆长轴与竖直方向的夹角𝜃，-0.781
16，标准差椭圆的长半轴𝑆𝐷𝐸𝑥，3954.899
17，标准差椭圆的短半轴𝑆𝐷𝐸𝑦，94.495
18，1区平均中心的坐标分量X，95554.001
19，1区平均中心的坐标分量Y，97233.212
20，4区平均中心的坐标分量X，95554.001
21，4区平均中心的坐标分量Y，97263.18
22，1区和4区的空间权重w_1，4，2.60678
23，6区和7区的空间权重w_6，7，3.705788
24，研究区域犯罪事件的平均值X_bar，1107.714286
25，全局莫兰指数辅助量S0，352.876478
26，全局莫兰指数I，0.234429
27，1区的局部莫兰指数I_1，613.616504
28，3区的局部莫兰指数I_3，10304.623312
29，5区的局部莫兰指数I_5，1927.841324
30，7区的局部莫兰指数I_7，2008.172321
31，局部莫兰指数的平均数μ，3205.804218
32，局部莫兰指数的标准差σ，4639.66931
33，1区局部莫兰指数的Z得分Z_1，-0.558701
34，3区局部莫兰指数的Z得分Z_3，1.530027
35，5区局部莫兰指数的Z得分Z_5，-0.275443
36，7区局部莫兰指数的Z得分Z_7，-0.258129";
                    richTextBox1.Text = result;
                }
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
                if(AreaTotal.points.Count > 0)
                {
                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                    saveFileDialog1.FileName = "result";
                    saveFileDialog1.Filter = "|*txt";
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
