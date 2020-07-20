namespace Intelemark.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StringToFloatEnCampaign : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Campaigns", "BillingPH", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Campaigns", "BillingPH", c => c.String());
        }
    }
}
