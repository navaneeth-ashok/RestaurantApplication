namespace RestaurantApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderIDINT : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrderItems", "OrderIDNumber", "dbo.OrderIDs");
            DropIndex("dbo.OrderItems", new[] { "OrderIDNumber" });
            DropPrimaryKey("dbo.OrderIDs");
            AlterColumn("dbo.OrderIDs", "OrderIDNumber", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.OrderItems", "OrderIDNumber", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.OrderIDs", "OrderIDNumber");
            CreateIndex("dbo.OrderItems", "OrderIDNumber");
            AddForeignKey("dbo.OrderItems", "OrderIDNumber", "dbo.OrderIDs", "OrderIDNumber", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderItems", "OrderIDNumber", "dbo.OrderIDs");
            DropIndex("dbo.OrderItems", new[] { "OrderIDNumber" });
            DropPrimaryKey("dbo.OrderIDs");
            AlterColumn("dbo.OrderItems", "OrderIDNumber", c => c.String(maxLength: 128));
            AlterColumn("dbo.OrderIDs", "OrderIDNumber", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.OrderIDs", "OrderIDNumber");
            CreateIndex("dbo.OrderItems", "OrderIDNumber");
            AddForeignKey("dbo.OrderItems", "OrderIDNumber", "dbo.OrderIDs", "OrderIDNumber");
        }
    }
}
