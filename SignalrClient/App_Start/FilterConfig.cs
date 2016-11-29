using SignalrClient.Filter;
using System.Web;
using System.Web.Mvc;

namespace SignalrClient
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ExtHandleErrorAttribute());//注册全局错误过滤器
        }
    }
}