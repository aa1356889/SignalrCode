using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SignalrClient.Model;
using SignalrClinet.DAL;

namespace SignalrClient.BLL
{
   public class UserFriendsBLL:BllBase<UserFriends>
    {
        private UserFriendsDAL _userFriendsdal = null;

        public UserFriendsBLL()
            : base(new UserFriendsDAL())
        {
           this._userFriendsdal=base.basedall as UserFriendsDAL;
        }

      public List<Users> GetFriedsByUserId(int id)
       {
           return _userFriendsdal.GetFriedsByUserId(id);
       }
    }
}
