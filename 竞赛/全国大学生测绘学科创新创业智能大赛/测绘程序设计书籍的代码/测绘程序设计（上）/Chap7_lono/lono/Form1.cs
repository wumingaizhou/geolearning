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

namespace lono
{
    public partial class Form1 : Form
    {
        private Time time;
        private List<satellite> satellites;
        public Form1()
        {
            InitializeComponent();
        }

        private void toolOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "|*.txt";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var reader = new StreamReader(openFileDialog1.FileName);
                var lineTime = reader.ReadLine();//读取第一行的时间数据，因为所有卫星都要用这个时间数据；
                time = new Time(lineTime);
                satellites = new List<satellite>();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var satellite =  fileHelper.Read(line,time);
                    satellites.Add(satellite);
                }
                richTextBox1.Text = "读取成功";
                reader.Close();
            }
        }

        private void toolCal_Click(object sender, EventArgs e)
        {
            var go = new Algo(satellites);
            richTextBox1.Text = go.Run();
        }
        private void toolSave_Click(object sender, EventArgs e)
        {
            
        }
    }
}
