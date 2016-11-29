using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalrClient.Model
{
   public class SYSLog
    {
       [Key]
        public int ID { get; set; }

       public DateTime? Dates { get; set; }


       [MaxLength(20)]
       public string Levels { get; set; }

       [MaxLength(200)]
       public string Logger { get; set; }

       [MaxLength(100)]
       public string ClientUser { get; set; }

       [MaxLength(20)]
       public string ClientIp { get; set; }

       [MaxLength(500)]
       public string RequestUrl{get;set;}

       [MaxLength(20)]
       public string Action { get; set; }

       [MaxLength(4000)]
       public string Message { get; set; }
       [MaxLength(4000)]
       public string Exception { get; set; }

       [MaxLength(2000)]
       public string PrameterInfo { get; set; }


    }
}
