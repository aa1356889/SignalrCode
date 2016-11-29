namespace SignalrClinet.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SYSLog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SYSLogs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Dates = c.DateTime(),
                        Levels = c.String(maxLength: 20),
                        Logger = c.String(maxLength: 200),
                        ClientUser = c.String(maxLength: 100),
                        ClientIp = c.String(maxLength: 20),
                        RequestUrl = c.String(maxLength: 500),
                        Action = c.String(maxLength: 20),
                        Message = c.String(maxLength: 4000),
                        Exception = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SYSLogs");
        }
    }
}
