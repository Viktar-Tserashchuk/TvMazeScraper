using System;
using System.Data.Entity.Infrastructure;

namespace TvMazeScraper.DataAccess
{
    public class TvShowsContextFactory : IDbContextFactory<TvShowsContext>
    {
        TvShowsContext IDbContextFactory<TvShowsContext>.Create()
        {
            var connectionstring = Environment.GetEnvironmentVariable("TvShowsConnectionString");
            return new TvShowsContext(connectionstring);
        }
    }
}
