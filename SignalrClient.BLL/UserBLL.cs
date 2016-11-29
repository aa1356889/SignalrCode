using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SignalrClient.Model;
using SignalrClient.Model.Extend;
using SignalrClinet.DAL;

namespace SignalrClient.BLL
{
    public class UserBLL:BllBase<Users>
    {
        private UsersDAL _userdal = null;
    
        public UserBLL()
            : base(new UsersDAL())
        {
           this._userdal=base.basedall as UsersDAL;
        }

        public List<Users> GetUserList()
        {
            return _userdal.LoadAll(c => true).ToList();
        }

    }
}
