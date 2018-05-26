using System;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using NLog;
using Topshelf;
using TvMazeScraper.Core.DataAccess;
using TvMazeScraper.Core.Model;
using TvMazeScraper.Core.Scenarios;
using TvMazeScraper.DataAccess.RemoteShowRepository;
using TvMazeScraper.DataAccess.ShowRepository;

namespace TvMazeScraper.Collector
{
    public class CollectorWindowsService : ServiceControl
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private Task grabTask;
        private CancellationTokenSource cancellationTokenSource;

        public bool Start(HostControl hostControl)
        {
            cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            grabTask = Task.Run(() => GrabShows(cancellationToken), cancellationToken);
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            cancellationTokenSource.Cancel();
            if (grabTask.IsCanceled)
            {
                grabTask.Wait();
            }
            return true;
        }

        public async Task GrabShows(CancellationToken cancellationToken)
        {
            try
            {
                using (var container = BuildContainer())
                {
                    var constantGrabbingScenario = container.Resolve<IConstantGrabbingScenario>();
                    constantGrabbingScenario.OnNextPageGrabbing += (sender, page) => logger.Info("Grabing page: {0}", page);
                    constantGrabbingScenario.OnFinished += (sender, page) => logger.Info("No more shows. Waiting for an update");
                    await constantGrabbingScenario.RunAsync(cancellationToken);
                }
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw;
            }
        }

        private static IContainer BuildContainer()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["TvShowsContext"].ConnectionString;

            var builder = new ContainerBuilder();
            builder.Register(c => new UnitOfWorkFactory(connectionString)).As<IUnitOfWorkFactory>();
            builder
                .Register(c => new RemoteShowRepository(
                    "http://api.tvmaze.com/shows/{0}?embed=cast",
                    "http://api.tvmaze.com/shows?page={0}")
                )
                .As<IRemoteShowRepository>();
            builder.RegisterType<GrabShowScenario>().As<IGrabShowScenario>();
            builder.RegisterType<GrabPageOfShowsScenario>().As<IGrabPageOfShowsScenario>();
            builder.RegisterType<ConstantGrabbingScenario>().As<IConstantGrabbingScenario>();
            return builder.Build();
        }
    }
}
