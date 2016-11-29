using System;
using System.Configuration;

namespace SignalrServices

{
    public class ConfigHelper
    {
        public static string Get(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
        /// <summary>
        /// 加密密码协议
        /// </summary>
        public static string GetPassWord { get { return Get("passWord"); } }

        /// <summary>
        /// websoket服务器监听ip
        /// </summary>
        public static string GetServiceAddress
        {
            get { return Get("ServiceAddress"); }
        }
        /// <summary>
        /// redis监听ip
        /// </summary>
        public static string GetRedisIp { get { return Get("redisIp"); } }
        /// <summary>
        /// redis急停地址
        /// </summary>
        public static int GetRedisPort { get { return Convert.ToInt32(Get("redisPort")); } }

        /// <summary>
        /// 消息中心跳转链接
        /// </summary>
        public static string MessageJumpUrlCenter { get { return Get("MessageJumpUrlCenter"); } }

    }

}
