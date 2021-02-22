namespace poopi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requests : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Requests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudentId = c.Int(nullable: false),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: true)
                .Index(t => t.StudentId)
                .Index(t => t.GroupId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requests", "StudentId", "dbo.Students");
            DropForeignKey("dbo.Requests", "GroupId", "dbo.Groups");
            DropIndex("dbo.Requests", new[] { "GroupId" });
            DropIndex("dbo.Requests", new[] { "StudentId" });
            DropTable("dbo.Requests");
        }
    }
}
