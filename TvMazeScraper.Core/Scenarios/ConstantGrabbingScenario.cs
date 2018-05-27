using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TvMazeScraper.Core.DataAccess;

namespace TvMazeScraper.Core.Scenarios
{
    public class ConstantGrabbingScenario : IConstantGrabbingScenario
    {
        private const int TvMazePageSize = 250;
        private const int PauseWhenNoShowsLeftInMs = 1000 * 60 * 60; // 1 hour

        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly IGrabPageOfShowsScenario grabPageOfShowsScenario;


        public ConstantGrabbingScenario(IUnitOfWorkFactory unitOfWorkFactory, IGrabPageOfShowsScenario grabPageOfShowsScenario)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.grabPageOfShowsScenario = grabPageOfShowsScenario;
        }
        public async Task RunAsync(CancellationToken cancellationToken)
        {
            var showRepository = unitOfWorkFactory
                .CreateUnitOfWork()
                .ShowRepository;
            var maxShowId = await showRepository.AnyAsync()
                ? await showRepository.GetMaxShowIdAsync()
                : 0;

            var pageToAsk = (int)maxShowId / TvMazePageSize;

            while (!cancellationToken.IsCancellationRequested)
            {
                OnNextPageGrabbing?.Invoke(this, pageToAsk);
                var moreShowsExist = await grabPageOfShowsScenario.RunAsync(pageToAsk, cancellationToken);
                if (!moreShowsExist)
                {
                    OnFinished?.Invoke(this, pageToAsk);
                    await Task.Delay(PauseWhenNoShowsLeftInMs, cancellationToken);
                }
                else
                {
                    pageToAsk++;
                }
            }
        }

        public event EventHandler<int> OnNextPageGrabbing;
        public event EventHandler<int> OnFinished;
    }
}