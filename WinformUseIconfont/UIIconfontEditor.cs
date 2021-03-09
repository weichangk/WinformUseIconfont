using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Reflection;
using System.Windows.Forms;

namespace WinformUseIconfont
{
    //修改该类时要重启编译器后图标集合才重新加载！！！

    /// <summary>
    /// 矢量图标编辑窗体
    /// </summary>
    public partial class UIIconfontEditor : Form
    {
        public static IconfontEnum iconfontEnum = IconfontEnum.AwesomeFont;

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
                    toolTip.SetToolTip(lbl, lbl.Tag.ToString());
                }
            }

            while (ElegantLabels.Count > 0)
            {
                if (ElegantLabels.TryDequeue(out Label lbl))
                {
                    lpElegant.Controls.Add(lbl);
                    toolTip.SetToolTip(lbl, lbl.Tag.ToString());
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
                AwesomeLabels.Enqueue(CreateLabel(value, IconfontEnum.AwesomeFont));
            }
        }

        private void AddElegantImageEx()
        {
            Type t = typeof(ElegantIconfont);
            FieldInfo[] fis = t.GetFields();
            foreach (var fieldInfo in fis)
            {
                int value = int.TryParse(fieldInfo.GetRawConstantValue().ToString(), out var result) ? result : default(int);
                ElegantLabels.Enqueue(CreateLabel(value, IconfontEnum.ElegantFont));
            }
        }

        private Label CreateLabel(int icon, IconfontEnum iconfontEnum)
        {
            Label lbl = new Label();
            lbl.AutoSize = false;
            lbl.Size = new Size(32, 32);
            switch (iconfontEnum)
            {
                case IconfontEnum.AwesomeFont:
                    lbl.Image = IconfontHelper.GetImage(icon, iconfontEnum);
                    break;
                case IconfontEnum.ElegantFont:
                    lbl.Image = IconfontHelper.GetImage(icon, iconfontEnum);
                    break;
                default:
                    break;
            }
            lbl.ImageAlign = ContentAlignment.MiddleCenter;
            lbl.TextAlign = ContentAlignment.MiddleLeft;
            lbl.Margin = new Padding(2);
            lbl.Click += lbl_DoubleClick;
            lbl.MouseEnter += Lbl_MouseEnter;
            lbl.MouseLeave += Lbl_MouseLeave;
            lbl.Tag = $"{iconfontEnum}:{icon}";
            return lbl;
        }

        /// <summary>
        /// 选中图标
        /// </summary>
        public string SelectIconText { get; private set; }

        private void lbl_DoubleClick(object sender, EventArgs e)
        {
            if (sender is Label lbl) SelectIconText = lbl.Tag.ToString();
            DialogResult = DialogResult.OK;
            Close();
        }
        private void Lbl_MouseLeave(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            lbl.ForeColor = Color.FromArgb(48, 48, 48);
        }

        private void Lbl_MouseEnter(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            lbl.ForeColor = Color.FromArgb(80, 160, 255);
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
                value = frm.SelectIconText;
            frm.Dispose();
            return value;
        }
    }
}