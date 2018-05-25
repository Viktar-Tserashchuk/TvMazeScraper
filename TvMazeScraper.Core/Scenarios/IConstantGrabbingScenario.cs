using System;
using System.Threading;
using System.Threading.Tasks;

namespace TvMazeScraper.Core.Scenarios
{
    public interface IConstantGrabbingScenario
    {
        Task RunAsync(CancellationToken cancellationToken);
        event EventHandler<int> OnNextPageGrabbing;
        event EventHandler<int> OnFinished;
    }
}
