using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SignalrClient.Model;
using SignalrClinet.DAL;

namespace SignalrClient.BLL
{
    public class MenusBLL : BllBase<Menus>
    {
        private MenusDAL _menudal = null;

        public MenusBLL()
            : base(new MenusDAL())
        {
            this._menudal = base.basedall as MenusDAL;
        }

    }
}
