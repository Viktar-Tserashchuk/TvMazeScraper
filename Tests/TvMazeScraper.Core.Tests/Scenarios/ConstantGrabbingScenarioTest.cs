using System.Threading;
using System.Threading.Tasks;
using Moq;
using TvMazeScraper.Core.DataAccess;
using TvMazeScraper.Core.Scenarios;
using Xunit;

namespace TvMazeScraper.Core.Tests.Scenarios
{
    public class ConstantGrabbingScenarioTest
    {
        private readonly Mock<IUnitOfWorkFactory> unitOfWorkFactoryRepositoryMock;
        private readonly Mock<IGrabPageOfShowsScenario> grabPagesOfShowsScenarioMock;

        public ConstantGrabbingScenarioTest()
        {
            var showRepsitoryMock = new Mock<IShowRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkFactoryRepositoryMock = new Mock<IUnitOfWorkFactory>();
            grabPagesOfShowsScenarioMock = new Mock<IGrabPageOfShowsScenario>();
            unitOfWorkMock
                .Setup(x => x.ShowRepository)
                .Returns(showRepsitoryMock.Object);
            unitOfWorkFactoryRepositoryMock
                .Setup(x => x.CreateUnitOfWork())
                .Returns(unitOfWorkMock.Object);
            showRepsitoryMock
                .Setup(x => x.AnyAsync())
                .Returns(Task.FromResult(false));
            grabPagesOfShowsScenarioMock
                .Setup(x => x.RunAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(true));
        }

        [Fact]
        public async void ItShouldGrabPagesUntilCancellation()
        {
            // Arrange
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            var constantGrabbingScenario = new ConstantGrabbingScenario(
                unitOfWorkFactoryRepositoryMock.Object,
                grabPagesOfShowsScenarioMock.Object);
            #pragma warning disable 4014
            // It needs to cancel grabbing after some time
            Task
                .Delay(10)
                .ContinueWith(task => tokenSource.Cancel());
            #pragma warning restore 4014

            // Act
            await constantGrabbingScenario.RunAsync(token);

            grabPagesOfShowsScenarioMock.Verify(x => x.RunAsync(It.IsAny<int>(), token), Times.AtLeastOnce);
        }
    }
}
