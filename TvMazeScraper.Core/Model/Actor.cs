using System;
using System.Collections.Generic;
using System.Linq;

namespace TvMazeScraper.Core.Model
{
    public class Actor
    {
        public long Id { get; private set; }
        public string Name { get; private set; }
        public DateTime? Birthday { get; private set; }
        internal ICollection<ShowToActor> ShowToActors { get; private set; }

        private Actor() { }

        public Actor(long id, string name, DateTime? birthday, IEnumerable<ShowToActor> showToActors)
        {
            if (id < 0) throw new ArgumentException($"{nameof(id)} can not be less than 0");
            if (string.IsNullOrEmpty(name)) throw new ArgumentException($"{nameof(name)} can not be null or empty");

            Id = id;
            Name = name;
            Birthday = birthday;
            ShowToActors = (showToActors ?? Enumerable.Empty<ShowToActor>()).ToList();
        }
    }
}
