using SignalrClient.Controllers;
using SignalrClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SignalrClient.Model.Extend;
using SignalrClient.Models;
using SignalrTool.Redis;
using WebHelper.FormPrincipal;

namespace SignalrClient.Filter
{
    public class LoginValidateAttribute:System.Web.Mvc.AuthorizeAttribute
    {
        public override void OnAuthorization(System.Web.Mvc.AuthorizationContext filterContext)
        {
            MyFormsPrincipal<UserInfo>.TrySetUserInfo(HttpContext.Current);
            if (!filterContext.HttpContext.Request.IsAuthenticated)
            {
                filterContext.Result = new RedirectResult("/Home/Login");
            }
            else
            {
                var baseContorller = filterContext.Controller as BaseController;
                baseContorller.CurrentUser = DepositHelper.GetAsync<UsersExtend>(RedisKeyManager.LoginUserInfo((HttpContext.Current.User as MyFormsPrincipal<UserInfo>).UserData.Id.ToString())); ;
            }
            base.OnAuthorization(filterContext);
        }
    }
}