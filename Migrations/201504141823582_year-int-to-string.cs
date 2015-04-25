namespace FlexRental.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class yearinttostring : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.InventoryViews", "Year", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.InventoryViews", "Year", c => c.Int(nullable: false));
        }
    }
}
