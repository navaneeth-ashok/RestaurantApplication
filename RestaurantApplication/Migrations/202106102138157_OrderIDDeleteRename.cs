namespace RestaurantApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderIDDeleteRename : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.OrderIDs", newName: "OrdersIDs");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.OrdersIDs", newName: "OrderIDs");
        }
    }
}
