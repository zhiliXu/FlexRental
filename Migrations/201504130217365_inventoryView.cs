namespace FlexRental.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inventoryView : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InventoryViews", "Description", c => c.String());
            AddColumn("dbo.InventoryViews", "Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.InventoryViews", "Date");
            DropColumn("dbo.InventoryViews", "Description");
        }
    }
}
