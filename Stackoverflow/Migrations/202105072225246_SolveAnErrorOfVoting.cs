namespace Stackoverflow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SolveAnErrorOfVoting : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Answers", "Votes", c => c.Int(nullable: false));
            AddColumn("dbo.Questions", "Votes", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Questions", "Votes");
            DropColumn("dbo.Answers", "Votes");
        }
    }
}
