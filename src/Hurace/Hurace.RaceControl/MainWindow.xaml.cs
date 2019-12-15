using Hurace.Core.BL;
using Hurace.Core.DAL.AdoPersistence;
using Hurace.Core.Db.Connection;
using System.Windows;

namespace Hurace.RaceControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(IRaceInformationManager raceManager)
        {
            InitializeComponent();

            var mainViewModel = new ViewModels.MainViewModel(raceManager);

            this.DataContext = mainViewModel;
            this.Loaded += async (sender, eventArgs) => await mainViewModel.InitializeAsync();
        }
    }
}
