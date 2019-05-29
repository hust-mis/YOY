namespace YOY.ModuleReader
{
    partial class ModuleReaderManager
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
            this.components = new System.ComponentModel.Container();
            this.BtnEnterConnect = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.BtnEnterDisconnect = new System.Windows.Forms.Button();
            this.LblEnter = new System.Windows.Forms.Label();
            this.TimerUpdate = new System.Windows.Forms.Timer(this.components);
            this.BtnExitDisconnect = new System.Windows.Forms.Button();
            this.LblExit = new System.Windows.Forms.Label();
            this.BtnExitConnect = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.BtnProjectDisconnect = new System.Windows.Forms.Button();
            this.LblProject = new System.Windows.Forms.Label();
            this.BtnProjectConnect = new System.Windows.Forms.Button();
            this.TimeProject = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnEnterConnect
            // 
            this.BtnEnterConnect.Location = new System.Drawing.Point(34, 78);
            this.BtnEnterConnect.Name = "BtnEnterConnect";
            this.BtnEnterConnect.Size = new System.Drawing.Size(108, 44);
            this.BtnEnterConnect.TabIndex = 0;
            this.BtnEnterConnect.Text = "连接并开始";
            this.BtnEnterConnect.UseVisualStyleBackColor = true;
            this.BtnEnterConnect.Click += new System.EventHandler(this.BtnEnterConnect_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.BtnEnterDisconnect);
            this.panel1.Controls.Add(this.LblEnter);
            this.panel1.Controls.Add(this.BtnEnterConnect);
            this.panel1.Location = new System.Drawing.Point(22, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(314, 152);
            this.panel1.TabIndex = 2;
            // 
            // BtnEnterDisconnect
            // 
            this.BtnEnterDisconnect.Enabled = false;
            this.BtnEnterDisconnect.Location = new System.Drawing.Point(176, 78);
            this.BtnEnterDisconnect.Name = "BtnEnterDisconnect";
            this.BtnEnterDisconnect.Size = new System.Drawing.Size(108, 44);
            this.BtnEnterDisconnect.TabIndex = 2;
            this.BtnEnterDisconnect.Text = "断开";
            this.BtnEnterDisconnect.UseVisualStyleBackColor = true;
            this.BtnEnterDisconnect.Click += new System.EventHandler(this.BtnEnterDisconnect_Click);
            // 
            // LblEnter
            // 
            this.LblEnter.AutoSize = true;
            this.LblEnter.Font = new System.Drawing.Font("宋体", 15F);
            this.LblEnter.Location = new System.Drawing.Point(29, 18);
            this.LblEnter.Name = "LblEnter";
            this.LblEnter.Size = new System.Drawing.Size(87, 25);
            this.LblEnter.TabIndex = 1;
            this.LblEnter.Text = "入口处";
            // 
            // TimerUpdate
            // 
            this.TimerUpdate.Enabled = true;
            this.TimerUpdate.Interval = 5000;
            this.TimerUpdate.Tick += new System.EventHandler(this.TimerUpdate_Tick);
            // 
            // BtnExitDisconnect
            // 
            this.BtnExitDisconnect.Enabled = false;
            this.BtnExitDisconnect.Location = new System.Drawing.Point(176, 78);
            this.BtnExitDisconnect.Name = "BtnExitDisconnect";
            this.BtnExitDisconnect.Size = new System.Drawing.Size(108, 44);
            this.BtnExitDisconnect.TabIndex = 2;
            this.BtnExitDisconnect.Text = "断开";
            this.BtnExitDisconnect.UseVisualStyleBackColor = true;
            this.BtnExitDisconnect.Click += new System.EventHandler(this.BtnExitDisconnect_Click);
            // 
            // LblExit
            // 
            this.LblExit.AutoSize = true;
            this.LblExit.Font = new System.Drawing.Font("宋体", 15F);
            this.LblExit.Location = new System.Drawing.Point(29, 18);
            this.LblExit.Name = "LblExit";
            this.LblExit.Size = new System.Drawing.Size(87, 25);
            this.LblExit.TabIndex = 1;
            this.LblExit.Text = "出口处";
            // 
            // BtnExitConnect
            // 
            this.BtnExitConnect.Location = new System.Drawing.Point(34, 78);
            this.BtnExitConnect.Name = "BtnExitConnect";
            this.BtnExitConnect.Size = new System.Drawing.Size(108, 44);
            this.BtnExitConnect.TabIndex = 0;
            this.BtnExitConnect.Text = "连接并开始";
            this.BtnExitConnect.UseVisualStyleBackColor = true;
            this.BtnExitConnect.Click += new System.EventHandler(this.BtnExitConnect_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.BtnExitDisconnect);
            this.panel2.Controls.Add(this.LblExit);
            this.panel2.Controls.Add(this.BtnExitConnect);
            this.panel2.Location = new System.Drawing.Point(22, 439);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(314, 152);
            this.panel2.TabIndex = 3;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.BtnProjectDisconnect);
            this.panel3.Controls.Add(this.LblProject);
            this.panel3.Controls.Add(this.BtnProjectConnect);
            this.panel3.Location = new System.Drawing.Point(22, 235);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(314, 152);
            this.panel3.TabIndex = 3;
            // 
            // BtnProjectDisconnect
            // 
            this.BtnProjectDisconnect.Enabled = false;
            this.BtnProjectDisconnect.Location = new System.Drawing.Point(176, 78);
            this.BtnProjectDisconnect.Name = "BtnProjectDisconnect";
            this.BtnProjectDisconnect.Size = new System.Drawing.Size(108, 44);
            this.BtnProjectDisconnect.TabIndex = 2;
            this.BtnProjectDisconnect.Text = "断开";
            this.BtnProjectDisconnect.UseVisualStyleBackColor = true;
            // 
            // LblProject
            // 
            this.LblProject.AutoSize = true;
            this.LblProject.Font = new System.Drawing.Font("宋体", 15F);
            this.LblProject.Location = new System.Drawing.Point(29, 18);
            this.LblProject.Name = "LblProject";
            this.LblProject.Size = new System.Drawing.Size(87, 25);
            this.LblProject.TabIndex = 1;
            this.LblProject.Text = "项目处";
            // 
            // BtnProjectConnect
            // 
            this.BtnProjectConnect.Location = new System.Drawing.Point(34, 78);
            this.BtnProjectConnect.Name = "BtnProjectConnect";
            this.BtnProjectConnect.Size = new System.Drawing.Size(108, 44);
            this.BtnProjectConnect.TabIndex = 0;
            this.BtnProjectConnect.Text = "连接并开始";
            this.BtnProjectConnect.UseVisualStyleBackColor = true;
            this.BtnProjectConnect.Click += new System.EventHandler(this.BtnProjectConnect_Click);
            // 
            // TimeProject
            // 
            this.TimeProject.Enabled = true;
            this.TimeProject.Interval = 1000;
            this.TimeProject.Tick += new System.EventHandler(this.TimeProject_Tick);
            // 
            // ModuleReaderManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(360, 609);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ModuleReaderManager";
            this.Text = "游无忧游乐园RFID读写管理";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnEnterConnect;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label LblEnter;
        private System.Windows.Forms.Button BtnEnterDisconnect;
        private System.Windows.Forms.Timer TimerUpdate;
        private System.Windows.Forms.Button BtnExitDisconnect;
        private System.Windows.Forms.Label LblExit;
        private System.Windows.Forms.Button BtnExitConnect;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button BtnProjectDisconnect;
        private System.Windows.Forms.Label LblProject;
        private System.Windows.Forms.Button BtnProjectConnect;
        private System.Windows.Forms.Timer TimeProject;
    }
}

