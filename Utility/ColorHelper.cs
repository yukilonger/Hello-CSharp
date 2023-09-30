using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public static class ColorHelper
    {

        /// <summary>
        /// 根据自定义范围生成颜色
        /// </summary>
        /// <param name="start">起始数值 0-255</param>
        /// <param name="end">结束数值 0-255</param>
        /// <returns>Color</returns>
        private static Color GetRandomColor(int start = 0, int end = 255)
        {

            if (start < 0 || start > 255) throw new Exception("起始数值只能为0-255之间的数字");
            if (end < 0 || end > 255) throw new Exception("结束数值只能为0-255之间的数字");
            if (start > end) throw new Exception("起始数值不能大于结束数值");


            Random ran = new Random(Guid.NewGuid().GetHashCode());

            int R, G, B;
            double Y;
            bool result;

            do
            {
                R = ran.Next(0, 255);
                G = ran.Next(0, 255);
                B = ran.Next(0, 255);

                //Y值计算公式
                Y = 0.299 * R + 0.587 * G + 0.114 * B;

                result = Y >= start && Y <= end;
            } while (!result);

            return Color.FromArgb(R, G, B);
        }
        /// <summary>
        /// 获取暗色
        /// </summary>
        /// <returns></returns>
        public static Color GetDarkColor()
        {
            return GetRandomColor(0, 180);
        }
        /// <summary>
        /// 获取亮色
        /// </summary>
        /// <returns></returns>
        public static Color GetLightColor()
        {
            return GetRandomColor(180, 255);
        }

        //public static Color GetRandomDeepColor()
        //{
        //    Random RandomNum_First = new Random((int)DateTime.Now.Ticks);
        //    //  对于C#的随机数，没什么好说的
        //    System.Threading.Thread.Sleep(RandomNum_First.Next(50));
        //    Random RandomNum_Sencond = new Random((int)DateTime.Now.Ticks);

        //    //  为了在白色背景上显示，尽量生成深色
        //    int int_Red = RandomNum_First.Next(256);
        //    int int_Green = RandomNum_Sencond.Next(256);
        //    int int_Blue = (int_Red + int_Green > 400) ? 0 : 400 - int_Red - int_Green;
        //    int_Blue = (int_Blue > 255) ? 255 : int_Blue;

        //    return Color.FromArgb(int_Red, int_Green, int_Blue);
        //}
    }
}
