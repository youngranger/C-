﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;

namespace YrUti.Net
{
     public static  class Http
    {

        //参数 paramsValue的格式 要和 Reques.ContentType一致，
        //如果 contentype为"application/x-www-form-urlencoded" 表单类型，那么 参数为a= 1 & b = 2 形式
        //如果contentype 为"application/json"  json 类型  那么参数就为  "{a:1,b:2}" 格式
        //2.可以添加自定义header, add（key, value）
        //接受获取header Request.Headers.Get(key)
        public static string HttpPostMath(string url, string paramsValue)
        {

            String result;
            byte[] byteArray = Encoding.UTF8.GetBytes(paramsValue);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            //request.ContentType = "application/x-www-form-urlencoded";
            request.ContentType = "multipart/form-data";
            //request.ContentType = "application/json";
            //request.Headers.Add("ennm", "城西湖");
            request.ContentLength = byteArray.Length;
            //request.Headers.Add("sign", "123");
            using (Stream newStream = request.GetRequestStream())
            {
                newStream.Write(byteArray, 0, byteArray.Length); //写入参数
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();//获取响应

            using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                result = sr.ReadToEnd();
            }
            return result;

        }

        /// <summary>
        /// 适配安徽服务器的http请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paramsValue"></param>
        /// <returns></returns>
        public static string HttpPostMathForAnHuiServer(string url, string paramsValue)
        {

            String result;
            byte[] byteArray = Encoding.UTF8.GetBytes(paramsValue);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0))";
            request.Headers.Add("Accept-Charset", "utf-8;q=0.7,*;q=0.7");
            request.ContentLength = byteArray.Length;
            using (Stream newStream = request.GetRequestStream())
            {
                newStream.Write(byteArray, 0, byteArray.Length); //写入参数
            }
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();//获取响应

                using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    result = sr.ReadToEnd();
                }
                return result;
            }
            catch(Exception e)
            {
                return "服务器错误："+e.Message;
            }
            

        }

        public static string HttpPostMath(string url)
        {

            String result;
            //byte[] byteArray = Encoding.UTF8.GetBytes(paramsValue);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            //request.ContentType = "application/json";
            //request.Headers.Add("sign", "123");
            //request.ContentLength = byteArray.Length;
            //request.Headers.Add("sign", "123");
            //using (Stream newStream = request.GetRequestStream())
            //{
            //    newStream.Write(byteArray, 0, byteArray.Length); //写入参数
            //}
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();//获取响应

            using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                result = sr.ReadToEnd();
            }
            return result;

        }

        public static string HttpGetMath(string url, string paramsValue)
        {
            string result = string.Empty;
            Uri uri = new Uri(url);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri + "?" + paramsValue);
            request.Method = "Get";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return result;
        }

        public static string HttpGetMath(string url)
        {
            //string result = string.Empty;
            Uri uri = new Uri(url);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "Get";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return retString;
        }
    }
}
