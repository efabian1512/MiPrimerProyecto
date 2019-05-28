namespace PeliculasEdwin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RetiroDeColumnaTelefonoAspNet : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "Telefono");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Telefono", c => c.String());
        }
    }
}
