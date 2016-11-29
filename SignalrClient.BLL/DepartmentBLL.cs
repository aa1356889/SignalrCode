using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SignalrClient.Model;
using SignalrClinet.DAL;

namespace SignalrClient.BLL
{
   public class DepartmentBLL:BllBase<Department>
    {
        private  DepartmentDAL _departmentdal = null;

        public DepartmentBLL()
            : base(new DepartmentDAL())
        {
            this._departmentdal = base.basedall as DepartmentDAL;
        }
    }
}
