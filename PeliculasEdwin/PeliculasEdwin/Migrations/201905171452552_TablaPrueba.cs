namespace PeliculasEdwin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TablaPrueba : DbMigration
    {
        public override void Up()
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
        
        public override void Down()
        {
            DropTable("dbo.Estudiantes");
        }
    }
}
