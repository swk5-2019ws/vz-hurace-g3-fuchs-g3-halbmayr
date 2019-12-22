using Hurace.Simulator;
using System.Windows;

namespace Hurace.RaceControl.Windows
{
    /// <summary>
    /// Interaction logic for SimulatorConfigWindow.xaml
    /// </summary>
    public partial class SimulatorConfigWindow : Window
    {
        public SimulatorConfigWindow(RaceClockSimulation simulation)
        {
            InitializeComponent();
            this.DataContext = new ViewModels.SimulatorConfigViewModel(simulation);
        }
    }
}
