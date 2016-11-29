using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalrServices.MessageProcess
{
  public  class MessageForwarderFactory
    {
      public static MessageForwarder CreateMessageForwarder(string messageType)
      {
          MessageForwarder forwarder = null;
          switch (messageType)
          {
              case "friend":
              
                 forwarder=new UserMessageForwarder();
                  break;
              case "group":
                  forwarder= new GroupMessageForwarder();
                  break;
              default:
                     forwarder=new UserMessageForwarder();
                  break;

          }
          return forwarder;

      }
    }
}
