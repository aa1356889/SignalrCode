using SignalrClient.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SignalrTool.Redis;
using SignalrClient.Model;

namespace SignalrServices.MessageProcess
{
   public class GroupMessageForwarder:MessageForwarder
   {
       private UserBLL userbll = new UserBLL();
        public override void MessageProcess(Message message, Microsoft.AspNet.SignalR.IHubContext hucontext)
        {
            int orgid=-1;
            int depid =-1;
           var ids=  message.ReceiveId.Split(new char[] {'-'}, StringSplitOptions.RemoveEmptyEntries);
            if (ids.Length == 2)
            {
                orgid = Convert.ToInt32(ids[0]);
                depid = Convert.ToInt32(ids[1]);
            }
            else
            {
                orgid = Convert.ToInt32(ids[0]);
            }
            List<Users> groupuser= null;
            if (depid == -1)
                groupuser = userbll.LoadAll(c => c.OrganId == orgid &&c.UserId!=message.SendId).ToList();
            else
                groupuser = userbll.LoadAll(c => c.OrganId == orgid&&c.DepartmentId==depid&&c.UserId!=message.SendId).ToList();
            groupuser.ForEach(c =>
            {
                if (MyHub.Usermananger.Contains(c.UserId.ToString())) return;
                DepositHelper.ListRightPush(RedisKeyManager.OfflineGroupMessageQueue(c.UserId.ToString()), message);

            });
       
            hucontext.Clients.Group(message.ReceiveId.ToString()).receive(new
            {
                username = message.SendName,
                avatar = message.SendHeadPath,
                type =message.MessageType,
                id = message.ReceiveId,
                sendid=message.SendId,
                content = message.MessageContent,
                mine = false,
                timestamp = DateTime.Now
            });
        }
    }
}
