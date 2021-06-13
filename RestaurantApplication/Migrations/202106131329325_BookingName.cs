namespace RestaurantApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BookingName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "BookingName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bookings", "BookingName");
        }
    }
}
