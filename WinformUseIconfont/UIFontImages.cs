/******************************************************************************
 * SunnyUI 开源控件库、工具类库、扩展类库、多页面开发框架。
 * CopyRight (C) 2012-2020 ShenYongHua(沈永华).
 * QQ群：56829229 QQ：17612584 EMail：SunnyUI@qq.com
 *
 * Blog:   https://www.cnblogs.com/yhuse
 * Gitee:  https://gitee.com/yhuse/SunnyUI
 * GitHub: https://github.com/yhuse/SunnyUI
 *
 * SunnyUI.dll can be used for free under the GPL-3.0 license.
 * If you use this code, please keep this note.
 * 如果您使用此代码，请保留此说明。
 ******************************************************************************
 * 文件名称: UIFontImages.cs
 * 文件说明: 字体图片属性窗体
 * 当前版本: V2.2
 * 创建日期: 2020-01-01
 *
 * 2020-01-01: V2.2.0 增加文件说明
******************************************************************************/

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
    /// 字体图标编辑器
    /// </summary>
    public partial class UIFontImages : Form
    {
        private readonly ConcurrentQueue<Label> AwesomeLabels = new ConcurrentQueue<Label>();
        private readonly ConcurrentQueue<Label> ElegantLabels = new ConcurrentQueue<Label>();

        /// <summary>
        /// 构造函数
        /// </summary>
        public UIFontImages()
        {
            InitializeComponent();
        }

        private void UIFontImages_Load(object sender, EventArgs e)
        {
            AddHighFreqImage();
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

        private void AddHighFreqImage()
        {
            AddLabel(AwesomeIconfont.fa_check);
            AddLabel(AwesomeIconfont.fa_close);

            AddLabel(AwesomeIconfont.fa_ellipsis_h);
            AddLabel(AwesomeIconfont.fa_file);
            AddLabel(AwesomeIconfont.fa_file_o);
            AddLabel(AwesomeIconfont.fa_save);
            AddLabel(AwesomeIconfont.fa_folder);
            AddLabel(AwesomeIconfont.fa_folder_o);
            AddLabel(AwesomeIconfont.fa_folder_open);
            AddLabel(AwesomeIconfont.fa_folder_open_o);

            AddLabel(AwesomeIconfont.fa_plus);
            AddLabel(AwesomeIconfont.fa_edit);
            AddLabel(AwesomeIconfont.fa_minus);
            AddLabel(AwesomeIconfont.fa_refresh);

            AddLabel(AwesomeIconfont.fa_exclamation);
            AddLabel(AwesomeIconfont.fa_exclamation_circle);
            AddLabel(AwesomeIconfont.fa_warning);
            AddLabel(AwesomeIconfont.fa_info);

            AddLabel(AwesomeIconfont.fa_info_circle);
            AddLabel(AwesomeIconfont.fa_check_circle);
            AddLabel(AwesomeIconfont.fa_check_circle_o);
            AddLabel(AwesomeIconfont.fa_times_circle);
            AddLabel(AwesomeIconfont.fa_times_circle_o);
            AddLabel(AwesomeIconfont.fa_question);
            AddLabel(AwesomeIconfont.fa_question_circle);
            AddLabel(AwesomeIconfont.fa_question_circle_o);
            AddLabel(AwesomeIconfont.fa_ban);

            AddLabel(AwesomeIconfont.fa_toggle_left);
            AddLabel(AwesomeIconfont.fa_toggle_right);
            AddLabel(AwesomeIconfont.fa_toggle_up);
            AddLabel(AwesomeIconfont.fa_toggle_down);

            AddLabel(AwesomeIconfont.fa_lock);
            AddLabel(AwesomeIconfont.fa_unlock);
            AddLabel(AwesomeIconfont.fa_unlock_alt);

            AddLabel(AwesomeIconfont.fa_cog);
            AddLabel(AwesomeIconfont.fa_cogs);

            AddLabel(AwesomeIconfont.fa_window_minimize);
            AddLabel(AwesomeIconfont.fa_window_maximize);
            AddLabel(AwesomeIconfont.fa_window_restore);
            AddLabel(AwesomeIconfont.fa_window_close);
            AddLabel(AwesomeIconfont.fa_window_close_o);

            AddLabel(AwesomeIconfont.fa_user);
            AddLabel(AwesomeIconfont.fa_user_o);
            AddLabel(AwesomeIconfont.fa_user_circle);
            AddLabel(AwesomeIconfont.fa_user_circle_o);
            AddLabel(AwesomeIconfont.fa_user_plus);
            AddLabel(AwesomeIconfont.fa_user_times);

            AddLabel(AwesomeIconfont.fa_tag);
            AddLabel(AwesomeIconfont.fa_tags);

            AddLabel(AwesomeIconfont.fa_plus_circle);
            AddLabel(AwesomeIconfont.fa_plus_square);
            AddLabel(AwesomeIconfont.fa_plus_square_o);

            AddLabel(AwesomeIconfont.fa_minus_circle);
            AddLabel(AwesomeIconfont.fa_minus_square);
            AddLabel(AwesomeIconfont.fa_minus_square_o);

            AddLabel(AwesomeIconfont.fa_search);
            AddLabel(AwesomeIconfont.fa_search_minus);
            AddLabel(AwesomeIconfont.fa_search_plus);

            AddLabel(AwesomeIconfont.fa_bar_chart);
            AddLabel(AwesomeIconfont.fa_area_chart);
            AddLabel(AwesomeIconfont.fa_line_chart);
            AddLabel(AwesomeIconfont.fa_pie_chart);
            AddLabel(AwesomeIconfont.fa_photo);

            AddLabel(AwesomeIconfont.fa_power_off);
            AddLabel(AwesomeIconfont.fa_print);
            AddLabel(AwesomeIconfont.fa_bars);

            AddLabel(AwesomeIconfont.fa_sign_in);
            AddLabel(AwesomeIconfont.fa_sign_out);

            AddLabel(AwesomeIconfont.fa_play);
            AddLabel(AwesomeIconfont.fa_pause);
            AddLabel(AwesomeIconfont.fa_stop);
            AddLabel(AwesomeIconfont.fa_fast_backward);
            AddLabel(AwesomeIconfont.fa_backward);
            AddLabel(AwesomeIconfont.fa_forward);
            AddLabel(AwesomeIconfont.fa_fast_forward);
            AddLabel(AwesomeIconfont.fa_eject);
        }

        private void AddLabel(int icon)
        {
            Label lbl = CreateLabel(icon);
            lpCustom.Controls.Add(lbl);
            int symbol = (int)lbl.Tag;
            toolTip.SetToolTip(lbl, symbol.ToString());
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
    /// 字体图标属性编辑器
    /// </summary>
    public class UIImagePropertyEditor : UITypeEditor
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
            UIFontImages frm = new UIFontImages();
            if (frm.ShowDialog() == DialogResult.OK)
                value = frm.SelectSymbol;
            frm.Dispose();
            return value;
        }
    }
}