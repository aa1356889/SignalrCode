using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace SignalrClient.Bind
{
    /// <summary>
    /// MVC复杂类型绑定
    /// <UpdateDate>2016-5-27</UpdateDate>
    /// <Author>李强</Author>
    /// </summary>
    public class MyArrayBind : IModelBinder
    {
       public ModelBindingContext bindingContext;
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {

            this.bindingContext = bindingContext;
             return BindingArray(bindingContext.ModelType,bindingContext.ModelName);
            
          
            throw new NotImplementedException();
        }

        public object BindingArray(Type type,string key)
        {
            //判断是否是数组 如果是数组类型 则执行数组的绑定
            if (type.IsArray)
            {
                //获得数组的储存类型
                var elemetType = type.GetElementType();
                //获得该类型的所有属性
                var propertys = elemetType.GetProperties();
               
                //反射定义一个集合。存储绑定数据
                Type listType = typeof(List<>);
                MethodInfo method = null;
                listType = listType.MakeGenericType(elemetType);//定义反射集合的储存类型
                object list = Activator.CreateInstance(listType);//创建该集合
                bool isValue = false;//用于判断是否进行第二次值提供器查找
                if (elemetType == typeof(string))
                {
                    var value= bindingContext.ValueProvider.GetValue(key+"[]");
                    if (value != null) {
                        return value.RawValue;
                    }
                }
                else
                {
                    for (int i = 0; true; i++)
                    {
                        var obj = Activator.CreateInstance(elemetType);
                        foreach (var property in propertys)
                        {
                            var valuekey = key + "[" + i + "]" + "[" + property.Name + "]";
                            var propertype = property.PropertyType;
                            if (propertype.IsArray)
                            {
                                valuekey = key + "[" + i + "]" + "[" + property.Name + "]";
                                property.SetValue(obj, BindingArray(propertype, valuekey));

                            }

                            var bb = bindingContext.ValueProvider.GetValue(valuekey);
                            if (bb != null)
                            {
                                property.SetValue(obj, Convert.ChangeType(bb.AttemptedValue, property.PropertyType));
                                isValue = true;//代表找到有值
                            }
                        }
                        if (!isValue) break;//如果以上没有成功绑定一次则表示值提供器没有值了 遍历到尾部了 结束循环
                        isValue = false;
                        //每次绑定的数据存入集合
                        method = listType.GetMethod("Add");
                        method.Invoke(list, new object[] { obj });

                    }
                }
                //将集合转换为数组返回
                method = listType.GetMethod("ToArray");
                return method.Invoke(list, null);
            }
                //执行普通绑定
            else
            {
                if (type.IsValueType || type == typeof (string))
                {
                    var keyobj = bindingContext.ValueProvider.GetValue(key);
                    return keyobj==null?null:keyobj.AttemptedValue;
                }
                else { 
                //非数组的绑定
                var propertys = type.GetProperties();
                var obj=Activator.CreateInstance(type);
                    foreach (var  property in propertys)
                    {
                        //如果是数组 则递归调用 进行数组的绑定逻辑
                        if (property.PropertyType.IsArray)
                        {
                            var modelObj = BindingArray(property.PropertyType, property.Name);
                            property.SetValue(obj, modelObj);
                        }
                        else
                            //如果是基本类型 直接在值提供器中拿值绑定
                            if (property.PropertyType.IsValueType || property.PropertyType == typeof (string))
                            {
                                var newkey = property.Name;
                                if (Regex.IsMatch(key, @"\[\w*\]"))
                                {
                                    newkey = key + "[" + newkey + "]";
                                }
                                var value = bindingContext.ValueProvider.GetValue(newkey);
                                if (value != null)
                                {
                                    if (string.IsNullOrEmpty(value.AttemptedValue)) continue;
                                    if (!property.PropertyType.IsGenericType)
                                    {
                                        //非泛型
                                        property.SetValue(obj,
                                            string.IsNullOrEmpty(value.AttemptedValue)
                                                ? null
                                                : Convert.ChangeType(value.AttemptedValue, property.PropertyType), null);
                                    }
                                    else
                                    {
                                        //泛型Nullable<>
                                        Type genericTypeDefinition = property.PropertyType.GetGenericTypeDefinition();
                                        if (genericTypeDefinition == typeof (Nullable<>))
                                        {
                                            property.SetValue(obj,
                                                string.IsNullOrEmpty(value.AttemptedValue)
                                                    ? null
                                                    : Convert.ChangeType(value.AttemptedValue,
                                                        Nullable.GetUnderlyingType(property.PropertyType)), null);
                                        }
                                    }
                                }
                            } //引用类型的绑定
                            else
                            {
                                var sonType = property.PropertyType;
                                var sonTypePropertys = sonType.GetProperties();
                                var sonOBJ = Activator.CreateInstance(sonType);
                                foreach (var item in sonTypePropertys)
                                {
                                    string newkey = property.Name + "[" + item.Name + "]";
                                    if (item.PropertyType.IsArray)
                                    {
                                        var modelObj = BindingArray(item.PropertyType, newkey);
                                        item.SetValue(sonOBJ, modelObj);
                                    }
                                    else if (item.PropertyType.IsValueType || item.PropertyType == typeof (string))
                                    {

                                        var value = bindingContext.ValueProvider.GetValue(newkey);
                                        if (value != null)
                                        {
                                            item.SetValue(sonOBJ,
                                                Convert.ChangeType(value.AttemptedValue, item.PropertyType));
                                        }
                                    }
                                    else
                                    {
                                        var modelObj = BindingArray(item.PropertyType, newkey);
                                        item.SetValue(sonOBJ, modelObj);
                                    }
                                }
                                property.SetValue(obj, sonOBJ);
                            }

                    }
                    return obj;
                }
              
            }
          
        }
    }
}