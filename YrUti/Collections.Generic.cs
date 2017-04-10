using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YrUti.Collections
{
    public static class  List
    {
        /// <summary>
        /// 删除元素，如果有同名元素，则都删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="item"></param>
        public static  void Delete<T>(this List<T> target,T item)
        {
            for (int i =target.Count-1; i >= 0; i--)
            {
                if (target[i].Equals(item))
                {
                    target.Remove(target[i]);
                }
            }
        }

        /// <summary>
        /// 判断一个字符串列中是否有匹配的项，可以是完全匹配，也可以是部分匹配
        /// </summary>
        /// <param name="target">字符串列</param>
        /// <param name="value">要匹配的字符串</param>
        /// <returns></returns>
        public static String Contains(this List<String> target, String value)
        {

            foreach (String v in target)
            {
                if (v.Contains(value))
                {
                    return v;
                }
            }

            return null;
        }
    }
}
