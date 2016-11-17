using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace YrUtility.Fil
{
    public class FileHelper
    {
        public static String GetExName(String _filePath)
        {
            if (!String.IsNullOrEmpty(_filePath))
            {
                if (_filePath.Contains("."))
                {
                    return _filePath.Substring(_filePath.LastIndexOf("."));
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 创建一个文件，并写入内容
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        public static void CreateFile(String path, String content)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                using (FileStream fs= new FileStream(path, FileMode.Create))
                {
                    StreamWriter sw = new StreamWriter(fs);
                    try
                    {
                        sw.Write(content);
                        sw.Flush();
                    }
                    catch
                    {
                        throw ;
                    }
                    finally
                    {
                        sw.Close();
                        fs.Close();
                    }
                     
                }
               
            }
            catch 
            {

                throw;
            }
        }

        public static String ReadFile(String path)
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    byte[] data = new byte[fs.Length];
                    fs.Read(data, 0, data.Length);
                    return System.Text.Encoding.Default.GetString(data);//从文件中获取的含有时间戳的验证码。
                }    
            }
            catch
            {
                throw;
            }
               

        }
    }
}
