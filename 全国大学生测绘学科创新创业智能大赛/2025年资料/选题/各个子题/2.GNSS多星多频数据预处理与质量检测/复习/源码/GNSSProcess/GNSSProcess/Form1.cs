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
        private List<Satellite> origin_satellites = new List<Satellite>();


        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog
                {
                    FileName = "输入数据",
                    Filter = "|*.txt"
                };
                if(openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    origin_satellites.Clear();

                    //var input = File.ReadAllText(openFileDialog1.FileName);
                    var input = File.ReadAllText(openFileDialog1.FileName);
                    string pattern = @"(?=^[A-Z0-9]{3} 卫星数据\s*$)";

                    string[] sections = Regex.Split(input, pattern, RegexOptions.Multiline);

                    for(int i = 1; i < sections.Length; i++)
                    {
                        List<Observation> temp_observations = new List<Observation>();

                        var section = Regex.Split(sections[i].Trim(), @"\n");

                        var satellite_name = Regex.Split(section[0].Trim(), @"\s+")[0];
                        
                        for(int j = 2;j < section.Length; j++)
                        {
                            var obs = new Observation(section[j]);
                            temp_observations.Add(obs);
                        }

                        var temp_satllite = new Satellite(temp_observations, satellite_name);

                        origin_satellites.Add(temp_satllite);
                    }

                    foreach(var satellite in origin_satellites)
                    {
                        foreach(var obs in satellite.satellite_observations)
                        {
                            dataGridView1.Rows.Add(satellite.satellite_name,obs.obs_name, obs.obs_L1, obs.obs_L2, obs.obs_P1, obs.obs_P2);
                        }
                    }
                    toolStripStatusLabel1.Text = "状态：导入成功";
                    tabControl1.SelectedIndex = 0;
                    MessageBox.Show("导入成功");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if(origin_satellites.Count > 0)
            {
                ZhouTiao zhouTiao = new ZhouTiao(origin_satellites);
                var text1 = zhouTiao.GetResult();
                richTextBox1.Text = text1;

                DuoLuJin duoLuJin = new DuoLuJin(origin_satellites);
                var text2 = duoLuJin.GetResult();
                richTextBox2.Text = text2;

                Hatch hatch = new Hatch(origin_satellites);
                var text3 = hatch.GetResult();
                richTextBox3.Text = text3;

                richTextBox4.Text = text1 + text2 + text3;

                toolStripStatusLabel1.Text = "状态：计算成功";
                tabControl1.SelectedIndex = 3;
                MessageBox.Show("计算成功");
            }
        }
    }
}
