using System;
using NLog;
using Topshelf;

namespace TvMazeScraper.Collector
{
    class Program
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {
            try
            {
                var host = HostFactory.New(x =>
                {
                    x.UseNLog();
                    x.Service<CollectorWindowsService>();
                    x.RunAsLocalSystem();
                    x.StartAutomaticallyDelayed();
                    x.SetDescription("TvMazeScraper.Collector");
                    x.SetDisplayName("TvMazeScraper.Collector");
                    x.SetServiceName("TvMazeScraper.Collector");
                });
                host.Run();
            }
            catch (Exception e)
            {
                logger.Error(e);
            }
        }
    }
}
