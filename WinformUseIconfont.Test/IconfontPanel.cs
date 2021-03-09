using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace WinformUseIconfont.Test
{
    public partial class IconfontPanel : Panel  
    {
        public IconfontPanel()
        {
            InitializeComponent();
            this.Size = new Size(32, 32);
            _iconfontBackColer = BackColor;
            old_iconfontBackColer = BackColor;
        }

        private bool _iconfontChange = false;
        private string _iconfont = string.Empty;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Editor(typeof(IconfontEditor), typeof(UITypeEditor))]
        [Description("矢量图标编码"), Category("自定义矢量图标属性")]
        public string Iconfont
        {
            get => _iconfont;
            set
            {
                _iconfont = value;
                _iconfontChange = true;
                Invalidate();
            }
        }

        int _iconSize = IconfontHelper.IconSize;

        [Description("图标大小"), Category("自定义矢量图标属性")]
        public int IconSize
        {
            get => _iconSize;
            set
            {
                _iconSize = value;
                _iconfontChange = true;
                Invalidate();
            }
        }
        
        private Color _iconfontForeColer = IconfontHelper.ForeColer;

        [Description("前景颜色"), Category("自定义矢量图标属性")]
        public Color IconfontForeColer
        {
            get => _iconfontForeColer;
            set 
            {
                _iconfontForeColer = value;
                _iconfontChange = true;
                Invalidate();
            }
        }

        private Color _iconfontBackColer;
        private Color old_iconfontBackColer;

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            _iconfontBackColer = BackColor;
            if (_iconfontBackColer != old_iconfontBackColer)
            {
                _iconfontChange = true;
                old_iconfontBackColer = BackColor;
            }

            string[] sArray = _iconfont.Split(':');
            if (_iconfontChange && sArray != null && sArray.Length == 2 && Enum.IsDefined(typeof(IconfontEnum), sArray[0]) && int.TryParse(sArray[1], out int iconText))
            {
                this.Text = "";
                this.AutoSize = false;
                this.BackgroundImageLayout = ImageLayout.Center;
                _iconfontChange = false;
                this.BackgroundImage = IconfontHelper.GetBitmap((IconfontEnum)Enum.Parse(typeof(IconfontEnum), sArray[0], false), iconText, _iconSize, _iconfontBackColer, IconfontForeColer);
            }

        }
    }
}
