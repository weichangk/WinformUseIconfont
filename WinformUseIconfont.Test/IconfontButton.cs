using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinformUseIconfont.Test
{
    /// <summary>
    /// 自定义矢量图标按键控件
    /// </summary>

    [DefaultProperty("Text")]
    public partial class IconfontButton : Button
    {
        public IconfontButton()
        {
            InitializeComponent();
        }

        private int _iconfont = AwesomeIconfont.fa_check;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Editor(typeof(UIImagePropertyEditor), typeof(UITypeEditor))]
        [DefaultValue(61543)]//61452
        [Description("矢量图标编码"), Category("自定义矢量图标属性")]
        public int Iconfont
        {
            get => _iconfont;
            set
            {
                _iconfont = value;
                Invalidate();
            }
        }

        private int _iconfontSize = 24;

        [DefaultValue(24)]
        [Description("矢量图标大小"), Category("自定义矢量图标属性")]
        public int IconfontSize
        {
            get => _iconfontSize;
            set
            {
                _iconfontSize = Math.Max(value, 16);
                _iconfontSize = Math.Min(value, 64);
                Invalidate();
            }
        }

        private int _iconfontInterval = 2;

        [DefaultValue(2)]
        [Description("矢量图标文字间间隔"), Category("自定义矢量图标属性")]
        public int IconfontInterval
        {
            get => _iconfontInterval;
            set
            {
                _iconfontInterval = Math.Max(0, value);
                Invalidate();
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            //重绘父类
            base.OnPaint(e);

            SizeF ImageSize = new SizeF(0, 0);
            if (Iconfont > 0)
                ImageSize = e.Graphics.GetFontImageSize(Iconfont, IconfontSize);
            if (Image != null)
                ImageSize = Image.Size;

            //矢量图标字体颜色由控件文本颜色属性ForeColor设置
            Color color = ForeColor;
            SizeF TextSize = e.Graphics.MeasureString(Text, Font);

            if (ImageAlign == ContentAlignment.MiddleCenter && TextAlign == ContentAlignment.MiddleCenter)
            {
                if (ImageSize.Width.Equals(0))
                {
                    if (TextSize.Width > 0)
                    {
                        using (Brush br = new SolidBrush(color))
                        {
                            e.Graphics.DrawString(Text, Font, br, (Width - TextSize.Width) / 2.0f, (Height - TextSize.Height) / 2.0f);
                        }
                    }
                }
                else if (TextSize.Width.Equals(0))
                {
                    if (ImageSize.Width > 0)
                    {
                        if (Iconfont > 0 && Image == null)
                        {
                            e.Graphics.DrawFontImage(Iconfont, IconfontSize, color,
                                new RectangleF((Width - ImageSize.Width) / 2.0f, (Height - ImageSize.Height) / 2.0f,
                                    ImageSize.Width, ImageSize.Height));
                        }

                        if (Image != null)
                        {
                            e.Graphics.DrawImage(Image, (Width - Image.Width) / 2.0f, (Height - Image.Height) / 2.0f,
                                ImageSize.Width, ImageSize.Height);
                        }
                    }
                }
                else
                {
                    float allWidth = ImageSize.Width + IconfontInterval + TextSize.Width;

                    if (Iconfont > 0 && Image == null)
                    {
                        e.Graphics.DrawFontImage(Iconfont, IconfontSize, color,
                            new RectangleF((Width - allWidth) / 2.0f, (Height - ImageSize.Height) / 2.0f, ImageSize.Width, ImageSize.Height));
                    }

                    if (Image != null)
                    {
                        e.Graphics.DrawImage(Image, (Width - allWidth) / 2.0f, (Height - ImageSize.Height) / 2.0f,
                            ImageSize.Width, ImageSize.Height);
                    }

                    using (Brush br = new SolidBrush(color))
                    {
                        e.Graphics.DrawString(Text, Font, br, (Width - allWidth) / 2.0f + ImageSize.Width + IconfontInterval, (Height - TextSize.Height) / 2.0f);
                    }
                }
            }
            else
            {
                float left = 0;
                float top = 0;

                if (ImageSize.Width > 0)
                {
                    switch (ImageAlign)
                    {
                        case ContentAlignment.TopLeft:
                            left = Padding.Left;
                            top = Padding.Top;
                            break;

                        case ContentAlignment.TopCenter:
                            left = (Width - ImageSize.Width) / 2.0f;
                            top = Padding.Top;
                            break;

                        case ContentAlignment.TopRight:
                            left = Width - Padding.Right - ImageSize.Width;
                            top = Padding.Top;
                            break;

                        case ContentAlignment.MiddleLeft:
                            left = Padding.Left;
                            top = (Height - ImageSize.Height) / 2.0f;
                            break;

                        case ContentAlignment.MiddleCenter:
                            left = (Width - ImageSize.Width) / 2.0f;
                            top = (Height - ImageSize.Height) / 2.0f;
                            break;

                        case ContentAlignment.MiddleRight:
                            left = Width - Padding.Right - ImageSize.Width;
                            top = (Height - ImageSize.Height) / 2.0f;
                            break;

                        case ContentAlignment.BottomLeft:
                            left = Padding.Left;
                            top = Height - Padding.Bottom - ImageSize.Height;
                            break;

                        case ContentAlignment.BottomCenter:
                            left = (Width - ImageSize.Width) / 2.0f;
                            top = Height - Padding.Bottom - ImageSize.Height;
                            break;

                        case ContentAlignment.BottomRight:
                            left = Width - Padding.Right - ImageSize.Width;
                            top = Height - Padding.Bottom - ImageSize.Height;
                            break;
                    }

                    if (Iconfont > 0 && Image == null)
                    {
                        e.Graphics.DrawFontImage(Iconfont, IconfontSize, color,
                            new RectangleF(left, top, ImageSize.Width, ImageSize.Height));
                    }

                    if (Image != null)
                    {
                        e.Graphics.DrawImage(Image, left, top, ImageSize.Width, ImageSize.Height);
                    }
                }

                left = 0;
                top = 0;
                if (TextSize.Width > 0)
                {
                    switch (TextAlign)
                    {
                        case ContentAlignment.TopLeft:
                            left = Padding.Left;
                            top = Padding.Top;
                            break;

                        case ContentAlignment.TopCenter:
                            left = (Width - TextSize.Width) / 2.0f;
                            top = Padding.Top;
                            break;

                        case ContentAlignment.TopRight:
                            left = Width - Padding.Right - TextSize.Width;
                            top = Padding.Top;
                            break;

                        case ContentAlignment.MiddleLeft:
                            left = Padding.Left;
                            top = (Height - TextSize.Height) / 2.0f;
                            break;

                        case ContentAlignment.MiddleCenter:
                            left = (Width - TextSize.Width) / 2.0f;
                            top = (Height - TextSize.Height) / 2.0f;
                            break;

                        case ContentAlignment.MiddleRight:
                            left = Width - Padding.Right - TextSize.Width;
                            top = (Height - TextSize.Height) / 2.0f;
                            break;

                        case ContentAlignment.BottomLeft:
                            left = Padding.Left;
                            top = Height - Padding.Bottom - TextSize.Height;
                            break;

                        case ContentAlignment.BottomCenter:
                            left = (Width - TextSize.Width) / 2.0f;
                            top = Height - Padding.Bottom - TextSize.Height;
                            break;

                        case ContentAlignment.BottomRight:
                            left = Width - Padding.Right - TextSize.Width;
                            top = Height - Padding.Bottom - TextSize.Height;
                            break;
                    }

                    using (Brush br = new SolidBrush(color))
                    {
                        e.Graphics.DrawString(Text, Font, br, left, top);
                    }
                }
            }
        }
    }
}
