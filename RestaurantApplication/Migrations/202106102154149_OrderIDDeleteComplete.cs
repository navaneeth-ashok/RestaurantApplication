namespace RestaurantApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderIDDeleteComplete : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrderItems", "OrderIDNumber", "dbo.OrderIDTemps");
            DropIndex("dbo.OrderItems", new[] { "OrderIDNumber" });
            DropColumn("dbo.OrderItems", "OrderIDNumber");
            DropTable("dbo.OrderIDTemps");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.OrderIDTemps",
                c => new
                    {
                        OrderIDNumber = c.Int(nullable: false, identity: true),
                        OrderIDTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.OrderIDNumber);
            
            AddColumn("dbo.OrderItems", "OrderIDNumber", c => c.Int(nullable: false));
            CreateIndex("dbo.OrderItems", "OrderIDNumber");
            AddForeignKey("dbo.OrderItems", "OrderIDNumber", "dbo.OrderIDTemps", "OrderIDNumber", cascadeDelete: true);
        }
    }
}
