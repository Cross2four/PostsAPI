namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AppUserUniqueUsername : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AppUsers", "UserName", c => c.String(maxLength: 100));
            CreateIndex("dbo.AppUsers", "UserName", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.AppUsers", new[] { "UserName" });
            AlterColumn("dbo.AppUsers", "UserName", c => c.String());
        }
    }
}
