using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hurace.Domain
{
    public class Season : DomainObjectBase
    {
        private string name;
        private DateTime startDate;
        private DateTime endDate;

        private readonly Lazy<Task<IEnumerable<Venue>>> lazyVenues;

        public Season(
            int seasonId,
            Func<Task<IEnumerable<Venue>>> venueLoader = null)
            : base(seasonId)
        {
            this.lazyVenues = venueLoader is null
                ? new Lazy<Task<IEnumerable<Venue>>>()
                : new Lazy<Task<IEnumerable<Venue>>>(venueLoader);

            PropertiesChanged = false;
        }

        public string Name
        {
            get => name;
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged();
                }
            }
        }
        public DateTime StartDate
        {
            get => startDate;
            set
            {
                startDate = value;
                OnPropertyChanged();
            }
        }
        public DateTime EndDate
        {
            get => endDate;
            set
            {
                endDate = value;
                OnPropertyChanged();
            }
        }

        public Task<IEnumerable<Venue>> Venues => this.lazyVenues.Value;
    }
}