using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Reflection;
using System.Windows.Forms;

namespace WinformUseIconfont
{
    /// <summary>
    /// 矢量图标编辑窗体
    /// </summary>
    public partial class UIIconfontEditor : Form
    {
        private readonly ConcurrentQueue<Label> AwesomeLabels = new ConcurrentQueue<Label>();
        private readonly ConcurrentQueue<Label> ElegantLabels = new ConcurrentQueue<Label>();

        /// <summary>
        /// 构造函数
        /// </summary>
        public UIIconfontEditor()
        {
            InitializeComponent();
        }

        private void UIIconfontEditor_Load(object sender, EventArgs e)
        {
            bg.RunWorkerAsync();
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();

            while (AwesomeLabels.Count > 0)
            {
                if (AwesomeLabels.TryDequeue(out Label lbl))
                {
                    lpAwesome.Controls.Add(lbl);
                    int symbol = (int)lbl.Tag;
                    toolTip.SetToolTip(lbl, symbol.ToString());
                }
            }

            while (ElegantLabels.Count > 0)
            {
                if (ElegantLabels.TryDequeue(out Label lbl))
                {
                    lpElegant.Controls.Add(lbl);
                    int symbol = (int)lbl.Tag;
                    toolTip.SetToolTip(lbl, symbol.ToString());
                }
            }

            timer.Start();
        }

        private void AddAwesomeImageEx()
        {
            Type t = typeof(AwesomeIconfont);
            FieldInfo[] fis = t.GetFields();
            foreach (var fieldInfo in fis)
            {
                int value = int.TryParse(fieldInfo.GetRawConstantValue().ToString(), out var result) ? result : default(int);
                AwesomeLabels.Enqueue(CreateLabel(value));
            }
        }

        private void AddElegantImageEx()
        {
            Type t = typeof(ElegantIconfont);
            FieldInfo[] fis = t.GetFields();
            foreach (var fieldInfo in fis)
            {
                int value = int.TryParse(fieldInfo.GetRawConstantValue().ToString(), out var result) ? result : default(int);
                ElegantLabels.Enqueue(CreateLabel(value));
            }
        }

        private Label CreateLabel(int icon)
        {
            Label lbl = new Label();
            lbl.AutoSize = false;
            lbl.Size = new Size(32, 32);
            lbl.ForeColor = Color.FromArgb(80, 160, 255);

            IconfontHelper.AwesomeFont.ForeColor = Color.FromArgb(48, 48, 48);
            IconfontHelper.ElegantFont.ForeColor = Color.FromArgb(48, 48, 48);

            lbl.Image = icon >= 0xF000 ?
                        IconfontHelper.AwesomeFont.GetImage(icon, 28) :
                        IconfontHelper.ElegantFont.GetImage(icon, 28);
            lbl.ImageAlign = ContentAlignment.MiddleCenter;
            lbl.TextAlign = ContentAlignment.MiddleLeft;
            lbl.Margin = new Padding(2);
            lbl.Click += lbl_DoubleClick;
            lbl.MouseEnter += Lbl_MouseEnter;
            lbl.MouseLeave += Lbl_MouseLeave;
            lbl.Tag = icon;
            return lbl;
        }

        private void Lbl_MouseLeave(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            IconfontHelper.AwesomeFont.ForeColor = Color.FromArgb(48, 48, 48);
            IconfontHelper.ElegantFont.ForeColor = Color.FromArgb(48, 48, 48);
            int icon = (int)lbl.Tag;
            lbl.Image = icon >= 0xF000 ?
                        IconfontHelper.AwesomeFont.GetImage(icon, 28) :
                        IconfontHelper.ElegantFont.GetImage(icon, 28);
        }

        private void Lbl_MouseEnter(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            IconfontHelper.AwesomeFont.ForeColor = Color.FromArgb(80, 160, 255);
            IconfontHelper.ElegantFont.ForeColor = Color.FromArgb(80, 160, 255);
            int icon = (int)lbl.Tag;
            lbl.Image = icon >= 0xF000 ?
                        IconfontHelper.AwesomeFont.GetImage(icon, 28) :
                        IconfontHelper.ElegantFont.GetImage(icon, 28);
        }

        /// <summary>
        /// 选中图标
        /// </summary>
        public int SelectSymbol { get; private set; }

        private void lbl_DoubleClick(object sender, EventArgs e)
        {
            if (sender is Label lbl) SelectSymbol = (int)lbl.Tag;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void bg_DoWork(object sender, DoWorkEventArgs e)
        {
            AddAwesomeImageEx();
            //scoreStep = 1;
            AddElegantImageEx();
        }

        private void bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }
    }

    /// <summary>
    /// 矢量图标属性编辑器
    /// </summary>
    public class IconfontEditor : UITypeEditor
    {
        /// <summary>
        /// GetEditStyle
        /// </summary>
        /// <param name="context">context</param>
        /// <returns>UITypeEditorEditStyle</returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            //指定为模式窗体属性编辑器类型
            return UITypeEditorEditStyle.Modal;
        }

        /// <summary>
        /// EditValue
        /// </summary>
        /// <param name="context">context</param>
        /// <param name="provider">provider</param>
        /// <param name="value">value</param>
        /// <returns>object</returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            //打开属性编辑器修改数据
            UIIconfontEditor frm = new UIIconfontEditor();
            if (frm.ShowDialog() == DialogResult.OK)
                value = frm.SelectSymbol;
            frm.Dispose();
            return value;
        }
    }
}