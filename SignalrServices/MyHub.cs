using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using SignalrTool.Redis;
using System.Threading;
namespace SignalrServices
{
    [HubName("MyHub")]
    public class MyHub : Hub
    {


        public static UserManager Usermananger = new UserManager();

        public void Login(string uid, string uname)
        {

        }

        /// <summary>
        /// 用户链接事件
        /// </summary>
        /// <returns></returns>
        public override Task OnConnected()
        {
            var user = new User() { Uid = Context.QueryString["uId"], UName = Context.QueryString["uName"], ConnectionId = Context.ConnectionId, OrganId = Context.QueryString["OrganId"], Depid = Context.QueryString["Depid"] };
            if (!Usermananger.Contains(user.Uid))
            {
                //加载用户离线消息
                PrcoessOfflineMessage(user.Uid);
                Program.from.AddItems(user);
            }
            if (user.OrganId != "-1")
                Groups.Add(Context.ConnectionId, user.OrganId);
            if (user.Depid != "-1")
                Groups.Add(Context.ConnectionId, user.OrganId.ToString() + "-" + user.Depid.ToString());

            Usermananger.Add(user);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var user = new User() { Uid = Context.QueryString["uId"], UName = Context.QueryString["uName"], ConnectionId = Context.ConnectionId, OrganId = Context.QueryString["OrganId"], Depid = Context.QueryString["Depid"] };
            Usermananger.Remove(user.Uid, user.ConnectionId);
            if (!Usermananger.Contains(user.Uid))
            {
                Program.from.RemoveItems(user);

            }
            if (user.OrganId != "-1")
                Groups.Remove(Context.ConnectionId, user.OrganId);
            if (user.Depid != "-1")
                Groups.Remove(Context.ConnectionId, user.OrganId.ToString() + "-" + user.Depid.ToString());
            return base.OnDisconnected(stopCalled);
        }

        public void PrcoessOfflineMessage(string uid)
        {

            string keya=RedisKeyManager.OfflineMessageQueue(uid);
            string keyb=RedisKeyManager.OfflineGroupMessageQueue(uid);
            //点对点离线消息
            List<Message> usermessage = DepositHelper.GetList<Message>(keya);
            //群聊消息
            List<Message> groupMessage = DepositHelper.GetList<Message>(keyb);
            SignalrClient.BLL.UserBLL user = new SignalrClient.BLL.UserBLL();
            List<object> objes = null;

            if (usermessage != null && usermessage.Count > 0)
            {
                var id = usermessage[0].SendId;
                var sendUser = user.Get(c => c.UserId == id);
                objes = new List<object>();
                usermessage.ForEach(message =>
                {


                    objes.Add(new
                    {
                        username = sendUser.UserName,
                        avatar = sendUser.HeadPath,
                        type = message.MessageType,
                        id = message.SendId,
                        content = message.MessageContent,
                        mine = false,
                        timestamp =message.CreateTime,
                    });
                });
                Clients.User(uid).receiveOffline(objes);
                //发送完则删除
                 DepositHelper.RemoveKey(keya);

            }
            if (groupMessage != null && groupMessage.Count > 0)
            {
                objes = new List<object>();
                groupMessage.ForEach(message =>
                {
                    objes.Add(new
            {
                username = message.SendName,
                avatar = message.SendHeadPath,
                type = message.MessageType,
                id = message.ReceiveId,
                sendid = message.SendId,
                content = message.MessageContent,
                mine = false,
                timestamp =message.CreateTime
            });
                });
                
                Clients.User(uid).receiveGroupOffline(objes);
                //发送完则删除
                DepositHelper.RemoveKey(keyb);
            }

        }
    }




}
