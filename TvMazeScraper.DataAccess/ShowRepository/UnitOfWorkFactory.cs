using Microsoft.EntityFrameworkCore;
using TvMazeScraper.Core.DataAccess;

namespace TvMazeScraper.DataAccess.ShowRepository
{
    public class UnitOfWorkFactory: IUnitOfWorkFactory
    {
        private readonly string connectionString;

        public UnitOfWorkFactory(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IUnitOfWork CreateUnitOfWork()
        {
            var options = new DbContextOptionsBuilder()
                .UseSqlServer(connectionString)
                .Options;
            return new UnitOfWork.UnitOfWork(new TvShowsContext(options));
        }
    }
}
