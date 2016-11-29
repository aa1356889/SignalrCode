using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SignalrClient.Filter;

namespace SignalrClient.Controllers
{
    /// <summary>
    /// 我的控制台
    /// </summary>
        [LoginValidate]
    public class MyConsoleController:BaseController
    {
        //
        // GET: /MyConsole/

       
        public ActionResult Index()
        {
            return View();
        }

    }
}
