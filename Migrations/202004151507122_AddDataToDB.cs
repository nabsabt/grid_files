namespace Feladat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDataToDB : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FileModels", "FileName", c => c.String(nullable: false, maxLength: 64));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.FileModels", "FileName", c => c.String());
        }
    }
}
