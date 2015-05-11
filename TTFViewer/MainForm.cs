using System;
using System.IO;
using System.Windows.Forms;
using stb_truetypeSharp;
using MarshalHelper;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace TTFViewer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void ShowGlyphButton_Click(object sender, EventArgs e)
        {
            var text = codepointTextBox.Text;
            if (text.Length == 0) return;
            var codepoint = text[0];

            #region Init
            string assemblyDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string solution_dir = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(assemblyDir)));
            #endregion

            //Read ttf file into byte array
            byte[] ttfFileContent = File.ReadAllBytes(solution_dir + @"\FontSamples\SIMHEI.TTF");
            using (var ttf = new PinnedArray<byte>(ttfFileContent))
            {
                //get pointer of the ttf file content
                var ttf_buffer = ttf.Pointer;
                //Initialize fontinfo
                FontInfo font = new FontInfo();
                STBTrueType.InitFont(ref font, ttf_buffer, STBTrueType.GetFontOffsetForIndex(ttf_buffer, 0));
                //set pixel height of the bitmap
                float pixelHeight = 32.0f;
                //get vertical metrics of the font
                int ascent = 0, descent = 0, lineGap = 0;
                STBTrueType.GetFontVMetrics(font, ref ascent, ref descent, ref lineGap);
                //calculate the vertical scale
                float scaleY = pixelHeight / (ascent - descent);
                //get bitmap of the codepoint as well as its width and height
                int width = 0, height = 0;
                var bitmapData = STBTrueType.GetCodepointBitmap(font, 0f, scaleY, codepoint & 0xFFFF, ref width, ref height, null, null);
                //Create bitmap form the raw data
                var bitmap = CreateBitmapFromRawData(bitmapData, width, height);
                //Show the bitmap
                bitmapPictureBox.Image = bitmap;
            }
        }

        private Bitmap CreateBitmapFromRawData(Byte[] data, int width, int height)
        {
            var b = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            var stride = (width % 4 == 0) ? width : (width + (4 - width % 4));
            var BoundsRect = new Rectangle(0, 0, width, height);
            BitmapData bmpData = b.LockBits(BoundsRect,
                                            ImageLockMode.WriteOnly,
                                            b.PixelFormat);
            IntPtr ptr = bmpData.Scan0;
            var rgbValues = new byte[bmpData.Stride * height];
            // fill in rgbValues, e.g. with a for loop over an input array
            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    var alpha = data[y * width + x];//the alpha value of 
                    //NOTE: Add 3*x because offset should advance three bytes while x advance 1 unit.
                    var offset = y * bmpData.Stride + 3*x;
                    rgbValues[offset] = (byte)(255 - alpha);//B
                    rgbValues[offset + 1] = (byte)(255 - alpha);//G
                    rgbValues[offset + 2] = (byte)(255 - alpha);//R
                }
            }
            Marshal.Copy(rgbValues, 0, ptr, rgbValues.Length);
            b.UnlockBits(bmpData);
            return b;
        }
    }
}
