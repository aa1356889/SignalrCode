using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalrClient.Model
{
   public class UserFriends
    {
       [Key]
       public int UserFriendsId { get; set; }


       public int UserId { get; set; }

       public int FriendsId { get; set; }
    }
}
