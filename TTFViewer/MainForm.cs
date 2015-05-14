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
        string assemblyDir;
        string solutionDir;
        string ttfSampleDir;

        int BuildType = 1;

        public MainForm()
        {
            InitializeComponent();

            assemblyDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            solutionDir = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(assemblyDir)));
            ttfSampleDir = solutionDir + @"\FontSamples\";

            string[] vSampleTTFFilePath = new string[]
            {
                "SIMHEI.TTF",
                "Windsong.ttf"
            };

            FontSelectorComboBox.Items.AddRange(vSampleTTFFilePath);
            FontSelectorComboBox.SelectedIndex = 0;
            FontSelectorComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        /// <summary>
        /// pixel height of the bitmap
        /// </summary>
        float pixelHeight = 32.0f;

        private void ShowGlyphButton_Click(object sender, EventArgs e)
        {
            byte[] bitmapData = null;
            int width = 0, height = 0;

            BuildType = BuildTypeTabControl.SelectedIndex;

            //Create bitmap form the raw data
            Bitmap bitmap = null;
            switch(BuildType)
            {
                default:
                case 0:
                    var text = codepointTextBox.Text;
                    if (text.Length == 0) return;
                    var codepoint = text[0];
                    bitmapData = CreateGlyph(codepoint, ref width, ref height);
                    break;
                case 1:
                    var firstChar = FirstCodepointTextBox.Text[0];
                    var charCount = (int)CodepointCountNumericUpDown.Value;
                    bitmapData = CreateGlyph(firstChar, charCount, ref width, ref height);
                    break;
            }
            bitmap = CreateBitmapFromRawData(bitmapData, width, height);
            
            //Show the bitmap
            bitmapPictureBox.Image = bitmap;
        }

        private byte[] CreateGlyph(char codepoint, ref int width, ref int height)
        {
            byte[] bitmapData = null;

            //Read ttf file into byte array
            byte[] ttfFileContent = File.ReadAllBytes(ttfSampleDir + '\\' + FontSelectorComboBox.SelectedItem as string);
            using (var ttf = new PinnedArray<byte>(ttfFileContent))
            {
                //get pointer of the ttf file content
                var ttf_buffer = ttf.Pointer;
                //Initialize fontinfo
                FontInfo font = new FontInfo();
                STBTrueType.InitFont(ref font, ttf_buffer, STBTrueType.GetFontOffsetForIndex(ttf_buffer, 0));
                //get vertical metrics of the font
                int ascent = 0, descent = 0, lineGap = 0;
                STBTrueType.GetFontVMetrics(font, ref ascent, ref descent, ref lineGap);
                //calculate the vertical scale
                float scaleY = pixelHeight / (ascent - descent);
                //get bitmap of the codepoint as well as its width and height
                bitmapData = STBTrueType.GetCodepointBitmap(font, 0f, scaleY, codepoint & 0xFFFF, ref width, ref height, null, null);
            }
            return bitmapData;
        }

        private byte[] CreateGlyph(char firstChar, int charCount, ref int width, ref int height)
        {
            byte[] bitmapData = null;

            //Read ttf file into byte array
            byte[] ttfFileContent = File.ReadAllBytes(ttfSampleDir + '\\' + FontSelectorComboBox.SelectedItem as string);
            using (var ttf = new PinnedArray<byte>(ttfFileContent))
            {
                //get pointer of the ttf file content
                var ttf_buffer = ttf.Pointer;
                //Initialize fontinfo
                FontInfo font = new FontInfo();
                STBTrueType.InitFont(ref font, ttf_buffer, STBTrueType.GetFontOffsetForIndex(ttf_buffer, 0));
                //set bitmap size
                const int BITMAP_W = 512;
                const int BITMAP_H = 512;
                //allocate bitmap buffer
                byte[] bitmapBuffer = new byte[BITMAP_W * BITMAP_W];
                BakedChar[] cdata = new BakedChar[charCount]; // ASCII 32..126 is 95 glyphs
                //bake bitmap for codepoint from firstChar to firstChar + charCount - 1
                STBTrueType.BakeFontBitmap(ttf_buffer, STBTrueType.GetFontOffsetForIndex(ttf_buffer, 0), pixelHeight, bitmapBuffer, BITMAP_W, BITMAP_H, firstChar, charCount, cdata); // no guarantee this fits!
                bitmapData = bitmapBuffer;
                width = BITMAP_W;
                height = BITMAP_H;
            }
            return bitmapData;
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

        private void FontHeightNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            pixelHeight = (float)FontHeightNumericUpDown.Value;
        }
    }
}
