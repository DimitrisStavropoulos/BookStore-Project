namespace Store01.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderOrderLines2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderLines",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OrderID = c.Int(nullable: false),
                        BookId = c.Int(),
                        Quantity = c.Int(nullable: false),
                        BookTitle = c.String(),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Books", t => t.BookId)
                .ForeignKey("dbo.Orders", t => t.OrderID, cascadeDelete: true)
                .Index(t => t.OrderID)
                .Index(t => t.BookId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderID = c.Int(nullable: false, identity: true),
                        UserID = c.String(),
                        DeliveryName = c.String(),
                        DeliveryAddress_AddressLine = c.String(nullable: false),
                        DeliveryAddress_City = c.String(nullable: false),
                        DeliveryAddress_Postcode = c.String(nullable: false),
                        TotalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.OrderID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderLines", "OrderID", "dbo.Orders");
            DropForeignKey("dbo.OrderLines", "BookId", "dbo.Books");
            DropIndex("dbo.OrderLines", new[] { "BookId" });
            DropIndex("dbo.OrderLines", new[] { "OrderID" });
            DropTable("dbo.Orders");
            DropTable("dbo.OrderLines");
        }
    }
}
