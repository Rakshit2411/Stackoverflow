namespace Stackoverflow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserIdToAnswer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Answers", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Answers", "UserId");
            AddForeignKey("dbo.Answers", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Answers", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Answers", new[] { "UserId" });
            DropColumn("dbo.Answers", "UserId");
        }
    }
}
