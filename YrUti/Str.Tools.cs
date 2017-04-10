using System;
using System.Collections.Generic;
using System.Text;

namespace YrUti.Str
{
    public static class Tools
    {
        /// <summary>
        /// 字符串倒序
        /// </summary>
        /// <param name="_str"></param>
        /// <returns></returns>
        public static String Reverse(String _str)
        {
            char[] cArray = _str.ToCharArray();
            String reverse = String.Empty;
            for (int i = cArray.Length - 1; i > -1; i--)
            {
                reverse += cArray[i];
            }

            return reverse;
        }

        /// <summary>
        /// 生成一个随机字符串
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public static String GetRandomString(int count)
        {
            int number;
            string checkCode = String.Empty;     //存放随机码的字符串   

            System.Random random = new Random();

            for (int i = 0; i < count; i++) //产生4位校验码   
            {
                number = random.Next();
                number = number % 36;
                if (number < 10)
                {
                    number += 48;    //数字0-9编码在48-57   
                }
                else
                {
                    number += 55;    //字母A-Z编码在65-90   
                }

                checkCode += ((char)number).ToString();
            }
            return checkCode;
        }



        /// <summary>
        /// 在原始字符串的每个字符前插入一个随机数。
        /// </summary>
        /// <param name="_str"></param>
        /// <returns></returns>
        public static String MixUp(String _str)
        {
            if (String.IsNullOrEmpty(_str))
            {
                return "a";
            }

            _str = _str.Replace("\n", "").Replace("\r", "").Replace(" ", "").Replace("-", "").Trim();//清除一些字符
            _str = Reverse(_str);//倒序。

            int length = _str.Length;
            char[] cArray = _str.ToCharArray();
            String sef = String.Empty;
            for (int i = 0; i < cArray.Length; i++)
            {
                sef += GetRandomString(1) + cArray[i];
            }
            return sef;

        }
        /// <summary>
        /// 按间隔在字符串中插入字符串
        /// </summary>
        /// <param name="DesString"></param>
        /// <param name="inserString"></param>
        /// <param name="regularly"></param>
        /// <returns></returns>
        public static String InsertStringToStringRegularly(String DesString, String inserString, int regularly)
        {
            if (String.IsNullOrEmpty(DesString))
            {
                return DesString;
            }

            if (String.IsNullOrEmpty(inserString))
            {
                return DesString;
            }
            if (DesString.Length < regularly)
            {
                return DesString;
            }
            int time = (int)DesString.Length / regularly;
            for (int i = time; i > 0; i--)
            {
                DesString = DesString.Insert(regularly * i, inserString);
            }
            return DesString;
        }

        public static String ConvertHexToXor2(string hex)
        {
            string xor = "";
            for (int i = 0; i < hex.Length; i += 2)
            {
                string str = hex.Substring(i, 2);
                if (xor != "")
                {
                    xor = (Convert.ToInt64(xor, 16) ^ Convert.ToInt64(str, 16)).ToString("X2");
                }
                else
                {
                    xor = str;
                }
            }
            return xor;
        }

        public static String CleanString(this String _str)
        {
            return _str.Replace("\n", "").Replace("\t", "").Replace(" ", "").Replace("\v", "").Replace("\f", "").Replace("\b", "").Replace("\r", "");
        }

        public static String  Wrap(String _str,String splitor)
        {
            String[] content = _str.Split(splitor.ToCharArray());
            String res = "";
            for (int i = 0; i < content.Length; i++)
            {
                if(String.IsNullOrWhiteSpace(content[i])){
                    continue;
                }
                if (i == content.Length - 1)
                {
                    res += content[i];
                }
                else
                {
                    res += content[i] + "\n";
                }
                
            }

            if (res.Substring(res.Length - 1, 1).Equals("\n"))
            {
                res = res.Substring(0, res.Length - 1);
            }
            return res;
        }
        /// <summary>
        /// 截取最后last位字符，如果字符长度超过，则截取，如果不足，则在前面补0。如果字符为null,返回null
        /// </summary>
        /// <param name="target"></param>
        /// <param name="last"></param>
        /// <returns></returns>
        public static String LastSub(String target, int last)
        {
            if (String.IsNullOrWhiteSpace(target))
            {
                return null;
            }
            else
            {
                if (target.Length >= last)
                {
                   return target.Substring(target.Length - last, last);
                }
                else
                {
                    int loop = last - target.Length;
                    for (int i = 0; i < loop; i++)
                    {
                        target = "0" + target;
                    }

                    return target;
                }
            }
        }


        /// <summary>
        /// 获取一个字符串中包含的两个值，例如：X=0.122Y=0.233,提取X和Y的值
        /// </summary>
        /// <param name="targer"></param>
        /// <param name="key1">第一个键，如X或者维度</param>
        /// <param name="key2">第二个键，如Y或者经度</param>
        /// <returns></returns>
        public static String[] GetValues(this String targer,String key1,String key2)
        {
            String[] splitor = { key2 };
            String[] content = targer.Split(splitor,StringSplitOptions.RemoveEmptyEntries);
            content[0]=content[0].Replace(key1, "");

            content[0] = content[0].Trim();
            content[1] = content[1].Trim();
            return content;
        }
    }
}
