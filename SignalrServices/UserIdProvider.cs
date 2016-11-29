using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace SignalrServices
{
    /// <summary>
    /// 自定义维护每个用户会话id的实现类
    /// </summary>
    public class UserIdProvider:IUserIdProvider
    {
        public string GetUserId(IRequest request)
        {
            return request.QueryString["uId"];
        }
    }
}
