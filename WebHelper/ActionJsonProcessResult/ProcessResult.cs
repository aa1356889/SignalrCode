using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebHelper.ActionJsonProcessResult
{
  public  class ProcessResult
    {
      /// <summary>
      /// 处理结果
      /// </summary>
      public ProcessEnum State { get; set; }
      /// <summary>
      /// 提示消息
      /// </summary>
      public string Message { get; set; }

      /// <summary>
      /// 跳转链接
      /// </summary
      public string Url { get; set; }

      /// <summary>
      /// 返回数据
      /// </summary>
      public object Data { get; set; }
    }
}
