namespace PeliculasEdwin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdicionCampoFechaDeacion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PeliculasEdwin", "FechaDeAdicion", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PeliculasEdwin", "FechaDeAdicion");
        }
    }
}
