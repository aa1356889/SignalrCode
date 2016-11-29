namespace SignalrClinet.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SYSLog1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SYSLogs", "PrameterInfo", c => c.String(maxLength: 2000));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SYSLogs", "PrameterInfo");
        }
    }
}
