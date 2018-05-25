using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using TvMazeScraper.Core.DataAccess;
using TvMazeScraper.Core.Model;

namespace TvMazeScraper.DataAccess.ShowRepository
{
    public class ShowRepository : IShowRepository
    {
        private readonly TvShowsContext dbContext;

        public ShowRepository(TvShowsContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddAsync(Show show)
        {
            var ids = new List<long>();
            foreach (var actor in show.Actors)
            {
                dbContext.Actors.AddOrUpdate(actor);
                ids.Add(actor.Id);
            }
            await dbContext.SaveChangesAsync();
            var addedActors = dbContext.Actors.Where(actor => ids.Contains(actor.Id));
            var showToAdd = new Show(show.Id, show.Name, addedActors);
            dbContext.Shows.Add(showToAdd);
        }

        public Task<Show> GetAsync(long id)
        {
            return dbContext
                .Shows
                .Where(show => show.Id == id)
                .Include(show => show.Actors)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Show>> GetAsync(int skip, int take)
        {
            return await dbContext
                .Shows
                .Include(show => show.Actors)
                .OrderBy(show => show.Id)
                .Skip(() => skip)
                .Take(() => take)
                .ToListAsync();
        }

        public async Task<long> GetMaxShowIdAsync()
        {
            return await dbContext
                .Shows
                .MaxAsync(show => show.Id);
        }
    }
}
