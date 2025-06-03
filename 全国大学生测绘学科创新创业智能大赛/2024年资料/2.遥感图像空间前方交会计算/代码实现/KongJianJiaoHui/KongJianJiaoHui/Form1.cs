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

namespace KongJianJiaoHui
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Session Session1 = new Session();
        Session Session2 = new Session();
        Output Result = new Output();

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
                    Session1.name = "1";
                    Session1.Xs = Convert.ToDouble(reader.ReadLine());
                    Session1.Ys = Convert.ToDouble(reader.ReadLine());
                    Session1.Zs = Convert.ToDouble(reader.ReadLine());
                    Session1.fai = Convert.ToDouble(reader.ReadLine());
                    Session1.omega = Convert.ToDouble(reader.ReadLine());
                    Session1.k = Convert.ToDouble(reader.ReadLine());
                    Session1.x = Convert.ToDouble(reader.ReadLine());
                    Session1.y = Convert.ToDouble(reader.ReadLine());
                    Session1.f = Convert.ToDouble(reader.ReadLine());

                    Session2.name = "2";
                    Session2.Xs = Convert.ToDouble(reader.ReadLine());
                    Session2.Ys = Convert.ToDouble(reader.ReadLine());
                    Session2.Zs = Convert.ToDouble(reader.ReadLine());
                    Session2.fai = Convert.ToDouble(reader.ReadLine());
                    Session2.omega = Convert.ToDouble(reader.ReadLine());
                    Session2.k = Convert.ToDouble(reader.ReadLine());
                    Session2.x = Convert.ToDouble(reader.ReadLine());
                    Session2.y = Convert.ToDouble(reader.ReadLine());
                    Session2.f = Convert.ToDouble(reader.ReadLine());
                    reader.Close();
                    dataGridView1.Rows.Clear();
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[0].Cells[0].Value = Session1.name;
                    dataGridView1.Rows[0].Cells[1].Value = Session1.Xs;
                    dataGridView1.Rows[0].Cells[2].Value = Session1.Ys;
                    dataGridView1.Rows[0].Cells[3].Value = Session1.Zs;
                    dataGridView1.Rows[0].Cells[4].Value = Session1.fai;
                    dataGridView1.Rows[0].Cells[5].Value = Session1.omega;
                    dataGridView1.Rows[0].Cells[6].Value = Session1.k;
                    dataGridView1.Rows[0].Cells[7].Value = Session1.y;
                    dataGridView1.Rows[0].Cells[8].Value = Session1.x;
                    dataGridView1.Rows[0].Cells[9].Value = Session1.f;


                    dataGridView1.Rows[1].Cells[0].Value = Session2.name;
                    dataGridView1.Rows[1].Cells[1].Value = Session2.Xs;
                    dataGridView1.Rows[1].Cells[2].Value = Session2.Ys;
                    dataGridView1.Rows[1].Cells[3].Value = Session2.Zs;
                    dataGridView1.Rows[1].Cells[4].Value = Session2.fai;
                    dataGridView1.Rows[1].Cells[5].Value = Session2.omega;
                    dataGridView1.Rows[1].Cells[6].Value = Session2.k;
                    dataGridView1.Rows[1].Cells[7].Value = Session2.y;
                    dataGridView1.Rows[1].Cells[8].Value = Session2.x;
                    dataGridView1.Rows[1].Cells[9].Value = Session2.f;
                    MessageBox.Show("导入成功");
                    toolStripStatusLabel1.Text = "状态：导入数据成功";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void toolCal_Click(object sender, EventArgs e)
        {
            if(Session1.name.Length > 0)
            {
                Algo.CalStep1(ref Session1,ref Session2);
                Algo.CalStep2and3(ref Session1, ref Session2, ref Result);
                string text = $"u    v    w\n{Session1.u}  {Session1.v}  {Session1.w}\n{Session2.u}  {Session2.v}  {Session2.w}\nN1:{Result.N1}  N2:{Result.N2}\n";
                text += $"X:{Result.X}  Y:{Result.Y}  Z:{Result.Z}";
                richTextBox1.Text = text;
                tabControl1.SelectedIndex = 1;
                MessageBox.Show("计算成功");
                toolStripStatusLabel1.Text = "状态：计算成功";
            }
            else
            {
                MessageBox.Show("未导入数据");
            }
        }

        private void toolSave_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {

                throw ex;
            }
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
