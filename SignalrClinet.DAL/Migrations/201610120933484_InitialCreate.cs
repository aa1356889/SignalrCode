namespace SignalrClinet.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        DepartmentId = c.Int(nullable: false, identity: true),
                        ParentId = c.Int(nullable: false),
                        DepartmentName = c.String(),
                        OrganId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DepartmentId);
            
            CreateTable(
                "dbo.Organs",
                c => new
                    {
                        OrganId = c.Int(nullable: false, identity: true),
                        ParentId = c.Int(nullable: false),
                        OrganName = c.String(),
                    })
                .PrimaryKey(t => t.OrganId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        LoginName = c.String(),
                        PassWord = c.String(),
                        HeadPath = c.String(),
                        OrganId = c.Int(nullable: false),
                        DepartmentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
            DropTable("dbo.Organs");
            DropTable("dbo.Departments");
        }
    }
}
