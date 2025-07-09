using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Imaging;

namespace VoronoiDiagram
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        List<Point> points = new List<Point>();
        List<Triangle> delaunay_triangles = new List<Triangle>();
        List<Point> tubao_points = new List<Point>();
        Dictionary<Point, List<Point>> voronoiCells = new Dictionary<Point, List<Point>>();

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.FileName = "输入数据";
                openFileDialog1.Filter = "|*.txt";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    var reader = new StreamReader(openFileDialog1.FileName);
                    reader.ReadLine(); //第一行不要

                    points.Clear();
                    dataGridView1.Rows.Clear(); // 清空表格

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (line.Length > 0)
                        {
                            var Regx = Regex.Split(line, @"\s+"); // 使用正则处理多个空格
                            if (Regx.Length >= 2)
                            {
                                var tempX = Convert.ToDouble(Regx[0]);
                                var tempY = Convert.ToDouble(Regx[1]);
                                var tempPoint = new Point(tempX, tempY);
                                points.Add(tempPoint);
                            }
                        }
                    }
                    reader.Close();

                    foreach (var p in points)
                    {
                        dataGridView1.Rows.Add(p.X, p.Y);
                    }

                    MessageBox.Show("导入成功");
                    toolStripStatusLabel1.Text = "状态：导入成功";
                    tabControl1.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导入失败: {ex.Message}");
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (points.Count > 0)
                {
                    // 构建Delaunay三角剖分
                    Delaunay delaunay = new Delaunay(points);
                    delaunay_triangles = delaunay.GetTriangles();

                    // 构建凸包
                    Tubao tubao = new Tubao(points);
                    tubao_points = tubao.GetTuBaoPoints();

                    // 构建泰森多边形
                    var voronoi = new VoronoiDiagram();
                    var result = voronoi.BuildVoronoiDiagram(points, delaunay_triangles);

                    // 去除边界点
                    const double delta = 1e-5;
                    bool IsBoundaryPoint(Point p) =>
                        tubao_points.Any(tp => Math.Abs(tp.X - p.X) < delta && Math.Abs(tp.Y - p.Y) < delta);

                    var result1 = result
                        .Where(kvp => !IsBoundaryPoint(kvp.Key))
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value.Select(p => new Point(p.X, p.Y)).ToList()
                        );

                    voronoiCells = result1;
                
                    pictureBox1.Invalidate();  // 刷新绘图
                    

                    MessageBox.Show("计算成功");
                    toolStripStatusLabel1.Text = "状态：计算完成";
                    tabControl1.SelectedIndex = 1;

                    // 报告输出
                    GetReport();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"计算失败: {ex.Message}");
            }
        }

        //绘图
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {

            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            int height = pictureBox1.Height;
            // 将原点移动到左下角
            g.TranslateTransform(0, height);
            // 翻转 Y 轴：让 Y 轴朝上
            g.ScaleTransform(1, -1);

            Pen trianglePen = new Pen(Color.Blue, 1);
            Pen voronoiPen = new Pen(Color.Green, 2);
            Pen tubaoPen = new Pen(Color.Red, 2);
            Brush pointBrush = Brushes.Red;

            // 绘制泰森多边形，不包含边界点
            if(voronoiCells.Count > 0)
            {
                foreach(var cell in voronoiCells)
                {
                    var vertices = cell.Value;

                    var vertices_points = vertices.Select(v => new PointF((float)v.X, (float)v.Y)).ToArray();
                    for (int i = 0; i < vertices_points.Length; i++)
                    {
                        var start = vertices_points[i];
                        var end = vertices_points[(i + 1) % vertices_points.Length];
                        g.DrawLine(voronoiPen, start, end);
                    }
                }
            }

            // 绘制三角网
            if (delaunay_triangles != null && delaunay_triangles.Count > 0)
            {
                foreach (var triangle in delaunay_triangles)
                {
                    float ax = (float)triangle.A.X;
                    float ay = (float)triangle.A.Y;
                    float bx = (float)triangle.B.X;
                    float by = (float)triangle.B.Y;
                    float cx = (float)triangle.C.X;
                    float cy = (float)triangle.C.Y;

                    // 画三角形的三条边（用浅色）
                    Pen lightTrianglePen = new Pen(Color.LightBlue, 1);
                    g.DrawLine(lightTrianglePen, ax, ay, bx, by);
                    g.DrawLine(lightTrianglePen, bx, by, cx, cy);
                    g.DrawLine(lightTrianglePen, cx, cy, ax, ay);
                }
            }

            // 绘制凸包
            if (tubao_points != null && tubao_points.Count > 0)
            {
                var tubao_draw = tubao_points.Select(p => new PointF((float)p.X, (float)p.Y)).ToArray();
                
                for (int i = 0; i < tubao_draw.Length; i++)
                {
                    var start = tubao_draw[i];
                    var end = tubao_draw[(i + 1) % tubao_draw.Length];
                    g.DrawLine(tubaoPen, start, end);
                }
            }

            // 绘制原始点
            foreach (var point in points)
            {
                float x = (float)point.X;
                float y = (float)point.Y;
                g.FillEllipse(pointBrush, x - 3, y - 3, 6, 6);
            }
        }


        public void GetReport()
        {
            var result_txt = "";

            // 凸包面积
            var tubao_area = ComputePolygonArea(tubao_points);
            result_txt += $"凸包面积： {tubao_area} \n";
            // 泰森多边形面积
            foreach(var cell in voronoiCells)
            {
                var cell_points = cell.Value;

                var cell_points_area = ComputePolygonArea(cell_points);

                var temp_txt = $"点 X 为{cell.Key.X}，Y 为{cell.Key.Y}的生成点的泰森多边形面积是：{cell_points_area} \n";

                result_txt += temp_txt;
            }

            richTextBox1.Text = result_txt;
        }

        // 计算多边形面积，前提是点是按照顺序排列的
        public static double ComputePolygonArea(List<Point> polygon)
        {
            int n = polygon.Count;
            if (n < 3) return 0;

            double area = 0;

            for (int i = 0; i < n; i++)
            {
                Point current = polygon[i];
                Point next = polygon[(i + 1) % n];
                area += current.X * next.Y - next.X * current.Y;
            }

            return Math.Abs(area) / 2.0;
        }

        // 保存
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog savePaint = new SaveFileDialog
                {
                    Filter = "PNG Image|*.png",
                    Title = "保存图片",
                    FileName = "voronoi.png"
                };
                if (savePaint.ShowDialog() == DialogResult.OK)
                {
                    Bitmap bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                    pictureBox1.DrawToBitmap(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
                    bitmap.Save(savePaint.FileName, ImageFormat.Png);
                    bitmap.Save(savePaint.FileName, ImageFormat.Png);
                }

                SaveFileDialog saveReport = new SaveFileDialog
                {
                    Filter = "结果|*.txt",
                    Title = "报告结果",
                    FileName = "报告结果.txt"
                };
                if(saveReport.ShowDialog() == DialogResult.OK)
                {
                    var writer = new StreamWriter(saveReport.FileName);
                    writer.Write(richTextBox1.Text);
                    writer.Close();
                }

                MessageBox.Show("保存成功");
                toolStripStatusLabel1.Text = "状态：保存完成";
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}