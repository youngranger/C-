using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace YrUti.Text
{
    public static  class Regular
    {
        /// <summary>
        /// 匹配url
        /// </summary>
        public static string toMatchUrl = @"(?<url>http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?)";
    }
}