namespace RestaurantApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FoodTypeID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Foods", "FoodTypeID_TypeID", c => c.Int());
            CreateIndex("dbo.Foods", "FoodTypeID_TypeID");
            AddForeignKey("dbo.Foods", "FoodTypeID_TypeID", "dbo.FoodTypes", "TypeID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Foods", "FoodTypeID_TypeID", "dbo.FoodTypes");
            DropIndex("dbo.Foods", new[] { "FoodTypeID_TypeID" });
            DropColumn("dbo.Foods", "FoodTypeID_TypeID");
        }
    }
}
