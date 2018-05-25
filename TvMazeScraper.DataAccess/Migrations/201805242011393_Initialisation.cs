namespace TvMazeScraper.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initialisation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Actors",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        Name = c.String(),
                        Birthday = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Shows",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ShowActor",
                c => new
                    {
                        ShowId = c.Long(nullable: false),
                        ActorId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.ShowId, t.ActorId })
                .ForeignKey("dbo.Shows", t => t.ShowId, cascadeDelete: true)
                .ForeignKey("dbo.Actors", t => t.ActorId, cascadeDelete: true)
                .Index(t => t.ShowId)
                .Index(t => t.ActorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShowActor", "ActorId", "dbo.Actors");
            DropForeignKey("dbo.ShowActor", "ShowId", "dbo.Shows");
            DropIndex("dbo.ShowActor", new[] { "ActorId" });
            DropIndex("dbo.ShowActor", new[] { "ShowId" });
            DropTable("dbo.ShowActor");
            DropTable("dbo.Shows");
            DropTable("dbo.Actors");
        }
    }
}
