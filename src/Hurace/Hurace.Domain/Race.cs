using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hurace.Domain
{
    public class Race : DomainObjectBase
    {
        private readonly Lazy<Task<IEnumerable<StartPosition>>> lazyFirstStartList;
        private readonly Lazy<Task<IEnumerable<StartPosition>>> lazySecondStartList;
        private readonly Lazy<Task<Venue>> lazyVenue;

        public Race(
            Func<Task<IEnumerable<StartPosition>>> firstStartListLoader = null,
            Func<Task<IEnumerable<StartPosition>>> secondStartListLoader = null,
            Func<Task<Venue>> venueLoader = null)
        {
            this.lazyFirstStartList = firstStartListLoader is null
                ? new Lazy<Task<IEnumerable<StartPosition>>>()
                : new Lazy<Task<IEnumerable<StartPosition>>>(firstStartListLoader);

            this.lazySecondStartList = secondStartListLoader is null
                ? new Lazy<Task<IEnumerable<StartPosition>>>()
                : new Lazy<Task<IEnumerable<StartPosition>>>(secondStartListLoader);

            this.lazyVenue = venueLoader is null
                ? new Lazy<Task<Venue>>()
                : new Lazy<Task<Venue>>(venueLoader);
        }

        public string RaceType { get; set; }
        public int NumberOfSensors { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }

        public Task<Venue> Venue => this.lazyVenue.Value;
        public Task<IEnumerable<StartPosition>> FirstStartList => this.lazyFirstStartList.Value;

        public Task<IEnumerable<StartPosition>> SecondStartList => this.lazySecondStartList.Value;
    }
}
