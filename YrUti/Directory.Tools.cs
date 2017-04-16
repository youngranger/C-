using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace YrUti.Dirctory
{
     public static class Tools
    {

        public static List<String> GetFilePathsOfFold(String directory)
        {
            if (Directory.Exists(directory))
            {
                List<String> filePaths = new List<string>();
                DirectoryInfo TheFolder = new DirectoryInfo(directory);
                foreach (FileInfo NextFile in TheFolder.GetFiles())
                    filePaths.Add(NextFile.FullName);

                return filePaths;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 获取一个文件夹下的所有文件的路径，包括子文件夹下的文件
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static  List<String> GetFilePathsOfFoldAndItsSubFold(String dictionary)
        {
            if(Directory.Exists(dictionary))
            {
                List<String> filePaths = new List<string>();
                DirectoryInfo TheFolder = new DirectoryInfo(dictionary);
                //遍历文件夹
                Loop(TheFolder, filePaths);

                return filePaths;
            }
            else
            {
                return null;
            }
            
        }

        /// <summary>
        /// 遍历文件夹及其子文件夹，将文件路径取出存放到List里
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="list"></param>
        private static void Loop(DirectoryInfo dic,List<String> list)
        {
            foreach (FileInfo NextFile in dic.GetFiles())
                list.Add(NextFile.FullName);

            foreach (DirectoryInfo NextFolder in dic.GetDirectories())
                Loop(NextFolder,list);
        }
    }
}
