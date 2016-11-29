using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SignalrClient.Model;
using SignalrClient.Model.Extend;
using SignalrClient.Models;
using WebHelper.ActionJsonProcessResult;
using SignalrTool.Log4;

namespace SignalrClient.Controllers
{
    public class BaseController : Controller
    {

        //当前登录用户
        public UsersExtend CurrentUser { get; set; }

        protected JsonResult Success(string message)
        {
            return Json(GetProcessResult(ProcessEnum.Success, message));
        }

        protected JsonResult Success(object data)
        {
            return Json(GetProcessResult(ProcessEnum.Success,string.Empty,data));
        }

        protected JsonResult Fail(string message)
        {
            return Json(GetProcessResult(ProcessEnum.Fail, message));
        }

        protected JsonResult NoLogin(string message, string url)
        {
            return Json(GetProcessResult(ProcessEnum.NoLogin, message));
        }

        private ProcessResult GetProcessResult(ProcessEnum state, string message,string url,object data)
        {
            return new ProcessResult() { State=state, Message = message ,Url=url,Data=data};
        }
        private ProcessResult GetProcessResult(ProcessEnum state, string message,object data=null)
        {
            return GetProcessResult(state, message,null,data);
        }
        //日志记录
       protected  IExtLog log = ExtLogManager.GetLogger("dblog");
    }
}
