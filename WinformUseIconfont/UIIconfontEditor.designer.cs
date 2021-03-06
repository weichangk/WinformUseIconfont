namespace WinformUseIconfont
{
    partial class UIIconfontEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageAwesome = new System.Windows.Forms.TabPage();
            this.lpAwesome = new System.Windows.Forms.FlowLayoutPanel();
            this.tabPageElegant = new System.Windows.Forms.TabPage();
            this.lpElegant = new System.Windows.Forms.FlowLayoutPanel();
            this.bg = new System.ComponentModel.BackgroundWorker();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPageAwesome.SuspendLayout();
            this.tabPageElegant.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageAwesome);
            this.tabControl1.Controls.Add(this.tabPageElegant);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(497, 450);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageAwesome
            // 
            this.tabPageAwesome.Controls.Add(this.lpAwesome);
            this.tabPageAwesome.Location = new System.Drawing.Point(4, 22);
            this.tabPageAwesome.Name = "tabPageAwesome";
            this.tabPageAwesome.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAwesome.Size = new System.Drawing.Size(489, 424);
            this.tabPageAwesome.TabIndex = 1;
            this.tabPageAwesome.Text = "AwesomeFont";
            this.tabPageAwesome.UseVisualStyleBackColor = true;
            // 
            // lpAwesome
            // 
            this.lpAwesome.AutoScroll = true;
            this.lpAwesome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lpAwesome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(158)))), ((int)(((byte)(255)))));
            this.lpAwesome.Location = new System.Drawing.Point(3, 3);
            this.lpAwesome.Name = "lpAwesome";
            this.lpAwesome.Size = new System.Drawing.Size(483, 418);
            this.lpAwesome.TabIndex = 1;
            // 
            // tabPageElegant
            // 
            this.tabPageElegant.Controls.Add(this.lpElegant);
            this.tabPageElegant.Location = new System.Drawing.Point(4, 22);
            this.tabPageElegant.Name = "tabPageElegant";
            this.tabPageElegant.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageElegant.Size = new System.Drawing.Size(489, 424);
            this.tabPageElegant.TabIndex = 2;
            this.tabPageElegant.Text = "ElegantFont";
            this.tabPageElegant.UseVisualStyleBackColor = true;
            // 
            // lpElegant
            // 
            this.lpElegant.AutoScroll = true;
            this.lpElegant.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lpElegant.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(158)))), ((int)(((byte)(255)))));
            this.lpElegant.Location = new System.Drawing.Point(3, 3);
            this.lpElegant.Name = "lpElegant";
            this.lpElegant.Size = new System.Drawing.Size(483, 418);
            this.lpElegant.TabIndex = 2;
            // 
            // bg
            // 
            this.bg.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bg_DoWork);
            this.bg.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bg_RunWorkerCompleted);
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // UIFontImages
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(497, 450);
            this.Controls.Add(this.tabControl1);
            this.Name = "UIFontImages";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "字体图标";
            this.Load += new System.EventHandler(this.UIIconfontEditor_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPageAwesome.ResumeLayout(false);
            this.tabPageElegant.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageAwesome;
        private System.Windows.Forms.FlowLayoutPanel lpAwesome;
        private System.Windows.Forms.TabPage tabPageElegant;
        private System.Windows.Forms.FlowLayoutPanel lpElegant;
        private System.ComponentModel.BackgroundWorker bg;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.ToolTip toolTip;
    }
}