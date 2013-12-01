namespace Julekalender.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Participant : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Participants",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Participants");
        }
    }
}
