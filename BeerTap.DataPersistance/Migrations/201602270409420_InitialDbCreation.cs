namespace BeerTap.DataPersistance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDbCreation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Kegs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BeerName = c.String(),
                        Volume = c.Int(nullable: false),
                        State = c.String(),
                        CreatedByUserId = c.Int(nullable: false),
                        CreatedDateUtc = c.DateTime(nullable: false),
                        UpdatedByUserId = c.Int(nullable: false),
                        UpdatedDateUtc = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Offices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CreatedByUserId = c.Int(nullable: false),
                        CreatedDateUtc = c.DateTime(nullable: false),
                        UpdatedByUserId = c.Int(nullable: false),
                        UpdatedDateUtc = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Taps",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OfficeId = c.Int(nullable: false),
                        KegId = c.Int(nullable: false),
                        CreatedByUserId = c.Int(nullable: false),
                        CreatedDateUtc = c.DateTime(nullable: false),
                        UpdatedByUserId = c.Int(nullable: false),
                        UpdatedDateUtc = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Offices", t => t.OfficeId, cascadeDelete: true)
                .Index(t => t.OfficeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Taps", "OfficeId", "dbo.Offices");
            DropIndex("dbo.Taps", new[] { "OfficeId" });
            DropTable("dbo.Taps");
            DropTable("dbo.Offices");
            DropTable("dbo.Kegs");
        }
    }
}
