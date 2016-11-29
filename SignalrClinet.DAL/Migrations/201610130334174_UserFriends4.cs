namespace SignalrClinet.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserFriends4 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserFriends",
                c => new
                    {
                        UserFriendsId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        FriendsId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserFriendsId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserFriends");
        }
    }
}
