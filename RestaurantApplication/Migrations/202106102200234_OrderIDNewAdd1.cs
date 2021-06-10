namespace RestaurantApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderIDNewAdd1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderIDs",
                c => new
                    {
                        OrderIDNumber = c.Int(nullable: false, identity: true),
                        OrderIDTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.OrderIDNumber);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.OrderIDs");
        }
    }
}
