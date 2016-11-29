using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalrTool.Redis
{
   public class DepositHelper
   {
       public static IDeposit rm = new RedisManager();
        public static  bool RemoveKey(string key)
        {
            return rm.RemoveKey(key);
        }

        public static  bool Set(string key, object value, TimeSpan? timeOut = null)
        {
            return rm.Set(key, value, timeOut);
        }

        public static  T GetAsync<T>(string key)
        {
            return rm.Get<T>(key);
        }

        public static  bool ListLeftPush(string key, object value, TimeSpan? ts = null)
        {
            return rm.ListLeftPush(key, value, ts);
        }

        public static bool ListRightPush(string key, object value, TimeSpan? ts = null)
        {
            return rm.ListRightPush(key, value, ts);
        }

        public static  T ListLeftPop<T>(string key)
        {
            return rm.ListLeftPop<T>(key);
        }

        public static  T ListRightPop<T>(string key)
        {
            return rm.ListRightPop<T>(key);
        }

       public static bool HashSet(string keyA, string keyB, object value)
       {
           return rm.HashSet(keyA, keyB, value);
       }

       public static T HashGet<T>(string keyA, string keyB)
       {
           return rm.HashGet<T>(keyA, keyB);
       }

       public static bool RemoveHash(string keyA, string keyB)
       {
           return rm.RemoveHash(keyA, keyB);
       }

       public static bool HashExists(string keyA, string keyB)
       {
           return rm.HashExists(keyA, keyB);
           
       }

       public static bool HashExists(string keyA)
       {
           return rm.HashExists(keyA);
       }

       public static List<T> GetList<T>(string key)
       {
           return rm.GetList<T>(key);
       }
    }
}
