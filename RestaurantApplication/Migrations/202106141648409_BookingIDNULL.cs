namespace RestaurantApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BookingIDNULL : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrderItems", "BookingID", "dbo.Bookings");
            DropIndex("dbo.OrderItems", new[] { "BookingID" });
            AlterColumn("dbo.OrderItems", "BookingID", c => c.Int());
            CreateIndex("dbo.OrderItems", "BookingID");
            AddForeignKey("dbo.OrderItems", "BookingID", "dbo.Bookings", "BookingID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderItems", "BookingID", "dbo.Bookings");
            DropIndex("dbo.OrderItems", new[] { "BookingID" });
            AlterColumn("dbo.OrderItems", "BookingID", c => c.Int(nullable: false));
            CreateIndex("dbo.OrderItems", "BookingID");
            AddForeignKey("dbo.OrderItems", "BookingID", "dbo.Bookings", "BookingID", cascadeDelete: true);
        }
    }
}
