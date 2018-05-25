using System.Collections.Generic;

namespace TvMazeScraper.DataAccess.RemoteShowRepository
{
    internal class RemoteShow
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public Embedded _embedded { get; set; }
    }

    internal class Embedded
    {
        public IEnumerable<RoleDetails> Cast { get; set; }
    }

    internal class RoleDetails
    {
        public Person Person { get; set; }
    }


    internal class Person
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Birthday { get; set; }
    }

}
