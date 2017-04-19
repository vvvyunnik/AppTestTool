namespace MVCTestTools.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class first : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        AdminID = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false),
                        Email = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.AdminID);
            
            CreateTable(
                "dbo.Applications",
                c => new
                    {
                        ApplicationID = c.Int(nullable: false, identity: true),
                        ProjectName = c.String(nullable: false, maxLength: 50),
                        Url = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.ApplicationID);
            
            CreateTable(
                "dbo.Tests",
                c => new
                    {
                        TestID = c.Int(nullable: false, identity: true),
                        Name = c.Int(nullable: false),
                        Runtime = c.Double(),
                        IsSuccessful = c.Boolean(),
                        SearchParameter = c.String(maxLength: 100),
                        TestDate = c.DateTime(),
                        Browser = c.String(maxLength: 20),
                        AppId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TestID)
                .ForeignKey("dbo.Applications", t => t.AppId, cascadeDelete: true)
                .Index(t => t.AppId);
            
            CreateTable(
                "dbo.AdminApplication",
                c => new
                    {
                        AdminRefId = c.Int(nullable: false),
                        AppRefId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AdminRefId, t.AppRefId })
                .ForeignKey("dbo.Admins", t => t.AdminRefId, cascadeDelete: true)
                .ForeignKey("dbo.Applications", t => t.AppRefId, cascadeDelete: true)
                .Index(t => t.AdminRefId)
                .Index(t => t.AppRefId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AdminApplication", "AppRefId", "dbo.Applications");
            DropForeignKey("dbo.AdminApplication", "AdminRefId", "dbo.Admins");
            DropForeignKey("dbo.Tests", "AppId", "dbo.Applications");
            DropIndex("dbo.AdminApplication", new[] { "AppRefId" });
            DropIndex("dbo.AdminApplication", new[] { "AdminRefId" });
            DropIndex("dbo.Tests", new[] { "AppId" });
            DropTable("dbo.AdminApplication");
            DropTable("dbo.Tests");
            DropTable("dbo.Applications");
            DropTable("dbo.Admins");
        }
    }
}
