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

namespace IDW
{
    public partial class Form1 : Form
    {
        DataEntity Data = new DataEntity();
        public string res = "点名    X（m）       Y（m）          H（m）      参与插值的点列表\n";
        public Form1()
        {
            InitializeComponent();
        }
         
        private void toolOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Data = FileHelper.Read(openFileDialog.FileName);
                richTextBox.Text = Data.ToString();
            }
        }

        private void toolCal_Click(object sender, EventArgs e)
        {
            Algo go = new Algo(Data,5);
            //string res = "点名    X（m）       Y（m）          H（m）      参与插值的点列表\n";
            var Q1 = new Point("Q1", 4310, 3600);
            var Q2 = new Point("Q2", 4330, 3600);
            var Q3 = new Point("Q3", 4310, 3620);
            res += go.idw(Q1);
            res += go.idw(Q2);
            res += go.idw(Q3);
            richTextBox.Text = res;
        }

        private void toolSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            //输出压缩后节点信息到TXT文档中
            saveFileDialog1.Filter = "压缩计算结果|*.txt";
            saveFileDialog1.FileName = "结果输出";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var write = new StreamWriter(saveFileDialog1.FileName);
                write.Write(res);
                write.Close();

            }
        }

        private void toolHelp_Click(object sender, EventArgs e)
        {
            richTextBox.Text = "第二次练习";
        }
    }
}
