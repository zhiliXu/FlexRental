namespace FlexRental.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class category : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InventoryViews", "Category", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.InventoryViews", "Category");
        }
    }
}
