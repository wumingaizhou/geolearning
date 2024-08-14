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
        private List<Satellite> satellites= new List<Satellite>();
        private void toolOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "|*.txt";
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    var reader = new StreamReader(openFileDialog1.FileName);
                    reader.ReadLine();
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (line.Length > 0)
                        {
                            var satellite = FileHelper.Read(line);
                            satellites.Add(satellite);
                        }
                    }
                    reader.Close();
                    richTextBox1.Text = "读取成功！";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void toolCal_Click(object sender, EventArgs e)
        {
            string res = "点名：   高度角：   干延迟：   湿延迟：   总延迟改正计算：\n";
            foreach(Satellite d in satellites)
            {
                res += $"{d.name}      {d.E}        {d.mw:F3}        {d.md:F3}      {d.s:F3}\n";
            }
            richTextBox1.Text = res;
        }

        private void toolSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "计算结果|*.txt"; 
            if(saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamWriter writer = new StreamWriter(saveFileDialog1.FileName); 
                writer.Write(richTextBox1.Text);
                writer.Close();
            }
        }

        private void toolHelp_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "这是帮助";
        }
    }
}
