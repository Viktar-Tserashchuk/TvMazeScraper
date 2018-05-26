using System.Threading.Tasks;
using Moq;
using TvMazeScraper.Core.DataAccess;
using TvMazeScraper.Core.Model;
using TvMazeScraper.Core.Scenarios;
using Xunit;

namespace TvMazeScraper.Core.Tests.Scenarios
{
    public class GrabShowScenarioTest
    {
        private readonly Mock<IShowRepository> showRepsitoryMock;
        private readonly Mock<IUnitOfWorkFactory> unitOfWorkFactoryRepositoryMock;
        private readonly Mock<IRemoteShowRepository> remoteShowRepositoryMock;

        public GrabShowScenarioTest()
        {
            showRepsitoryMock = new Mock<IShowRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkFactoryRepositoryMock = new Mock<IUnitOfWorkFactory>();
            remoteShowRepositoryMock = new Mock<IRemoteShowRepository>();
            unitOfWorkMock
                .Setup(x => x.ShowRepository)
                .Returns(showRepsitoryMock.Object);
            unitOfWorkFactoryRepositoryMock
                .Setup(x => x.CreateUnitOfWork())
                .Returns(unitOfWorkMock.Object);
            remoteShowRepositoryMock
                .Setup(x => x.GetWithActorsAsync(It.IsAny<long>()))
                .Returns(Task.FromResult(new Show(1, "Name", null)));
        }

        [Fact]
        public async Task ItShouldNotAddExistingShow()
        {
            // Arrange
            showRepsitoryMock
                .Setup(x => x.GetAsync(It.IsAny<long>()))
                .Returns(Task.FromResult(new Show(1, "Name", null)));
            var grabShowScenario = new GrabShowScenario(unitOfWorkFactoryRepositoryMock.Object, remoteShowRepositoryMock.Object);

            // Act
            await grabShowScenario.RunAsync(1);

            // Accert
            showRepsitoryMock.Verify(x => x.AddAsync(It.IsAny<Show>()), Times.Never());
        }

        [Fact]
        public async Task ItShouldAddNewShow()
        {
            // Arrange
            showRepsitoryMock
                .Setup(x => x.GetAsync(It.IsAny<long>()))
                .Returns(Task.FromResult((Show)null));
            var grabShowScenario = new GrabShowScenario(unitOfWorkFactoryRepositoryMock.Object, remoteShowRepositoryMock.Object);

            // Act
            await grabShowScenario.RunAsync(1);

            // Accert
            showRepsitoryMock.Verify(x => x.AddAsync(It.IsAny<Show>()), Times.Once());
        }
    }
}
