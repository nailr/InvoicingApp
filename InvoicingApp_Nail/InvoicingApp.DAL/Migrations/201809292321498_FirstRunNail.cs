namespace InvoicingApp.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstRunNail : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Charges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ChargeSymbol = c.String(maxLength: 450),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.ChargeSymbol, unique: true);
            
            CreateTable(
                "dbo.Invoices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InvoiceNumber = c.String(nullable: false, maxLength: 400),
                        ClientId = c.Int(nullable: false),
                        InvoiceDate = c.DateTime(),
                        StratDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        ChargeId = c.Int(nullable: false),
                        Address = c.String(),
                        Rate = c.Decimal(precision: 18, scale: 2),
                        Units = c.Int(),
                        Amount = c.Decimal(precision: 18, scale: 2),
                        Tax = c.Decimal(precision: 18, scale: 2),
                        Total = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Charges", t => t.ChargeId, cascadeDelete: true)
                .ForeignKey("dbo.Clients", t => t.ClientId, cascadeDelete: true)
                .Index(t => new { t.InvoiceNumber, t.ClientId }, unique: true, name: "IX_InvoiceNumberAndClientId")
                .Index(t => t.ChargeId);
            
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyName = c.String(nullable: false, maxLength: 450),
                        City = c.String(),
                        Address = c.String(),
                        Phone = c.String(),
                        Email = c.String(nullable: false),
                        ContactPerson = c.String(),
                        ContractDateStart = c.DateTime(),
                        ContractDateEnd = c.DateTime(),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.CompanyName, unique: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Invoices", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.Invoices", "ChargeId", "dbo.Charges");
            DropIndex("dbo.Clients", new[] { "CompanyName" });
            DropIndex("dbo.Invoices", new[] { "ChargeId" });
            DropIndex("dbo.Invoices", "IX_InvoiceNumberAndClientId");
            DropIndex("dbo.Charges", new[] { "ChargeSymbol" });
            DropTable("dbo.Clients");
            DropTable("dbo.Invoices");
            DropTable("dbo.Charges");
        }
    }
}
