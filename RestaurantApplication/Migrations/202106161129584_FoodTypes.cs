namespace RestaurantApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FoodTypes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FoodTypes",
                c => new
                    {
                        TypeID = c.Int(nullable: false, identity: true),
                        TypeName = c.String(),
                        TypeDisplayName = c.String(),
                    })
                .PrimaryKey(t => t.TypeID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.FoodTypes");
        }
    }
}
