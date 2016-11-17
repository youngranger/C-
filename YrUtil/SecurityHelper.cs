using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace YrUtility.Security
{
    public class SecurityHelper
    {
        /// <summary>
        /// 返回大写的32位MD5值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static String Md5v32(string input)
        {
            try
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                var data = Encoding.UTF8.GetBytes(input);
                var encs = md5.ComputeHash(data);
                return BitConverter.ToString(encs).Replace("-", "");
            }
            catch
            {
                throw new Exception("求32位MD5值时出现异常");
            }

        }
        /// <summary>
        /// 返回大写16位MD5
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static String Md5v16(string input)
        {
            try
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                var data = Encoding.UTF8.GetBytes(input);
                var encs = md5.ComputeHash(data);
                return BitConverter.ToString(encs).Replace("-", "").Substring(8,16);
            }
            catch
            {
                throw new Exception("求16位MD5值时出现异常");
            }
        }
    }
}
