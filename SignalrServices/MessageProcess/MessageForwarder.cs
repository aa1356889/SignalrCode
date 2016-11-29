using Microsoft.AspNet.SignalR;

namespace SignalrServices.MessageProcess
{
   public abstract class MessageForwarder
   {
       public abstract void MessageProcess(Message message,IHubContext hucontext);
   }
}
