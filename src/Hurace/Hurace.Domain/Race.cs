using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hurace.Domain
{
    public class Race : DomainObjectBase
    {
        private DateTime date;
        private int numberOfSensors;
        private string raceType;
        private string description;

        private readonly Lazy<Task<IEnumerable<StartPosition>>> lazyFirstStartList;
        private readonly Lazy<Task<IEnumerable<StartPosition>>> lazySecondStartList;
        private readonly Lazy<Task<Venue>> lazyVenue;

        public Race(
            int raceId,
            DateTime date,
            string description,
            int numberOfSensors,
            string raceType,
            Func<Task<IEnumerable<StartPosition>>> firstStartListLoader = null,
            Func<Task<IEnumerable<StartPosition>>> secondStartListLoader = null,
            Func<Task<Venue>> venueLoader = null)
            : base(raceId)
        {
            this.date = date;
            this.description = description;
            this.numberOfSensors = numberOfSensors;
            this.raceType = raceType;

            this.lazyFirstStartList = firstStartListLoader is null
                ? new Lazy<Task<IEnumerable<StartPosition>>>()
                : new Lazy<Task<IEnumerable<StartPosition>>>(firstStartListLoader);

            this.lazySecondStartList = secondStartListLoader is null
                ? new Lazy<Task<IEnumerable<StartPosition>>>()
                : new Lazy<Task<IEnumerable<StartPosition>>>(secondStartListLoader);

            this.lazyVenue = venueLoader is null
                ? new Lazy<Task<Venue>>()
                : new Lazy<Task<Venue>>(venueLoader);

            PropertiesChanged = false;
        }

        public DateTime Date
        {
            get => date;
            set
            {
                if (date != value)
                {
                    date = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Description
        {
            get => description;
            set
            {
                if (description != value)
                {
                    description = value;
                    OnPropertyChanged();
                }
            }
        }

        public int NumberOfSensors
        {
            get => numberOfSensors;
            set
            {
                if (numberOfSensors != value)
                {
                    numberOfSensors = value;
                    OnPropertyChanged();
                }
            }
        }

        public string RaceType
        {
            get => raceType;
            set
            {
                if (raceType != value)
                {
                    raceType = value;
                    OnPropertyChanged();
                }
            }
        }

        public Task<Venue> Venue => this.lazyVenue.Value;
        public Task<IEnumerable<StartPosition>> FirstStartList => this.lazyFirstStartList.Value;
        public Task<IEnumerable<StartPosition>> SecondStartList => this.lazySecondStartList.Value;
    }
}
