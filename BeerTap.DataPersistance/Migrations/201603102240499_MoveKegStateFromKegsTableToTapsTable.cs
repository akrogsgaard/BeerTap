namespace BeerTap.DataPersistance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoveKegStateFromKegsTableToTapsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Taps", "KegState", c => c.String());
            DropColumn("dbo.Kegs", "State");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Kegs", "State", c => c.String());
            DropColumn("dbo.Taps", "KegState");
        }
    }
}
