using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Utility
{
    public class ValidateHelper
    {
        /// <summary>
        /// 验证电话号码
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static bool IsTel(string str_telephone)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_telephone, @"^(\d{3,4}-)?\d{6,8}$");
        }
        /// <summary>
        ///  验证手机号码
        /// </summary>
        /// <param name="str_handset"></param>
        /// <returns></returns>
        public static bool IsPhone(string str_handset)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_handset, @"^1[0-9]{10}$"); 
        }
        /// <summary>
        /// 验证身份证号的
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static bool IsIDcard(string str_idcard)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_idcard, @"(^\d{18}$)|(^\d{15}$)");
        }
        /// <summary>
        /// 验证输入为数字
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static bool IsNumber(string str_number)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_number, @"^[0-9]*$");
        }
        /// <summary>
        ///  验证邮编
        /// </summary>
        /// <param name="str_postalcode"></param>
        public static bool IsPostcode(string str_postalcode)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_postalcode, @"^\d{6}$");
        }
    }
}
