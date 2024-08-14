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

namespace ZhongHeng2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        double H0;
        List<Point> ImportantPoint = new List<Point>();
        List<Point> Points = new List<Point>();
        Point pointA = new Point();
        Point pointB = new Point();
        Output Result = new Output();

        private void toolOpen_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Title = "计算数据";
                openFileDialog1.Filter = "|*.txt";
                if(openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    var reader = new StreamReader(openFileDialog1.FileName);
                    H0 = Convert.ToDouble(reader.ReadLine().Trim().Split(',')[1]);
                    string ImportantPointstring = reader.ReadLine();
                    
                    var line3 = reader.ReadLine().Trim().Split(',');
                    pointA.name = line3[0];
                    pointA.X = Convert.ToDouble(line3[1]);
                    pointA.Y = Convert.ToDouble(line3[2]);
                    
                    var line4 = reader.ReadLine().Trim().Split(',');
                    pointB.name = line4[0];
                    pointB.X = Convert.ToDouble(line4[1]);
                    pointB.Y = Convert.ToDouble(line4[2]);
                    reader.ReadLine();
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if(line.Length > 0)
                        {
                            Point point = new Point(line);
                            Points.Add(point);
                        }
                    }
                    ImportantPoint = FindImportantPoint(ImportantPointstring, Points);
                    reader.Close();

                    dataGridView1.Rows.Clear();
                    for (int i = 0; i < Points.Count; i++)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[i].Cells[0].Value = Points[i].name;
                        dataGridView1.Rows[i].Cells[1].Value = Points[i].X;
                        dataGridView1.Rows[i].Cells[2].Value = Points[i].Y;
                        dataGridView1.Rows[i].Cells[3].Value = Points[i].Z;
                    }
                    toolStripStatusLabel1.Text = "状态：读入数据成功";
                    MessageBox.Show("读取数据成功");
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
                    Result.AngleAB = Algo.Calddmmssss(Algo.CalAngle(pointA, pointB));
                    double Ha = Algo.NeiCha(pointA, Points);
                    Result.Ha = Ha;
                    pointA.Z = Ha;
                    double Hb = Algo.NeiCha(pointB, Points); ;
                    Result.Hb = Hb;
                    pointB.Z = Hb;
                    Result.Sab = Algo.CalS(pointA, pointB, H0);
                    Result.distanceK0K1K2 = (Algo.CalDistance(ImportantPoint[0], ImportantPoint[1]) + Algo.CalDistance(ImportantPoint[1], ImportantPoint[2]));
                    Result.Stotal = Algo.NeiChaZhongDuanMian(ImportantPoint, Points,H0);
                    Algo.CalStep3(ImportantPoint,Points,H0);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private List<Point> FindImportantPoint(string line, List<Point> points)
        {
            ///<summary>
            ///查找关键点
            /// </summary>
            List<Point> ImportantPoint = new List<Point>();
            var buf = line.Trim().Split(',');
            foreach (Point point in points)
            {
                if (point.name == buf[0])
                {
                    Point pointK0 = new Point();
                    pointK0.name = point.name;
                    pointK0.X = point.X;
                    pointK0.Y = point.Y;
                    pointK0.Z = point.Z;
                    ImportantPoint.Add(pointK0);
                }
                if (point.name == buf[1])
                {
                    Point pointK1 = new Point();
                    pointK1.name = point.name;
                    pointK1.X = point.X;
                    pointK1.Y = point.Y;
                    pointK1.Z = point.Z;
                    ImportantPoint.Add(pointK1);
                }
                if (point.name == buf[2])
                {
                    Point pointK2 = new Point();
                    pointK2.name = point.name;
                    pointK2.X = point.X;
                    pointK2.Y = point.Y;
                    pointK2.Z = point.Z;
                    ImportantPoint.Add(pointK2);
                }
            }
            return ImportantPoint;
        }
    }
}
