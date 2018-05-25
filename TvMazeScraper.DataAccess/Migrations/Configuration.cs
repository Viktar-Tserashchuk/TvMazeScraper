using System.Data.Entity.Migrations;

namespace TvMazeScraper.DataAccess.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<TvShowsContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
    }
}
