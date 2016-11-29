using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalrClient.Model
{
  public class Organ
    {

      public Organ()
      {
          ParentId = -1;
      }
      [Key]
      public int OrganId { get; set; }

      public int ParentId { get; set; }
      public string OrganName { get; set; }

      public string Path { get; set; }
    }
}
