using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalrTool.Redis
{
   public interface IDeposit
   {
       bool RemoveKey(string key);
       bool Set(string key, object value, TimeSpan? timeOut = null);
        T Get <T>(string key);
      bool ListLeftPush(string key, object value, TimeSpan? ts = null);
       bool ListRightPush(string key, object value, TimeSpan? ts = null);
       T ListLeftPop<T>(string key);

       List<T> GetList<T>(string key);

       T ListRightPop<T>(string key);

       bool HashSet(string keyA, string keyB, object value);

       T HashGet<T>(string keyA, string keyB);

       bool RemoveHash(string keyA, string keyB);

       bool HashExists(string keyA, string keyB);
       bool HashExists(string keyA);

   }
}
