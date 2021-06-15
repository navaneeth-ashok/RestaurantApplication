namespace RestaurantApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TotalAMTOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderIDs", "TotalAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderIDs", "TotalAmount");
        }
    }
}
