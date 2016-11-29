namespace SignalrClinet.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Users5 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "LoginName", c => c.String(maxLength: 10));
            AlterColumn("dbo.Users", "PassWord", c => c.String(nullable: false, maxLength: 40));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "PassWord", c => c.String(maxLength: 40));
            AlterColumn("dbo.Users", "LoginName", c => c.String(maxLength: 40));
        }
    }
}
