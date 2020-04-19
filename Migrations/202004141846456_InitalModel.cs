namespace Feladat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitalModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FileModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        Extension = c.String(),
                        UploadedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.FileModels");
        }
    }
}
