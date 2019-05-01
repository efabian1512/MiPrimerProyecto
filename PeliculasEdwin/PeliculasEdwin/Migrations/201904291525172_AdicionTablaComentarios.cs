 namespace PeliculasEdwin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdicionTablaComentarios : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comentarios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Contenido = c.String(),
                        Pelicula_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PeliculasEdwin", t => t.Pelicula_Id, cascadeDelete: true)
                .Index(t => t.Pelicula_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comentarios", "Pelicula_Id", "dbo.PeliculasEdwin");
            DropIndex("dbo.Comentarios", new[] { "Pelicula_Id" });
            DropTable("dbo.Comentarios");
        }
    }
}
