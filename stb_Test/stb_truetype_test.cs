//#define DrawOutlineOnly //open this to draw outline only(ignore the depth)
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using stb_truetypeSharp;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text;
using MarshalHelper;

namespace stb_Test
{
    [TestClass]
    public class stb_truetype_test
    {
        [DllImport("kernel32", SetLastError = true)]
        static extern bool FreeLibrary(IntPtr hModule);

        /// <summary>
        /// Unload the native dll stb_truetype.dll
        /// </summary>
        /// <remarks>
        /// This is just a walkaround in that the dll won't be unloaded automatically
        /// in the unit test system of VS2013.
        /// </remarks>
        [AssemblyCleanup]
        public static void Teardown()
        {
            foreach (ProcessModule mod in Process.GetCurrentProcess().Modules)
            {
                if (mod.ModuleName == "stb_truetype.dll")
                {
                    FreeLibrary(mod.BaseAddress);
                }
            }
        }

        private void OpenFile(string path)
        {
            Process process = new Process();
            process.StartInfo.FileName = "notepad.exe";
            process.StartInfo.Arguments = path;
            process.Start();
            process.WaitForExit(1000);
        }

        /// <summary>
        /// Load a ttf file and print bitmap of codepoint 'A' in the ttf file as text. 
        /// </summary>
        /// <remarks>
        /// function used:
        /// InitFont
        /// GetFontOffsetForIndex
        /// GetFontVMetrics
        /// GetCodepointBitmap
        /// GetBakedQuad
        /// </remarks>
        [TestMethod]
        public void Test_BakeSingleCodepoint()
        {
            #region Init
            string assemblyDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string solution_dir = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(assemblyDir)));
            #endregion

            //Read ttf file into byte array
            byte[] ttfFileContent = File.ReadAllBytes(solution_dir + @"\FontSamples\Windsong.ttf");
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
                //get bitmap of one codepoint ‘A’ as well as its width and height
                int width = 0, height = 0;
                var bitmap = STBTrueType.GetCodepointBitmap(font, 0f, scaleY, 'A' & 0xFF, ref width, ref height, null, null);
                //output the bitmap to a text file
                WriteBitmapToFileAsText("testOuput.txt", height, width, bitmap);
                //Open the text file
                OpenFile("testOuput.txt");
            }
        }

        /// <summary>
        /// Load a ttf file and print bitmap of a range of codepoints in the ttf file as text. 
        /// </summary>
        /// <remarks>
        /// function used:
        /// InitFont
        /// GetFontOffsetForIndex
        /// BakeFontBitmap
        /// </remarks>
        [TestMethod]
        public void Test_BakeMultipleCodepoint()
        {
            #region Init
            string assemblyDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string solution_dir = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(assemblyDir)));
            #endregion

            //Read ttf file into byte array
            byte[] ttfFileContent = File.ReadAllBytes(solution_dir + @"\FontSamples\Windsong.ttf");
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
                BakedChar[] cdata = new BakedChar[256 * 2]; // ASCII 32..126 is 95 glyphs
                //bake bitmap for codepoint from 32 to 126
                STBTrueType.BakeFontBitmap(ttf_buffer, STBTrueType.GetFontOffsetForIndex(ttf_buffer, 0), 32.0f, bitmapBuffer, BITMAP_W, BITMAP_H, 32, 96, cdata); // no guarantee this fits!
                //output the bitmap to a text file
                WriteBitmapToFileAsText("testOuput.txt", BITMAP_H, BITMAP_W, bitmapBuffer);
                //Open the text file
                OpenFile("testOuput.txt");
            }
        }

        /// <summary>
        /// Load a ttf file and print bitmap of packed ranges of codepoints in the ttf file as text. 
        /// </summary>
        /// <remarks>
        /// function used:
        /// InitFont
        /// GetFontOffsetForIndex
        /// PackBegin
        /// PackFontRanges
        /// PackEnd
        /// </remarks>
        [TestMethod]
        public void Test_BakePackedCodepoint()
        {
            #region Init
            string assemblyDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string solution_dir = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(assemblyDir)));
            #endregion

            //Read ttf file into byte array
            byte[] ttfFileContent = File.ReadAllBytes(solution_dir + @"\FontSamples\Windsong.ttf");
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
                byte[] bitmapBuffer = new byte[BITMAP_W * BITMAP_H];
                using (var bitmapManaged = new PinnedArray<byte>(bitmapBuffer))
                {
                    //get pointer of the bitmap buffer
                    var temp_bitmap = bitmapManaged.Pointer;
                    //Initialize a pack context
                    PackContext pc = new PackContext();
                    STBTrueType.PackBegin(ref pc, temp_bitmap, BITMAP_W, BITMAP_H, 0, 1, IntPtr.Zero);
                    //allocate packed char buffer
                    PackedChar[] pdata = new PackedChar[256 * 2];
                    using (var pin_pdata = new PinnedArray<PackedChar>(pdata))
                    {
                        //get pointer of the bitmap buffer
                        var ptr_pdata = pin_pdata.Pointer;
                        //set pack ranges
                        PackRange[] ranges = new PackRange[2];
                        ranges[0].chardata_for_range = ptr_pdata;
                        ranges[0].first_unicode_char_in_range = 32;
                        ranges[0].num_chars_in_range = 95;
                        ranges[0].font_size = 20.0f;
                        ranges[1].chardata_for_range = IntPtr.Add(ptr_pdata, 256 * Marshal.SizeOf(pdata[0]));
                        ranges[1].first_unicode_char_in_range = 0xa0;
                        ranges[1].num_chars_in_range = 0x100 - 0xa0;
                        ranges[1].font_size = 20.0f;
                        //Bake bitmap
                        STBTrueType.PackFontRanges(ref pc, ttf_buffer, 0, ranges, 2);
                        //Clean up
                        STBTrueType.PackEnd(ref pc);
                    }
                }
                //output the bitmap to a text file
                WriteBitmapToFileAsText("testOuput.txt", BITMAP_H, BITMAP_W, bitmapBuffer);
                //Open the text file
                OpenFile("testOuput.txt");
            }
        }

        /// <summary>
        /// Output a 8-bit bitmap as a text file
        /// </summary>
        /// <param name="filePath">text file path</param>
        /// <param name="height">bitmap height</param>
        /// <param name="width">bitmap width</param>
        /// <param name="bitmap">bitmap data</param>
        public void WriteBitmapToFileAsText(string filePath, int height, int width, byte[] bitmap)
        {
            #region Write bitmap text to file
            StringBuilder sb = new StringBuilder(height * (width + 1));
            for (var y = 0; y < height; ++y)
            {
                for (var x = 0; x < width; ++x)
                {
                    var b = bitmap[y * width + x];
#if !DrawOutlineOnly
                    sb.Append(" .:ioVM@"[b >> 5]);
#else
                        if (b > 0)
                            sb.Append('*');
                        else
                            sb.Append('.');
#endif
                }
                sb.Append("\r\n");
            }
            using (StreamWriter outfile = new StreamWriter(filePath, false))
            {
                outfile.Write(sb.ToString());
            }
            #endregion
        }

    }
}
