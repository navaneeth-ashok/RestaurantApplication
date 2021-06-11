namespace RestaurantApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderItemOrdeIDPrimaryKey : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.OrderItems");
            AddPrimaryKey("dbo.OrderItems", new[] { "FoodID", "BookingID", "OrderIDNumber" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.OrderItems");
            AddPrimaryKey("dbo.OrderItems", new[] { "FoodID", "BookingID" });
        }
    }
}
