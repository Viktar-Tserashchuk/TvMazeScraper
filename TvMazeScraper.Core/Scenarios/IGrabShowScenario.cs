using System.Threading.Tasks;

namespace TvMazeScraper.Core.Scenarios
{
    public interface IGrabShowScenario
    {
        Task RunAsync(long id);
    }
}
