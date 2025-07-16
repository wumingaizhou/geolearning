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
namespace GNSSProcess
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public List<Satellite> satellites  = new List<Satellite>();

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog
                {
                    Title = "输入数据",
                    FileName = "输入数据",
                    Filter = "文本|*.txt"
                };
                if(openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    satellites.Clear();

                    // 使用正则表达式查找每组卫星数据
                    var input = File.ReadAllText(openFileDialog1.FileName);

                    string pattern = @"(?=^[A-Z0-9]{3} 卫星数据\s*$)"; // 匹配每个卫星数据段的开始（例如 G01 卫星数据），？=零宽度匹配，只检查条件，不消耗字符。

                    string[] sections = Regex.Split(input, pattern, RegexOptions.Multiline);

                    foreach(var section in sections)
                    {
                        if (section == "") continue; //第一个 section为空
                        var lines = Regex.Split(section.Trim(), @"\r");
                        var satellite_name = Regex.Split(lines[0], @"\s+")[0];

                        List<Observation> satellite_obs = new List<Observation>();
                        for (int i = 2; i < lines.Length; i++)
                        {
                            var obs_temp = new Observation(lines[i].Trim());
                            satellite_obs.Add(obs_temp);
                        }

                        Satellite satellite = new Satellite(satellite_name, satellite_obs);
                        satellites.Add(satellite);
                    }



                    foreach (var s in satellites)
                    {
                        foreach(var p in s.satellite_observations)
                        {
                            dataGridView1.Rows.Add(s.satellite_name, p.obs_name, p.obs_p1, p.obs_p2, p.obs_fai1, p.obs_fai2);
                        }
                    }
                    toolStripStatusLabel1.Text = "状态：导入数据成功";
                    MessageBox.Show("导入成功");
                    tabControl1.SelectedIndex = 0;

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if(satellites.Count == 0)
                {
                    MessageBox.Show("还未导入数据");
                    return;
                }

                ZhouTiao zhouTiao = new ZhouTiao(satellites);
                richTextBox1.Text = zhouTiao.GetZhoutiaoResult();

                DuoLuJin duoLuJin = new DuoLuJin(satellites);
                richTextBox2.Text = duoLuJin.GetDuoLuJinResult();

                Hatch hatch = new Hatch(satellites);
                richTextBox3.Text = hatch.GetHatchResult();

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
