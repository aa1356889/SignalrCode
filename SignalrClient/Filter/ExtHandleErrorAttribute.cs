using SignalrClient.Bind;
using SignalrClient.Model.Extend;
using SignalrClient.Models;
using SignalrTool.JsonHelper;
using SignalrTool.Log4;
using SignalrTool.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebHelper.ActionJsonProcessResult;
using WebHelper.ClientIp;
using WebHelper.FormPrincipal;

namespace SignalrClient.Filter
{
    /// <summary>
    /// 自定义错误过滤器。全局捕获错误信息
    /// </summary>
    public class ExtHandleErrorAttribute : HandleErrorAttribute
    {
        //日志记录
        protected IExtLog log = ExtLogManager.GetLogger("dblog");
        public override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;//表示已经处理 不需要mvc框架处理
            var UserData =HttpContext.Current.User as MyFormsPrincipal<UserInfo>;//当前操作用户
            UsersExtend curentUser = null;
            var routeValueDic = filterContext.RouteData.Values;
            string actionParameterInfo;
            try
            {
                actionParameterInfo = GetActionParametersInfoStr(filterContext);
            }
            catch
            {
                actionParameterInfo = "参数信息：不能处理参数绑定";
            }
            if (UserData != null)
            {
                curentUser = DepositHelper.GetAsync<UsersExtend>(RedisKeyManager.LoginUserInfo(UserData.UserData.Id.ToString()));
            }
            //如果是ajax请求则返回json格式数据  否则跳转到错误页面
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                JsonResult json = new JsonResult();
                json.Data = new ProcessResult() { State = ProcessEnum.Fail, Message = filterContext.Exception.Message };
                filterContext.Result = json;
            }
            else
            {

                filterContext.Result = new RedirectResult("/plug/Error/Error404.html");
             
            }
            log.Error(ClientIP.GetIp(filterContext.RequestContext.HttpContext.ApplicationInstance.Request), curentUser == null ? "未登陆" : curentUser.LoginName, filterContext.HttpContext.Request.UrlReferrer.ToString(), routeValueDic["action"].ToString(), "mvc全局捕获异常", filterContext.Exception, actionParameterInfo);//写入日志
            base.OnException(filterContext);
        }

        /// <summary>
        /// 获得错误action的参数信息 与值
        /// 2016-09-22 新增 李强
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>

        public string GetActionParametersInfoStr(ExceptionContext filterContext)
        {
            var sb = new StringBuilder();
            var bind = new MyArrayBind();
            var c = new ModelBindingContext { ValueProvider = filterContext.Controller.ValueProvider };
            var type = filterContext.Controller.GetType();
            var methods = type.GetMethods();
            var actionMethod =
                methods.Where(b => b.Name.ToLower().Equals(filterContext.RouteData.Values["action"].ToString().ToLower())).ToList();
            MethodInfo processMethed = null;
            if (actionMethod.Count >= 2)
            {
                processMethed = filterContext.HttpContext.Request.HttpMethod.ToLower().Equals("get") ? actionMethod.FirstOrDefault(d => d.GetCustomAttribute<HttpGetAttribute>() != null) : actionMethod.FirstOrDefault(d => d.GetCustomAttribute<HttpPostAttribute>() != null);
            }
            else
            {
                processMethed = actionMethod[0];
            }
            bind.bindingContext = c;
            if (processMethed != null)
            {
                var parameters = processMethed.GetParameters();
                foreach (var parameterInfo in parameters)
                {

                    var obj = bind.BindingArray(parameterInfo.ParameterType, parameterInfo.Name);
                    string parameterValue = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                    sb.Append(string.Format("参数名:{0}, 参数类型:{1}, 参数值:{2}\r\n", parameterInfo.Name, parameterInfo.ParameterType.ToString(), parameterValue));
                }
            }
            return sb.ToString();



        }
    }
}