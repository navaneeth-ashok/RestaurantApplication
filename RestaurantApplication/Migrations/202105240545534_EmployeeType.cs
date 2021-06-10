namespace RestaurantApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmployeeType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "EmployeeType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "EmployeeType");
        }
    }
}
