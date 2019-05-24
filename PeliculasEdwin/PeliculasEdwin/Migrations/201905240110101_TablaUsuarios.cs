namespace PeliculasEdwin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TablaUsuarios : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Usuarios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 30),
                        Apellidos = c.String(nullable: false, maxLength: 50),
                        NombreUsuario = c.String(nullable: false, maxLength: 15),
                        FechaNacimiento = c.DateTime(nullable: false),
                        CorreoElectronico = c.String(nullable: false, maxLength: 50),
                        Telefono = c.String(maxLength: 30),
                        Password = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Usuarios");
        }
    }
}
