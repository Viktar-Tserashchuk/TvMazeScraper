using System.Collections.Generic;
using System.Threading.Tasks;
using TvMazeScraper.Core.Model;

namespace TvMazeScraper.Core.DataAccess
{
    public interface IShowRepository
    {
        Task AddAsync(Show show);
        Task<Show> GetAsync(long id);
        Task<IEnumerable<Show>> GetAsync(int skip, int take);
        Task<long> GetMaxShowIdAsync();
    }
}
