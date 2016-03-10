namespace BeerTap.DataPersistance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTapIdToKegsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Kegs", "TapId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Kegs", "TapId");
        }
    }
}
