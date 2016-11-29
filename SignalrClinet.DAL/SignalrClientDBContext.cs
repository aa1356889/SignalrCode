using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SignalrClient.Model;

namespace SignalrClinet.DAL
{
    public class SignalrClientDbContext : DbContext
    {
        public SignalrClientDbContext()
            : base("EFDbContext")
        {
            
        }
        public DbSet<Department> Department { get; set; }
        public DbSet<Organ> Organ { get; set; }
        public DbSet<Users> Users { get; set; }

        public DbSet<UserFriends> UserFriends { get; set; }
        public DbSet<Menus> Menus { get; set; }


        public DbSet<SYSLog> SYSLog { get; set; }
        public void FixEfProviderServicesProblem() { 
            //The Entity Framework provider type 'System.Data.Entity.SqlServer.SqlProviderServices, EntityFramework.SqlServer'
            //for the 'System.Data.SqlClient' ADO.NET provider could not be loaded. 
            //Make sure the provider assembly is available to the running application. 
            //See http://go.microsoft.com/fwlink/?LinkId=260882 for more information. 
            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance; 
        }
    }
}
