namespace PeliculasEdwin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProcedimientosAlmacenados : DbMigration
    {
        public override void Up()
        {
            //CreateStoredProcedure("sp_BuscarPeliculas",, @"SELECT Título from PeliculasEdwin where Título LIKE %@valorBusqueda%")
        }
        
        public override void Down()
        {
        }
    }
}
