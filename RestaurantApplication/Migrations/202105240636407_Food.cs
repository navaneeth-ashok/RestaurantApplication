namespace RestaurantApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Food : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Foods",
                c => new
                    {
                        FoodID = c.Int(nullable: false, identity: true),
                        FoodName = c.String(),
                        FoodDescription = c.String(),
                        FoodPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.FoodID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Foods");
        }
    }
}
