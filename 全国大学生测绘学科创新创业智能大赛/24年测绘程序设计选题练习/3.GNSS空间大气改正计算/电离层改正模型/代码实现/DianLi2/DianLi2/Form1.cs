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

namespace DianLi2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        double SecondsOfDay;
        List<Session> Sessions = new List<Session>();
        Session sessionP = new Session("P1",-2225669.7744,4998936.1598,3265908.9678);

        private void toolOpen_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = "|*.txt";
                openFileDialog1.FileName = "原始数据";
                if(openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    var reader = new StreamReader(openFileDialog1.FileName);
                    SecondsOfDay = Algo.CalSeconds(reader.ReadLine());
                    while(!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if(line.Length > 0)
                        {
                            Session session = new Session();
                            var buf = Regex.Split(line, @"\s+");
                            session.name = buf[0];
                            session.X = Convert.ToDouble(buf[1]) * 1000;
                            session.Y = Convert.ToDouble(buf[2]) * 1000;
                            session.Z = Convert.ToDouble(buf[3]) * 1000;
                            Sessions.Add(session);
                        }
                    }
                    reader.Close();
                    dataGridView1.Rows.Clear();
                    for(int i = 0;i < Sessions.Count;i++)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[i].Cells[0].Value = Sessions[i].name;
                        dataGridView1.Rows[i].Cells[1].Value = Sessions[i].X;
                        dataGridView1.Rows[i].Cells[2].Value = Sessions[i].Y;
                        dataGridView1.Rows[i].Cells[3].Value = Sessions[i].Z;
                    }
                    tabControl1.SelectedIndex = 0;
                    toolStripStatusLabel1.Text = "状态：已导入数据";
                    MessageBox.Show("导入成功");
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
                    Algo.GetAandE(ref Sessions, sessionP);
                    Algo.CalFaim(ref Sessions);
                    Algo.CalIone(ref Sessions, SecondsOfDay);
                    string resultTXT = "卫星标识    地磁纬度    距离延迟量\n";
                    foreach(Session d in Sessions)
                    {
                        resultTXT += $"{d.name}    {d.faiM:F3}    {d.Dion:F4}\n";
                    }
                    richTextBox1.Text = resultTXT;
                    tabControl1.SelectedIndex = 1;
                    MessageBox.Show("计算成功");
                    toolStripStatusLabel1.Text = "状态：计算成功";
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void toolSave_Click(object sender, EventArgs e)
        {
            try
            {
                if(Sessions.Count > 0)
                {
                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                    saveFileDialog1.FileName = "计算结果";
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
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
