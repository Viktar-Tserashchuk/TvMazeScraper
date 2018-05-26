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
        public ICollection<ShowToActor> ShowToActors { get; private set; }

        public IEnumerable<Actor> GetActorsOrderedByBirthday()
        {
            return ShowToActors
                .Select(sta => sta.Actor)
                .OrderByDescending(actor => actor.Birthday);
        }

        private Show() { }

        public Show(long id, string name, IEnumerable<ShowToActor> showToActors)
        {
            if (id < 0) throw new ArgumentException($"{nameof(id)} can not be less than 0");
            if (string.IsNullOrEmpty(name)) throw new ArgumentException($"{nameof(name)} can not be null or empty");

            Id = id;
            Name = name;
            ShowToActors = (showToActors ?? Enumerable.Empty<ShowToActor>()).ToList();
        }
    }
}
