namespace RestaurantApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderIDNewUpdateFK : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderItems", "OrderIDNumber", c => c.Int(nullable: false));
            CreateIndex("dbo.OrderItems", "OrderIDNumber");
            AddForeignKey("dbo.OrderItems", "OrderIDNumber", "dbo.OrderIDs", "OrderIDNumber", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderItems", "OrderIDNumber", "dbo.OrderIDs");
            DropIndex("dbo.OrderItems", new[] { "OrderIDNumber" });
            DropColumn("dbo.OrderItems", "OrderIDNumber");
        }
    }
}
