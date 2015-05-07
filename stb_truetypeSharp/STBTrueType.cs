using System;
using System.Runtime.InteropServices;
using System.ComponentModel;

//using stbtt_uint8 = System.Byte;
//using stbtt_int8 = System.SByte;
//using stbtt_uint16 = System.UInt16;
//using stbtt_int16 = System.Int16;
//using stbtt_uint32 = System.UInt32;
//using stbtt_int32 = System.Int32;

namespace stb_truetypeSharp
{

#region Structrute Definitions
    /// <summary>
    /// Baked character data
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct BakedChar
    {
        /// <summary>top left x position coordinate of bbox in bitmap</summary>
        public ushort x0;
        /// <summary>top left y position coordinate of bbox in bitmap</summary>
        public ushort y0;
        /// <summary>bottom right y position coordinate of bbox in bitmap</summary>
        public ushort x1;
        /// <summary>bottom right y position coordinate of bbox in bitmap</summary>
        public ushort y1;

        public float xoff;
        public float yoff;
        public float xadvance;
    }

    /// <summary>
    /// Aligned quad
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct AlignedQuad
    {
        /// <summary>top-left position: x</summary>
        public float x0;
        /// <summary>top-left position: y</summary>
        public float y0;
        /// <summary>top-left texture coordinate: u</summary>
        public float s0;
        /// <summary>top-left texture coordinate: v</summary>
        public float t0;

        /// <summary>bottom-right position: x</summary>
        public float x1;
        /// <summary>bottom-right position: y</summary>
        public float y1;
        /// <summary>bottom-right texture coordinate: u</summary>
        public float s1;
        /// <summary>bottom-right texture coordinate: v</summary>
        public float t1;

        public override string ToString()
        {
            return string.Format(@"Top left position({0},{1}), texture coordinate({2},{3})
Bottom right position({4},{5}), texture coordinate({6},{7})", x0, y0, s0, t0, x1, y1, s1, t1);
        }
    }

    /// <summary>
    /// Packed chararacter
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PackedChar
    {
        /// <summary>top left x position coordinate of bbox in bitmap</summary>
        public ushort x0;
        /// <summary>top left y position coordinate of bbox in bitmap</summary>
        public ushort y0;
        /// <summary>bottom right y position coordinate of bbox in bitmap</summary>
        public ushort x1;
        /// <summary>bottom right y position coordinate of bbox in bitmap</summary>
        public ushort y1;

        public float xoff;
        public float yoff;
        public float xadvance;
        public float xoff2;
        public float yoff2;
    }

    /// <summary>
    /// Pack context needed from PackBegin to PackEnd.
    /// </summary>
    /// <remarks>
    /// an opaque structure that you shouldn't mess with which holds
    /// all the context needed from PackBegin to PackEnd.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct PackContext
    {
        public IntPtr user_allocator_context;
        public IntPtr pack_info;
        public int width;
        public int height;
        public int stride_in_bytes;
        public int padding;
        public uint h_oversample;
        public uint v_oversample;
        public IntPtr pixels;
        public IntPtr nodes;
    }


    /// <summary>
    /// Font info
    /// </summary>
    /// <remarks>
    /// The following structure is defined publically so you can declare one on
    /// the stack or as a global or etc, but you should treat it as opaque.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct FontInfo
    {
        public IntPtr userdata;

        /// <summary>
        /// pointer to .ttf file
        /// </summary>
        public IntPtr data;

        /// <summary>
        /// offset of start of font
        /// </summary>
        public int fontstart;

        /// <summary>
        /// number of glyphs, needed for range checking
        /// </summary>
        public int numGlyphs;

        /// <summary>
        /// table locations as offset from start of .ttf
        /// </summary>
        public int loca;
        public int head;
        public int glyf;
        public int hhea;
        public int hmtx;
        public int kern;
        
        /// <summary>
        /// a cmap mapping for our chosen character encoding
        /// </summary>
        public int index_map;
        
        /// <summary>
        /// format needed to map from glyph index to glyph
        /// </summary>
        public int indexToLocFormat;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PackRange
    {
        public float font_size;
        public int first_unicode_char_in_range;
        public int num_chars_in_range;

        /// <summary>
        /// output
        /// </summary>
        public IntPtr/* stbtt_packedchar* */ chardata_for_range;
    }

#endregion

    /// <summary>
    /// STB truetype, containing all stb_truetype API as static method
    /// </summary>
    public class STBTrueType
    {
        #region Simple 3D API
        /*
         *  (don't ship this, but it's fine for tools and quick start)
         */

        /// <summary>
        /// bake a font to a bitmap for use as texture
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offset">font location (use offset=0 for plain .ttf)</param>
        /// <param name="pixel_height">height of font in pixels</param>
        /// <param name="pixels">bitmap to be filled in</param>
        /// <param name="pw">width of bitmap</param>
        /// <param name="ph">height of bitmap</param>
        /// <param name="first_char">first character index</param>
        /// <param name="num_chars">character number</param>
        /// <param name="chardata">character data. you allocate this, it's num_chars long</param>
        /// <returns>
        /// positive, the first unused row of the bitmap
        /// negative, returns the negative of the number of characters that fit
        /// 0, no characters fit and no rows were used
        /// </returns>
        /// <remarks>This uses a very crappy packing.</remarks>
        [DllImport("stb_truetype.dll", EntryPoint="stbtt_BakeFontBitmap", CallingConvention=CallingConvention.Cdecl)]
        public static extern int BakeFontBitmap(IntPtr data, int offset,
                                float pixel_height,
                                byte[] pixels, int pw, int ph,
                                int first_char, int num_chars,
                                [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=7)]BakedChar[] chardata);

        /// <summary>
        /// compute quad to draw for a given char
        /// </summary>
        /// <param name="chardata">character data</param>
        /// <param name="pw">width of bitmap</param>
        /// <param name="ph">width of bitmap</param>
        /// <param name="char_index">indexes of characters to display</param>
        /// <param name="xpos">current x position in screen pixel space</param>
        /// <param name="ypos">current y position in screen pixel space</param>
        /// <param name="q">output, quad to draw</param>
        /// <param name="opengl_fillrule">true if opengl fill rule; false if DX9 or earlier</param>
        /// <remarks>
        /// Call GetBakedQuad with char_index = 'character - first_char', and it
        /// creates the quad you need to draw and advances the current position.
        ///
        /// The coordinate system used assumes y increases downwards.
        ///
        /// Characters will extend both above and below the current position;
        /// see discussion of "BASELINE" above.
        ///
        /// It's inefficient; you might want to c&amp;p it and optimize it.
        /// </remarks>
        [DllImport("stb_truetype.dll", EntryPoint="stbtt_GetBakedQuad", CallingConvention=CallingConvention.Cdecl)]
        public static extern void GetBakedQuad(BakedChar[] chardata, int pw, int ph,
                                       int char_index,
                                       ref float xpos, ref float ypos,
                                       out AlignedQuad q,
                                       int opengl_fillrule);
        #endregion

        #region Improved 3D API
        /*
         * #include "stb_rect_pack.h"           -- optional, but you really want it
         */

        /// <summary>
        /// Initializes a packing context stored in the passed-in PackContext.
        /// Future calls using this context will pack characters into the bitmap passed
        /// in here.
        /// </summary>
        /// <param name="spc">Pack context</param>
        /// <param name="pixels">bitmap</param>
        /// <param name="width">bitmap width</param>
        /// <param name="height">bitmap height</param>
        /// <param name="stride_in_bytes">the distance from one row to the next
        /// (or 0 to mean they are packed tightly together)</param>
        /// <param name="padding">the amount of padding to leave between each
        /// character (normally you want '1' for bitmaps you'll use as textures with
        /// bilinear filtering).</param>
        /// <param name="alloc_context"></param>
        /// <returns>0 on failure, 1 on success.</returns>
        /// <remarks>a 1-channel bitmap that is weight x height.</remarks>
        [DllImport("stb_truetype.dll", EntryPoint="stbtt_PackBegin", CallingConvention=CallingConvention.Cdecl)]
        public static extern int PackBegin(ref PackContext spc, IntPtr pixels, int width, int height, int stride_in_bytes, int padding, IntPtr alloc_context);

        /// <summary>
        /// Creates character bitmaps from the font_index'th font found in fontdata
        /// </summary>
        /// <param name="spc">the packing context</param>
        /// <param name="fontdata">the full height of the character from ascender to descender</param>
        /// <param name="font_index">use 0 if you don't know what that is</param>
        /// <param name="font_size">the full height of the character from ascender to descender</param>
        /// <param name="first_unicode_char_in_range">first unicode character index</param>
        /// <param name="num_chars_in_range">number of characters</param>
        /// <param name="chardata_for_range">character data for how to render them</param>
        /// <returns>(?)</returns>
        /// <remarks>
        /// Creates character bitmaps from the font_index'th font found in fontdata (use
        /// font_index=0 if you don't know what that is). It creates num_chars_in_range
        /// bitmaps for characters with unicode values starting at first_unicode_char_in_range
        /// and increasing. Data for how to render them is stored in chardata_for_range;
        /// pass these to stbtt_GetPackedQuad to get back renderable quads.
        ///
        /// font_size is the full height of the character from ascender to descender,
        /// as computed by stbtt_ScaleForPixelHeight. To use a point size as computed
        /// by stbtt_ScaleForMappingEmToPixels, wrap the point size in STBTT_POINT_SIZE()
        /// and pass that result as 'font_size':
        ///       ...,                  20 , ... // font max minus min y is 20 pixels tall
        ///       ..., STBTT_POINT_SIZE(20), ... // 'M' is 20 pixels tall
        /// </remarks>
        //[DllImport("stb_truetype.dll", EntryPoint="stbtt_PackFontRange", CallingConvention=CallingConvention.Cdecl)]
        //public static extern int PackFontRange(
        //    ref PackContext spc,
        //    byte[] fontdata,
        //    int font_index,
        //    float font_size,
        //    int first_unicode_char_in_range,
        //    int num_chars_in_range,
        //    [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=5)]
        //    PackedChar[] chardata_for_range);

        /// <summary>
        /// Oversampling a font increases the quality by allowing higher-quality subpixel
        /// positioning, and is especially valuable at smaller text sizes.
        /// </summary>
        /// <param name="spc">the packing context</param>
        /// <param name="h_oversample">horizon oversample</param>
        /// <param name="v_oversample">vertical oversample</param>
        /// <remarks>
        /// for improved quality on small fonts
        /// 
        /// Oversampling a font increases the quality by allowing higher-quality subpixel
        /// positioning, and is especially valuable at smaller text sizes.
        ///
        /// This function sets the amount of oversampling for all following calls to
        /// stbtt_PackFontRange(s). The default (no oversampling) is achieved by
        /// h_oversample=1, v_oversample=1. The total number of pixels required is
        /// h_oversample*v_oversample larger than the default; for example, 2x2
        /// oversampling requires 4x the storage of 1x1. For best results, render
        /// oversampled textures with bilinear filtering. Look at the readme in
        /// stb/tests/oversample for information about oversampled fonts
        /// </remarks>
        [DllImport("stb_truetype.dll", EntryPoint = "stbtt_PackSetOversampling", CallingConvention = CallingConvention.Cdecl)]
        public static extern void PackSetOversampling(
            ref PackContext spc,
            uint h_oversample,
            uint v_oversample);

        /// <summary>
        /// Creates character bitmaps from multiple ranges of characters
        /// </summary>
        /// <param name="spc">the packing context</param>
        /// <param name="fontdata">the full height of the character from ascender to descender</param>
        /// <param name="font_index">use 0 if you don't know what that is</param>
        /// <param name="ranges">multiple ranges of characters</param>
        /// <param name="num_ranges">number of ranges</param>
        /// <returns>(?)</returns>
        /// <remarks>
        /// pack and renders
        /// 
        /// Creates character bitmaps from multiple ranges of characters stored in
        /// ranges. This will usually create a better-packed bitmap than multiple
        /// calls to stbtt_PackFontRange.
        /// </remarks>
        [DllImport("stb_truetype.dll", EntryPoint="stbtt_PackFontRanges", CallingConvention=CallingConvention.Cdecl)]
        public static extern int PackFontRanges(
            ref PackContext spc,
            IntPtr fontdata,
            int font_index,
            [In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=4)]
            PackRange[] ranges,
            int num_ranges);

        /// <summary>
        ///  Cleans up the packing context and frees all memory.
        /// </summary>
        /// <param name="spc">the packing context</param>
        [DllImport("stb_truetype.dll", EntryPoint = "stbtt_PackEnd", CallingConvention = CallingConvention.Cdecl)]
        public static extern void PackEnd(ref PackContext spc);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chardata">character data</param>
        /// <param name="pw"></param>
        /// <param name="ph"></param>
        /// <param name="char_index">index of the character to display</param>
        /// <param name="xpos">current position x in screen pixel space</param>
        /// <param name="ypos">current position y in screen pixel space</param>
        /// <param name="q">quad to draw</param>
        /// <param name="align_to_integer"></param>
        [DllImport("stb_truetype.dll", EntryPoint = "stbtt_GetPackedQuad", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetPackedQuad(
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)]
            PackedChar[] chardata,
            int pw,
            int ph,
            int char_index,
            float xpos, float ypos,
            out AlignedQuad q,
            int align_to_integer);

        #endregion

        #region Load a font file
        /*
         * "Load" a font file from a memory buffer (you have to keep the buffer loaded)
         */

        /// <summary>
        /// Init font, builds the necessary cached info for the rest of the system
        /// </summary>
        /// <param name="info">font info</param>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <returns>Returns 0 on failure.</returns>
        /// <remarks>
        /// use for TTC font collections.
        /// 
        /// Given an offset into the file that defines a font, this function builds
        /// the necessary cached info for the rest of the system. You must allocate
        /// the stbtt_fontinfo yourself, and stbtt_InitFont will fill it out. You don't
        /// need to do anything special to free it, because the contents are pure
        /// value data with no additional data structures. Returns 0 on failure.
        /// </remarks>
        [DllImport("stb_truetype.dll", EntryPoint = "stbtt_InitFont", CallingConvention = CallingConvention.Cdecl)]
        public static extern int InitFont(
            ref FontInfo info,
            IntPtr data,
            int offset);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="index"></param>
        /// <returns>
        /// returns -1 if the index is out of range;
        /// return '0' for index 0, and -1 for all other indices
        /// </returns>
        ///<remarks>
        /// Each .ttf/.ttc file may have more than one font. Each font has a sequential
        /// index number starting from 0. Call this function to get the font offset for
        /// a given index; it returns -1 if the index is out of range. A regular .ttf
        /// file will only define one font and it always be at offset 0, so it will
        /// return '0' for index 0, and -1 for all other indices. You can just skip
        /// this step if you know it's that kind of font.
        ///</remarks>
        [DllImport("stb_truetype.dll", EntryPoint = "stbtt_GetFontOffsetForIndex", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetFontOffsetForIndex(
            IntPtr data,
            int index);

        #endregion

        #region Render a unicode codepoint to a bitmap

        #region GetCodepointBitmap
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DllImport("stb_truetype.dll", EntryPoint = "stbtt_GetCodepointBitmap", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GetCodepointBitmap(
            ref FontInfo info,
            float scale_x,
            float scale_y,
            int codepoint,
            ref int width,
            ref int height,
            ref int xoff,
            ref int yoff);

        [EditorBrowsable(EditorBrowsableState.Never)]
        [DllImport("stb_truetype.dll", EntryPoint = "stbtt_GetCodepointBitmap", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GetCodepointBitmap(
            ref FontInfo info,
            float scale_x,
            float scale_y,
            int codepoint,
            ref int width,
            ref int height,
            ref int xoff,
            IntPtr yoff);

        [EditorBrowsable(EditorBrowsableState.Never)]
        [DllImport("stb_truetype.dll", EntryPoint = "stbtt_GetCodepointBitmap", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GetCodepointBitmap(
            ref FontInfo info,
            float scale_x,
            float scale_y,
            int codepoint,
            ref int width,
            ref int height,
            IntPtr xoff,
            ref int yoff);

        [EditorBrowsable(EditorBrowsableState.Never)]
        [DllImport("stb_truetype.dll", EntryPoint = "stbtt_GetCodepointBitmap", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GetCodepointBitmap(
            ref FontInfo info,
            float scale_x,
            float scale_y,
            int codepoint,
            ref int width,
            ref int height,
            IntPtr xoff,
            IntPtr yoff);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="scale_x"></param>
        /// <param name="scale_y"></param>
        /// <param name="codepoint"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="xoff"></param>
        /// <param name="yoff"></param>
        /// <returns></returns>
        /// <remarks>
        /// allocates a large-enough single-channel 8bpp bitmap and renders the
        /// specified character/glyph at the specified scale into it, with
        /// antialiasing. 0 is no coverage (transparent), 255 is fully covered (opaque).
        /// *width &amp; *height are filled out with the width &amp; height of the bitmap,
        /// which is stored left-to-right, top-to-bottom.
        ///
        /// xoff/yoff are the offset it pixel space from the glyph origin to the top-left of the bitmap
        ///</remarks>
        public static byte[] GetCodepointBitmap(
            FontInfo info,
            float scale_x,
            float scale_y,
            int codepoint,
            ref int width,
            ref int height,
            int? xoff,
            int? yoff)
        {
            //TODO: What if width and height can be null? There will be 6 combinations!
            byte[] result;
            if (xoff.HasValue && yoff.HasValue)
            {
                int xoffTmp = 0, yoffTmp = 0;
                var ptr = GetCodepointBitmap(ref info, scale_x, scale_y, codepoint,
                    ref width, ref height,
                    ref xoffTmp,
                    ref yoffTmp);
                if (ptr == IntPtr.Zero)
                {
                    throw new Exception("GetCodepointBitmap Failed");
                }
                else
                {
                    result = new byte[width * height];
                    Marshal.Copy(ptr, result, 0, result.Length);
                }
                xoff = xoffTmp;
                yoff = xoffTmp;
            }
            else if (xoff.HasValue)
            {
                int xoffTmp = 0;
                var ptr = GetCodepointBitmap(ref info, scale_x, scale_y, codepoint,
                    ref width, ref height,
                    ref xoffTmp,
                    IntPtr.Zero);
                if (ptr == IntPtr.Zero)
                {
                    throw new Exception("GetCodepointBitmap Failed");
                }
                else
                {
                    result = new byte[width * height];
                    Marshal.Copy(ptr, result, 0, result.Length);
                }
                xoff = xoffTmp;
            } 
            else if (yoff.HasValue)
            {
                int yoffTmp = 0;
                var ptr = GetCodepointBitmap(ref info, scale_x, scale_y, codepoint,
                    ref width, ref height,
                    IntPtr.Zero,
                    ref yoffTmp);
                if (ptr == IntPtr.Zero)
                {
                    throw new Exception("GetCodepointBitmap Failed");
                }
                else
                {
                    result = new byte[width * height];
                    Marshal.Copy(ptr, result, 0, result.Length);
                }
                yoff = yoffTmp;
            }
            else
            {
                var ptr = GetCodepointBitmap(ref info, scale_x, scale_y, codepoint,
                    ref width, ref height,
                    IntPtr.Zero,
                    IntPtr.Zero);
                if (ptr == IntPtr.Zero)
                {
                    throw new Exception("GetCodepointBitmap Failed");
                }
                else
                {
                    result = new byte[width * height];
                    Marshal.Copy(ptr, result, 0, result.Length);
                }
            }
            return result;
        }
#endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="output"></param>
        /// <param name="out_w"></param>
        /// <param name="out_h"></param>
        /// <param name="out_stride"></param>
        /// <param name="scale_x"></param>
        /// <param name="scale_y"></param>
        /// <param name="codepoint"></param>
        /// <remarks>
        /// the same as stbtt_GetCodepointBitmap, but you pass in storage for the bitmap
        /// in the form of 'output', with row spacing of 'out_stride' bytes. the bitmap
        /// is clipped to out_w/out_h bytes. Call stbtt_GetCodepointBitmapBox to get the
        /// width and height and positioning info for it first.
        /// </remarks>
        [DllImport("stb_truetype.dll", EntryPoint = "stbtt_MakeCodepointBitmap", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MakeCodepointBitmap(
            FontInfo info,
            byte[] output,
            int out_w,
            int out_h,
            int out_stride,
            float scale_x,
            float scale_y,
            int codepoint);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="font"></param>
        /// <param name="codepoint"></param>
        /// <param name="scale_x"></param>
        /// <param name="scale_y"></param>
        /// <param name="ix0"></param>
        /// <param name="iy0"></param>
        /// <param name="ix1"></param>
        /// <param name="iy1"></param>
        /// <remarks>
        /// get the bbox of the bitmap centered around the glyph origin; so the
        /// bitmap width is ix1-ix0, height is iy1-iy0, and location to place
        /// the bitmap top left is (leftSideBearing*scale,iy0).
        /// (Note that the bitmap uses y-increases-down, but the shape uses
        /// y-increases-up, so CodepointBitmapBox and CodepointBox are inverted.)
        /// </remarks>
        [DllImport("stb_truetype.dll", EntryPoint = "stbtt_GetCodepointBitmapBox", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetCodepointBitmapBox(
            FontInfo font,
            int codepoint,
            float scale_x,
            float scale_y,
            ref int ix0,
            ref int iy0,
            ref int ix1,
            ref int iy1);


        #endregion

        #region Character advance/positioning
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="codepoint"></param>
        /// <param name="advanceWidth">the offset from the current horizontal position to the next horizontal position</param>
        /// <param name="leftSideBearing">the offset from the current horizontal position to the left edge of the character</param>
        /// <remarks>
        /// leftSideBearing is the offset from the current horizontal position to the left edge of the character
        /// advanceWidth is the offset from the current horizontal position to the next horizontal position
        ///   these are expressed in unscaled coordinates
        /// </remarks>
        [DllImport("stb_truetype.dll", EntryPoint = "stbtt_GetCodepointHMetrics", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetCodepointHMetrics(
            FontInfo info,
            int codepoint,
            ref int advanceWidth,
            ref int leftSideBearing);

        [EditorBrowsable(EditorBrowsableState.Never)]
        [DllImport("stb_truetype.dll", EntryPoint = "stbtt_GetFontVMetrics", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GetFontVMetrics(
            ref FontInfo info,
            ref int ascent,
            ref int descent,
            ref int lineGap);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ascent">the coordinate above the baseline the font extends</param>
        /// <param name="descent">the coordinate below the baseline the font extends</param>
        /// <param name="lineGap">the spacing between one row's descent and
        /// the next row's ascent...</param>
        /// <remarks>
        /// ascent is the coordinate above the baseline the font extends; descent
        /// is the coordinate below the baseline the font extends (i.e. it is typically negative)
        /// lineGap is the spacing between one row's descent and the next row's ascent...
        /// so you should advance the vertical position by "*ascent - *descent + *lineGap"
        ///   these are expressed in unscaled coordinates, so you must multiply by
        ///   the scale factor for a given size
        /// </remarks>
        public static void GetFontVMetrics(
            FontInfo info,
            ref int ascent,
            ref int descent,
            ref int lineGap)
        {
            GetFontVMetrics(ref info, ref ascent, ref descent, ref lineGap);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ch1"></param>
        /// <param name="ch2"></param>
        /// <returns></returns>
        /// <remarks>
        /// an additional amount to add to the 'advance' value between ch1 and ch2
        /// </remarks>
        [DllImport("stb_truetype.dll", EntryPoint = "stbtt_GetCodepointKernAdvance", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetCodepointKernAdvance(
            FontInfo info,
            int ch1,
            int ch2);
        
        #endregion
    }
}
