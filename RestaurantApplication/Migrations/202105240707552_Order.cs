namespace RestaurantApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Order : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderIDs",
                c => new
                    {
                        OrderIDNumber = c.String(nullable: false, maxLength: 128),
                        OrderIDTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.OrderIDNumber);
            
            CreateTable(
                "dbo.OrderItems",
                c => new
                    {
                        FoodID = c.Int(nullable: false),
                        BookingID = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        FoodPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SoldPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OrderIDNumber = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.FoodID, t.BookingID })
                .ForeignKey("dbo.Bookings", t => t.BookingID, cascadeDelete: true)
                .ForeignKey("dbo.Foods", t => t.FoodID, cascadeDelete: true)
                .ForeignKey("dbo.OrderIDs", t => t.OrderIDNumber)
                .Index(t => t.FoodID)
                .Index(t => t.BookingID)
                .Index(t => t.OrderIDNumber);
            
            AddColumn("dbo.Foods", "OfferPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderItems", "OrderIDNumber", "dbo.OrderIDs");
            DropForeignKey("dbo.OrderItems", "FoodID", "dbo.Foods");
            DropForeignKey("dbo.OrderItems", "BookingID", "dbo.Bookings");
            DropIndex("dbo.OrderItems", new[] { "OrderIDNumber" });
            DropIndex("dbo.OrderItems", new[] { "BookingID" });
            DropIndex("dbo.OrderItems", new[] { "FoodID" });
            DropColumn("dbo.Foods", "OfferPrice");
            DropTable("dbo.OrderItems");
            DropTable("dbo.OrderIDs");
        }
    }
}
