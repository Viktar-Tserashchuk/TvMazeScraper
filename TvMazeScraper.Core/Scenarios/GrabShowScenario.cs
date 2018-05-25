using System.Threading.Tasks;
using TvMazeScraper.Core.DataAccess;

namespace TvMazeScraper.Core.Scenarios
{
    public class GrabShowScenario : IGrabShowScenario
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly IRemoteShowRepository remoteShowRepository;

        public GrabShowScenario(IUnitOfWorkFactory unitOfWorkFactory, IRemoteShowRepository remoteShowRepository)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.remoteShowRepository = remoteShowRepository;
        }

        public async Task RunAsync(long id)
        {
            var unitOfWork = unitOfWorkFactory.CreateUnitOfWork();
            if (await ShowExistsLocallyAsync(unitOfWork, id)) return;

            var show = await remoteShowRepository.GetWithActorsAsync(id);

            if (show != null)
            {
                await unitOfWork.ShowRepository.AddAsync(show);
                await unitOfWork.SaveCangesAsync();
            }
        }

        private async Task<bool> ShowExistsLocallyAsync(IUnitOfWork unitOfWork, long id)
        {
            return await unitOfWork.ShowRepository.GetAsync(id) != null;
        }
    }
}