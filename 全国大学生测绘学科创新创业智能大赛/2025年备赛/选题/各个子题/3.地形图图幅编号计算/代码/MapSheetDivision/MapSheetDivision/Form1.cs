using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace MapSheetDivision
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //所有点数据
        List<Point> points = new List<Point>();

        //点击“添加”按钮后，dataGridView 展示信息，并且存储点位信息。
        private void button4_Click(object sender, EventArgs e)
        {
            // 需要选择“经纬度计算图幅编号”
            if (radioButton1.Checked)
            {
                var Ltext = textBox2.Text; //经度文本
                var Btext = textBox3.Text; //纬度文本
                if(IS_BL_right(Ltext) && IS_BL_right(Btext))
                {
                    var L = DDMMSS_to_Degrees(Ltext); //转为度
                    var B = DDMMSS_to_Degrees(Btext);
                    var scaleButton = GetSelectedRadioButton(groupBox3);
                    if(scaleButton == null)
                    {
                        //判断是否选择了比例尺
                        MessageBox.Show("未选择比例尺!");
                        return;
                    }
                    var scale = scaleButton.Text;

                    // 获取当前行数作为 ID
                    int newId = dataGridView1.Rows.Count;

                    // 添加用户输入的数据
                    dataGridView1.Rows.Add(newId, L, B, scale);

                    Point point = new Point(newId, L, B, scale);
                    points.Add(point);
                }
                else
                {
                    MessageBox.Show("经度或纬度输入错误！");
                }
            }
            else
            {
                MessageBox.Show("计算方式选择错误");
            }
        }
        //用于判断经度和纬度格式是否规范。
        private bool IS_BL_right(string text)
        {
            // 检查是否为空或null
            if (string.IsNullOrWhiteSpace(text))
                return false;

            // 移除空格并检查格式
            text = text.Trim();

            // 检查是否包含小数点
            if (!text.Contains("."))
                return false;

            // 分割整数部分和小数部分
            string[] parts = text.Split('.');
            if (parts.Length != 2)
                return false;

            string integerPart = parts[0]; // 度
            string decimalPart = parts[1]; // 分秒

            // 检查整数部分（度）是否为有效数字
            if (!int.TryParse(integerPart, out int degrees) || degrees < 0)
                return false;

            // 检查经度（0-180）或纬度（0-90）
            if (integerPart.Length > 3 || degrees > 180)
                return false;

            // 检查小数部分（MMSS格式，4位数字）
            if (decimalPart.Length != 4 || !int.TryParse(decimalPart, out int mmss))
                return false;

            // 验证分（0-59）和秒（0-59）
            int minutes = mmss / 100;
            int seconds = mmss % 100;
            if (minutes > 59 || seconds > 59)
                return false;

            return true;
        }
        //DDMMSS转度
        private double DDMMSS_to_Degrees(string text)
        {
            // 分割整数部分（度）和小数部分（分秒）
            string[] parts = text.Split('.');
            string integerPart = parts[0]; // 度
            string decimalPart = parts[1]; // 分秒

            // 提取度、分、秒
            int degrees = int.Parse(integerPart); // 度
            int mmss = int.Parse(decimalPart);   // 分秒（MMSS）
            int minutes = mmss / 100;            // 分
            int seconds = mmss % 100;            // 秒

            // 转换为十进制度数：度 + 分/60 + 秒/3600
            double result = degrees + (minutes / 60.0) + (seconds / 3600.0);

            return result;
        }
        // 用于获得用户选择的比例尺
        private RadioButton GetSelectedRadioButton(GroupBox groupBox)
        {
            // 遍历 GroupBox 中的所有控件
            foreach (Control control in groupBox.Controls)
            {
                // 检查控件是否为 RadioButton 且被选中
                if (control is RadioButton radioButton && radioButton.Checked)
                {
                    return radioButton;
                }
            }
            // 如果没有选中任何 RadioButton，返回 null
            return null;
        }

        // 点击导入按钮后，可以选择文本导入
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.FileName = "请选择数据";
                openFileDialog.Filter = "文本文档|*.txt";
                if(openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    points.Clear();
                    dataGridView1.Rows.Clear();

                    StreamReader streamReader = new StreamReader(openFileDialog.FileName);
                    streamReader.ReadLine(); //第一行不要
                    while(!streamReader.EndOfStream)
                    {
                        var line = streamReader.ReadLine();
                        if(line.Length > 0)
                        {
                            var tempRegex = Regex.Split(line, @"\,");

                            int id = dataGridView1.Rows.Count;
                            var B = DDMMSS_to_Degrees(tempRegex[1]);
                            var L = DDMMSS_to_Degrees(tempRegex[2]);
                            var scale = tempRegex[3];

                            var point = new Point(id, L, B, scale);
                            points.Add(point);

                            // 添加数据
                            dataGridView1.Rows.Add(id, L, B, scale);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        // 点击 “计算”按钮后，能够执行主算法。
        private void button1_Click(object sender, EventArgs e)
        {
            if(points.Count > 0)
            {
                Algo algo = new Algo(ref points);

                dataGridView1.Rows.Clear();

                for (int i = 0; i < points.Count; i++)
                {
                    dataGridView1.Rows.Add(i + 1, points[i].L, points[i].B, points[i].scale, points[i].oldMapCode, points[i].newMapCode);
                    var besideMapCode = points[i].OldBesideMapList;
                    textBox5.Text = besideMapCode[0];
                    textBox6.Text = besideMapCode[1];
                    textBox7.Text = besideMapCode[2];
                    textBox8.Text = besideMapCode[3];
                    textBox10.Text = besideMapCode[4];
                    textBox11.Text = besideMapCode[5];
                    textBox9.Text = besideMapCode[6];
                    textBox12.Text = besideMapCode[7];
                    textBox13.Text = besideMapCode[8];
                }
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            points.Clear();
            dataGridView1.Rows.Clear();
        }
    }
}
