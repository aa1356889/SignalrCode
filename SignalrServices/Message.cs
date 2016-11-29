using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalrServices
{
  public  class Message
    {
      /// <summary>
      /// 发送人id
      /// </summary>
      public int SendId { get; set; }


      /// <summary>
      /// 发送人名字
      /// </summary>
      public string SendName { get; set; }

      /// <summary>
      /// 发送人头像
      /// </summary>
      public string SendHeadPath { get; set; }


      /// <summary>
      /// 接收id
      /// </summary>
      public string ReceiveId { get; set; }


      /// <summary>
      /// 发送消息
      /// </summary>
      public string MessageContent { get; set; }


      /// <summary>
      /// 消息类型
      /// </summary>
      public string MessageType { get; set; }

      /// <summary>
      /// 消息产生时间
      /// </summary>
      public DateTime CreateTime { get; set; }
    }
}
