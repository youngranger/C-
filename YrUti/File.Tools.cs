using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace YrUti.File
{
    public static class Tools
    {
        /// <summary>
        /// 获取文件扩展名
        /// </summary>
        /// <param name="_filePath">文件全路径</param>
        /// <returns>文件扩展名，不含"."</returns>
        public static String GetExName(String _filePath)
        {
            if (!String.IsNullOrEmpty(_filePath))
            {
                if (_filePath.Contains("."))
                {
                    return _filePath.Substring(_filePath.LastIndexOf(".")+1);
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
        /// <param name="path">文件完整路径</param>
        /// <param name="content"></param>
        public static void CreateFile(String path, String content)
        {
            try
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

                FileInfo fi = new FileInfo(path);
                var di = fi.Directory;
                if (!di.Exists)
                    di.Create();

                ///System.IO.File.Create(path);
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
                    return System.Text.Encoding.Default.GetString(data);//从文件中获取字符串。
                }    
            }
            catch
            {
                throw;
            }
               

        }
    }
}
