using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalrServices
{
   public class Startup
    {
       public void Configuration(IAppBuilder app)
       {
           //注入signlar自定义维护会话id
           GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => new UserIdProvider());
           app.UseCors(CorsOptions.AllowAll);
           app.MapSignalR();
       }
    }
}
