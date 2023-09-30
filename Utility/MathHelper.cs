 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility
{
    public static class MathHelper
    {
        /// <summary>
        /// 求标准差
        /// </summary>
        /// <param name="valList"></param>
        /// <param name="isAll">数据是否完整</param>
        /// <returns></returns>
        public static double Stdev(List<double> valList, bool isAll = false)
        {
            double varValue = Variance(valList, isAll);
            return Math.Sqrt(varValue);
        }
        /// <summary>
        /// 求方差
        /// </summary>
        /// <param name="valList"></param>
        /// <param name="isAll">数据是否完整</param>
        /// <returns></returns>
        public static double Variance(List<double> valList, bool isAll = false)
        {
            double avg = valList.Average();
            return valList.Sum(v => Math.Pow(v - avg, 2)) / (valList.Count - (isAll ? 0 : 1));//方差
        }
        /// <summary>
        /// 相关系数,要求两个集合数量必须相同
        /// </summary>
        /// <param name="array1">数组一</param>
        /// <param name="array2">数组二</param>
        /// <returns></returns>
        public static double Correl(List<double> array1, List<double> array2)
        {
            //数组一
            double avg1 = array1.Average();
            double stdev1 = Stdev(array1);
            //数组二
            double avg2 = array2.Average();
            double stdev2 = Stdev(array2);

            double sum = 0;
            for (int i = 0; i < array1.Count && i < array2.Count; i++)
            {
                sum = sum + ((array1[i] - avg1) / stdev1) * ((array2[i] - avg2) / stdev2);
            }
            return sum;
        }
        /// <summary>
        /// 获得最小公倍数
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        public static int GetLCM(int[] numbers)
        {
            int x, y;
            int num = numbers[0];
            int gcd;
            for (int i = 0; i < numbers.Length - 1; i++)
            {
                x = num;
                y = numbers[i + 1];
                gcd = GetGCD(x, y);
                num = x / gcd * y / gcd * gcd;
            }
            return num;
        }
        /// <summary>
        /// 获取最大公约数
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int GetGCD(int a, int b)
        {
            int temp;
            if (a < b)
            {
                temp = a;
                a = b;
                b = temp;
            }
            if (a % b == 0)
                return b;
            else
                return GetGCD(b, a % b);
        }

        /// <summary>
        /// 取时间戳
        /// </summary>
        /// <param name="dt">时间</param>
        /// <param name="isMilliseconds">精确到毫秒</param>
        /// <returns>返回一个长整数时间戳</returns>
        public static long GetTimeStamp(DateTime dt, bool isMilliseconds = false)
        {
            if (isMilliseconds)
            {
                return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;
            }
            else
            {
                return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            }
        }
    }
}
