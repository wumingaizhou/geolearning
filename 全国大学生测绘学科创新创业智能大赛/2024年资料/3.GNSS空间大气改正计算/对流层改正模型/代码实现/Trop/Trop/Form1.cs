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

namespace Trop
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        List<Session> Sessions = new List<Session>();

        private void toolOpen_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.FileName = "原始数据";
                openFileDialog1.Filter = "|*.txt";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    var reader = new StreamReader(openFileDialog1.FileName);
                    Sessions.Clear();
                    reader.ReadLine();
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (line.Length > 0)
                        {
                            var tempSession = new Session(line);
                            Sessions.Add(tempSession);
                        }
                    }
                    reader.Close();
                    dataGridView1.Rows.Clear();
                    for (int i = 0; i < Sessions.Count; i++)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[i].Cells[0].Value = Sessions[i].name;
                        dataGridView1.Rows[i].Cells[1].Value = Sessions[i].days;
                        dataGridView1.Rows[i].Cells[2].Value = Sessions[i].B;
                        dataGridView1.Rows[i].Cells[3].Value = Sessions[i].L;
                        dataGridView1.Rows[i].Cells[4].Value = Sessions[i].H;
                        dataGridView1.Rows[i].Cells[5].Value = Sessions[i].E;
                    }
                    toolStripStatusLabel1.Text = "状态：已导入数据";
                    MessageBox.Show("导入成功");
                    tabControl1.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void toolCal_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (Sessions.Count > 0)
                {
                    Algo.GetMw(ref Sessions);
                    Algo.GetMd(ref Sessions);
                    Algo.GetS(ref Sessions);
                    string result = "测站名：    湿分量：    干分量：    总延迟：    \n";
                    foreach (Session d in Sessions)
                    {
                        result += $"{d.name:F3}   {d.Mw:F3}   {d.Md:F3}   {d.S:F3}\n";
                    }
                    richTextBox1.Text = result;
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

        private void toolSave_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (Sessions.Count > 0)
                {
                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                    saveFileDialog1.FileName = "结果输出";
                    saveFileDialog1.Filter = "|*.txt";
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
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
