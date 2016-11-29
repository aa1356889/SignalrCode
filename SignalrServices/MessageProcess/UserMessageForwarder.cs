using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SignalrTool.Redis;

namespace SignalrServices.MessageProcess
{
  public class UserMessageForwarder:MessageForwarder
    {
//      layim.getMessage({
//  username: "纸飞机" //消息来源用户名
//  ,avatar: "http://tp1.sinaimg.cn/1571889140/180/40030060651/1" //消息来源用户头像
//  ,id: "100000" //聊天窗口来源ID（如果是私聊，则是用户id，如果是群聊，则是群组id）
//  ,type: "friend" //聊天窗口来源类型，从发送消息传递的to里面获取
//  ,content: "嗨，你好！本消息系离线消息。" //消息内容
//  ,mine: false //是否我发送的消息，如果为true，则会显示在右方
//  ,timestamp: 1467475443306 //服务端动态时间戳
//});
        SignalrClient.BLL.UserBLL user = new SignalrClient.BLL.UserBLL();
        public override void MessageProcess(Message message, Microsoft.AspNet.SignalR.IHubContext hubcontext)
        {
            var sendUser = user.Get(c => c.UserId == message.SendId);
            if (MyHub.Usermananger.Contains(message.ReceiveId.ToString()))
            {
                var id=Convert.ToInt32(message.ReceiveId);
              
                hubcontext.Clients.User(message.ReceiveId.ToString()).receive(new{
                  username=message.SendName,
                  avatar = message.SendHeadPath,
                  type=message.MessageType,
                  id=message.SendId,
                  content=message.MessageContent,
                  mine=false,
                  timestamp=DateTime.Now
                });

            }
            else
            {
                DepositHelper.ListRightPush(RedisKeyManager.OfflineMessageQueue(message.ReceiveId.ToString()), message);
            }
           
        }
    }
}
