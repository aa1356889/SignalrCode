using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalrClient.Model
{
    public class Department
    {

        [Key]
        public int DepartmentId { get; set; }

        [DefaultValue("-1")]
        public int ParentId { get; set; }

        public string DepartmentName { get; set; }

         [DefaultValue("-1")]
        public int OrganId { get; set; }


         public string Path { get; set; }

    }
}
