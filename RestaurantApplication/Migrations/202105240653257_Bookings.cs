namespace RestaurantApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Bookings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        BookingID = c.Int(nullable: false, identity: true),
                        TableNumber = c.Int(nullable: false),
                        NumberOfOccupants = c.Int(nullable: false),
                        Allergies = c.String(),
                        BookingDateTime = c.DateTime(nullable: false),
                        UserID = c.String(),
                    })
                .PrimaryKey(t => t.BookingID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Bookings");
        }
    }
}
