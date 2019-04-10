namespace PeliculasEdwin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreacionTablaPeliculasEdwin : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PeliculasEdwin",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Título = c.String(nullable: false, maxLength: 100),
                        Género = c.String(nullable: false, maxLength: 20),
                        Duración = c.String(nullable: false, maxLength: 10),
                        País = c.String(maxLength: 20),
                        EnCarTelera = c.Boolean(nullable: false),
                        Sinopsis = c.String(nullable: false, maxLength: 500),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PeliculasEdwin");
        }
    }
}
