namespace Stackoverflow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCommentClass : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        QuestionId = c.Int(),
                        AnswerId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Answers", t => t.AnswerId)
                .ForeignKey("dbo.Questions", t => t.QuestionId)
                .Index(t => t.QuestionId)
                .Index(t => t.AnswerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.Comments", "AnswerId", "dbo.Answers");
            DropIndex("dbo.Comments", new[] { "AnswerId" });
            DropIndex("dbo.Comments", new[] { "QuestionId" });
            DropTable("dbo.Comments");
        }
    }
}
