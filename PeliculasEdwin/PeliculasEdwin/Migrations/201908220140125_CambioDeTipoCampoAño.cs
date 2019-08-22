namespace PeliculasEdwin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CambioDeTipoCampoAño : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PeliculasEdwin", "Año", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PeliculasEdwin", "Año", c => c.String(nullable: false, maxLength: 8));
        }
    }
}
