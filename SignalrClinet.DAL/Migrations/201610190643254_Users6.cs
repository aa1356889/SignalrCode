namespace SignalrClinet.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Users6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Remark", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Remark");
        }
    }
}
