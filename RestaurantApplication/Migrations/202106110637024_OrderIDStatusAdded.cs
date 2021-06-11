namespace RestaurantApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderIDStatusAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderIDs", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderIDs", "Status");
        }
    }
}
