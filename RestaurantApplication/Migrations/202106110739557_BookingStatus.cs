namespace RestaurantApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BookingStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bookings", "status");
        }
    }
}
