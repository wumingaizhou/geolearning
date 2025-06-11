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

        List<Point> Points = new List<Point>(); // 用于存储控制点
        Algo algo = new Algo();

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.FileName = "请选择数据";
                openFileDialog.Filter = "文本文件|*.txt";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Points.Clear(); // 清除原有数据

                    var reader = new StreamReader(openFileDialog.FileName);
                    reader.ReadLine(); // 跳过第一行
                    reader.ReadLine(); // 跳过第二行
                    reader.ReadLine(); // 跳过第三行

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (line.Length > 0)
                        {
                            var pointtmp = new Point(line);
                            Points.Add(pointtmp);
                        }
                    }
                    reader.Close();

                    // 更新数据表格
                    dataGridView1.Rows.Clear();
                    for (int i = 0; i < Points.Count; i++)
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
            catch (Exception ex)
            {
                MessageBox.Show($"导入数据时发生错误: {ex.Message}");
                toolStripStatusLabel1.Text = "状态：导入失败";
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (Points.Count > 0)
            {
                try
                {
                    // 检查点数量
                    if (Points.Count < 3)
                    {
                        MessageBox.Show("需要至少3个点才能生成泰森多边形");
                        return;
                    }

                    // 执行Voronoi计算
                    algo.go(Points);

                    // 检查是否成功生成
                    if (algo.VoronoiCells.Count == 0)
                    {
                        MessageBox.Show("生成泰森多边形失败，请检查输入数据");
                        return;
                    }

                    // 绘制结果
                    DrawVoronoi();

                    // 更新状态
                    toolStripStatusLabel1.Text = "状态：计算完成";
                    tabControl1.SelectedIndex = 1;
                    MessageBox.Show("计算完成");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"计算过程中发生错误: {ex.Message}\n\n详细信息: {ex.StackTrace}");
                    toolStripStatusLabel1.Text = "状态：计算出错";
                }
            }
            else
            {
                MessageBox.Show("还未导入数据");
            }
        }

        /// <summary>
        /// 绘制Voronoi图到PictureBox控件
        /// </summary>
        private void DrawVoronoi()
        {
            if (algo.VoronoiCells.Count == 0) return;

            // 计算绘图区域大小
            double minX = Points.Min(p => p.X) - 50;
            double maxX = Points.Max(p => p.X) + 50;
            double minY = Points.Min(p => p.Y) - 50;
            double maxY = Points.Max(p => p.Y) + 50;

            int width = Math.Max(600, (int)(maxX - minX));
            int height = Math.Max(600, (int)(maxY - minY));

            // 创建位图
            Bitmap bitmap = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                // 设置抗锯齿
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // 设置坐标变换，使绘图区域居中
                g.TranslateTransform((float)(width / 2 - (maxX + minX) / 2),
                                   (float)(height / 2 - (maxY + minY) / 2));

                // 调用算法类的绘图方法
                algo.DrawVoronoiDiagram(g);
            }

            // 释放之前的图像并设置新图像
            pictureBox1.Image?.Dispose();
            pictureBox1.Image = bitmap;
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
            saveFileDialog.Filter = "PNG图像|*.png|JPEG图像|*.jpg|位图|*.bmp";
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

                    switch (extension)
                    {
                        case ".jpg":
                        case ".jpeg":
                            format = System.Drawing.Imaging.ImageFormat.Jpeg;
                            break;
                        case ".bmp":
                            format = System.Drawing.Imaging.ImageFormat.Bmp;
                            break;
                        default:
                            format = System.Drawing.Imaging.ImageFormat.Png;
                            break;
                    }

                    // 保存图像
                    pictureBox1.Image.Save(saveFileDialog.FileName, format);
                    toolStripStatusLabel1.Text = "状态：保存成功";
                    MessageBox.Show("图像保存成功！");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"保存图像时发生错误: {ex.Message}");
                    toolStripStatusLabel1.Text = "状态：保存失败";
                }
            }
        }

        // 添加一个手动添加点的功能（可选）
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // 将鼠标坐标转换为逻辑坐标
                Point newPoint = new Point(e.X, e.Y);
                Points.Add(newPoint);

                // 更新数据表格
                int rowIndex = dataGridView1.Rows.Add();
                dataGridView1.Rows[rowIndex].Cells[0].Value = Points.Count;
                dataGridView1.Rows[rowIndex].Cells[1].Value = newPoint.X;
                dataGridView1.Rows[rowIndex].Cells[2].Value = newPoint.Y;

                toolStripStatusLabel1.Text = $"状态：已添加点 ({newPoint.X}, {newPoint.Y})";
            }
        }

        // 清空所有点
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Points.Clear();
            dataGridView1.Rows.Clear();
            pictureBox1.Image?.Dispose();
            pictureBox1.Image = null;
            algo = new Algo(); // 重新初始化算法对象
            toolStripStatusLabel1.Text = "状态：已清空所有数据";
        }
    }
}