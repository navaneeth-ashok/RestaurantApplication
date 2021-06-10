namespace RestaurantApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FoodCountReview : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Foods", "OrderCount", c => c.Int(nullable: false));
            AddColumn("dbo.Foods", "FoodReviewStar", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Foods", "FoodReviewStar");
            DropColumn("dbo.Foods", "OrderCount");
        }
    }
}
