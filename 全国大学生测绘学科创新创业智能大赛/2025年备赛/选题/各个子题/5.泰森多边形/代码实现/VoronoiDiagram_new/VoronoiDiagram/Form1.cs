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
using System.Text.RegularExpressions;

namespace VoronoiDiagram
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //最方便的方法，比赛里谁管你按txt读取，直接按结果写过程，原点在左上角
        Point pointLeftBottom = new Point(0, 400);
        Point pointLeftTop = new Point(0, 0);
        Point pointRightBottom = new Point(400, 400);
        Point pointRightTop = new Point(400, 0);

        List<Point> Points = new List<Point>(); // 用于存储控制点

        Algo algo = new Algo();

  
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.FileName = "请选择数据";
                openFileDialog.Filter = "文本文件|*.txt";
                if(openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Points.Clear(); // 先去除原本的数据

                    var reader = new StreamReader(openFileDialog.FileName);
                    reader.ReadLine();
                    reader.ReadLine(); //第二行直接在上面就写了
                    reader.ReadLine();
                    while(!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if(line.Length > 0)
                        {
                            var pointtmp = new Point(line);
                            Points.Add(pointtmp);
                        }         
                    }
                    reader.Close();

                    dataGridView1.Rows.Clear();
                    for(int i = 0;i < Points.Count;i++)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[i].Cells[0].Value = i + 1;
                        dataGridView1.Rows[i].Cells[1].Value = Points[i].X;
                        dataGridView1.Rows[i].Cells[2].Value = Points[i].Y;
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
            if(Points.Count > 0)
            {
                try
                {
                    // 执行 Voronoi 计算，传入点集和边界信息
                    // 假设 algo.go 方法内部会调用 SetBoundingRectangle 来确定边界
                    algo.go(Points, 0, 0, (int)pointRightBottom.X, (int)pointRightBottom.Y);

                    // 调用绘图方法，将 Voronoi 图绘制到 PictureBox 的 Image 属性上
                    DrawVoronoi();

                    // 更新状态栏文本
                    toolStripStatusLabel1.Text = "状态：计算完成";
                    // 切换到显示图像的 Tab 页
                    tabControl1.SelectedIndex = 1;
                    // 显示计算完成的消息框
                    MessageBox.Show("计算完成");

                }
                catch (Exception ex) // 捕获异常
                {
                    // 最好记录详细错误日志
                    MessageBox.Show($"计算或绘图过程中发生错误: {ex.Message}");
                    toolStripStatusLabel1.Text = "状态：计算出错";
                    // throw; // 避免直接抛出，除非上层需要处理
                }
            }
            else
            {
                // 提示用户需要先导入数据
                MessageBox.Show("还未导入数据");
            }
        }

       

        /// <summary>
        /// 绘制Voronoi图（泰森多边形）到PictureBox控件
        /// </summary>
        private void DrawVoronoi()
        {
            // 如果算法对象为空或者还未计算Voronoi图，则直接返回
            if (algo.VoronoiCells.Count == 0) return;

            // 创建位图对象
            var bitMapWidth = (int)(pointRightBottom.X + 100);
            var bitMapHeight = (int)(pointRightBottom.Y + 100);
            Bitmap bitmap = new Bitmap(bitMapWidth, bitMapHeight);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                // 设置抗锯齿，使绘制的线条更平滑，可以不考虑。从简
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // 调用算法类的绘图方法，传入Graphics对象
                algo.DrawVoronoiDiagram(g);
            }

            // 将绘制好的位图显示到PictureBox控件
            pictureBox1.Image?.Dispose();  // 释放之前的图像资源，避免内存泄漏
            pictureBox1.Image = bitmap;  // 设置新的图像
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            // 检查是否有图像可以保存
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("没有可保存的图像，请先生成泰森多边形。");
                return;
            }

            // 创建保存文件对话框
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PNG图像|*.png";
            saveFileDialog.Title = "保存泰森多边形图像";
            saveFileDialog.FileName = "泰森多边形";

            // 显示保存对话框
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // 根据选择的文件格式保存图像
                    string extension = Path.GetExtension(saveFileDialog.FileName).ToLower();
                    System.Drawing.Imaging.ImageFormat format;
                    format = System.Drawing.Imaging.ImageFormat.Png;
                    // 保存图像
                    pictureBox1.Image.Save(saveFileDialog.FileName, format);
                    toolStripStatusLabel1.Text = "状态：保存成功";
                    MessageBox.Show("图像保存成功！");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
