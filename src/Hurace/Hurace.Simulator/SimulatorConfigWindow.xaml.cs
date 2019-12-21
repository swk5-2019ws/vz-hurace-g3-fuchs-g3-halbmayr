using System;
using System.Windows;

namespace Hurace.Simulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //test: remove in prod!
            //RaceClockSimulation.Instance.TimingTriggered +=
            //    (sensorId, time) => Console.WriteLine($"Sensor-{sensorId} triggered around {time}");

            this.DataContext = new ViewModels.SimulatorConfigViewModel();
        }
    }
}
