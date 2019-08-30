namespace PeliculasEdwin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OtraColumnaMasTablaVisitas : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Visitas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NumeroVisitas = c.Int(nullable: true),
                        FechaVisita = c.DateTime(nullable: false),
                        Pelicula_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PeliculasEdwin", t => t.Pelicula_Id, cascadeDelete: true)
                .Index(t => t.Pelicula_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Visitas", "Pelicula_Id", "dbo.PeliculasEdwin");
            DropIndex("dbo.Visitas", new[] { "Pelicula_Id" });
            DropTable("dbo.Visitas");
        }
    }
}
