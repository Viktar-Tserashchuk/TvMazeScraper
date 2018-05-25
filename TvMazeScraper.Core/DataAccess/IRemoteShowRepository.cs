using System.Collections.Generic;
using System.Threading.Tasks;
using TvMazeScraper.Core.Model;

namespace TvMazeScraper.Core.DataAccess
{
    public interface IRemoteShowRepository
    {
        Task<Show> GetWithActorsAsync(long id);
        Task<IEnumerable<Show>> GetPaginatedShowsAsync(long pageNum);
    }
}
