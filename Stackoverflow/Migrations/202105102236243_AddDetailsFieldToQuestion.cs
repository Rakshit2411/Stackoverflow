namespace Stackoverflow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDetailsFieldToQuestion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "Details", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Questions", "Details");
        }
    }
}
