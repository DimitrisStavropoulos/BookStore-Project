namespace Store01.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reseed3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Books", "Code", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.Books", "Title", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Books", "Description", c => c.String(nullable: false, maxLength: 500));
            AlterColumn("dbo.Books", "Isbn", c => c.String(nullable: false, maxLength: 14));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Books", "Isbn", c => c.String());
            AlterColumn("dbo.Books", "Description", c => c.String());
            AlterColumn("dbo.Books", "Title", c => c.String());
            AlterColumn("dbo.Books", "Code", c => c.String());
        }
    }
}
