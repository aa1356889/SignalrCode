using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalrClient.Model.ModelView
{
   public class LoginModelView
    {
       [Required(ErrorMessage="请填写登录名"), DisplayName("登陆账号")]
       public string LoginName { get; set; }

       [Required(ErrorMessage = "请输入密码"), DisplayName("登陆密码")]
       public string Password { get; set; }
    }
}
