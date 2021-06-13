namespace RestaurantApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BookingName1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Bookings", "EMailID", c => c.String(nullable: false));
            AlterColumn("dbo.Bookings", "BookingName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Bookings", "BookingName", c => c.String());
            AlterColumn("dbo.Bookings", "EMailID", c => c.String());
        }
    }
}
