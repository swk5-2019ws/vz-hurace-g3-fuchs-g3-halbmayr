using System;
using System.Threading.Tasks;

namespace Hurace.Domain
{
    public sealed class StartPosition : DomainObjectBase
    {
        private int position;

        private readonly Lazy<Task<Race>> lazyRace;
        private readonly Lazy<Task<Skier>> lazySkier;

        public StartPosition(
            int startPositionId,
            Func<Task<Race>> raceLoader = null,
            Func<Task<Skier>> skierLoader = null)
            : base(startPositionId)
        {
            this.lazyRace = raceLoader is null
                ? new Lazy<Task<Race>>()
                : new Lazy<Task<Race>>(raceLoader);

            this.lazySkier = skierLoader is null
                ? new Lazy<Task<Skier>>()
                : new Lazy<Task<Skier>>(skierLoader);

            PropertiesChanged = false;
        }

        public int Position
        {
            get => position;
            set
            {
                position = value;
                OnPropertyChanged();
            }
        }

        public Task<Race> Race => this.lazyRace.Value;
        public Task<Skier> Skier => this.lazySkier.Value;
    }
}