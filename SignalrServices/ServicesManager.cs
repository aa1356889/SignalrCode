using System.Reflection;
using System.Threading;
using Microsoft.Owin;
using Microsoft.Owin.Hosting;
using SignalrServices;
using SignalrServices.MessageProcess;
using SignalrTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using SignalrTool.Redis;
[assembly: OwinStartup(typeof(Startup))]
namespace SignalrServices
{
    public class ServicesManager
    {
   
        private IDisposable SignalR { get; set; }

        private IHubContext HubContext { get; set; }
        /// <summary>
        /// 在线用户管理类
        /// </summary>
        UserManager _userManager = new UserManager();

        private string _servicesUrl;//服务器监听地址
        public ServicesManager(string servicesUrl)
        {
            _servicesUrl = servicesUrl;
        }

        /// <summary>
        /// 开启服务
        /// </summary>
        /// <param name="successCallback"></param>
        public void Star(Action<bool,string> successCallback)
        {
          
            try
            {
                SignalR = WebApp.Start(_servicesUrl);
                successCallback(true, "服务器启动成功");
                //开始监听消息队列
          
                Task.Factory.StartNew(() =>
                {
                    while (true)
                    {
                        Thread.Sleep(1000);
                        try
                        {
                           
                            var message= SignalrTool.Redis.DepositHelper.ListLeftPop<Message>(RedisKeyManager.MessageQueue);
                            if (message == null) continue;
                            HubContext = (GlobalHost.ConnectionManager.GetHubContext<MyHub>() as IHubContext);
                           var messageForwarder= MessageForwarderFactory.CreateMessageForwarder(message.MessageType);
                            messageForwarder.MessageProcess(message, HubContext);

                        }
                        catch (Exception e)
                        {
                            
                        }
                    }
                });

            }
            catch (TargetInvocationException e)
            {
                successCallback(false, e.Message);
            }
            
        }
    }

   
}
