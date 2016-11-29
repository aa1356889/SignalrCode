using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SignalrServices.Log4;
using SignalrTool.Redis;
using System.Collections.Concurrent;

namespace SignalrServices
{
   public class UserManager
    {
       //在线用户信息访问的回调地址储存
        public ConcurrentDictionary<string, List<string>> usersAddress;
        //保存在线用户的基本信息
        public ConcurrentDictionary<string,User> users;


        private Object lockobj = new object();
        public UserManager()
        {
            usersAddress =new ConcurrentDictionary<string, List<string>>();
            users = new ConcurrentDictionary<string, User>();
        }

        /// <summary>
        /// 将制定用户从在线列表中中删除
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public bool Remove(string uid)
        {
           
            List<string> keys;
            User user;
            try
            {
                bool isDelA = users.TryRemove(uid, out user);
                bool isDelB = usersAddress.TryRemove(uid, out keys);
                return isDelA && isDelB;
            }
            catch (Exception e)
            {
                Log4Helper.WriteLog(typeof(UserManager), e);
                return false;
            }
        }

        /// <summary>
        /// 删除指定用户的链接地址
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public bool Remove(string uid,string address)
        {
           
            lock (lockobj)
            {
                try
                {
                    bool isDel = usersAddress[uid].Remove(address);
                    if (usersAddress[uid].Count == 0)
                    {
                        return Remove(uid);
                    }
                    return isDel;
                }
                catch (Exception e)
                {
                    Log4Helper.WriteLog(typeof(UserManager), e);
                    return false;
                }
             
            }
           
        }


        public bool Contains(string uid)
        {
            return users.ContainsKey(uid);
        }

        /// <summary>
        /// 将指定用户添加到在线用户信息列表
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool Add(User user)
        {

            try
            {
                User userer = users.GetOrAdd(user.Uid, user);
                List<string> list = usersAddress.GetOrAdd(user.Uid, new List<string>());
                lock (list)
                {
                    list.Add(user.ConnectionId);
                }
            }
            catch (Exception e)
            {
                Log4Helper.WriteLog(typeof(UserManager), e);
                return false;
            }
                return true;
   
        }
    }
}
