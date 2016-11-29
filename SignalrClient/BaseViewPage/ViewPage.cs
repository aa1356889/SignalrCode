using SignalrClient.Model;
using SignalrClient.Model.Extend;
using SignalrClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SignalrTool.Redis;
using WebHelper.FormPrincipal;

namespace System.Web.Mvc
{
    public abstract class ViewPageBase<T>: System.Web.Mvc.WebViewPage<T>
    {
        public ViewPageBase()
        {
            if (HttpContext.Current.Request.IsAuthenticated)
            {
                var data=HttpContext.Current.User as MyFormsPrincipal<UserInfo>;
                if (data != null)
                {
                    this.CurrentUser = DepositHelper.GetAsync<UsersExtend>(RedisKeyManager.LoginUserInfo((data).UserData.Id.ToString()));
                }
            }
        }
        public UsersExtend CurrentUser { get; private set; }

        public string PlugUrl(string url)
        {
            return "/plug/" + url;
        }

        public string ScriptUrl(string url)
        {
            return "/Scripts/" + url;
        }
    }

    public abstract class ViewPageBase : WebViewPage
    {

        public string PlugUrl(string url)
        {
            return "/plug/" + url;
        }

        public string ScriptUrl(string url)
        {
            return "/Scripts/" + url;
        }
    }
}