namespace BeerTap.DataPersistance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCapacityColumnToKegsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Kegs", "Capacity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Kegs", "Capacity");
        }
    }
}
