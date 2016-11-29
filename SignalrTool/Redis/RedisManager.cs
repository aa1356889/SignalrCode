using System;
using System.Threading.Tasks;
using StackExchange.Redis;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace SignalrTool.Redis
{
    public class RedisManager:IDeposit
    {
        private  ConnectionMultiplexer _redis;
        private  object _locker = new object();
        public  ConnectionMultiplexer Manager
        {
            get
            {
                if (_redis == null)
                {
                    lock (_locker)
                    {
                        if (_redis != null) return _redis;
                        _redis = GetManager();
                        return _redis;
                    }
                }
                return _redis;
            }
        }


        public  IDatabase DB
        {
            get
            {
                return Manager.GetDatabase();
            }

        }

        private  ConnectionMultiplexer GetManager()
        {
            ConfigurationOptions co = new ConfigurationOptions()
            {
                EndPoints =
                     {
        {"127.0.0.1", 6379 }
    }
            };

            return ConnectionMultiplexer.Connect(co);
        }


        public bool RemoveKey(string key)
        {
            return  DB.KeyDelete(key);
        }

        public bool Set(string key, object value, TimeSpan? timeOut=null)
        {
            Type t = value.GetType();
            bool isOk;
            if (t.IsClass && t != typeof(string))
            {
                var str = JsonConvert.SerializeObject(value);
                isOk = Manager.GetDatabase().StringSet(key, str);

            }
            else
            {
                isOk = Manager.GetDatabase().StringSet(key,value.ToString());
            }
        
            //设置key的失效时间
             if (isOk && timeOut != null)
             {
                 DB.KeyExpire(key, timeOut);
             }
            return isOk;
        }

        public T Get<T>(string key)
        {
            try
            {
                if (DB.KeyExists(key))
                {
                    Type t = typeof(T);
                    string str = DB.StringGet(key);
                    if (t.IsClass&&t!=typeof(string))
                    {
                        return JsonConvert.DeserializeObject<T>(str);
                    }
                    else
                    {
                        return (T)Convert.ChangeType(str, t);
                    }
                }
                else
                {
                    return default(T);
                }
            }
            catch (Exception e)
            {
                throw e;
            } 
        }


        /// <summary>
        /// 左侧入队
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        public bool ListLeftPush(string key,object value,TimeSpan?ts=null) { 
               Type t = value.GetType();
               bool isOk;
               long oldLength = DB.ListLength(key);
            long length=0;
            if (t.IsClass && t != typeof(string))
            {
                   var str = JsonConvert.SerializeObject(value);
                   length = DB.ListLeftPush(key, str);
            }else{
                length = DB.ListLeftPush(key, value.ToString());
            }
           isOk=oldLength < length;
           //设置key的失效时间
           if (isOk && ts != null)
           {
               DB.KeyExpire(key, ts);
           }
           return isOk;
        }

        //右侧入队
        public bool ListRightPush(string key, object value, TimeSpan? ts = null)
        {
            Type t = value.GetType();
            bool isOk;
            long oldLength = DB.ListLength(key);
            long length = 0;
            if (t.IsClass && t != typeof(string))
            {
                var str = JsonConvert.SerializeObject(value);
                length = DB.ListRightPush(key, str);
            }
            else
            {
                length = DB.ListRightPush(key, value.ToString());
            }
            isOk= oldLength < length;
            //设置key的失效时间
            if (isOk && ts != null)
            {
                DB.KeyExpire(key, ts);
            }
            return isOk;
        }


        /// <summary>
        /// 左侧出队
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T ListLeftPop<T>(string key)
        {
            Type t = typeof(T);
            if (DB.KeyExists(key))
            {
                var values = DB.ListLeftPop(key);
                if (t.IsClass && t != typeof(string))
                {
                    return JsonConvert.DeserializeObject<T>(values); ;
                }
                else
                {
                    return (T)Convert.ChangeType(values, t);
                }
            }
            else
            {
                return default(T);
            }
        }

        /// <summary>
        /// 右侧出队
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T ListRightPop<T>(string key)
        {
            Type t = typeof(T);
            if (DB.KeyExists(key))
            {
                var values = DB.ListRightPop(key);
                if (t.IsClass && t != typeof(string))
                {
                    return JsonConvert.DeserializeObject<T>(values); ;
                }
                else
                {
                    return (T)Convert.ChangeType(values, t);
                }
            }
            else
            {
                return default(T);
            }
        }

        public bool HashSet(string keyA, string keyB,object value)
        {
            if (DB == null) return false;
            var t = value.GetType();
            bool isOk;
          
            if (t.IsClass && t != typeof(string))
            {
                var str = JsonConvert.SerializeObject(value);
              isOk=  DB.HashSet(keyA, keyB, str);
            }
            else
            {
              isOk=  DB.HashSet(keyA,keyB, value.ToString());
            }
            return isOk;
        }

        public T HashGet<T>(string keyA, string keyB)
        {
            if (DB == null) return default(T);
            var t = typeof(T);

                var values = DB.HashGet(keyA,keyB);
                if (string.IsNullOrEmpty(values)) return default(T);
                if (t.IsClass && t != typeof(string))
                {
                    return JsonConvert.DeserializeObject<T>(values); ;
                }
                else
                {
                    return (T)Convert.ChangeType(values, t);
                }

               
        }

        public bool RemoveHash(string keyA, string keyB)
        {
            return DB != null && DB.HashDelete(keyA, keyB);
        }


        public bool HashExists(string keyA, string keyB)
        {
            if (DB == null) return false;
            return DB.HashExists(keyA, keyB);
        }

        public bool HashExists(string keyA)
        {
            if (DB == null) return false;
            var values= DB.HashGetAll(keyA);
            return values != null && values.Length>0;
        }


        public List<T> GetList<T>(string key)
        {

            if (DB == null) return null;
            var values = DB.ListRange(key, 0, -1);
            if (values == null || values.Length == 0) return null;
            Type t = typeof(T);
            List<T> datas = new List<T>();
            foreach (var data in values)
            {
                if (t.IsClass && t != typeof(string))
                {
                    datas.Add(JsonConvert.DeserializeObject<T>(data));
                }
                else
                {
                    datas.Add((T)Convert.ChangeType(data, t));
                }
            }
            return datas;
        }
       
    }
}
