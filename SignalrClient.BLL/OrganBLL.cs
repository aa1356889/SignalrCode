using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SignalrClient.Model;
using SignalrClinet.DAL;

namespace SignalrClient.BLL
{
   public class OrganBLL : BllBase<Organ>
    {
        private OrganDAL _menudal = null;

        public OrganBLL()
            : base(new OrganDAL())
        {
            this._menudal = base.basedall as OrganDAL;
        }

    }
}
