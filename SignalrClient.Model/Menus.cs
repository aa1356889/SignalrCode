using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalrClient.Model
{
   public class Menus
    {
       public Menus()
       {
           ParentId = -1;
           Seq = 1;
       }
       /// <summary>
       /// 菜单id
       /// </summary>
       [Key]
       public int MenuId { get; set; }


       /// <summary>
       /// 父菜单id
       /// </summary>
       public int ParentId { get; set; }

        /// <summary>
        ///菜单名字
        /// </summary>
        [Required,MaxLength(10)]
        public string MenuName { get; set; }

        /// <summary>
        /// 菜单排序号
        /// </summary>
        public int Seq { get; set; }

        /// <summary>
        ///图标class
        /// </summary>
        public string Icon { get; set; }

       /// <summary>
       /// 区域
       /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 控制器
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// action
        /// </summary>
        public string Action { get; set; }
    }
}
