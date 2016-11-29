using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SignalrClient.BLL;
using SignalrClient.Filter;
using SignalrClient.Model.Extend;
using SignalrClient.Model.ModelView;
using SignalrClient.Model;
using SignalrClient.Models;
using SignalrTool.Redis;
using SignalrServices;

namespace SignalrClient.Controllers
{

 
  
    public class HomeController : BaseController
    {
        private UserBLL userBll = new UserBLL();

        private MenusBLL menuBll = new MenusBLL();
        private OrganBLL _organBll = new OrganBLL();
        private DepartmentBLL _departmentBll = new DepartmentBLL();
        private UserFriendsBLL _userFreiendsBll = new UserFriendsBLL();
        // GET: /Home/
           [LoginValidate]
        public ActionResult Index()
           {
            return View();
        }


        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [LoginValidate]
        public ActionResult GetMenus()
        {
            MenusBLL bll = new MenusBLL();
            var menus = bll.LoadAll(c => true).OrderBy(c => c.Seq).ToList<Menus>();
            return base.Success(menus);
        }


        [HttpPost]
        public ActionResult Login(LoginModelView loginInfo)
        {   
            if (ModelState.IsValid)
            {
               var model= userBll.Get(c => c.LoginName == loginInfo.LoginName&&c.PassWord==loginInfo.Password);
                if (model != null)
                {
                       var extends=  model.toEntity<UsersExtend>();
                        if (model.OrganId != null)
                        {
                            var organ = _organBll.Get(c => c.OrganId == model.OrganId);
                            extends.OrganName = organ.OrganName;
                        }
                    if (model.DepartmentId != null)
                    {
                        var dep = _departmentBll.Get(c => c.DepartmentId == model.DepartmentId);
                        extends.DepartmentName = dep.DepartmentName;
                    }

                    WebHelper.FormPrincipal.MyFormsPrincipal<UserInfo>.SignIn(model.LoginName,new UserInfo(){Id=model.UserId}, 40);
                    //将用户信息存入缓存
                    DepositHelper.Set(RedisKeyManager.LoginUserInfo(model.UserId.ToString()), extends);
                    return Success("登陆成功");
                }
                else
                {
                    return Fail("登陆失败");
                }
            }
            return View(loginInfo);
        }

        /// <summary>
        /// 获得当前用户的信息以及好友信息
        /// </summary>
        /// <returns></returns>
        [LoginValidate]
        public ActionResult GetUserInfo()
        {
            var data= _userFreiendsBll.GetFriedsByUserId(CurrentUser.UserId);
            var groups = new List<object>();
           if(CurrentUser.OrganId!=-1){
               groups.Add( new
                        {
                            groupname=CurrentUser.OrganName,
                            id=CurrentUser.OrganId,
                            avatar="/plug/HeadPath/机构-999999.png",
                        });
               if(CurrentUser.DepartmentId!=-1){
                           groups.Add(new {
                            groupname=CurrentUser.DepartmentName,
                            id=CurrentUser.OrganId+"-"+CurrentUser.DepartmentId,
                            avatar="/plug/HeadPath/部门.png",
                        }); 
               }
           }
          
            return Json(new
            {
                code = 0,
                msg = "",
                data = new
                {
                    mine = new
                    {
                        username = CurrentUser.UserName,
                        id = CurrentUser.UserId,
                        status = "online",
                        remark =CurrentUser.Remark??"这个人很懒什么都没留下",
                        avatar = CurrentUser.HeadPath
                    },
                    friend = new List<object> { 
                         new
                         {
                             groupname="我的好友",id="1",online=0,
                             list=data.Select(c=>new{username=c.UserName,id=c.UserId,avatar=c.HeadPath,sign=c.Remark}).ToArray()
                         }
                    },
                    group = groups
                }
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetMenber(string id)
        {
            var ids= id.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            int orgid =Convert.ToInt32(ids[0]);
            int depid = ids.Length > 1 ?Convert.ToInt32(ids[1]) : -1;
            List<Users> users = null;
            if (depid == -1)
            {
                 users = userBll.LoadAll(c => c.OrganId == orgid).ToList<Users>();
            }
            else
            {
                users = userBll.LoadAll(c => c.OrganId == orgid && c.DepartmentId == depid).ToList<Users>();
            }
           List<object> objes = new List<object>();
           users.ForEach(c =>
           {
               objes.Add(new
               {
                   username = c.UserName,
                   id = c.UserId,
                   avatar = c.HeadPath,
                   sign = c.Remark,
               });
           });
           return Json(new
           {
               code = 0,
               msg = "",
               data = new
               {
                   list = objes
               }
           }, JsonRequestBehavior.AllowGet);

        }


        [LoginValidate]
        public ActionResult Sendmessage(Message message)
        {
            message.CreateTime = DateTime.Now;
            message.SendHeadPath = CurrentUser.HeadPath;
            message.SendName=CurrentUser.UserName;
            DepositHelper.ListLeftPush(RedisKeyManager.MessageQueue, message);
            return base.Success("发送成功");
        }

    }
}
