using System.Threading.Tasks;
using Xunit;

namespace Hurace.Simulator.Tests
{
    public class MainViewModelTests
    {
        //[Fact]
        //public async Task ReceiveTriggeredEventsTest()
        //{
        //    //var timerEventList = new List<(int sensorId, DateTime time)>();

        //    //Simulator.RaceClockSimulation.Instance.TimingTriggered +=
        //    //    (sensorId, time) => timerEventList.Add((sensorId, time));

        //    //var viewModel = new ViewModels.MainViewModel();

        //    //Assert.True(viewModel.CanStartSensorSimulation(null));
        //    //Assert.False(viewModel.CanStopSensorSimulation(null));

        //    //await viewModel.StartSensorSimulation();

        //    //Thread.Sleep(5000);

        //    //Assert.NotEmpty(timerEventList);
        //    //var expectedElementCount = timerEventList.Count;

        //    //await viewModel.StopSensorSimulation();

        //    //var actualElementCount = timerEventList.Count;
        //    //Assert.Equal(expectedElementCount, actualElementCount);
        //    Assert.False(true);
        //}

        //[Fact]
        //public async Task ValidateIntervalFunctionalityTest()
        //{
        //    await Task.CompletedTask;
        //    Assert.True(false);
        //}

        //todo:
        // andere Properties im MainViewModel noch testen, wie sich das auf die Zeitberechnung auswirkt.
    }
}
