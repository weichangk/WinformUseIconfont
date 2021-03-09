using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinformUseIconfont.Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void iconfontButton1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Iconfont AwesomeFont;
            //AwesomeFont = new Iconfont(ReadFontFileFromResource("WinformUseIconfont.ttf.FontAwesome.ttf"));
            //this.BackgroundImage = IconfontHelper.GetBitmap(UIIconfontEditor.iconfontEnum, 61543, 128, Color.Transparent, Color.Gray, Color.Black, false);
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

        private void iconfontPanel1_Click(object sender, EventArgs e)
        {

        }
    }
}
