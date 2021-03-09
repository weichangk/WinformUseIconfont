using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace WinformUseIconfont
{
    public class IconfontHelper
    {

        #region Fileds
        /// <summary>
        /// FontCollection object
        /// </summary>
        private static readonly PrivateFontCollection AwesomeFontCollection = new PrivateFontCollection();
        private static readonly PrivateFontCollection ElegantFontCollection = new PrivateFontCollection();
        /// <summary>
        /// icon image size
        /// </summary>
        public static int IconSize { get; set; } = 32;
        public static int _IconSize { get; set; } = 32;
        /// <summary>
        /// border visible
        /// </summary>
        public static bool BorderVisible { get; set; } = false;
        public static bool _BorderVisible { get; set; } = false;
        /// <summary>
        /// icon image backcolor
        /// </summary>
        public static Color BackColer { get; set; } = Color.Transparent;
        public static Color _BackColer { get; set; } = Color.Transparent;
        /// <summary>
        /// icon image forecolor
        /// </summary>
        public static Color ForeColer { get; set; } = Color.FromArgb(48, 48, 48);
        public static Color _ForeColer { get; set; } = Color.FromArgb(48, 48, 48);
        /// <summary>
        /// icon image border color
        /// </summary>
        public static Color BorderColer { get; set; } = Color.Gray;
        public static Color _BorderColer { get; set; } = Color.Gray;

        #endregion

        #region //ctor with no params

        /// <summary>
        /// ctor with no params
        /// </summary>
        static IconfontHelper()
        {
            IntPtr memoryFont = IntPtr.Zero;
            byte[] buffer = null;
            buffer = ReadFontFileFromResource("WinformUseIconfont.ttf.FontAwesome.ttf");
            if (buffer != null )
            {
                memoryFont = Marshal.AllocCoTaskMem(buffer.Length);
                Marshal.Copy(buffer, 0, memoryFont, buffer.Length);
                AwesomeFontCollection.AddMemoryFont(memoryFont, buffer.Length);
            }
            else
            {
                throw new Exception("AwesomeFont font file not found");
            }

            buffer = ReadFontFileFromResource("WinformUseIconfont.ttf.ElegantIcons.ttf");
            if (buffer != null)
            {
                memoryFont = Marshal.AllocCoTaskMem(buffer.Length);
                Marshal.Copy(buffer, 0, memoryFont, buffer.Length);
                ElegantFontCollection.AddMemoryFont(memoryFont, buffer.Length);
            }
            else
            {
                throw new Exception("ElegantFont font file not found");
            }
        }

        #endregion

        #region //get icon

        /// <summary>
        /// get icon
        /// </summary>
        /// <param name="iconText">Font icon hex code</param>
        /// <returns></returns>
        public static Icon GetIcon(int iconText, IconfontEnum iconfontEnum)
        {
            Bitmap bmp = GetImage(iconText, iconfontEnum);
            if (bmp != null)
            {
                return ToIcon(bmp, IconSize);
            }
            return null;
        }

        #endregion

        #region //get image

        /// <summary>
        /// get image
        /// </summary>
        /// <param name="iconText">Font icon hex code</param>
        /// <returns></returns>
        public static Bitmap GetImage(int iconText, IconfontEnum iconfontEnum)
        {
            //get icon really size
            Size size = GetIconSize(iconText, iconfontEnum);
            var bmp = new Bitmap(size.Width, size.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                //convert font code
                string unicode = char.ConvertFromUtf32(iconText);
                Font font = GetFont(iconfontEnum);

                //setting graphics
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                Rectangle rect = new Rectangle(new Point(0, 0), size);

                //Draw background color
                Brush backBrush = new SolidBrush(BackColer);
                g.FillRectangle(backBrush, rect);
                backBrush.Dispose();

                //draw icon
                Brush iconBrush = new SolidBrush(ForeColer);
                g.DrawString(unicode, font, iconBrush, new PointF(0, 0));

                iconBrush.Dispose();

                //draw icon to bmp
                g.DrawImage(bmp, 0, 0);
            }
            //resizer image
            bmp = Resizer(bmp, new Size(IconSize, IconSize),
                new Point((int)Math.Ceiling(IconSize * 0.04), (int)Math.Ceiling(IconSize * 0.05)), BackColer,
                BorderVisible, BorderColer);
            return bmp;
        }

        #endregion

        #region //get icon really size

        /// <summary>
        /// get icon really size
        /// </summary>
        /// <param name="iconText">Font icon hex code</param>
        /// <returns></returns>
        private static Size GetIconSize(int iconText, IconfontEnum iconfontEnum)
        {
            using (var bmp = new Bitmap(IconSize, IconSize))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    string unicode = char.ConvertFromUtf32(iconText);
                    Font font = GetFont(iconfontEnum);
                    SizeF size = g.MeasureString(unicode, font);
                    return size.ToSize();
                }
            }
        }

        #endregion

        #region //image resizer funtion

        /// <summary>
        /// image resizer funtion
        /// </summary>
        /// <param name="srcImage">source Bitmap object</param>
        /// <param name="destSize">dest image's size</param>
        /// <param name="offset">dest image offset point</param>
        /// <param name="backColor">dest image's background color,default value is <value>Color.Transparent</value></param>
        /// <param name="drawBorder">dest image's size</param>
        /// <param name="borderColor">dest image's border color,default value is Color.Gray</param>
        /// <returns></returns>
        private static Bitmap Resizer(Bitmap srcImage, Size destSize, Point offset, Color backColor, bool drawBorder,
            Color borderColor)
        {
            if (srcImage == null)
            {
                throw new ArgumentNullException("srcImage");
            }
            if (destSize == null)
            {
                throw new ArgumentNullException("destSize");
            }

            var bmp = new Bitmap(destSize.Width, destSize.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                Size os = srcImage.Size;

                int max = Math.Max(os.Height, os.Width);

                int width = max;
                int height = max;
                int x = (os.Width - max) / 2 - offset.X;
                int y = (os.Height - max) / 2 - offset.Y;

                //get dest & src image's draw rectangle
                Rectangle destRectangle = new Rectangle(new Point(0, 0), bmp.Size);
                Rectangle srcRectangle = new Rectangle(x, y, width, height);

                //fill background color
                Brush brush = new SolidBrush(backColor);
                g.FillRectangle(brush, destRectangle);
                brush.Dispose();

                //resizer image
                g.DrawImage(srcImage, destRectangle, srcRectangle, GraphicsUnit.Pixel);

                if (drawBorder)
                {
                    Pen pen = new Pen(borderColor);
                    g.DrawRectangle(pen, destRectangle);
                    pen.Dispose();
                }
                return bmp;
            }
        }

        #endregion

        #region //convert image to icon

        /// <summary>
        /// convert image to icon
        /// </summary>
        /// <param name="srcBitmap">The input stream</param>
        /// <param name="size">The size (16x16 px by default)</param>
        /// <returns>Icon</returns>
        private static Icon ToIcon(Bitmap srcBitmap, int size)
        {
            if (srcBitmap == null)
            {
                throw new ArgumentNullException("srcBitmap");
            }
            Icon icon = null;

            Bitmap bmp = new Bitmap(srcBitmap, new Size(size, size));

            // save the resized png into a memory stream for future use
            using (MemoryStream tmpStream = new MemoryStream())
            {
                bmp.Save(tmpStream, ImageFormat.Png);

                Stream outStraem = new MemoryStream();
                BinaryWriter writer = new BinaryWriter(outStraem);
                if (outStraem.Length <= 0)
                {
                    return null;
                }
                // 0-1 reserved, 0
                writer.Write((byte)0);
                writer.Write((byte)0);

                // 2-3 image type, 1 = icon, 2 = cursor
                writer.Write((short)1);

                // 4-5 number of images
                writer.Write((short)1);

                // image entry 1
                // 0 image width
                writer.Write((byte)size);
                // 1 image height
                writer.Write((byte)size);

                // 2 number of colors
                writer.Write((byte)0);

                // 3 reserved
                writer.Write((byte)0);

                // 4-5 color planes
                writer.Write((short)0);

                // 6-7 bits per pixel
                writer.Write((short)32);

                // 8-11 size of image data
                writer.Write((int)tmpStream.Length);

                // 12-15 offset of image data
                writer.Write((int)(6 + 16));

                // write image data
                // png data must contain the whole png data file
                writer.Write(tmpStream.ToArray());

                writer.Flush();
                writer.Seek(0, SeekOrigin.Begin);
                icon = new Icon(outStraem);
                outStraem.Dispose();
            }
            return icon;
        }

        #endregion

        #region //get font function
        /// <summary>
        /// get font function
        /// </summary>
        /// <returns></returns>
        private static Font GetFont(IconfontEnum iconfontEnum)
        {
            Font font;
            var size = IconSize * (3f / 4f);
            switch (iconfontEnum)
            {
                case IconfontEnum.AwesomeFont:
                    font = new Font(AwesomeFontCollection.Families[0], size, FontStyle.Regular, GraphicsUnit.Point);
                    break;
                case IconfontEnum.ElegantFont:
                    font = new Font(ElegantFontCollection.Families[0], size, FontStyle.Regular, GraphicsUnit.Point);
                    break;
                default:
                    throw new Exception("FontCollection No Data");
            }
            return font;
        }
        #endregion

        //#region //Type Dictionary
        ///// <summary>
        ///// FontAwesome.ttf V4.7
        ///// </summary>
        //public static Dictionary<string, int> AwesomeIconfontTypeDict = new Dictionary<string, int>()
        //{
        //    {"fa_500px", 0xf26e},

        //    {"fa_address_book", 0xf2b9},
        //    {"fa_address_book_o", 0xf2ba},
        //    {"fa_address_card", 0xf2bb},
        //    {"fa_address_card_o", 0xf2bc},
        //    {"fa_adjust", 0xf042},
        //    {"fa_adn", 0xf170},
        //    {"fa_align_center", 0xf037},
        //    {"fa_align_justify", 0xf039},
        //    {"fa_align_left", 0xf036},
        //    {"fa_align_right", 0xf038},
        //    {"fa_amazon", 0xf270},
        //    {"fa_ambulance", 0xf0f9},
        //    {"fa_american_sign_language_interpreting", 0xf2a3},
        //    {"fa_anchor", 0xf13d},
        //    {"fa_android", 0xf17b},
        //    {"fa_angellist", 0xf209},
        //    {"fa_angle_double_down", 0xf103},
        //    {"fa_angle_double_left", 0xf100},
        //    {"fa_angle_double_right", 0xf101},
        //    {"fa_angle_double_up", 0xf102},
        //    {"fa_angle_down", 0xf107},
        //    {"fa_angle_left", 0xf104},
        //    {"fa_angle_right", 0xf105},
        //    {"fa_angle_up", 0xf106},
        //    {"fa_apple", 0xf179},
        //    {"fa_archive", 0xf187},
        //    {"fa_area_chart", 0xf1fe},
        //    {"fa_arrow_circle_down", 0xf0ab},
        //    {"fa_arrow_circle_left", 0xf0a8},
        //    {"fa_arrow_circle_o_down", 0xf01a},
        //    {"fa_arrow_circle_o_left", 0xf190},
        //    {"fa_arrow_circle_o_right", 0xf18e},
        //    {"fa_arrow_circle_o_up", 0xf01b},
        //    {"fa_arrow_circle_right", 0xf0a9},
        //    {"fa_arrow_circle_up", 0xf0aa},
        //    {"fa_arrow_down", 0xf063},
        //    {"fa_arrow_left", 0xf060},
        //    {"fa_arrow_right", 0xf061},
        //    {"fa_arrow_up", 0xf062},
        //    {"fa_arrows", 0xf047},
        //    {"fa_arrows_alt", 0xf0b2},
        //    {"fa_arrows_h", 0xf07e},
        //    {"fa_arrows_v", 0xf07d},
        //    {"fa_asl_interpreting", 0xf2a3},
        //    {"fa_assistive_listening_systems", 0xf2a2},
        //    {"fa_asterisk", 0xf069},
        //    {"fa_at", 0xf1fa},
        //    {"fa_audio_description", 0xf29e},
        //    {"fa_automobile", 0xf1b9},

        //    {"fa_backward", 0xf04a},
        //    {"fa_balance_scale", 0xf24e},
        //    {"fa_ban", 0xf05e},
        //    {"fa_bandcamp", 0xf2d5},
        //    {"fa_bank", 0xf19c},
        //    {"fa_bar_chart", 0xf080},
        //    {"fa_bar_chart_o", 0xf080},
        //    {"fa_barcode", 0xf02a},
        //    {"fa_bars", 0xf0c9},
        //    {"fa_bath", 0xf2cd},
        //    {"fa_bathtub", 0xf2cd},
        //    {"fa_battery", 0xf240},
        //    {"fa_battery_0", 0xf244},
        //    {"fa_battery_1", 0xf243},
        //    {"fa_battery_2", 0xf242},
        //    {"fa_battery_3", 0xf241},
        //    {"fa_battery_4", 0xf240},
        //    {"fa_battery_empty", 0xf244},
        //    {"fa_battery_full", 0xf240},
        //    {"fa_battery_half", 0xf242},
        //    {"fa_battery_quarter", 0xf243},
        //    {"fa_battery_three_quarters", 0xf241},
        //    {"fa_bed", 0xf236},
        //    {"fa_beer", 0xf0fc},
        //    {"fa_behance", 0xf1b4},
        //    {"fa_behance_square", 0xf1b5},
        //    {"fa_bell", 0xf0f3},
        //    {"fa_bell_o", 0xf0a2},
        //    {"fa_bell_slash", 0xf1f6},
        //    {"fa_bell_slash_o", 0xf1f7},
        //    {"fa_bicycle", 0xf206},
        //    {"fa_binoculars", 0xf1e5},
        //    {"fa_birthday_cake", 0xf1fd},
        //    {"fa_bitbucket", 0xf171},
        //    {"fa_bitbucket_square", 0xf172},
        //    {"fa_bitcoin", 0xf15a},
        //    {"fa_black_tie", 0xf27e},
        //    {"fa_blind", 0xf29d},
        //    {"fa_bluetooth", 0xf293},
        //    {"fa_bluetooth_b", 0xf294},
        //    {"fa_bold", 0xf032},
        //    {"fa_bolt", 0xf0e7},
        //    {"fa_bomb", 0xf1e2},
        //    {"fa_book", 0xf02d},
        //    {"fa_bookmark", 0xf02e},
        //    {"fa_bookmark_o", 0xf097},
        //    {"fa_braille", 0xf2a1},
        //    {"fa_briefcase", 0xf0b1},
        //    {"fa_btc", 0xf15a},
        //    {"fa_bug", 0xf188},
        //    {"fa_building", 0xf1ad},
        //    {"fa_building_o", 0xf0f7},
        //    {"fa_bullhorn", 0xf0a1},
        //    {"fa_bullseye", 0xf140},
        //    {"fa_bus", 0xf207},
        //    {"fa_buysellads", 0xf20d},

        //    {"fa_cab", 0xf1ba},
        //    {"fa_calculator", 0xf1ec},
        //    {"fa_calendar", 0xf073},
        //    {"fa_calendar_check_o", 0xf274},
        //    {"fa_calendar_minus_o", 0xf272},
        //    {"fa_calendar_o", 0xf133},
        //    {"fa_calendar_plus_o", 0xf271},
        //    {"fa_calendar_times_o", 0xf273},
        //    {"fa_camera", 0xf030},
        //    {"fa_camera_retro", 0xf083},
        //    {"fa_car", 0xf1b9},
        //    {"fa_caret_down", 0xf0d7},
        //    {"fa_caret_left", 0xf0d9},
        //    {"fa_caret_right", 0xf0da},
        //    {"fa_caret_square_o_down", 0xf150},
        //    {"fa_caret_square_o_left", 0xf191},
        //    {"fa_caret_square_o_right", 0xf152},
        //    {"fa_caret_square_o_up", 0xf151},
        //    {"fa_caret_up", 0xf0d8},
        //    {"fa_cart_arrow_down", 0xf218},
        //    {"fa_cart_plus", 0xf217},
        //    {"fa_cc", 0xf20a},
        //    {"fa_cc_amex", 0xf1f3},
        //    {"fa_cc_diners_club", 0xf24c},
        //    {"fa_cc_discover", 0xf1f2},
        //    {"fa_cc_jcb", 0xf24b},
        //    {"fa_cc_mastercard", 0xf1f1},
        //    {"fa_cc_paypal", 0xf1f4},
        //    {"fa_cc_stripe", 0xf1f5},
        //    {"fa_cc_visa", 0xf1f0},
        //    {"fa_certificate", 0xf0a3},
        //    {"fa_chain", 0xf0c1},
        //    {"fa_chain_broken", 0xf127},
        //    {"fa_check", 0xf00c},
        //    {"fa_check_circle", 0xf058},
        //    {"fa_check_circle_o", 0xf05d},
        //    {"fa_check_square", 0xf14a},
        //    {"fa_check_square_o", 0xf046},
        //    {"fa_chevron_circle_down", 0xf13a},
        //    {"fa_chevron_circle_left", 0xf137},
        //    {"fa_chevron_circle_right", 0xf138},
        //    {"fa_chevron_circle_up", 0xf139},
        //    {"fa_chevron_down", 0xf078},
        //    {"fa_chevron_left", 0xf053},
        //    {"fa_chevron_right", 0xf054},
        //    {"fa_chevron_up", 0xf077},
        //    {"fa_child", 0xf1ae},
        //    {"fa_chrome", 0xf268},
        //    {"fa_circle", 0xf111},
        //    {"fa_circle_o", 0xf10c},
        //    {"fa_circle_o_notch", 0xf1ce},
        //    {"fa_circle_thin", 0xf1db},
        //    {"fa_clipboard", 0xf0ea},
        //    {"fa_clock_o", 0xf017},
        //    {"fa_clone", 0xf24d},
        //    {"fa_close", 0xf00d},
        //    {"fa_cloud", 0xf0c2},
        //    {"fa_cloud_download", 0xf0ed},
        //    {"fa_cloud_upload", 0xf0ee},
        //    {"fa_cny", 0xf157},
        //    {"fa_code", 0xf121},
        //    {"fa_code_fork", 0xf126},
        //    {"fa_codepen", 0xf1cb},
        //    {"fa_codiepie", 0xf284},
        //    {"fa_coffee", 0xf0f4},
        //    {"fa_cog", 0xf013},
        //    {"fa_cogs", 0xf085},
        //    {"fa_columns", 0xf0db},
        //    {"fa_comment", 0xf075},
        //    {"fa_comment_o", 0xf0e5},
        //    {"fa_commenting", 0xf27a},
        //    {"fa_commenting_o", 0xf27b},
        //    {"fa_comments", 0xf086},
        //    {"fa_comments_o", 0xf0e6},
        //    {"fa_compass", 0xf14e},
        //    {"fa_compress", 0xf066},
        //    {"fa_connectdevelop", 0xf20e},
        //    {"fa_contao", 0xf26d},
        //    {"fa_copy", 0xf0c5},
        //    {"fa_copyright", 0xf1f9},
        //    {"fa_creative_commons", 0xf25e},
        //    {"fa_credit_card", 0xf09d},
        //    {"fa_credit_card_alt", 0xf283},
        //    {"fa_crop", 0xf125},
        //    {"fa_crosshairs", 0xf05b},
        //    {"fa_css3", 0xf13c},
        //    {"fa_cube", 0xf1b2},
        //    {"fa_cubes", 0xf1b3},
        //    {"fa_cut", 0xf0c4},
        //    {"fa_cutlery", 0xf0f5},

        //    {"fa_dashboard", 0xf0e4},
        //    {"fa_dashcube", 0xf210},
        //    {"fa_database", 0xf1c0},
        //    {"fa_deaf", 0xf2a4},
        //    {"fa_deafness", 0xf2a4},
        //    {"fa_dedent", 0xf03b},
        //    {"fa_delicious", 0xf1a5},
        //    {"fa_desktop", 0xf108},
        //    {"fa_deviantart", 0xf1bd},
        //    {"fa_diamond", 0xf219},
        //    {"fa_digg", 0xf1a6},
        //    {"fa_dollar", 0xf155},
        //    {"fa_dot_circle_o", 0xf192},
        //    {"fa_download", 0xf019},
        //    {"fa_dribbble", 0xf17d},
        //    {"fa_drivers_license", 0xf2c2},
        //    {"fa_drivers_license_o", 0xf2c3},
        //    {"fa_dropbox", 0xf16b},
        //    {"fa_drupal", 0xf1a9},

        //    {"fa_edge", 0xf282},
        //    {"fa_edit", 0xf044},
        //    {"fa_eercast", 0xf2da},
        //    {"fa_eject", 0xf052},
        //    {"fa_ellipsis_h", 0xf141},
        //    {"fa_ellipsis_v", 0xf142},
        //    {"fa_empire", 0xf1d1},
        //    {"fa_envelope", 0xf0e0},
        //    {"fa_envelope_o", 0xf003},
        //    {"fa_envelope_open", 0xf2b6},
        //    {"fa_envelope_open_o", 0xf2b7},
        //    {"fa_envelope_square", 0xf199},
        //    {"fa_envira", 0xf299},
        //    {"fa_eraser", 0xf12d},
        //    {"fa_etsy", 0xf2d7},
        //    {"fa_eur", 0xf153},
        //    {"fa_euro", 0xf153},
        //    {"fa_exchange", 0xf0ec},
        //    {"fa_exclamation", 0xf12a},
        //    {"fa_exclamation_circle", 0xf06a},
        //    {"fa_exclamation_triangle", 0xf071},
        //    {"fa_expand", 0xf065},
        //    {"fa_expeditedssl", 0xf23e},
        //    {"fa_external_link", 0xf08e},
        //    {"fa_external_link_square", 0xf14c},
        //    {"fa_eye", 0xf06e},
        //    {"fa_eye_slash", 0xf070},
        //    {"fa_eyedropper", 0xf1fb},

        //    {"fa_fa", 0xf2b4},
        //    {"fa_facebook", 0xf09a},
        //    {"fa_facebook_f", 0xf09a},
        //    {"fa_facebook_official", 0xf230},
        //    {"fa_facebook_square", 0xf082},
        //    {"fa_fast_backward", 0xf049},
        //    {"fa_fast_forward", 0xf050},
        //    {"fa_fax", 0xf1ac},
        //    {"fa_feed", 0xf09e},
        //    {"fa_female", 0xf182},
        //    {"fa_fighter_jet", 0xf0fb},
        //    {"fa_file", 0xf15b},
        //    {"fa_file_archive_o", 0xf1c6},
        //    {"fa_file_audio_o", 0xf1c7},
        //    {"fa_file_code_o", 0xf1c9},
        //    {"fa_file_excel_o", 0xf1c3},
        //    {"fa_file_image_o", 0xf1c5},
        //    {"fa_file_movie_o", 0xf1c8},
        //    {"fa_file_o", 0xf016},
        //    {"fa_file_pdf_o", 0xf1c1},
        //    {"fa_file_photo_o", 0xf1c5},
        //    {"fa_file_picture_o", 0xf1c5},
        //    {"fa_file_powerpoint_o", 0xf1c4},
        //    {"fa_file_sound_o", 0xf1c7},
        //    {"fa_file_text", 0xf15c},
        //    {"fa_file_text_o", 0xf0f6},
        //    {"fa_file_video_o", 0xf1c8},
        //    {"fa_file_word_o", 0xf1c2},
        //    {"fa_file_zip_o", 0xf1c6},
        //    {"fa_files_o", 0xf0c5},
        //    {"fa_film", 0xf008},
        //    {"fa_filter", 0xf0b0},
        //    {"fa_fire", 0xf06d},
        //    {"fa_fire_extinguisher", 0xf134},
        //    {"fa_firefox", 0xf269},
        //    {"fa_first_order", 0xf2b0},
        //    {"fa_flag", 0xf024},
        //    {"fa_flag_checkered", 0xf11e},
        //    {"fa_flag_o", 0xf11d},
        //    {"fa_flash", 0xf0e7},
        //    {"fa_flask", 0xf0c3},
        //    {"fa_flickr", 0xf16e},
        //    {"fa_floppy_o", 0xf0c7},
        //    {"fa_folder", 0xf07b},
        //    {"fa_folder_o", 0xf114},
        //    {"fa_folder_open", 0xf07c},
        //    {"fa_folder_open_o", 0xf115},
        //    {"fa_font", 0xf031},
        //    {"fa_font_awesome", 0xf2b4},
        //    {"fa_fonticons", 0xf280},
        //    {"fa_fort_awesome", 0xf286},
        //    {"fa_forumbee", 0xf211},
        //    {"fa_forward", 0xf04e},
        //    {"fa_foursquare", 0xf180},
        //    {"fa_free_code_camp", 0xf2c5},
        //    {"fa_frown_o", 0xf119},
        //    {"fa_futbol_o", 0xf1e3},

        //    {"fa_gamepad", 0xf11b},
        //    {"fa_gavel", 0xf0e3},
        //    {"fa_gbp", 0xf154},
        //    {"fa_ge", 0xf1d1},
        //    {"fa_gear", 0xf013},
        //    {"fa_gears", 0xf085},
        //    {"fa_genderless", 0xf22d},
        //    {"fa_get_pocket", 0xf265},
        //    {"fa_gg", 0xf260},
        //    {"fa_gg_circle", 0xf261},
        //    {"fa_gift", 0xf06b},
        //    {"fa_git", 0xf1d3},
        //    {"fa_git_square", 0xf1d2},
        //    {"fa_github", 0xf09b},
        //    {"fa_github_alt", 0xf113},
        //    {"fa_github_square", 0xf092},
        //    {"fa_gitlab", 0xf296},
        //    {"fa_gittip", 0xf184},
        //    {"fa_glass", 0xf000},
        //    {"fa_glide", 0xf2a5},
        //    {"fa_glide_g", 0xf2a6},
        //    {"fa_globe", 0xf0ac},
        //    {"fa_google", 0xf1a0},
        //    {"fa_google_plus", 0xf0d5},
        //    {"fa_google_plus_circle", 0xf2b3},
        //    {"fa_google_plus_official", 0xf2b3},
        //    {"fa_google_plus_square", 0xf0d4},
        //    {"fa_google_wallet", 0xf1ee},
        //    {"fa_graduation_cap", 0xf19d},
        //    {"fa_gratipay", 0xf184},
        //    {"fa_grav", 0xf2d6},
        //    {"fa_group", 0xf0c0},

        //    {"fa_h_square", 0xf0fd},
        //    {"fa_hacker_news", 0xf1d4},
        //    {"fa_hand_grab_o", 0xf255},
        //    {"fa_hand_lizard_o", 0xf258},
        //    {"fa_hand_o_down", 0xf0a7},
        //    {"fa_hand_o_left", 0xf0a5},
        //    {"fa_hand_o_right", 0xf0a4},
        //    {"fa_hand_o_up", 0xf0a6},
        //    {"fa_hand_paper_o", 0xf256},
        //    {"fa_hand_peace_o", 0xf25b},
        //    {"fa_hand_pointer_o", 0xf25a},
        //    {"fa_hand_rock_o", 0xf255},
        //    {"fa_hand_scissors_o", 0xf257},
        //    {"fa_hand_spock_o", 0xf259},
        //    {"fa_hand_stop_o", 0xf256},
        //    {"fa_handshake_o", 0xf2b5},
        //    {"fa_hard_of_hearing", 0xf2a4},
        //    {"fa_hashtag", 0xf292},
        //    {"fa_hdd_o", 0xf0a0},
        //    {"fa_header", 0xf1dc},
        //    {"fa_headphones", 0xf025},
        //    {"fa_heart", 0xf004},
        //    {"fa_heart_o", 0xf08a},
        //    {"fa_heartbeat", 0xf21e},
        //    {"fa_history", 0xf1da},
        //    {"fa_home", 0xf015},
        //    {"fa_hospital_o", 0xf0f8},
        //    {"fa_hotel", 0xf236},
        //    {"fa_hourglass", 0xf254},
        //    {"fa_hourglass_1", 0xf251},
        //    {"fa_hourglass_2", 0xf252},
        //    {"fa_hourglass_3", 0xf253},
        //    {"fa_hourglass_end", 0xf253},
        //    {"fa_hourglass_half", 0xf252},
        //    {"fa_hourglass_o", 0xf250},
        //    {"fa_hourglass_start", 0xf251},
        //    {"fa_houzz", 0xf27c},
        //    {"fa_html5", 0xf13b},

        //    {"fa_i_cursor", 0xf246},
        //    {"fa_id_badge", 0xf2c1},
        //    {"fa_id_card", 0xf2c2},
        //    {"fa_id_card_o", 0xf2c3},
        //    {"fa_ils", 0xf20b},
        //    {"fa_image", 0xf03e},
        //    {"fa_imdb", 0xf2d8},
        //    {"fa_inbox", 0xf01c},
        //    {"fa_indent", 0xf03c},
        //    {"fa_industry", 0xf275},
        //    {"fa_info", 0xf129},
        //    {"fa_info_circle", 0xf05a},
        //    {"fa_inr", 0xf156},
        //    {"fa_instagram", 0xf16d},
        //    {"fa_institution", 0xf19c},
        //    {"fa_internet_explorer", 0xf26b},
        //    {"fa_intersex", 0xf224},
        //    {"fa_ioxhost", 0xf208},
        //    {"fa_italic", 0xf033},

        //    {"fa_joomla", 0xf1aa},
        //    {"fa_jpy", 0xf157},
        //    {"fa_jsfiddle", 0xf1cc},

        //    {"fa_key", 0xf084},
        //    {"fa_keyboard_o", 0xf11c},
        //    {"fa_krw", 0xf159},

        //    {"fa_language", 0xf1ab},
        //    {"fa_laptop", 0xf109},
        //    {"fa_lastfm", 0xf202},
        //    {"fa_lastfm_square", 0xf203},
        //    {"fa_leaf", 0xf06c},
        //    {"fa_leanpub", 0xf212},
        //    {"fa_legal", 0xf0e3},
        //    {"fa_lemon_o", 0xf094},
        //    {"fa_level_down", 0xf149},
        //    {"fa_level_up", 0xf148},
        //    {"fa_life_bouy", 0xf1cd},
        //    {"fa_life_buoy", 0xf1cd},
        //    {"fa_life_ring", 0xf1cd},
        //    {"fa_life_saver", 0xf1cd},
        //    {"fa_lightbulb_o", 0xf0eb},
        //    {"fa_line_chart", 0xf201},
        //    {"fa_link", 0xf0c1},
        //    {"fa_linkedin", 0xf0e1},
        //    {"fa_linkedin_square", 0xf08c},
        //    {"fa_linode", 0xf2b8},
        //    {"fa_linux", 0xf17c},
        //    {"fa_list", 0xf03a},
        //    {"fa_list_alt", 0xf022},
        //    {"fa_list_ol", 0xf0cb},
        //    {"fa_list_ul", 0xf0ca},
        //    {"fa_location_arrow", 0xf124},
        //    {"fa_lock", 0xf023},
        //    {"fa_long_arrow_down", 0xf175},
        //    {"fa_long_arrow_left", 0xf177},
        //    {"fa_long_arrow_right", 0xf178},
        //    {"fa_long_arrow_up", 0xf176},
        //    {"fa_low_vision", 0xf2a8},

        //    {"fa_magic", 0xf0d0},
        //    {"fa_magnet", 0xf076},
        //    {"fa_mail_forward", 0xf064},
        //    {"fa_mail_reply", 0xf112},
        //    {"fa_mail_reply_all", 0xf122},
        //    {"fa_male", 0xf183},
        //    {"fa_map", 0xf279},
        //    {"fa_map_marker", 0xf041},
        //    {"fa_map_o", 0xf278},
        //    {"fa_map_pin", 0xf276},
        //    {"fa_map_signs", 0xf277},
        //    {"fa_mars", 0xf222},
        //    {"fa_mars_double", 0xf227},
        //    {"fa_mars_stroke", 0xf229},
        //    {"fa_mars_stroke_h", 0xf22b},
        //    {"fa_mars_stroke_v", 0xf22a},
        //    {"fa_maxcdn", 0xf136},
        //    {"fa_meanpath", 0xf20c},
        //    {"fa_medium", 0xf23a},
        //    {"fa_medkit", 0xf0fa},
        //    {"fa_meetup", 0xf2e0},
        //    {"fa_meh_o", 0xf11a},
        //    {"fa_mercury", 0xf223},
        //    {"fa_microchip", 0xf2db},
        //    {"fa_microphone", 0xf130},
        //    {"fa_microphone_slash", 0xf131},
        //    {"fa_minus", 0xf068},
        //    {"fa_minus_circle", 0xf056},
        //    {"fa_minus_square", 0xf146},
        //    {"fa_minus_square_o", 0xf147},
        //    {"fa_mixcloud", 0xf289},
        //    {"fa_mobile", 0xf10b},
        //    {"fa_mobile_phone", 0xf10b},
        //    {"fa_modx", 0xf285},
        //    {"fa_money", 0xf0d6},
        //    {"fa_moon_o", 0xf186},
        //    {"fa_mortar_board", 0xf19d},
        //    {"fa_motorcycle", 0xf21c},
        //    {"fa_mouse_pointer", 0xf245},
        //    {"fa_music", 0xf001},

        //    {"fa_navicon", 0xf0c9},
        //    {"fa_neuter", 0xf22c},
        //    {"fa_newspaper_o", 0xf1ea},

        //    {"fa_object_group", 0xf247},
        //    {"fa_object_ungroup", 0xf248},
        //    {"fa_odnoklassniki", 0xf263},
        //    {"fa_odnoklassniki_square", 0xf264},
        //    {"fa_opencart", 0xf23d},
        //    {"fa_openid", 0xf19b},
        //    {"fa_opera", 0xf26a},
        //    {"fa_optin_monster", 0xf23c},
        //    {"fa_outdent", 0xf03b},

        //    {"fa_pagelines", 0xf18c},
        //    {"fa_paint_brush", 0xf1fc},
        //    {"fa_paper_plane", 0xf1d8},
        //    {"fa_paper_plane_o", 0xf1d9},
        //    {"fa_paperclip", 0xf0c6},
        //    {"fa_paragraph", 0xf1dd},
        //    {"fa_paste", 0xf0ea},
        //    {"fa_pause", 0xf04c},
        //    {"fa_pause_circle", 0xf28b},
        //    {"fa_pause_circle_o", 0xf28c},
        //    {"fa_paw", 0xf1b0},
        //    {"fa_paypal", 0xf1ed},
        //    {"fa_pencil", 0xf040},
        //    {"fa_pencil_square", 0xf14b},
        //    {"fa_pencil_square_o", 0xf044},
        //    {"fa_percent", 0xf295},
        //    {"fa_phone", 0xf095},
        //    {"fa_phone_square", 0xf098},
        //    {"fa_photo", 0xf03e},
        //    {"fa_picture_o", 0xf03e},
        //    {"fa_pie_chart", 0xf200},
        //    {"fa_pied_piper", 0xf2ae},
        //    {"fa_pied_piper_alt", 0xf1a8},
        //    {"fa_pied_piper_pp", 0xf1a7},
        //    {"fa_pinterest", 0xf0d2},
        //    {"fa_pinterest_p", 0xf231},
        //    {"fa_pinterest_square", 0xf0d3},
        //    {"fa_plane", 0xf072},
        //    {"fa_play", 0xf04b},
        //    {"fa_play_circle", 0xf144},
        //    {"fa_play_circle_o", 0xf01d},
        //    {"fa_plug", 0xf1e6},
        //    {"fa_plus", 0xf067},
        //    {"fa_plus_circle", 0xf055},
        //    {"fa_plus_square", 0xf0fe},
        //    {"fa_plus_square_o", 0xf196},
        //    {"fa_podcast", 0xf2ce},
        //    {"fa_power_off", 0xf011},
        //    {"fa_print", 0xf02f},
        //    {"fa_product_hunt", 0xf288},
        //    {"fa_puzzle_piece", 0xf12e},

        //    {"fa_qq", 0xf1d6},
        //    {"fa_qrcode", 0xf029},
        //    {"fa_question", 0xf128},
        //    {"fa_question_circle", 0xf059},
        //    {"fa_question_circle_o", 0xf29c},
        //    {"fa_quora", 0xf2c4},
        //    {"fa_quote_left", 0xf10d},
        //    {"fa_quote_right", 0xf10e},

        //    {"fa_ra", 0xf1d0},
        //    {"fa_random", 0xf074},
        //    {"fa_ravelry", 0xf2d9},
        //    {"fa_rebel", 0xf1d0},
        //    {"fa_recycle", 0xf1b8},
        //    {"fa_reddit", 0xf1a1},
        //    {"fa_reddit_alien", 0xf281},
        //    {"fa_reddit_square", 0xf1a2},
        //    {"fa_refresh", 0xf021},
        //    {"fa_registered", 0xf25d},
        //    {"fa_remove", 0xf00d},
        //    {"fa_renren", 0xf18b},
        //    {"fa_reorder", 0xf0c9},
        //    {"fa_repeat", 0xf01e},
        //    {"fa_reply", 0xf112},
        //    {"fa_reply_all", 0xf122},
        //    {"fa_resistance", 0xf1d0},
        //    {"fa_retweet", 0xf079},
        //    {"fa_rmb", 0xf157},
        //    {"fa_road", 0xf018},
        //    {"fa_rocket", 0xf135},
        //    {"fa_rotate_left", 0xf0e2},
        //    {"fa_rotate_right", 0xf01e},
        //    {"fa_rouble", 0xf158},
        //    {"fa_rss", 0xf09e},
        //    {"fa_rss_square", 0xf143},
        //    {"fa_rub", 0xf158},
        //    {"fa_ruble", 0xf158},
        //    {"fa_rupee", 0xf156},

        //    {"fa_s15", 0xf2cd},
        //    {"fa_safari", 0xf267},
        //    {"fa_save", 0xf0c7},
        //    {"fa_scissors", 0xf0c4},
        //    {"fa_scribd", 0xf28a},
        //    {"fa_search", 0xf002},
        //    {"fa_search_minus", 0xf010},
        //    {"fa_search_plus", 0xf00e},
        //    {"fa_sellsy", 0xf213},
        //    {"fa_send", 0xf1d8},
        //    {"fa_send_o", 0xf1d9},
        //    {"fa_server", 0xf233},
        //    {"fa_share", 0xf064},
        //    {"fa_share_alt", 0xf1e0},
        //    {"fa_share_alt_square", 0xf1e1},
        //    {"fa_share_square", 0xf14d},
        //    {"fa_share_square_o", 0xf045},
        //    {"fa_shekel", 0xf20b},
        //    {"fa_sheqel", 0xf20b},
        //    {"fa_shield", 0xf132},
        //    {"fa_ship", 0xf21a},
        //    {"fa_shirtsinbulk", 0xf214},
        //    {"fa_shopping_bag", 0xf290},
        //    {"fa_shopping_basket", 0xf291},
        //    {"fa_shopping_cart", 0xf07a},
        //    {"fa_shower", 0xf2cc},
        //    {"fa_sign_in", 0xf090},
        //    {"fa_sign_language", 0xf2a7},
        //    {"fa_sign_out", 0xf08b},
        //    {"fa_signal", 0xf012},
        //    {"fa_signing", 0xf2a7},
        //    {"fa_simplybuilt", 0xf215},
        //    {"fa_sitemap", 0xf0e8},
        //    {"fa_skyatlas", 0xf216},
        //    {"fa_skype", 0xf17e},
        //    {"fa_slack", 0xf198},
        //    {"fa_sliders", 0xf1de},
        //    {"fa_slideshare", 0xf1e7},
        //    {"fa_smile_o", 0xf118},
        //    {"fa_snapchat", 0xf2ab},
        //    {"fa_snapchat_ghost", 0xf2ac},
        //    {"fa_snapchat_square", 0xf2ad},
        //    {"fa_snowflake_o", 0xf2dc},
        //    {"fa_soccer_ball_o", 0xf1e3},
        //    {"fa_sort", 0xf0dc},
        //    {"fa_sort_alpha_asc", 0xf15d},
        //    {"fa_sort_alpha_desc", 0xf15e},
        //    {"fa_sort_amount_asc", 0xf160},
        //    {"fa_sort_amount_desc", 0xf161},
        //    {"fa_sort_asc", 0xf0de},
        //    {"fa_sort_desc", 0xf0dd},
        //    {"fa_sort_down", 0xf0dd},
        //    {"fa_sort_numeric_asc", 0xf162},
        //    {"fa_sort_numeric_desc", 0xf163},
        //    {"fa_sort_up", 0xf0de},
        //    {"fa_soundcloud", 0xf1be},
        //    {"fa_space_shuttle", 0xf197},
        //    {"fa_spinner", 0xf110},
        //    {"fa_spoon", 0xf1b1},
        //    {"fa_spotify", 0xf1bc},
        //    {"fa_square", 0xf0c8},
        //    {"fa_square_o", 0xf096},
        //    {"fa_stack_exchange", 0xf18d},
        //    {"fa_stack_overflow", 0xf16c},
        //    {"fa_star", 0xf005},
        //    {"fa_star_half", 0xf089},
        //    {"fa_star_half_empty", 0xf123},
        //    {"fa_star_half_full", 0xf123},
        //    {"fa_star_half_o", 0xf123},
        //    {"fa_star_o", 0xf006},
        //    {"fa_steam", 0xf1b6},
        //    {"fa_steam_square", 0xf1b7},
        //    {"fa_step_backward", 0xf048},
        //    {"fa_step_forward", 0xf051},
        //    {"fa_stethoscope", 0xf0f1},
        //    {"fa_sticky_note", 0xf249},
        //    {"fa_sticky_note_o", 0xf24a},
        //    {"fa_stop", 0xf04d},
        //    {"fa_stop_circle", 0xf28d},
        //    {"fa_stop_circle_o", 0xf28e},
        //    {"fa_street_view", 0xf21d},
        //    {"fa_strikethrough", 0xf0cc},
        //    {"fa_stumbleupon", 0xf1a4},
        //    {"fa_stumbleupon_circle", 0xf1a3},
        //    {"fa_subscript", 0xf12c},
        //    {"fa_subway", 0xf239},
        //    {"fa_suitcase", 0xf0f2},
        //    {"fa_sun_o", 0xf185},
        //    {"fa_superpowers", 0xf2dd},
        //    {"fa_superscript", 0xf12b},
        //    {"fa_support", 0xf1cd},

        //    {"fa_table", 0xf0ce},
        //    {"fa_tablet", 0xf10a},
        //    {"fa_tachometer", 0xf0e4},
        //    {"fa_tag", 0xf02b},
        //    {"fa_tags", 0xf02c},
        //    {"fa_tasks", 0xf0ae},
        //    {"fa_taxi", 0xf1ba},
        //    {"fa_telegram", 0xf2c6},
        //    {"fa_television", 0xf26c},
        //    {"fa_tencent_weibo", 0xf1d5},
        //    {"fa_terminal", 0xf120},
        //    {"fa_text_height", 0xf034},
        //    {"fa_text_width", 0xf035},
        //    {"fa_th", 0xf00a},
        //    {"fa_th_large", 0xf009},
        //    {"fa_th_list", 0xf00b},
        //    {"fa_themeisle", 0xf2b2},
        //    {"fa_thermometer", 0xf2c7},
        //    {"fa_thermometer_0", 0xf2cb},
        //    {"fa_thermometer_1", 0xf2ca},
        //    {"fa_thermometer_2", 0xf2c9},
        //    {"fa_thermometer_3", 0xf2c8},
        //    {"fa_thermometer_4", 0xf2c7},
        //    {"fa_thermometer_empty", 0xf2cb},
        //    {"fa_thermometer_full", 0xf2c7},
        //    {"fa_thermometer_half", 0xf2c9},
        //    {"fa_thermometer_quarter", 0xf2ca},
        //    {"fa_thermometer_three_quarters", 0xf2c8},
        //    {"fa_thumb_tack", 0xf08d},
        //    {"fa_thumbs_down", 0xf165},
        //    {"fa_thumbs_o_down", 0xf088},
        //    {"fa_thumbs_o_up", 0xf087},
        //    {"fa_thumbs_up", 0xf164},
        //    {"fa_ticket", 0xf145},
        //    {"fa_times", 0xf00d},
        //    {"fa_times_circle", 0xf057},
        //    {"fa_times_circle_o", 0xf05c},
        //    {"fa_times_rectangle", 0xf2d3},
        //    {"fa_times_rectangle_o", 0xf2d4},
        //    {"fa_tint", 0xf043},
        //    {"fa_toggle_down", 0xf150},
        //    {"fa_toggle_left", 0xf191},
        //    {"fa_toggle_off", 0xf204},
        //    {"fa_toggle_on", 0xf205},
        //    {"fa_toggle_right", 0xf152},
        //    {"fa_toggle_up", 0xf151},
        //    {"fa_trademark", 0xf25c},
        //    {"fa_train", 0xf238},
        //    {"fa_transgender", 0xf224},
        //    {"fa_transgender_alt", 0xf225},
        //    {"fa_trash", 0xf1f8},
        //    {"fa_trash_o", 0xf014},
        //    {"fa_tree", 0xf1bb},
        //    {"fa_trello", 0xf181},
        //    {"fa_tripadvisor", 0xf262},
        //    {"fa_trophy", 0xf091},
        //    {"fa_truck", 0xf0d1},
        //    {"fa_try", 0xf195},
        //    {"fa_tty", 0xf1e4},
        //    {"fa_tumblr", 0xf173},
        //    {"fa_tumblr_square", 0xf174},
        //    {"fa_turkish_lira", 0xf195},
        //    {"fa_tv", 0xf26c},
        //    {"fa_twitch", 0xf1e8},
        //    {"fa_twitter", 0xf099},
        //    {"fa_twitter_square", 0xf081},

        //    {"fa_umbrella", 0xf0e9},
        //    {"fa_underline", 0xf0cd},
        //    {"fa_undo", 0xf0e2},
        //    {"fa_universal_access", 0xf29a},
        //    {"fa_university", 0xf19c},
        //    {"fa_unlink", 0xf127},
        //    {"fa_unlock", 0xf09c},
        //    {"fa_unlock_alt", 0xf13e},
        //    {"fa_unsorted", 0xf0dc},
        //    {"fa_upload", 0xf093},
        //    {"fa_usb", 0xf287},
        //    {"fa_usd", 0xf155},
        //    {"fa_user", 0xf007},
        //    {"fa_user_circle", 0xf2bd},
        //    {"fa_user_circle_o", 0xf2be},
        //    {"fa_user_md", 0xf0f0},
        //    {"fa_user_o", 0xf2c0},
        //    {"fa_user_plus", 0xf234},
        //    {"fa_user_secret", 0xf21b},
        //    {"fa_user_times", 0xf235},
        //    {"fa_users", 0xf0c0},

        //    {"fa_vcard", 0xf2bb},
        //    {"fa_vcard_o", 0xf2bc},
        //    {"fa_venus", 0xf221},
        //    {"fa_venus_double", 0xf226},
        //    {"fa_venus_mars", 0xf228},
        //    {"fa_viacoin", 0xf237},
        //    {"fa_viadeo", 0xf2a9},
        //    {"fa_viadeo_square", 0xf2aa},
        //    {"fa_video_camera", 0xf03d},
        //    {"fa_vimeo", 0xf27d},
        //    {"fa_vimeo_square", 0xf194},
        //    {"fa_vine", 0xf1ca},
        //    {"fa_vk", 0xf189},
        //    {"fa_volume_control_phone", 0xf2a0},
        //    {"fa_volume_down", 0xf027},
        //    {"fa_volume_off", 0xf026},
        //    {"fa_volume_up", 0xf028},

        //    {"fa_warning", 0xf071},
        //    {"fa_wechat", 0xf1d7},
        //    {"fa_weibo", 0xf18a},
        //    {"fa_weixin", 0xf1d7},
        //    {"fa_whatsapp", 0xf232},
        //    {"fa_wheelchair", 0xf193},
        //    {"fa_wheelchair_alt", 0xf29b},
        //    {"fa_wifi", 0xf1eb},
        //    {"fa_wikipedia_w", 0xf266},
        //    {"fa_window_close", 0xf2d3},
        //    {"fa_window_close_o", 0xf2d4},
        //    {"fa_window_maximize", 0xf2d0},
        //    {"fa_window_minimize", 0xf2d1},
        //    {"fa_window_restore", 0xf2d2},
        //    {"fa_windows", 0xf17a},
        //    {"fa_won", 0xf159},
        //    {"fa_wordpress", 0xf19a},
        //    {"fa_wpbeginner", 0xf297},
        //    {"fa_wpexplorer", 0xf2de},
        //    {"fa_wpforms", 0xf298},
        //    {"fa_wrench", 0xf0ad},

        //    {"fa_xing", 0xf168},
        //    {"fa_xing_square", 0xf169},

        //    {"fa_y_combinator", 0xf23b},
        //    {"fa_y_combinator_square", 0xf1d4},
        //    {"fa_yahoo", 0xf19e},
        //    {"fa_yc", 0xf23b},
        //    {"fa_yc_square", 0xf1d4},
        //    {"fa_yelp", 0xf1e9},
        //    {"fa_yen", 0xf157},
        //    {"fa_yoast", 0xf2b1},
        //    {"fa_youtube", 0xf167},
        //    {"fa_youtube_play", 0xf16a},
        //    {"fa_youtube_square", 0xf166}
        //};

        ///// <summary>
        ///// ElegantIcons.ttf V1.0
        ///// </summary>
        //public static Dictionary<string, int> ElegantIconfontTypeDict = new Dictionary<string, int>()
        //{
        //    {"arrow_up", 0x21},
        //    {"arrow_down", 0x22},
        //    {"arrow_left", 0x23},
        //    {"arrow_right", 0x24},
        //    {"arrow_left_up", 0x25},
        //    {"arrow_right_up", 0x26},
        //    {"arrow_right_down", 0x27},
        //    {"arrow_left_down", 0x28},
        //    {"arrow_up_down", 0x29},
        //    {"arrow_up_down_alt", 0x2a},
        //    {"arrow_left_right_alt", 0x2b},
        //    {"arrow_left_right", 0x2c},
        //    {"arrow_expand_alt2", 0x2d},
        //    {"arrow_expand_alt", 0x2e},
        //    {"arrow_condense", 0x2f},
        //    {"arrow_expand", 0x30},
        //    {"arrow_move", 0x31},
        //    {"arrow_carrot_up", 0x32},
        //    {"arrow_carrot_down", 0x33},
        //    {"arrow_carrot_left", 0x34},
        //    {"arrow_carrot_right", 0x35},
        //    {"arrow_carrot_2up", 0x36},
        //    {"arrow_carrot_2down", 0x37},
        //    {"arrow_carrot_2left", 0x38},
        //    {"arrow_carrot_2right", 0x39},
        //    {"arrow_carrot_up_alt2", 0x3a},
        //    {"arrow_carrot_down_alt2", 0x3b},
        //    {"arrow_carrot_left_alt2", 0x3c},
        //    {"arrow_carrot_right_alt2", 0x3d},
        //    {"arrow_carrot_2up_alt2", 0x3e},
        //    {"arrow_carrot_2down_alt2", 0x3f},
        //    {"arrow_carrot_2left_alt2", 0x40},
        //    {"arrow_carrot_2right_alt2", 0x41},
        //    {"arrow_triangle_up", 0x42},
        //    {"arrow_triangle_down", 0x43},
        //    {"arrow_triangle_left", 0x44},
        //    {"arrow_triangle_right", 0x45},
        //    {"arrow_triangle_up_alt2", 0x46},
        //    {"arrow_triangle_down_alt2", 0x47},
        //    {"arrow_triangle_left_alt2", 0x48},
        //    {"arrow_triangle_right_alt2", 0x49},
        //    {"arrow_back", 0x4a},
        //    {"icon_minus_06", 0x4b},
        //    {"icon_plus", 0x4c},
        //    {"icon_close", 0x4d},
        //    {"icon_check", 0x4e},
        //    {"icon_minus_alt2", 0x4f},
        //    {"icon_plus_alt2", 0x50},
        //    {"icon_close_alt2", 0x51},
        //    {"icon_check_alt2", 0x52},
        //    {"icon_zoom_out_alt", 0x53},
        //    {"icon_zoom_in_alt", 0x54},
        //    {"icon_search", 0x55},
        //    {"icon_box_empty", 0x56},
        //    {"icon_box_selected", 0x57},
        //    {"icon_minus_box", 0x58},
        //    {"icon_plus_box", 0x59},
        //    {"icon_box_checked", 0x5a},
        //    {"icon_circle_empty", 0x5b},
        //    {"icon_circle_slelected", 0x5c},
        //    {"icon_stop_alt2", 0x5d},
        //    {"icon_stop", 0x5e},
        //    {"icon_pause_alt2", 0x5f},
        //    {"icon_pause", 0x60},
        //    {"icon_menu", 0x61},
        //    {"icon_menu_square_alt2", 0x62},
        //    {"icon_menu_circle_alt2", 0x63},
        //    {"icon_ul", 0x64},
        //    {"icon_ol", 0x65},
        //    {"icon_adjust_horiz", 0x66},
        //    {"icon_adjust_vert", 0x67},
        //    {"icon_document_alt", 0x68},
        //    {"icon_documents_alt", 0x69},
        //    {"icon_pencil", 0x6a},
        //    {"icon_pencil_edit_alt", 0x6b},
        //    {"icon_pencil_edit", 0x6c},
        //    {"icon_folder_alt", 0x6d},
        //    {"icon_folder_open_alt", 0x6e},
        //    {"icon_folder_add_alt", 0x6f},
        //    {"icon_info_alt", 0x70},
        //    {"icon_error_oct_alt", 0x71},
        //    {"icon_error_circle_alt", 0x72},
        //    {"icon_error_triangle_alt", 0x73},
        //    {"icon_question_alt2", 0x74},
        //    {"icon_question", 0x75},
        //    {"icon_comment_alt", 0x76},
        //    {"icon_chat_alt", 0x77},
        //    {"icon_vol_mute_alt", 0x78},
        //    {"icon_volume_low_alt", 0x79},
        //    {"icon_volume_high_alt", 0x7a},
        //    {"icon_quotations", 0x7b},
        //    {"icon_quotations_alt2", 0x7c},
        //    {"icon_clock_alt", 0x7d},
        //    {"icon_lock_alt", 0x7e},
        //    {"icon_lock_open_alt", 0xe000},
        //    {"icon_key_alt", 0xe001},
        //    {"icon_cloud_alt", 0xe002},
        //    {"icon_cloud_upload_alt", 0xe003},
        //    {"icon_cloud_download_alt", 0xe004},
        //    {"icon_image", 0xe005},
        //    {"icon_images", 0xe006},
        //    {"icon_lightbulb_alt", 0xe007},
        //    {"icon_gift_alt", 0xe008},
        //    {"icon_house_alt", 0xe009},
        //    {"icon_genius", 0xe00a},
        //    {"icon_mobile", 0xe00b},
        //    {"icon_tablet", 0xe00c},
        //    {"icon_laptop", 0xe00d},
        //    {"icon_desktop", 0xe00e},
        //    {"icon_camera_alt", 0xe00f},
        //    {"icon_mail_alt", 0xe010},
        //    {"icon_cone_alt", 0xe011},
        //    {"icon_ribbon_alt", 0xe012},
        //    {"icon_bag_alt", 0xe013},
        //    {"icon_creditcard", 0xe014},
        //    {"icon_cart_alt", 0xe015},
        //    {"icon_paperclip", 0xe016},
        //    {"icon_tag_alt", 0xe017},
        //    {"icon_tags_alt", 0xe018},
        //    {"icon_trash_alt", 0xe019},
        //    {"icon_cursor_alt", 0xe01a},
        //    {"icon_mic_alt", 0xe01b},
        //    {"icon_compass_alt", 0xe01c},
        //    {"icon_pin_alt", 0xe01d},
        //    {"icon_pushpin_alt", 0xe01e},
        //    {"icon_map_alt", 0xe01f},
        //    {"icon_drawer_alt", 0xe020},
        //    {"icon_toolbox_alt", 0xe021},
        //    {"icon_book_alt", 0xe022},
        //    {"icon_calendar", 0xe023},
        //    {"icon_film", 0xe024},
        //    {"icon_table", 0xe025},
        //    {"icon_contacts_alt", 0xe026},
        //    {"icon_headphones", 0xe027},
        //    {"icon_lifesaver", 0xe028},
        //    {"icon_piechart", 0xe029},
        //    {"icon_refresh", 0xe02a},
        //    {"icon_link_alt", 0xe02b},
        //    {"icon_link", 0xe02c},
        //    {"icon_loading", 0xe02d},
        //    {"icon_blocked", 0xe02e},
        //    {"icon_archive_alt", 0xe02f},
        //    {"icon_heart_alt", 0xe030},
        //    {"icon_star_alt", 0xe031},
        //    {"icon_star_half_alt", 0xe032},
        //    {"icon_star", 0xe033},
        //    {"icon_star_half", 0xe034},
        //    {"icon_tools", 0xe035},
        //    {"icon_tool", 0xe036},
        //    {"icon_cog", 0xe037},
        //    {"icon_cogs", 0xe038},
        //    {"arrow_up_alt", 0xe039},
        //    {"arrow_down_alt", 0xe03a},
        //    {"arrow_left_alt", 0xe03b},
        //    {"arrow_right_alt", 0xe03c},
        //    {"arrow_left_up_alt", 0xe03d},
        //    {"arrow_right_up_alt", 0xe03e},
        //    {"arrow_right_down_alt", 0xe03f},
        //    {"arrow_left_down_alt", 0xe040},
        //    {"arrow_condense_alt", 0xe041},
        //    {"arrow_expand_alt3", 0xe042},
        //    {"arrow_carrot_up_alt", 0xe043},
        //    {"arrow_carrot_down_alt", 0xe044},
        //    {"arrow_carrot_left_alt", 0xe045},
        //    {"arrow_carrot_right_alt", 0xe046},
        //    {"arrow_carrot_2up_alt", 0xe047},
        //    {"arrow_carrot_2dwnn_alt", 0xe048},
        //    {"arrow_carrot_2left_alt", 0xe049},
        //    {"arrow_carrot_2right_alt", 0xe04a},
        //    {"arrow_triangle_up_alt", 0xe04b},
        //    {"arrow_triangle_down_alt", 0xe04c},
        //    {"arrow_triangle_left_alt", 0xe04d},
        //    {"arrow_triangle_right_alt", 0xe04e},
        //    {"icon_minus_alt", 0xe04f},
        //    {"icon_plus_alt", 0xe050},
        //    {"icon_close_alt", 0xe051},
        //    {"icon_check_alt", 0xe052},
        //    {"icon_zoom_out", 0xe053},
        //    {"icon_zoom_in", 0xe054},
        //    {"icon_stop_alt", 0xe055},
        //    {"icon_menu_square_alt", 0xe056},
        //    {"icon_menu_circle_alt", 0xe057},
        //    {"icon_document", 0xe058},
        //    {"icon_documents", 0xe059},
        //    {"icon_pencil_alt", 0xe05a},
        //    {"icon_folder", 0xe05b},
        //    {"icon_folder_open", 0xe05c},
        //    {"icon_folder_add", 0xe05d},
        //    {"icon_folder_upload", 0xe05e},
        //    {"icon_folder_download", 0xe05f},
        //    {"icon_info", 0xe060},
        //    {"icon_error_circle", 0xe061},
        //    {"icon_error_oct", 0xe062},
        //    {"icon_error_triangle", 0xe063},
        //    {"icon_question_alt", 0xe064},
        //    {"icon_comment", 0xe065},
        //    {"icon_chat", 0xe066},
        //    {"icon_vol_mute", 0xe067},
        //    {"icon_volume_low", 0xe068},
        //    {"icon_volume_high", 0xe069},
        //    {"icon_quotations_alt", 0xe06a},
        //    {"icon_clock", 0xe06b},
        //    {"icon_lock", 0xe06c},
        //    {"icon_lock_open", 0xe06d},
        //    {"icon_key", 0xe06e},
        //    {"icon_cloud", 0xe06f},
        //    {"icon_cloud_upload", 0xe070},
        //    {"icon_cloud_download", 0xe071},
        //    {"icon_lightbulb", 0xe072},
        //    {"icon_gift", 0xe073},
        //    {"icon_house", 0xe074},
        //    {"icon_camera", 0xe075},
        //    {"icon_mail", 0xe076},
        //    {"icon_cone", 0xe077},
        //    {"icon_ribbon", 0xe078},
        //    {"icon_bag", 0xe079},
        //    {"icon_cart", 0xe07a},
        //    {"icon_tag", 0xe07b},
        //    {"icon_tags", 0xe07c},
        //    {"icon_trash", 0xe07d},
        //    {"icon_cursor", 0xe07e},
        //    {"icon_mic", 0xe07f},
        //    {"icon_compass", 0xe080},
        //    {"icon_pin", 0xe081},
        //    {"icon_pushpin", 0xe082},
        //    {"icon_map", 0xe083},
        //    {"icon_drawer", 0xe084},
        //    {"icon_toolbox", 0xe085},
        //    {"icon_book", 0xe086},
        //    {"icon_contacts", 0xe087},
        //    {"icon_archive", 0xe088},
        //    {"icon_heart", 0xe089},
        //    {"icon_profile", 0xe08a},
        //    {"icon_group", 0xe08b},
        //    {"icon_grid_2x2", 0xe08c},
        //    {"icon_grid_3x3", 0xe08d},
        //    {"icon_music", 0xe08e},
        //    {"icon_pause_alt", 0xe08f},
        //    {"icon_phone", 0xe090},
        //    {"icon_upload", 0xe091},
        //    {"icon_download", 0xe092},
        //    {"social_facebook", 0xe093},
        //    {"social_twitter", 0xe094},
        //    {"social_pinterest", 0xe095},
        //    {"social_googleplus", 0xe096},
        //    {"social_tumblr", 0xe097},
        //    {"social_tumbleupon", 0xe098},
        //    {"social_wordpress", 0xe099},
        //    {"social_instagram", 0xe09a},
        //    {"social_dribbble", 0xe09b},
        //    {"social_vimeo", 0xe09c},
        //    {"social_linkedin", 0xe09d},
        //    {"social_rss", 0xe09e},
        //    {"social_deviantart", 0xe09f},
        //    {"social_share", 0xe0a0},
        //    {"social_myspace", 0xe0a1},
        //    {"social_skype", 0xe0a2},
        //    {"social_youtube", 0xe0a3},
        //    {"social_picassa", 0xe0a4},
        //    {"social_googledrive", 0xe0a5},
        //    {"social_flickr", 0xe0a6},
        //    {"social_blogger", 0xe0a7},
        //    {"social_spotify", 0xe0a8},
        //    {"social_delicious", 0xe0a9},
        //    {"social_facebook_circle", 0xe0aa},
        //    {"social_twitter_circle", 0xe0ab},
        //    {"social_pinterest_circle", 0xe0ac},
        //    {"social_googleplus_circle", 0xe0ad},
        //    {"social_tumblr_circle", 0xe0ae},
        //    {"social_stumbleupon_circle", 0xe0af},
        //    {"social_wordpress_circle", 0xe0b0},
        //    {"social_instagram_circle", 0xe0b1},
        //    {"social_dribbble_circle", 0xe0b2},
        //    {"social_vimeo_circle", 0xe0b3},
        //    {"social_linkedin_circle", 0xe0b4},
        //    {"social_rss_circle", 0xe0b5},
        //    {"social_deviantart_circle", 0xe0b6},
        //    {"social_share_circle", 0xe0b7},
        //    {"social_myspace_circle", 0xe0b8},
        //    {"social_skype_circle", 0xe0b9},
        //    {"social_youtube_circle", 0xe0ba},
        //    {"social_picassa_circle", 0xe0bb},
        //    {"social_googledrive_alt2", 0xe0bc},
        //    {"social_flickr_circle", 0xe0bd},
        //    {"social_blogger_circle", 0xe0be},
        //    {"social_spotify_circle", 0xe0bf},
        //    {"social_delicious_circle", 0xe0c0},
        //    {"social_facebook_square", 0xe0c1},
        //    {"social_twitter_square", 0xe0c2},
        //    {"social_pinterest_square", 0xe0c3},
        //    {"social_googleplus_square", 0xe0c4},
        //    {"social_tumblr_square", 0xe0c5},
        //    {"social_stumbleupon_square", 0xe0c6},
        //    {"social_wordpress_square", 0xe0c7},
        //    {"social_instagram_square", 0xe0c8},
        //    {"social_dribbble_square", 0xe0c9},
        //    {"social_vimeo_square", 0xe0ca},
        //    {"social_linkedin_square", 0xe0cb},
        //    {"social_rss_square", 0xe0cc},
        //    {"social_deviantart_square", 0xe0cd},
        //    {"social_share_square", 0xe0ce},
        //    {"social_myspace_square", 0xe0cf},
        //    {"social_skype_square", 0xe0d0},
        //    {"social_youtube_square", 0xe0d1},
        //    {"social_picassa_square", 0xe0d2},
        //    {"social_googledrive_square", 0xe0d3},
        //    {"social_flickr_square", 0xe0d4},
        //    {"social_blogger_square", 0xe0d5},
        //    {"social_spotify_square", 0xe0d6},
        //    {"social_delicious_square", 0xe0d7},
        //    {"icon_printer", 0xe103},
        //    {"icon_calulator", 0xe0ee},
        //    {"icon_building", 0xe0ef},
        //    {"icon_floppy", 0xe0e8},
        //    {"icon_drive", 0xe0ea},
        //    {"icon_search_2", 0xe101},
        //    {"icon_id", 0xe107},
        //    {"icon_id_2", 0xe108},
        //    {"icon_puzzle", 0xe102},
        //    {"icon_like", 0xe106},
        //    {"icon_dislike", 0xe0eb},
        //    {"icon_mug", 0xe105},
        //    {"icon_currency", 0xe0ed},
        //    {"icon_wallet", 0xe100},
        //    {"icon_pens", 0xe104},
        //    {"icon_easel", 0xe0e9},
        //    {"icon_flowchart", 0xe109},
        //    {"icon_datareport", 0xe0ec},
        //    {"icon_briefcase", 0xe0fe},
        //    {"icon_shield", 0xe0f6},
        //    {"icon_percent", 0xe0fb},
        //    {"icon_globe", 0xe0e2},
        //    {"icon_globe_2", 0xe0e3},
        //    {"icon_target", 0xe0f5},
        //    {"icon_hourglass", 0xe0e1},
        //    {"icon_balance", 0xe0ff},
        //    {"icon_rook", 0xe0f8},
        //    {"icon_printer_alt", 0xe0fa},
        //    {"icon_calculator_alt", 0xe0e7},
        //    {"icon_building_alt", 0xe0fd},
        //    {"icon_floppy_alt", 0xe0e4},
        //    {"icon_drive_alt", 0xe0e5},
        //    {"icon_search_alt", 0xe0f7},
        //    {"icon_id_alt", 0xe0e0},
        //    {"icon_id_2_alt", 0xe0fc},
        //    {"icon_puzzle_alt", 0xe0f9},
        //    {"icon_like_alt", 0xe0dd},
        //    {"icon_dislike_alt", 0xe0f1},
        //    {"icon_mug_alt", 0xe0dc},
        //    {"icon_currency_alt", 0xe0f3},
        //    {"icon_wallet_alt", 0xe0d8},
        //    {"icon_pens_alt", 0xe0db},
        //    {"icon_easel_alt", 0xe0f0},
        //    {"icon_flowchart_alt", 0xe0df},
        //    {"icon_datareport_alt", 0xe0f2},
        //    {"icon_briefcase_alt", 0xe0f4},
        //    {"icon_shield_alt", 0xe0d9},
        //    {"icon_percent_alt", 0xe0da},
        //    {"icon_globe_alt", 0xe0de},
        //    {"icon_clipboard", 0xe0e6},
        //};
        //#endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="iconfontEnum"></param>
        /// <param name="iconText"></param>
        /// <param name="iconSize"></param>
        /// <param name="backColer"></param>
        /// <param name="borderColer"></param>
        /// <param name="foreColer"></param>
        /// <param name="borderVisible"></param>
        /// <returns></returns>
        public static Bitmap GetBitmap(IconfontEnum iconfontEnum, int iconText, int iconSize, Color backColer, Color foreColer)
        {

            IconSize = iconSize;
            BackColer = backColer;
            ForeColer = foreColer;
            BorderVisible = false;

            Bitmap bmp = GetImage(iconText, iconfontEnum);

            IconSize = _IconSize;
            BackColer = _BackColer;
            ForeColer = _ForeColer;
            BorderVisible = _BorderVisible;
            return bmp;
        }
    }
}
