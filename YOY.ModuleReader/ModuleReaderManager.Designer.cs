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
            this.BtnEnterConnect = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.LblEnter = new System.Windows.Forms.Label();
            this.BtnEnterDisconnect = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
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
            // ModuleReaderManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel1);
            this.Name = "ModuleReaderManager";
            this.Text = "游无忧游乐园RFID读写管理";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnEnterConnect;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label LblEnter;
        private System.Windows.Forms.Button BtnEnterDisconnect;
    }
}

