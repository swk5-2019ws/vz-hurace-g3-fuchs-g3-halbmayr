using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hurace.Domain
{
    public class Venue : DomainObjectBase
    {
        private string name;

        private readonly Lazy<Task<IEnumerable<Season>>> lazySeasons;
        private readonly Lazy<Task<Country>> lazyCountry;

        public Venue(
            int venueId,
            Func<Task<IEnumerable<Season>>> seasonLoader = null,
            Func<Task<Country>> countryLoader = null)
            : base(venueId)
        {
            this.lazySeasons = seasonLoader is null
                ? new Lazy<Task<IEnumerable<Season>>>()
                : new Lazy<Task<IEnumerable<Season>>>(seasonLoader);

            this.lazyCountry = countryLoader is null
                ? new Lazy<Task<Country>>()
                : new Lazy<Task<Country>>(countryLoader);

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

        public Task<Country> Country => this.lazyCountry.Value;
        public Task<IEnumerable<Season>> Seasons => this.lazySeasons.Value;
    }
}