namespace RestaurantApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FoodIngre : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Foods", "IngredientsList", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Foods", "IngredientsList");
        }
    }
}
