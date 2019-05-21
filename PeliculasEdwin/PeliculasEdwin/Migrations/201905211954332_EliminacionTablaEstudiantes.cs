namespace PeliculasEdwin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EliminacionTablaEstudiantes : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.Estudiantes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Estudiantes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        semestre = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
    }
}
