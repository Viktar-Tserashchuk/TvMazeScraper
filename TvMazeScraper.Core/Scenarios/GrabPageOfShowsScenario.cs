using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TvMazeScraper.Core.DataAccess;

namespace TvMazeScraper.Core.Scenarios
{
    public class GrabPageOfShowsScenario: IGrabPageOfShowsScenario
    {
        private const int DelayBeetwinBanchOfRequestsInMs = 1000;
        private readonly IGrabShowScenario grabShowScenario;
        private readonly IRemoteShowRepository remoteRepository;

        public GrabPageOfShowsScenario(IGrabShowScenario grabShowScenario, IRemoteShowRepository remoteRepository)
        {
            this.grabShowScenario = grabShowScenario;
            this.remoteRepository = remoteRepository;
        }

        public async Task<bool> RunAsync(int page, CancellationToken cancellationToken)
        {
            var shows = (await remoteRepository.GetPaginatedShowsAsync(page)).ToList();
            foreach (var show in shows)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return true;
                }
                try
                {
                    await grabShowScenario.RunAsync(show.Id);
                }
                catch (TooManyRequestsException)
                {
                    await Task.Delay(DelayBeetwinBanchOfRequestsInMs, cancellationToken);
                }
            }
            return shows.Any();
        }
    }
}
