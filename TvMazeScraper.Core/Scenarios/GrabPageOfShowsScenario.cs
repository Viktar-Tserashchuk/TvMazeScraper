using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TvMazeScraper.Core.DataAccess;
using TvMazeScraper.Core.Model;

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
            var showQueue = new Queue<Show>(shows);
            while(showQueue.Any() && !cancellationToken.IsCancellationRequested)
            {
                var show = showQueue.Peek();
                try
                {
                    await grabShowScenario.RunAsync(show.Id);
                    showQueue.Dequeue();
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
