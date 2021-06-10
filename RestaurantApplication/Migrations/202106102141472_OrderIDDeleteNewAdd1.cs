namespace RestaurantApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderIDDeleteNewAdd1 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.OrdersIDs", newName: "OrderIDTemps");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.OrderIDTemps", newName: "OrdersIDs");
        }
    }
}
