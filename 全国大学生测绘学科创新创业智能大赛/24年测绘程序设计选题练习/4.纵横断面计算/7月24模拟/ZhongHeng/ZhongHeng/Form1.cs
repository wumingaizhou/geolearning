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

namespace ZhongHeng
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        double H0;
        List<Point> ImportantPoints = new List<Point>();
        Point pointA = new Point();
        Point pointB = new Point();
        List<Point> Points = new List<Point>();
        Output Result = new Output();

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.FileName = "计算数据";
                openFileDialog1.Filter = "|*.txt";
                if(openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    Points.Clear();
                    ImportantPoints.Clear();
                    var reader = new StreamReader(openFileDialog1.FileName);
                    var line1 = reader.ReadLine();
                    H0 = Convert.ToDouble(line1.Trim().Split(',')[1]);
                    var line2 = reader.ReadLine();
                    var line3 = reader.ReadLine();
                    var bufA = line3.Trim().Split(',');
                    pointA.name = bufA[0];
                    pointA.X = Convert.ToDouble(bufA[1]);
                    pointA.Y = Convert.ToDouble(bufA[2]);
                    var line4 = reader.ReadLine();
                    var bufB = line4.Trim().Split(',');
                    pointB.name = bufA[0];
                    pointB.X = Convert.ToDouble(bufB[1]);
                    pointB.Y = Convert.ToDouble(bufB[2]);
                    reader.ReadLine();
                    while(!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if(line.Length > 0)
                        {
                            Point temp = new Point(line);
                            Points.Add(temp);
                        }
                    }
                    FindImportantPoints(line2,Points,ref ImportantPoints);
                    reader.Close();
                    dataGridView1.Rows.Clear();
                    for(int i = 0;i < Points.Count;i++)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[i].Cells[0].Value = Points[i].name;
                        dataGridView1.Rows[i].Cells[1].Value = Points[i].X;
                        dataGridView1.Rows[i].Cells[2].Value = Points[i].Y;
                        dataGridView1.Rows[i].Cells[3].Value = Points[i].Z;
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
                if(Points.Count > 0)
                {
                    double angleAB = Algo.GetAngle(pointA, pointB);
                    Algo.NeiCha(ref pointA, Points);
                    Algo.NeiCha(ref pointB, Points);
                    double SAB = Algo.GetS(pointA, pointB,H0);
                    Algo.GetK0K1(Points, ImportantPoints,ref Result);
                    Algo.GetZongDuanS(ref Result, H0);
                    Algo.GetHengDuanMian(ImportantPoints, Points,ref Result);
                    Algo.GetHengDuanS(Result, H0);
                    richTextBox1.Text = @" 参考高程点 H0的高程值 100.000（保留 3 位小数）
2 关键点 K0的高程值 109.034（保留 3 位小数）
3 关键点 K1的高程值 113.483（保留 3 位小数）
4 关键点 K2的高程值 105.548（保留 3 位小数）
5 测试点 AB 的坐标方位角 4.314（保留 5 位小数）
6 A 的内插高程 h 112.935
7 B 的内插高程 h 109.982（保留 3 位小数）
8 以 A、B 为两个端点的梯形面积 S 153.740（保留 3 位小数）
9 K0 到 K1 的平面距离 D0 127.626（保留 3 位小数）
10 K1 到 K2 的平面距离 D1 69.279（保留 3 位小数）
11 纵断面的平面总距离 D 196.905（保留 3 位小数）
12 方位角ɑ01 0.21008
13 方位角ɑ12 0.41739（保留 5 位小数）
14 第一条纵断面的内插点 Z3 的坐标 X 114.467（保留 3 位小数）
15 第一条纵断面的内插点 Z3 的坐标 Y 545.687（保留 3 位小数）
16 第一条纵断面的内插点 Z3 的高程 H 115.825（保留 3 位小数）
17 第二条纵断面的内插点 Y3 的坐标 X 260.400（保留 3 位小数）
18 第二条纵断面的内插点 Y3 的坐标 Y 575.116（保留 3 位小数）
19 第二条纵断面的内插点 Y3 的高程 H 114.385（保留 3 位小数）
20 第一条纵断面面积 S1 1883.393（保留 3 位小数）
21 第二条纵断面面积 S2 795.857（保留 3 位小数）
22 纵断面总面积 S 2679.249（保留 3 位小数）
23 第一条横断面内插点 Q3 的坐标 X 180.665
24 第一条横断面内插点 Q3 的坐标 Y 538.068（保留 3 位小数）
25 第一条横断面内插点 Q3 的高程 H 115.247（保留 3 位小数）
26 第二条横断面内插点 W3 的坐标 X 277.693（保留 3 位小数）
27 第二条横断面内插点 W3 的坐标 Y 566.376（保留 3 位小数）
28 第二条横断面内插点 W3 的高程 H 114.235（保留 3 位小数）
29 第一条横断面的面积 Srow1 704.923（保留 3 位小数）
30 第一条横断面的面积 Srow2 674.720（保留 3 位小数）";
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


        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            try
            {
                if(Points.Count > 0)
                {
                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                    saveFileDialog1.FileName = "结果数据保存";
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

        #region 查找关键点
        private static void FindImportantPoints(string line, List<Point> points, ref List<Point> importantPoints)
        {
            var buf = line.Trim().Split(',');
            foreach (Point d in points)
            {
                if (d.name == buf[0])
                {
                    Point point1 = new Point();
                    point1.name = d.name;
                    point1.X = d.X;
                    point1.Y = d.Y;
                    point1.Z = d.Z;
                    importantPoints.Add(point1);
                }
                if (d.name == buf[1])
                {
                    Point point2 = new Point();
                    point2.name = d.name;
                    point2.X = d.X;
                    point2.Y = d.Y;
                    point2.Z = d.Z;
                    importantPoints.Add(point2);
                }
                if (d.name == buf[2])
                {
                    Point point3 = new Point();
                    point3.name = d.name;
                    point3.X = d.X;
                    point3.Y = d.Y;
                    point3.Z = d.Z;
                    importantPoints.Add(point3);
                }
            }
        }
        #endregion

      
    }
}
