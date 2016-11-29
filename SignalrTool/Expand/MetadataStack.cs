using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.Concurrent;
using System.Data;
using Hammer.FastInvoke;
namespace Hammer.Cache
{
    /// <summary>
    ///  存储实体对象的元数据
    /// </summary>
    public class MetadataStack
    {
        #region  获取单例
        private MetadataStack()
        { }

        //private static MetadataStack _metadataStack = null;
        //private static Object Lock = new object();
        public static MetadataStack GetMetadataStack
        {
            /*
            get
            {
                if (_metadataStack == null)
                {
                    lock (Lock)
                    {
                        if (_metadataStack == null)
                        {
                            _metadataStack = new MetadataStack();
                        }
                    }
                }
                return _metadataStack;
            }*/
            get
            {
                return new MetadataStack();
            }
        }
        #endregion

        private static string attributekeyformat = "{0}.{1}";
        private static Dictionary<string, List<PropertyInfo>> propertyList = new Dictionary<string, List<PropertyInfo>>();
        private static Dictionary<string, List<Attribute>> attributeList = new Dictionary<string, List<Attribute>>();
        private static Dictionary<string, FastInvokeHandler> propertydelegateList = new Dictionary<string, FastInvokeHandler>();
        /// <summary>
        /// 当前缓存的所有属性信息
        /// </summary>
        public Dictionary<string, List<PropertyInfo>> PropertyList
        {
            get
            {
                return propertyList;
            }
        }

        /// <summary>
        ///  当前缓存的所有特性信息
        /// </summary>
        public Dictionary<string, List<Attribute>> AttributeList
        {
            get
            {
                return attributeList;
            }
        }

        public List<PropertyInfo> this[string TypeFullName]
        {
            get
            {
                if (this.PropertyList.Keys.Contains(TypeFullName))
                {
                    return this.PropertyList[TypeFullName];
                }
                return null;
            }
        }


        public FastInvokeHandler GetFastInvokeHandler(string name)
        {
            name = name.ToUpper();
            if (propertydelegateList.Keys.Contains(name))
            {
                return propertydelegateList[name];
            }
            return null;
            
        }

        public void SetFastInvokeHandler(string name, MethodInfo m)
        {
            name = name.ToUpper();

            if (!propertydelegateList.Keys.Contains(name))
            {
                propertydelegateList[name] = FastMethodInvoker.GetMethodInvoker(m);
            }

        }
        

        /// <summary>
        ///  根据一个属性信息获取它的特性集合
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public List<Attribute> this[PropertyInfo p]
        {
            get
            {
                string key = string.Format(attributekeyformat, p.DeclaringType.FullName, p.Name);
                if (!attributeList.Keys.Contains(key))
                {
                    return null;
                }

                return attributeList[key];
            }
        }

        /// <summary>
        ///  获取一个对象的特性集合
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public List<Attribute> this[Type t]
        {
            get
            {

                if (!attributeList.Keys.Contains(t.FullName))
                {
                    return null;
                }
                return attributeList[string.Format(t.FullName)];
            }
        }
        
    }
}
