using System;
using System.Threading;
using System.Threading.Tasks;
using TvMazeScraper.Core.DataAccess;

namespace TvMazeScraper.Core.Scenarios
{
    public class ConstantGrabbingScenario : IConstantGrabbingScenario
    {
        private const int TvMazePageSize = 250;
        private const int PauseWhenNoShowsAnymoreInMs = 1000 * 60 * 60; // 1 hour

        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly IGrabPageOfShowsScenario grabPageOfShowsScenario;


        public ConstantGrabbingScenario(IUnitOfWorkFactory unitOfWorkFactory, IGrabPageOfShowsScenario grabPageOfShowsScenario)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.grabPageOfShowsScenario = grabPageOfShowsScenario;
        }
        public async Task RunAsync(CancellationToken cancellationToken)
        {
            var maxShowId = await unitOfWorkFactory
                .CreateUnitOfWork()
                .ShowRepository
                .GetMaxShowIdAsync();
            var pageToAsk = (int)maxShowId / TvMazePageSize;

            while (!cancellationToken.IsCancellationRequested)
            {
                OnNextPageGrabbing?.Invoke(this, pageToAsk);
                var moreShowsExist = await grabPageOfShowsScenario.RunAsync(pageToAsk++, cancellationToken);
                if (!moreShowsExist)
                {
                    OnFinished?.Invoke(this, pageToAsk);
                    await Task.Delay(PauseWhenNoShowsAnymoreInMs, cancellationToken);
                }
            }
        }

        public event EventHandler<int> OnNextPageGrabbing;
        public event EventHandler<int> OnFinished;
    }
}