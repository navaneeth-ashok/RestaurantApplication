namespace RestaurantApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FoodTypeID1 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Foods", name: "FoodTypeID_TypeID", newName: "FoodTypeID");
            RenameIndex(table: "dbo.Foods", name: "IX_FoodTypeID_TypeID", newName: "IX_FoodTypeID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Foods", name: "IX_FoodTypeID", newName: "IX_FoodTypeID_TypeID");
            RenameColumn(table: "dbo.Foods", name: "FoodTypeID", newName: "FoodTypeID_TypeID");
        }
    }
}
