namespace RestaurantApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class foodstring : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Foods", "IngredientsList");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Foods", "IngredientsList", c => c.String());
        }
    }
}
