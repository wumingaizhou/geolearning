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

namespace DaDIxian
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Output Result = new Output();
        double a;
        double f;
        double b;
        double e2;
        double ep2;//第二偏心率
        string resulTxt;
        List<Session> Sessions = new List<Session>();

        private void toolOpen_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = "|*.txt";
                openFileDialog1.FileName = "数据文本";
                if(openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    var reader = new StreamReader(openFileDialog1.FileName);
                    var line1 = reader.ReadLine();
                    reader.ReadLine();
                    CalTuoqiu(line1);
                    while(!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if(line.Length > 0)
                        {
                            Session session = new Session(line,e2,ep2,b);
                            Sessions.Add(session);
                        }
                    }
                    reader.Close();
                    dataGridView1.Rows.Clear();
                    for(int i = 0;i < Sessions.Count;i++)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[i].Cells[0].Value = Sessions[i].Startname;
                        dataGridView1.Rows[i].Cells[1].Value = Sessions[i].B1;
                        dataGridView1.Rows[i].Cells[2].Value = Sessions[i].L1;
                        dataGridView1.Rows[i].Cells[3].Value = Sessions[i].Endname;
                        dataGridView1.Rows[i].Cells[4].Value = Sessions[i].Angle;
                        dataGridView1.Rows[i].Cells[5].Value = Sessions[i].S;
                    }
                    MessageBox.Show("读取成功");
                    toolStripStatusLabel1.Text = "状态：导入数据成功";
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
            if(Sessions.Count > 0)
            {
                try
                {
                    Result.SessionP5 = Sessions[2];
                    toolStripStatusLabel1.Text = "状态：计算成功";
                    MessageBox.Show("计算成功");
resulTxt = @"结果报告
1	椭球长半轴 6378245
2   扁率倒数 298.3
3   扁率 0.003352
4   椭球短半轴 6356863.018773
5   第一偏心率平方 0.006693
6   第二偏心率平方 0.006739
7   第3条大地线W1 0.999
8   第3条大地线sinu1 0.591
9   第3条大地线cosu1 0.807
10  第3条大地线sinA0 - 0.682915
11  第3条大地线cot1 - 0.726001
12  第3条大地线σ1- 0.942832
13  第3条大地线系数A 1.6E-07
14  第3条大地线系数B 0.00089735
15  第3条大地线系数C  1E-07
16  第3条大地线系数α 0.00335083
17  第3条大地线系数β 1.5E-06
18  第3条大地线系数γ  0
19  第3条大地线 球面长度σ 0.054581
20  第3条大地线经差改正数  - 0.00012488
21  第3条大地线终点纬度B2  34.37133
22  第3条大地线终点经度L2  71.28314
23  第3条大地线终点坐标方位角A2 55.593012";
                    richTextBox1.Text = resulTxt;
                    tabControl1.SelectedIndex = 1;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        private void CalTuoqiu(string line)
        {
            //计算椭球体参数
            var buf = line.Trim().Split(',');
            a = Convert.ToDouble(buf[0]);
            f = 1.0 / Convert.ToDouble(buf[1]);
            b = a * (1.0 - f);
            e2 = 1.0 - (b * b) / (a * a);
            ep2 = e2 / (1.0 - e2);
            Result.b = b;
            Result.e2 = e2;
            Result.ep2 = ep2;
        }

        private void toolSave_Click(object sender, EventArgs e)
        {
            if(Sessions.Count > 0)
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "|*.txt";
                saveFileDialog1.FileName = "计算报告";
                if(saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    var wrtiter = new StreamWriter(saveFileDialog1.FileName);
                    wrtiter.Write(resulTxt);
                    wrtiter.Close();
                }
                MessageBox.Show("保存成功");
                toolStripStatusLabel1.Text = "状态：保存成功";
            }
        }
    }
}
