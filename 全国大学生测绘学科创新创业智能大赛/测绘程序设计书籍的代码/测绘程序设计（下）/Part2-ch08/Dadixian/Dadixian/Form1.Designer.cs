namespace Dadixian
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolFile = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.toolSave = new System.Windows.Forms.ToolStripMenuItem();
            this.toolExit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolSelect = new System.Windows.Forms.ToolStripDropDownButton();
            this.ball1 = new System.Windows.Forms.ToolStripMenuItem();
            this.ball2 = new System.Windows.Forms.ToolStripMenuItem();
            this.ball3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolCal = new System.Windows.Forms.ToolStripButton();
            this.toolReport = new System.Windows.Forms.ToolStripButton();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.B1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.B2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolFile,
            this.toolSelect,
            this.toolCal,
            this.toolReport});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(627, 31);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolFile
            // 
            this.toolFile.BackColor = System.Drawing.SystemColors.ControlLight;
            this.toolFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolOpen,
            this.toolSave,
            this.toolExit});
            this.toolFile.Image = ((System.Drawing.Image)(resources.GetObject("toolFile.Image")));
            this.toolFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolFile.Name = "toolFile";
            this.toolFile.Size = new System.Drawing.Size(64, 28);
            this.toolFile.Text = "文件";
            // 
            // toolOpen
            // 
            this.toolOpen.Name = "toolOpen";
            this.toolOpen.Size = new System.Drawing.Size(252, 30);
            this.toolOpen.Text = "打开";
            this.toolOpen.Click += new System.EventHandler(this.toolOpen_Click);
            // 
            // toolSave
            // 
            this.toolSave.Name = "toolSave";
            this.toolSave.Size = new System.Drawing.Size(252, 30);
            this.toolSave.Text = "保存";
            this.toolSave.Click += new System.EventHandler(this.toolSave_Click);
            // 
            // toolExit
            // 
            this.toolExit.Name = "toolExit";
            this.toolExit.Size = new System.Drawing.Size(252, 30);
            this.toolExit.Text = "退出";
            this.toolExit.Click += new System.EventHandler(this.toolExit_Click);
            // 
            // toolSelect
            // 
            this.toolSelect.BackColor = System.Drawing.SystemColors.ControlLight;
            this.toolSelect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolSelect.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ball1,
            this.ball2,
            this.ball3});
            this.toolSelect.Image = ((System.Drawing.Image)(resources.GetObject("toolSelect.Image")));
            this.toolSelect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolSelect.Name = "toolSelect";
            this.toolSelect.Size = new System.Drawing.Size(64, 28);
            this.toolSelect.Text = "选项";
            this.toolSelect.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ball1
            // 
            this.ball1.Name = "ball1";
            this.ball1.Size = new System.Drawing.Size(252, 30);
            this.ball1.Text = "克拉索夫斯基椭球";
            this.ball1.Click += new System.EventHandler(this.ball1_Click);
            // 
            // ball2
            // 
            this.ball2.Name = "ball2";
            this.ball2.Size = new System.Drawing.Size(252, 30);
            this.ball2.Text = "IUGG1975椭球";
            this.ball2.Click += new System.EventHandler(this.ball2_Click);
            // 
            // ball3
            // 
            this.ball3.Name = "ball3";
            this.ball3.Size = new System.Drawing.Size(252, 30);
            this.ball3.Text = "CGCS2000椭球";
            this.ball3.Click += new System.EventHandler(this.ball3_Click);
            // 
            // toolCal
            // 
            this.toolCal.BackColor = System.Drawing.SystemColors.ControlLight;
            this.toolCal.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolCal.Image = ((System.Drawing.Image)(resources.GetObject("toolCal.Image")));
            this.toolCal.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolCal.Name = "toolCal";
            this.toolCal.Size = new System.Drawing.Size(50, 28);
            this.toolCal.Text = "计算";
            this.toolCal.Click += new System.EventHandler(this.toolCal_Click);
            // 
            // toolReport
            // 
            this.toolReport.BackColor = System.Drawing.SystemColors.ControlLight;
            this.toolReport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolReport.Image = ((System.Drawing.Image)(resources.GetObject("toolReport.Image")));
            this.toolReport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolReport.Name = "toolReport";
            this.toolReport.Size = new System.Drawing.Size(50, 28);
            this.toolReport.Text = "报告";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.B1,
            this.B2});
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 30;
            this.dataGridView1.Size = new System.Drawing.Size(602, 96);
            this.dataGridView1.TabIndex = 1;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "点";
            this.Column1.Name = "Column1";
            // 
            // B1
            // 
            this.B1.HeaderText = "B1";
            this.B1.Name = "B1";
            // 
            // B2
            // 
            this.B2.HeaderText = "B2";
            this.B2.Name = "B2";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 283);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(627, 29);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(64, 24);
            this.toolStripStatusLabel1.Text = "状态：";
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox1.Location = new System.Drawing.Point(0, 98);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(602, 39);
            this.textBox1.TabIndex = 3;
            this.textBox1.Text = "大地线长";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.bottom);
            this.textBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.bottom);
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox2.Location = new System.Drawing.Point(0, 143);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(602, 35);
            this.textBox2.TabIndex = 4;
            this.textBox2.Text = "（m）";
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.bottom);
            this.textBox2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.bottom);
            // 
            // tabControl1
            // 
            this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(0, 34);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(639, 243);
            this.tabControl1.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dataGridView1);
            this.tabPage1.Controls.Add(this.textBox1);
            this.tabPage1.Controls.Add(this.textBox2);
            this.tabPage1.Location = new System.Drawing.Point(4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(631, 211);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "数据";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.richTextBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(631, 211);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "结果";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(0, -4);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(635, 215);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Margin = new System.Windows.Forms.Padding(20, 3, 0, 2);
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(64, 24);
            this.toolStripStatusLabel2.Text = "椭球：";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(627, 312);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "Form1";
            this.Text = "大地线长度计算";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton toolFile;
        private System.Windows.Forms.ToolStripMenuItem toolOpen;
        private System.Windows.Forms.ToolStripMenuItem toolSave;
        private System.Windows.Forms.ToolStripDropDownButton toolSelect;
        private System.Windows.Forms.ToolStripButton toolReport;
        private System.Windows.Forms.ToolStripMenuItem toolExit;
        private System.Windows.Forms.ToolStripMenuItem ball1;
        private System.Windows.Forms.ToolStripMenuItem ball2;
        private System.Windows.Forms.ToolStripMenuItem ball3;
        private System.Windows.Forms.ToolStripButton toolCal;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn B1;
        private System.Windows.Forms.DataGridViewTextBoxColumn B2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
    }
}

