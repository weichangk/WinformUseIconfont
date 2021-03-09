namespace WinformUseIconfont.Test
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
            this.iconfontPanel1 = new WinformUseIconfont.Test.IconfontPanel();
            this.SuspendLayout();
            // 
            // iconfontPanel1
            // 
            this.iconfontPanel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.iconfontPanel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("iconfontPanel1.BackgroundImage")));
            this.iconfontPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.iconfontPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.iconfontPanel1.Iconfont = "AwesomeFont:61980";
            this.iconfontPanel1.IconfontForeColer = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.iconfontPanel1.IconSize = 128;
            this.iconfontPanel1.Location = new System.Drawing.Point(0, 0);
            this.iconfontPanel1.Name = "iconfontPanel1";
            this.iconfontPanel1.Size = new System.Drawing.Size(769, 450);
            this.iconfontPanel1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(769, 450);
            this.Controls.Add(this.iconfontPanel1);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private IconfontPanel iconfontPanel1;
    }
}

