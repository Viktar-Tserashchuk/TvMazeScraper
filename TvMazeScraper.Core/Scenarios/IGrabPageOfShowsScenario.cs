using System.Threading;
using System.Threading.Tasks;

namespace TvMazeScraper.Core.Scenarios
{
    public interface IGrabPageOfShowsScenario
    {
        Task<bool> RunAsync(int page, CancellationToken cancellationToken);
    }
}
