namespace PeliculasEdwin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProcedimientosAlmacenados : DbMigration
    {
        public override void Up()
        {
            //CreateStoredProcedure("sp_BuscarPeliculas",, @"SELECT T�tulo from PeliculasEdwin where T�tulo LIKE %@valorBusqueda%")
        }
        
        public override void Down()
        {
        }
    }
}
