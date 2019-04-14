namespace PeliculasEdwin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdicionDeCampos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PeliculasEdwin", "Año", c => c.String(nullable: false, maxLength: 8));
            AddColumn("dbo.PeliculasEdwin", "RutaDeImagen", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PeliculasEdwin", "RutaDeImagen");
            DropColumn("dbo.PeliculasEdwin", "Año");
        }
    }
}
