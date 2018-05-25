using System.Collections.Generic;

namespace TvMazeScraper.WebApi.Model
{
    public class ShowDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<ActorDto> Cast { get; set; }
    }
}
