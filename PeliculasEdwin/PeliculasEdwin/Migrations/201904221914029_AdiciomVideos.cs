namespace PeliculasEdwin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdiciomVideos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PeliculasEdwin", "RutaDeVideo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PeliculasEdwin", "RutaDeVideo");
        }
    }
}
