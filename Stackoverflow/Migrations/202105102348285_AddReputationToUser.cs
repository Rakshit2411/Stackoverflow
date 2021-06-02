namespace Stackoverflow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReputationToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Reputation", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Reputation");
        }
    }
}
