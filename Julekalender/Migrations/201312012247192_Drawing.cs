namespace Julekalender.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Drawing : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Drawings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Time = c.DateTime(nullable: false),
                        Winner_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Participants", t => t.Winner_Id)
                .Index(t => t.Winner_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Drawings", "Winner_Id", "dbo.Participants");
            DropIndex("dbo.Drawings", new[] { "Winner_Id" });
            DropTable("dbo.Drawings");
        }
    }
}
