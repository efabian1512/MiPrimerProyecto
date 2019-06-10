namespace PeliculasEdwin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdicionColumnaEstado : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Estado", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Estado");
        }
    }
}
