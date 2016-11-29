namespace SignalrClinet.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Users3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "UserName", c => c.String(maxLength: 50));
            AlterColumn("dbo.Users", "LoginName", c => c.String(maxLength: 40));
            AlterColumn("dbo.Users", "PassWord", c => c.String(maxLength: 40));
            AlterColumn("dbo.Users", "HeadPath", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "HeadPath", c => c.String());
            AlterColumn("dbo.Users", "PassWord", c => c.String());
            AlterColumn("dbo.Users", "LoginName", c => c.String());
            AlterColumn("dbo.Users", "UserName", c => c.String());
        }
    }
}
