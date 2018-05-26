using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
            var actorIds = show.ShowToActors.Select(sta => sta.Actor.Id);
            var idsOfStoredActors = new HashSet<long>(
                await dbContext
                    .Actors
                    .Where(actor => actorIds.Contains(actor.Id))
                    .Select(actor => actor.Id)
                    .ToListAsync()
            );
            var showToAdd = new Show(show.Id, show.Name, null);
            dbContext.Shows.Add(showToAdd);
            foreach (var showToActor in show.ShowToActors)
            {
                if (!idsOfStoredActors.Contains(showToActor.Actor.Id))
                {
                    dbContext.Actors.Add(showToActor.Actor);
                }
                dbContext.ShowToActors.Add(new ShowToActor(show.Id, showToActor.Actor.Id));
            }
        }

        public Task<Show> GetAsync(long id)
        {
            return dbContext
                .Shows
                .Where(show => show.Id == id)
                .Include(show => show.ShowToActors)
                .ThenInclude(sta => sta.Actor)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Show>> GetAsync(int skip, int take)
        {
            return await dbContext
                .Shows
                .Include(show => show.ShowToActors)
                .ThenInclude(sta => sta.Actor)
                .OrderBy(show => show.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<long> GetMaxShowIdAsync()
        {
            return await dbContext
                .Shows
                .MaxAsync(show => show.Id);
        }

        public async Task<bool> AnyAsync()
        {
            return await dbContext.Shows.AnyAsync();
        }
    }
}
