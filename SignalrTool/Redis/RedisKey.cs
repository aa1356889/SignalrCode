using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalrTool.Redis
{

   public class RedisKeyManager
   {
       public const string OnlineUser = "OnlineUser";

       public const string MessageQueue = "MessageQueue";

       public static string OfflineMessageQueue(string userid)
       {
           return "Offline" + userid;
       }
        public static string OfflineGroupMessageQueue(string userid)
       {
           return "OfflineGroup" + userid;
       }

      public static  string LoginUserInfo(string userid)
       {
           return "LoginUserInfo" + userid;
       }
   }
}
