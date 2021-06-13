namespace RestaurantApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BookingEmailPhoneNumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "PhoneNumber", c => c.String());
            AddColumn("dbo.Bookings", "EMailID", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bookings", "EMailID");
            DropColumn("dbo.Bookings", "PhoneNumber");
        }
    }
}
