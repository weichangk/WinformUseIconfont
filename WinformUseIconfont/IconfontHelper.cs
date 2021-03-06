using System.Drawing;
using System.Drawing.Text;
using System.IO;

namespace WinformUseIconfont
{
    /// <summary>
    /// 矢量图标帮助类
    /// </summary>
    public static class IconfontHelper
    {
        /// <summary>
        /// AwesomeFont
        /// </summary>
        public static Iconfont AwesomeFont;

        /// <summary>
        /// ElegantFont
        /// </summary>
        public static Iconfont ElegantFont;

        /// <summary>
        /// 构造函数
        /// </summary>
        static IconfontHelper()
        {
            AwesomeFont = new Iconfont(ReadFontFileFromResource("WinformUseIconfont.ttf.FontAwesome.ttf"));
            ElegantFont = new Iconfont(ReadFontFileFromResource("WinformUseIconfont.ttf.ElegantIcons.ttf"));
        }


        /// <summary>
        /// 从系统资源中保存字体文件
        /// </summary>
        /// <param name="file">字体文件名</param>
        /// <param name="resource">资源名称</param>
        private static void CreateFontFile(string file, string resource)
        {
            if (!File.Exists(file))
            {
                Stream fontStream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(resource);
                if (fontStream != null)
                {
                    byte[] buffer = new byte[fontStream.Length];
                    fontStream.Read(buffer, 0, (int)fontStream.Length);
                    fontStream.Close();

                    File.WriteAllBytes(file, buffer);
                }
            }
        }

        private static byte[] ReadFontFileFromResource(string name)
        {
            byte[] buffer = null;
            Stream fontStream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
            if (fontStream != null)
            {
                buffer = new byte[fontStream.Length];
                fontStream.Read(buffer, 0, (int)fontStream.Length);
                fontStream.Close();
            }
            return buffer;
        }

        /// <summary>
        /// 获取字体大小
        /// </summary>
        /// <param name="graphics">GDI绘图</param>
        /// <param name="symbol">字符</param>
        /// <param name="symbolSize">大小</param>
        /// <returns>字体大小</returns>
        public static SizeF GetFontImageSize(this Graphics graphics, int symbol, int symbolSize)
        {
            Font font = GetFont(symbol, symbolSize);
            if (font == null)
            {
                return new SizeF(0, 0);
            }

            return graphics.MeasureString(char.ConvertFromUtf32(symbol), font);
        }

        /// <summary>
        /// 绘制字体图片
        /// </summary>
        /// <param name="graphics">GDI绘图</param>
        /// <param name="symbol">字符</param>
        /// <param name="symbolSize">大小</param>
        /// <param name="color">颜色</param>
        /// <param name="left">左</param>
        /// <param name="top">上</param>
        /// <param name="xOffset">左右偏移</param>
        /// <param name="yOffSet">上下偏移</param>
        public static void DrawFontImage(this Graphics graphics, int symbol, int symbolSize, Color color, float left, float top, int xOffset = 0, int yOffSet = 0)
        {
            //字体
            Font font = GetFont(symbol, symbolSize);
            if (font == null)
            {
                return;
            }

            string text = char.ConvertFromUtf32(symbol);
            graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            using (Brush br = new SolidBrush(color))
            {
                graphics.DrawString(text, font, br, xOffset, top + yOffSet);
            }

            graphics.TextRenderingHint = TextRenderingHint.SystemDefault;
        }

        /// <summary>
        /// 绘制字体图片
        /// </summary>
        /// <param name="graphics">GDI绘图</param>
        /// <param name="symbol">字符</param>
        /// <param name="symbolSize">大小</param>
        /// <param name="color">颜色</param>
        /// <param name="rect">区域</param>
        /// <param name="xOffset">左右偏移</param>
        /// <param name="yOffSet">上下偏移</param>
        public static void DrawFontImage(this Graphics graphics, int symbol, int symbolSize, Color color, Rectangle rect, int xOffset = 0, int yOffSet = 0)
        {
            SizeF sf = graphics.GetFontImageSize(symbol, symbolSize);
            graphics.DrawFontImage(symbol, symbolSize, color, rect.Left + (int)(((rect.Width - sf.Width) / 2.0f) + 0.5f), 
                rect.Top + (int)(((rect.Height - sf.Height) / 2.0f) + 0.5f), xOffset, yOffSet); 
        }

        /// <summary>
        /// 绘制字体图片
        /// </summary>
        /// <param name="graphics">GDI绘图</param>
        /// <param name="symbol">字符</param>
        /// <param name="symbolSize">大小</param>
        /// <param name="color">颜色</param>
        /// <param name="rect">区域</param>
        /// <param name="xOffset">左右偏移</param>
        /// <param name="yOffSet">上下偏移</param>
        public static void DrawFontImage(this Graphics graphics, int symbol, int symbolSize, Color color, RectangleF rect, int xOffset = 0, int yOffSet = 0)
        {
            SizeF sf = graphics.GetFontImageSize(symbol, symbolSize);
            graphics.DrawFontImage(symbol, symbolSize, color, rect.Left + (int)(((rect.Width - sf.Width) / 2.0f) + 0.5f), 
                rect.Top + (int)(((rect.Height - sf.Height) / 2.0f) + 0.5f), xOffset, yOffSet);    
        }

        /// <summary>
        /// 创建图片
        /// </summary>
        /// <param name="symbol">字符</param>
        /// <param name="size">大小</param>
        /// <param name="color">颜色</param>
        /// <returns>图片</returns>
        public static Image CreateImage(int symbol, int size, Color color)
        {
            Bitmap image = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(image))
            {
                SizeF sf = g.GetFontImageSize(symbol, size);
                g.DrawFontImage(symbol, size, color, (image.Width - sf.Width) / 2.0f, (image.Height - sf.Height) / 2.0f);
            }

            return image;
        }

        /// <summary>
        /// 获取字体
        /// </summary>
        /// <param name="symbol">字符</param>
        /// <param name="imageSize">大小</param>
        /// <returns>字体</returns>
        public static Font GetFont(int symbol, int imageSize)
        {
            if (symbol > 0xF000)
                return AwesomeFont.GetFont(symbol, imageSize);
            else
                return ElegantFont.GetFont(symbol, imageSize);
        }
    }
}
