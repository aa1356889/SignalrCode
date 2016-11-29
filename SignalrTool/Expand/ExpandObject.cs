
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
    ///  扩展object类
    /// </summary>
    public static class ExpandObject
    {
        private static MetadataStack _MetadataStack = MetadataStack.GetMetadataStack;
        private static MetadataStack _metadatastack = _MetadataStack;
        /// <summary>
        ///  获取指定类型的特性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <returns></returns>
        public static T GetAttribute<T>(this object o) where T : class
        {
            List<Attribute> attrs = null;
            Type t = o as Type;
            if (!_MetadataStack.AttributeList.Keys.Contains(t.FullName))
            {
                return null;
            }
            attrs = _MetadataStack.AttributeList[t.FullName];
            

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
            return null;

        }

        /// <summary>
        ///  判断一个对象是否有特性元数据缓存
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool ExistsAttribute(this object o)
        {
            List<Attribute> attrs = null;
            Type t = o.GetType();
            if (!_MetadataStack.AttributeList.Keys.Contains(t.FullName))
            {
                return false;
            }
            attrs = _MetadataStack.AttributeList[t.FullName];
            if (attrs != null && attrs.Count > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        ///  对象转实体
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static Dictionary<string, object> ToDictionary(this object o)
        {

            Type t = o.GetType();
            Dictionary<string, object> dic = new Dictionary<string, object>();

            if (!t.ExistsCacheProperty())
            {
                t.AddProperty();
            }

            List<PropertyInfo> p = _MetadataStack.PropertyList[t.FullName];

            for (int i = 0; i < p.Count; i++)
            {
                //_metadatastack
                //if (p[i].Name.ToUpper().IndexOf("ITEM") >= 0) continue; //剔除掉索引器
                //_metadatastack.GetFastInvokeHandler(t.FullName + p[i].Name + "get").Invoke(o, null);
                //dic[p[i].Name] = p[i].GetValue(o, null);
                dic[p[i].Name] = _metadatastack.GetFastInvokeHandler(t.FullName + p[i].Name + "get").Invoke(o, null);
            }

            return dic;
        }


        public static List<Dictionary<string, object>> ToDictionaryList<T>(this IList<T> ilist)
        {
            List<Dictionary<string, object>> dics = new List<Dictionary<string, object>>();
            foreach (T t in ilist)
            {
                dics.Add(t.ToDictionary());
            }
            return dics;
        }

        /// <summary>
        ///  实体互转
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <returns></returns>
        public static T toEntity<T>(this object o) where T : class
        {
            Type t1 = o.GetType();
            List<PropertyInfo> m1 = null;
            if (!t1.ExistsCacheProperty())
            {
                t1.AddProperty();
            }
            m1 = _MetadataStack.PropertyList[t1.FullName];

            Type t2 = typeof(T);
            //ConstructorInfo Constructor = t2.GetConstructor(Type.EmptyTypes);
            //Object obj = Constructor.Invoke(null);

            object obj = Activator.CreateInstance(t2);


            List<PropertyInfo> m2 = null;

            if (!t2.ExistsCacheProperty())
            {
                t2.AddProperty();
            }
            m2 = _MetadataStack.PropertyList[t2.FullName];

            PropertyInfo temp1 = null;
            PropertyInfo temp2 = null;
            for (int i = 0; i < m1.Count; i++)
            {

                temp2 = m2.Where(j => j.Name.ToUpper() == m1[i].Name.ToUpper()).SingleOrDefault();
                if (temp2 == null) continue;
                temp1 = m1[i];
                //object value = temp1.GetValue(o, null);

                object value = _metadatastack.GetFastInvokeHandler(t1.FullName + temp1.Name + "get").Invoke(o, null);

                if (value != null && !string.IsNullOrEmpty(value.ToString()))
                {
                    if (temp2.PropertyType.IsGenericType)
                    {
                        _metadatastack.GetFastInvokeHandler(t2.FullName + m1[i].Name + "set").Invoke(obj, new object[] { Convert.ChangeType(value, temp2.PropertyType.GenericTypeArguments[0]) });

                        //可空处理
                       // temp2.SetValue(obj, Convert.ChangeType(value, temp2.PropertyType.GenericTypeArguments[0]), null);
                    }
                    else
                    {
                        //temp2.SetValue(obj, Convert.ChangeType(value, temp2.PropertyType), null);
                        _metadatastack.GetFastInvokeHandler(t2.FullName + m1[i].Name + "set").Invoke(obj, new object[] { Convert.ChangeType(value, temp2.PropertyType) });
                    }
                }

            }
            return obj as T;
        }


    }
}
