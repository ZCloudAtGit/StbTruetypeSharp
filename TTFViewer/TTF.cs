using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MarshalHelper;
using stb_truetypeSharp;

namespace TTFViewer
{
    class TTF
    {
        public string Name { get; set; }

        public Bitmap PackedCharBitmap;

        public PackedChar[] PackedCharData;

        private byte[] _data;

        public bool LoadTTFFont(string path, string name)
        {
            //Read ttf file into byte array
            _data = File.ReadAllBytes(path);
            using (var ttf = new PinnedArray<byte>(_data))
            {
                //get pointer of the ttf file content
                var ttfBuffer = ttf.Pointer;
                //Initialize fontinfo
                FontInfo font = new FontInfo();
                STBTrueType.InitFont(ref font, ttfBuffer, STBTrueType.GetFontOffsetForIndex(ttfBuffer, 0));
            }

            Name = name;

            return true;
        }

        public bool CreatePackedGlyphMap(string charactersToPack, int fontSize, int width, int height)
        {
            width = 512;
            height = 512;
            var bitmapData = new byte[width * height];

            using (var ttf = new PinnedArray<byte>(_data))
            {
                //get pointer of the ttf file content
                var ttfBuffer = ttf.Pointer;

                //Begin pack
                PackContext pc = new PackContext();
                STBTrueType.PackBegin(ref pc, bitmapData, width, height, 0, 1, IntPtr.Zero);

                //Ref: https://github.com/nothings/stb/blob/bdef693b7cc89efb0c450b96a8ae4aecf27785c8/tests/test_truetype.c
                //allocate packed char buffer
                PackedCharData = new PackedChar[charactersToPack.Length];

                using (var pin_pdata = new PinnedArray<PackedChar>(PackedCharData))
                {
                    //get pointer of the pdata
                    var ptr_pdata = pin_pdata.Pointer;
                    PackRange[] vPackRange = new PackRange[charactersToPack.Length];
                    for (var i = 0; i < charactersToPack.Length; ++i)
                    {
                        //create a PackRange of one character
                        PackRange pr = new PackRange
                        {
                            chardata_for_range = IntPtr.Add(ptr_pdata, i*Marshal.SizeOf(typeof (PackedChar))),
                            first_unicode_char_in_range = charactersToPack[i] & 0xFFFF,
                            num_chars_in_range = 1,
                            font_size = fontSize
                        };
                        //add it to the range list
                        vPackRange[i] = pr;
                    }
                    //STBTrueType.PackSetOversampling(ref pc, 2, 2);
                    STBTrueType.PackFontRanges(ref pc, ttfBuffer, 0, vPackRange, vPackRange.Length);
                    STBTrueType.PackEnd(ref pc);
                }
            }

            PackedCharBitmap = CreateBitmapFromRawData(bitmapData, width, height);

            return true;
        }

        public Bitmap CreateGlyphForText(string text, float fontSize, int width, int height)
        {
            Bitmap bmp = new Bitmap(512, 512, PixelFormat.Format24bppRgb);

            //Draw characters to a bitmap by order
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            float x = 0, y = 0;
            for (var i = 0; i < text.Length; ++i)
            {
                AlignedQuad aq;
                //get character bitmaps in packed bitmap
                STBTrueType.GetPackedQuad(PackedCharData, width, height, text[i] - ' ', ref x, ref y, out aq, 0);
                var rectSrc = RectangleF.FromLTRB(aq.s0 * width, aq.t0 * height, aq.s1 * width, aq.t1 * height);
                var rectDest = RectangleF.FromLTRB(aq.x0, aq.y0, aq.x1, aq.y1);
                rectDest.Offset(x, y + fontSize);//ATTENTION! The offset of lineHeight(fontSize here) should be appended.
                g.DrawImage(PackedCharBitmap, rectDest, rectSrc, GraphicsUnit.Pixel);
            }
            g.Flush();
            return bmp;
        }

        static TTF()
        {
            PrepareCommonGlyphMapInfo();
        }

        private static Bitmap CreateBitmapFromRawData(Byte[] data, int width, int height)
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
                    var offset = y * bmpData.Stride + 3 * x;
                    rgbValues[offset] = (byte)(255 - alpha);//B
                    rgbValues[offset + 1] = (byte)(255 - alpha);//G
                    rgbValues[offset + 2] = (byte)(255 - alpha);//R
                }
            }
            Marshal.Copy(rgbValues, 0, ptr, rgbValues.Length);
            b.UnlockBits(bmpData);
            return b;
        }

        public static Dictionary<string, TTF> Common;

        //build packed characters' bitmap for ASCII/Chinese/Japanse/Korean characters etc.
        public static bool PrepareCommonGlyphMapInfo()
        {
            bool result = false;

            Common = new Dictionary<string, TTF>(3);

            TTF asciiTTF = new TTF();

            var assemblyDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var solutionDir = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(assemblyDir)));
            var ttfSampleDir = solutionDir + @"\FontSamples\";
            var ttfPath = ttfSampleDir + '\\' + "arial.ttf";
            result = asciiTTF.LoadTTFFont(ttfPath, "arial");
            if (!result)
            {
                Debug.WriteLine("LoadTTFFont arial.ttf failed.");
                return false;
            }

            var asciiCharacters = @" !""#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
            result = asciiTTF.CreatePackedGlyphMap(asciiCharacters, 32, 512, 512);
            if (!result)
            {
                Debug.WriteLine("CreatePackedGlyphMap for ASCII Characters failed.");
                return false;
            }

            Common["ASCII"] = asciiTTF;



            return true;
        }

    }
}
