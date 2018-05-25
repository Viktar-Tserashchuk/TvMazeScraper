using System.Threading.Tasks;
using TvMazeScraper.Core.DataAccess;

namespace TvMazeScraper.DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TvShowsContext dbContext;

        public UnitOfWork(TvShowsContext dbContext)
        {
            this.dbContext = dbContext;
            ShowRepository = new ShowRepository.ShowRepository(dbContext);
        }

        public IShowRepository ShowRepository { get; }
        public async Task SaveCangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
