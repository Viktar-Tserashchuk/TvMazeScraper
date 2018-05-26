using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TvMazeScraper.Core.DataAccess;
using TvMazeScraper.Core.Model;
using TvMazeScraper.Core.Scenarios;

namespace TvMazeScraper.DataAccess.RemoteShowRepository
{
    public class RemoteShowRepository : IRemoteShowRepository
    {
        private const int TooManyRequestHttpStatusCode = 429;

        private readonly string showInformationUrlPattern;
        private readonly string showIndexUrlPattern;

        public RemoteShowRepository(string showInformationUrlPattern, string showIndexUrlPattern)
        {
            this.showInformationUrlPattern = showInformationUrlPattern;
            this.showIndexUrlPattern = showIndexUrlPattern;
        }

        public async Task<Show> GetWithActorsAsync(long id)
        {
            var showInformationUrl = string.Format(showInformationUrlPattern, id);

            return await RequestRemoteData(
                showInformationUrl,
                (RemoteShow remoteShow) => ExtractShowFromRemoteShow(remoteShow)
            );
        }

        public async Task<IEnumerable<Show>> GetPaginatedShowsAsync(long pageNum)
        {
            var showIndexUrl = string.Format(showIndexUrlPattern, pageNum);

            var shows = await RequestRemoteData(
                showIndexUrl,
                (IEnumerable<RemoteShow> remoteShows) => remoteShows.Select(ExtractShowFromRemoteShow)
            );
            return shows ?? Enumerable.Empty<Show>();
        }

        private async Task<TOut> RequestRemoteData<TOut, TRequestedData>(string url, Func<TRequestedData, TOut> dataTransformation) where TOut: class 
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                TOut result = null;
                if (response.IsSuccessStatusCode)
                {
                    var requestedData = await response.Content.ReadAsAsync<TRequestedData>();
                    result = dataTransformation(requestedData);
                }
                else if ((int)response.StatusCode == TooManyRequestHttpStatusCode)
                {
                    throw new TooManyRequestsException();
                }
                return result;
            }
        }

        private Show ExtractShowFromRemoteShow(RemoteShow remoteShow)
        {
            var showToActors = remoteShow._embedded?.Cast?.Select(
                role => new Actor(
                    role.Person.Id,
                    role.Person.Name,
                    role.Person.Birthday,
                    null)
                )
                .GroupBy(actor => actor.Id)
                .Select(g => g.First())
                .Select(actor => new ShowToActor(null, actor))
                .ToList();
            return new Show(remoteShow.Id, remoteShow.Name, showToActors);
        }
    }
}
