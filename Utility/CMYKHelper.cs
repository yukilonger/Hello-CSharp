using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public enum ImageQuality
    {
        /// <summary>
        /// 高质量
        /// </summary>
        High,
        /// <summary>
        /// 普通质量
        /// </summary>
        Normal,
        /// <summary>
        /// 高质量
        /// </summary>
        Lower,

    }
    public class CMYKHelper
    {
        /// <summary>
        /// 是否CMYK模式图片
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static bool IsCMYK(Image img)
        {
            PixelFormat pixelformat = img.PixelFormat;
            return pixelformat.ToString() == "8207";
        }

        /// <summary>
        /// 把CMYK模式图片转为RGB模式
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="rate"></param>
        /// <returns></returns>
        public static Bitmap ConvertCMYKToRGB(Bitmap bmp, ImageQuality rate)
        {
            //rate = rate > 1.0 ? 1.0 : rate;
            //rate = rate < 0.5 ? 0.5 : rate;
            Bitmap tmpBmp = new Bitmap(bmp.Width, bmp.Height, PixelFormat.Format24bppRgb);
            Graphics g = Graphics.FromImage(tmpBmp);
            if (rate == ImageQuality.Lower)//低质量 
            {
                g.CompositingQuality = CompositingQuality.HighSpeed;
                g.SmoothingMode = SmoothingMode.HighSpeed;
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
            }
            else if (rate == ImageQuality.High)//高质量
            {
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            }
            else
            {
                //一般质量
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            }
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            // 将CMYK图片重绘一遍,此时GDI+自动将CMYK格式转换为RGB了
            g.DrawImage(bmp, rect);
            Bitmap returnBmp = new Bitmap(tmpBmp);
            g.Dispose();
            tmpBmp.Dispose();
            bmp.Dispose();
            return returnBmp;

        }
    }
}
