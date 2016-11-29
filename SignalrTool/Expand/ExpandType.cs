using Hammer.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    ///  扩展Type类
    /// </summary>
    public static class ExpandType
    {


        private static MetadataStack _MetadataStack = MetadataStack.GetMetadataStack;
        private static Object obj = new object();
        private static Object obj2 = new object();
        /// <summary>
        ///  为一个类型获取元数据并加入元数据缓存集合
        /// </summary>
        /// <param name="t"></param>
        public static void AddProperty(this Type t)
        {
            if (!_MetadataStack.PropertyList.Keys.Contains(t.FullName))
            {
                lock (obj)
                {
                    List<PropertyInfo> ps = null;

                    ps = t.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty).ToList();

                    _MetadataStack.PropertyList[t.FullName] = ps;

                    foreach (PropertyInfo p in ps)
                    {
                        _MetadataStack.SetFastInvokeHandler(t.FullName + p.Name + "set", p.SetMethod);
                        _MetadataStack.SetFastInvokeHandler(t.FullName + p.Name + "get", p.GetMethod);
                        p.AddAttribute();
                    }
                    t.AddAttribute();
                }
            }
        }


        /// <summary>
        ///  为一个类型获取特性数据并加入元数据缓存集合
        /// </summary>
        /// <param name="t"></param>
        public static void AddAttribute(this Type t)
        {
            if (!_MetadataStack.AttributeList.Keys.Contains(t.FullName) || _MetadataStack.AttributeList[t.FullName]==null)
            {
                lock (obj2)
                {
                    object[] objs = t.GetCustomAttributes(false);
                    List<Attribute> attrs = null;
                    if (objs.Length > 0)
                    {
                        attrs = new List<Attribute>();
                        Attribute ar = null;
                        foreach (object temp in objs)
                        {
                            ar = temp as Attribute;
                            if (ar != null)
                            {
                                attrs.Add(ar);
                            }
                        }
                    }

                    _MetadataStack.AttributeList[t.FullName] = attrs;
                }
            }
        }

        /// <summary>
        ///  判断一个对象是否有属性元数据缓存
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool ExistsCacheProperty(this Type t)
        {
            if (!_MetadataStack.PropertyList.Keys.Contains(t.FullName))
            {
                return false;
            }
            List<PropertyInfo> ps = _MetadataStack.PropertyList[t.FullName];

            if (ps == null || ps.Count <= 0)
            {
                return false;
            }
            return true;

        }

        /// <summary>
        /// 根据Type对象创建本对象默认实例
        /// 蒋军 2016.9.28新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static T CreateInstance<T>(this Type t) where T : class
        {
            //Type tt = typeof(T);
            T obj = t.GetConstructor(Type.EmptyTypes).Invoke(null) as T;
            return obj;
        }
    }
}
