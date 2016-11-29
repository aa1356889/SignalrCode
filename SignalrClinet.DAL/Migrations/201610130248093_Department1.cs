namespace SignalrClinet.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Department1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Departments", "Path", c => c.String());
            AddColumn("dbo.Organs", "Path", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Organs", "Path");
            DropColumn("dbo.Departments", "Path");
        }
    }
}
