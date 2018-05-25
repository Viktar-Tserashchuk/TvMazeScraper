using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TvMazeScraper.Core.Model
{
    public class Show
    {
        public long Id { get; private set; }
        public string Name { get; private set; }
        public ICollection<Actor> Actors { get; private set; }

        public IEnumerable<Actor> GetActorsOrderedByBirthday()
        {
            return Actors.OrderByDescending(actor => actor.Birthday);
        }

        private Show() { }

        public Show(long id, string name, IEnumerable<Actor> actors)
        {
            if (id < 0) throw new ArgumentException("Id can not be less than 0");
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name can not be null or empty");

            Id = id;
            Name = name;
            Actors = (actors ?? Enumerable.Empty<Actor>()).ToList();
        }
    }
}
