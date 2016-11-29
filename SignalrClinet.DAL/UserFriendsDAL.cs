using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SignalrClient.Model;

namespace SignalrClinet.DAL
{
    public class UserFriendsDAL : RepositoryBase<UserFriends>
    {

        public List<Users> GetFriedsByUserId(int id)
        {
           var userQueryAble=Context.UserFriends.Where(c => c.UserId == id);
            var data = from f in userQueryAble
                join u in Context.Users on f.FriendsId equals u.UserId
                select u;

            return data.ToList();
        }
    }
}
