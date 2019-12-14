using Hurace.Core.BL;
using Hurace.Core.DAL.AdoPersistence;
using Hurace.Core.Db.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hurace.RaceControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var connectionFactory = new DefaultConnectionFactory();

            var countryDao = new GenericDao<Entities.Country>(connectionFactory);
            var raceDao = new GenericDao<Entities.Race>(connectionFactory);
            var raceTypeDao = new GenericDao<Entities.RaceType>(connectionFactory);
            var seasonDao = new GenericDao<Entities.Season>(connectionFactory);
            var seasonPlanDao = new GenericDao<Entities.SeasonPlan>(connectionFactory);
            var venueDao = new GenericDao<Entities.Venue>(connectionFactory);

            var raceManager = new RaceManager(countryDao, raceDao, raceTypeDao, seasonDao, seasonPlanDao, venueDao);

            var mainViewModel = new ViewModels.MainViewModel(raceManager);

            this.DataContext = mainViewModel;
            this.Loaded += async (sender, eventArgs) => await mainViewModel.InitializeAsync();
        }
    }
}
