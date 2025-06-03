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

namespace CalArea
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private string Calres;
        private List<Point> pts;//所有点

        private void toolOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "|*.txt";
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pts = fileHelper.Read(openFileDialog1.FileName);
                string res = "读取结果：点名   x(m)   y(m)\n";
                foreach(Point d in pts)
                {
                    res += $"{d.PointName}  {d.X}  {d.Y}\n";
                }
                richTextBox1.Text = res;
            }
        }

        private void toolCal_Click(object sender, EventArgs e)
        {
            Algo go = new Algo();
            Calres = go.Run(pts);
            richTextBox1.Text = Calres;
        }

        private void toolSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "计算结果|*.txt";
            saveFileDialog1.FileName = "计算结果";
            if(saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamWriter writer = new StreamWriter(saveFileDialog1.FileName);
                writer.Write(Calres);
                writer.Close();
            }
        }

        private void toolHelp_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "第六次练习";
        }
    }
}
