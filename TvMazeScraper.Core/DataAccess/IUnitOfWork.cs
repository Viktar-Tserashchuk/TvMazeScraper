using System.Threading.Tasks;

namespace TvMazeScraper.Core.DataAccess
{
    public interface IUnitOfWork
    {
        IShowRepository ShowRepository { get; }
        Task SaveCangesAsync();
    }
}
