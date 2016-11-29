using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalrClient.Model.Extend
{
  public  class UsersExtend:Users
    {
      public string OrganName { get; set; }

      public string DepartmentName { get; set; }
    }
}
