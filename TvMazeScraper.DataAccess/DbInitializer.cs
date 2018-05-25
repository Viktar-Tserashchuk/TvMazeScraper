using System.Data.Entity;
using TvMazeScraper.DataAccess.Migrations;

namespace TvMazeScraper.DataAccess
{
    internal class DbInitializer : MigrateDatabaseToLatestVersion<TvShowsContext, Configuration>
    {
    }
}
