using System;

namespace SignalrServices
{
    /// <summary>
    /// 在线用户信息管理类
    /// </summary>
    public class User
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public string Uid { get; set; }

        /// <summary>
        /// 用户名字
        /// </summary>
        public string UName { get; set; }

        /// <summary>
        /// 上线时间
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string HeadPath { get; set; }


        /// <summary>
        /// 当前用户所在机构id
        /// </summary>
        public string OrganId{ get; set; }

        /// <summary>
        /// 当前用户所在部门id
        /// </summary>
        public string Depid { get; set; }

        /// <summary>
        /// signalr 链接id
        /// </summary>
        public string ConnectionId { get; set; }
    }
}
