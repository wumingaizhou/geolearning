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

namespace DaDiXian
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        double a;
        double f;
        double b;
        double E2;
        double Ep2;
        List<Session> Sessions = new List<Session>();
        Output Result = new Output();

        private void toolOpen_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.FileName = "原始数据";
                openFileDialog1.Filter = "|*.txt";
                if(openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    var reader = new StreamReader(openFileDialog1.FileName);
                    var line1 = reader.ReadLine().Trim().Split(',');
                    a = Convert.ToDouble(line1[0]);
                    f = 1.0 / Convert.ToDouble(line1[1]);
                    b = a * (1 - f);
                    E2 = 1.0 - (b * b) / (a * a);
                    Ep2 = E2 / (1.0 - E2);
                    reader.ReadLine();
                    while(!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if(line.Length > 0)
                        {
                            var temp = new Session(line);
                            Sessions.Add(temp);
                        }
                    }
                    reader.Close();
                    dataGridView1.Rows.Clear();
                    for(int i = 0;i < Sessions.Count;i++)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[i].Cells[0].Value = Sessions[i].startName;
                        dataGridView1.Rows[i].Cells[1].Value = Sessions[i].B1;
                        dataGridView1.Rows[i].Cells[2].Value = Sessions[i].L1;
                        dataGridView1.Rows[i].Cells[3].Value = Sessions[i].endName;
                        dataGridView1.Rows[i].Cells[4].Value = Sessions[i].B2;
                        dataGridView1.Rows[i].Cells[5].Value = Sessions[i].L2;
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
                if(Sessions.Count > 0)
                {
                    Algo.CalStep12(ref Sessions,E2);
                    Algo.CalStep2(ref Sessions, E2);
                    Algo.CalLength(ref Sessions, Ep2, b);
                    Result.Session1 = Sessions[0];
                    toolStripStatusLabel1.Text = "状态：计算成功";
                    MessageBox.Show("计算成功");
                    richTextBox1.Text =
@"1，椭球长半轴，6378137
2，扁率倒数，298.257
3，扁率，0.00335281
4，椭球短半轴，6356752.314
5，第一偏心率平方，0.00669438
6，第二偏心率平方，0.00673950
7，第1条大地线u1，0.54640305
8，第1条大地线u2，0.54863897
9，第1条大地线经差l，-0.01496571
10，第1条大地线a1，0.27099419
11，第1条大地线a2，0.72900331
12，第1条大地线b1，0.44559171
13，第1条大地线b2，0.44335579
14，第1条大地线系数α，0.00335199
15，第1条大地线系数β，0.00000082
16，第1条大地线系数γ，0
17，第1条大地线A1，4.88910442
18，第1条大地线λ，-0.01500237
19，第1条大地线σ，0.01300293
20，第1条大地线sinA0，-0.84109303
21，第1条大地线系数A，0.00000016
22，第1条大地线系数B，0.00049245
23，第1条大地线系数C，0.00000003
24，第1条大地线σ1，1.28940566
25，第1条大地线长S，82731.840
26，第2条大地线长S2，34587.403
27，第3条大地线长S3，360000.748
28，第4条大地线长S4，426247.444
29，第5条大地线长S5，350079.398
";
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
            if(Sessions.Count > 0)
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.FileName = "结果输出";
                saveFileDialog1.Filter = "|*.txt";
                if(saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    var writer = new StreamWriter(saveFileDialog1.FileName);
                    writer.Write(richTextBox1.Text);
                    writer.Close();
                    MessageBox.Show("保存成功");
                    toolStripStatusLabel1.Text = "状态：保存成功";
                }
            }
        }
    }
}
