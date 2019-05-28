namespace PeliculasEdwin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdicionDeColumnasAspUsers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Nombre", c => c.String(nullable: false, maxLength: 30));
            AddColumn("dbo.AspNetUsers", "Apellidos", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.AspNetUsers", "FechaNacimiento", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "Telefono", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Telefono");
            DropColumn("dbo.AspNetUsers", "FechaNacimiento");
            DropColumn("dbo.AspNetUsers", "Apellidos");
            DropColumn("dbo.AspNetUsers", "Nombre");
        }
    }
}
