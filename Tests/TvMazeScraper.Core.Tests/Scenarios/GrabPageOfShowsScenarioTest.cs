using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using TvMazeScraper.Core.DataAccess;
using TvMazeScraper.Core.Model;
using TvMazeScraper.Core.Scenarios;
using Xunit;

namespace TvMazeScraper.Core.Tests.Scenarios
{
    public class GrabPageOfShowsScenarioTest
    {
        private readonly Mock<IRemoteShowRepository> remoteShowRepositoryMock;
        private readonly Mock<IGrabShowScenario> grabShowScenarioMock;

        public GrabPageOfShowsScenarioTest()
        {
            grabShowScenarioMock = new Mock<IGrabShowScenario>();
            remoteShowRepositoryMock = new Mock<IRemoteShowRepository>();
        }

        [Fact]
        public async Task ItShouldReturnFalseWhenNoNewShows()
        {
            // Arrange
            remoteShowRepositoryMock
                .Setup(x => x.GetPaginatedShowsAsync(It.IsAny<long>()))
                .Returns(Task.FromResult(Enumerable.Empty<Show>()));
            var grabPageOfShowsScenario = new GrabPageOfShowsScenario(grabShowScenarioMock.Object, remoteShowRepositoryMock.Object);

            // Act
            var actualResult = await grabPageOfShowsScenario.RunAsync(1, new CancellationToken());

            // Accert
            Assert.False(actualResult);
        }

        [Fact]
        public async Task ItShouldReturnTrueWhenThereAreNewShows()
        {
            // Arrange
            PrepareRemoteShowRepositoryToReturnShows();
            var grabPageOfShowsScenario = new GrabPageOfShowsScenario(grabShowScenarioMock.Object, remoteShowRepositoryMock.Object);

            // Act
            var actualResult = await grabPageOfShowsScenario.RunAsync(1, new CancellationToken());

            // Accert
            Assert.True(actualResult);
        }

        [Fact]
        public async Task ItShouldAddNewShows()
        {
            // Arrange
            var showIds = PrepareRemoteShowRepositoryToReturnShows();
            var grabPageOfShowsScenario = new GrabPageOfShowsScenario(grabShowScenarioMock.Object, remoteShowRepositoryMock.Object);

            // Act
            await grabPageOfShowsScenario.RunAsync(1, new CancellationToken());

            // Accert
            grabShowScenarioMock.Verify(x => x.RunAsync(It.IsAny<long>()), Times.Exactly(showIds.Count()));
        }

        private IEnumerable<long> PrepareRemoteShowRepositoryToReturnShows()
        {
            var pageOfShows = new[]
                {
                    new Show(1, "Name", null)
                };
            remoteShowRepositoryMock
                .Setup(x => x.GetPaginatedShowsAsync(It.IsAny<long>()))
                .Returns(Task.FromResult(pageOfShows.AsEnumerable()));
            return pageOfShows.Select(shoe => shoe.Id);
        }
    }
}
