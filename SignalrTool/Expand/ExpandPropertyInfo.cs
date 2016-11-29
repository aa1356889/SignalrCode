
using Hammer.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace System.Reflection
{
    /// <summary>
    ///  扩展PropertyInfo类
    /// </summary>
    public static class ExpandPropertyInfo
    {
        private static string attributekeyformat = "{0}.{1}";
        private static MetadataStack _MetadataStack = MetadataStack.GetMetadataStack;
        private static Object objj = new object();
        public static void AddAttribute(this PropertyInfo p)
        {
            string key = string.Format(attributekeyformat, p.DeclaringType.FullName, p.Name);

            if (!_MetadataStack.AttributeList.Keys.Contains(key) || _MetadataStack.AttributeList[key]==null)
            {
                lock (objj)
                {
                    object[] objs = p.GetCustomAttributes(false);
                    List<Attribute> attrs = null;
                    if (objs.Length > 0)
                    {
                        attrs = new List<Attribute>();
                        Attribute ar = null;
                        foreach (object obj in objs)
                        {
                            ar = obj as Attribute;
                            if (ar != null)
                            {
                                attrs.Add(ar);
                            }
                        }
                    }
                    _MetadataStack.AttributeList[key] = attrs;
                }
            }
        }

        /// <summary>
        ///  获取一个对象的指定特性，有则返回，无则null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <returns></returns>
        public static T GetAttribute<T>(this PropertyInfo o) where T : class
        {
            List<Attribute> attrs = null;
            PropertyInfo p = o as PropertyInfo;
            string key = string.Format(attributekeyformat, p.DeclaringType.FullName, p.Name);
            if (!_MetadataStack.AttributeList.Keys.Contains(key))
            {
                return null;
            }
            attrs = _MetadataStack.AttributeList[key];
            if (attrs != null && attrs.Count > 0)
            {
                foreach (Attribute i in attrs)
                {
                    if (i is T)
                    {
                        return i as T;
                    }
                }
                return null;
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        ///  判断一个对象是否有特性元数据缓存
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool ExistsAttribute(this PropertyInfo o)
        {
            List<Attribute> attrs = null;
            PropertyInfo p = o as PropertyInfo;
            string key = string.Format(attributekeyformat, p.DeclaringType.FullName, p.Name);
            if (!_MetadataStack.AttributeList.Keys.Contains(key))
            {
                return false;
            }
            attrs = _MetadataStack.AttributeList[key];
            if (attrs != null && attrs.Count > 0)
            {
                return true;
            }
            return false;
        }
    }
}
