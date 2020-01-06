using System.Collections.ObjectModel;

namespace Hurace.RaceControl.ViewModels
{
    public class RankViewModel : BaseViewModel
    {
        private string firstName;
        private string lastName;
        private int rank;

        public RankViewModel()
        {
            this.Measurements = new ObservableCollection<int>();
        }

        public int SkierId { get; set; }

        public string FirstName
        {
            get => firstName;
            set => base.Set(ref this.firstName, value);
        }

        public string LastName
        {
            get => lastName;
            set => base.Set(ref this.lastName, value);
        }

        public int Rank
        {
            get => rank;
            set => base.Set(ref this.rank, value);
        }

        public ObservableCollection<int> Measurements { get; }
    }
}