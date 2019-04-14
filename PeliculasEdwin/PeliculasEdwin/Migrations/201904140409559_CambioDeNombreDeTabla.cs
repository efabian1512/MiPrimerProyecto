namespace PeliculasEdwin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CambioDeNombreDeTabla : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.PeliculasEdwin", newName: "PeliculasEdwin");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.PeliculasEdwin", newName: "PeliculasEdwin");
        }
    }
}
