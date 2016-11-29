using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalrClient.Model
{
    public class Users
    {

        public Users()
        {
            OrganId = -1;
            DepartmentId = -1;
            IsOnline = false;
        }
        [Key]
        public int  UserId { get; set; }

        [MaxLength(50)]
        public string UserName { get; set; }

        [MinLength(6),MaxLength(10)]
        public string LoginName { get; set; }

        [MaxLength(40),Required]

        public string PassWord { get; set; }

         [MaxLength(200)]
        public string HeadPath { get; set; }
        public int OrganId { get; set; }

        [MaxLength(50)]
        public string Remark { get; set; }
        public int DepartmentId { get; set; }
        public string Phone { get; set; }

        public bool IsOnline { get; set; }
    }
}
