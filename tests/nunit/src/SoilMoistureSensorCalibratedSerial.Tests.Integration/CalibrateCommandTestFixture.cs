using NUnit.Framework;

namespace SoilMoistureSensorCalibratedSerial.Tests.Integration
{
	[TestFixture(Category = "Integration")]
	public class CalibrateCommandTestFixture : BaseTestFixture
	{
		[Test]
		public void Test_CalibrateDryToCurrentValueCommand()
		{
			using (var helper = new CalibrateCommandTestHelper())
			{
				helper.Label = "dry";
				helper.Letter = "D";
				helper.SimulatedSoilMoisturePercentage = 20;

				helper.DevicePort = GetDevicePort();
				helper.DeviceBaudRate = 9600; // TODO: Should this be configurable via environment variables?

				helper.SimulatorPort = GetSimulatorPort();
				helper.SimulatorBaudRate = 9600; // TODO: Should this be configurable via environment variables?

				helper.TestCalibrateCommand();
			}
		}

		[Test]
		public void Test_CalibrateDryToSpecifiedValueCommand()
		{
			using (var helper = new CalibrateCommandTestHelper())
			{
				helper.Label = "dry";
				helper.Letter = "D";
				helper.RawSoilMoistureValue = 220;

				helper.DevicePort = GetDevicePort();
				helper.DeviceBaudRate = 9600; // TODO: Should this be configurable via environment variables?

				helper.SimulatorPort = GetSimulatorPort();
				helper.SimulatorBaudRate = 9600; // TODO: Should this be configurable via environment variables?

				helper.TestCalibrateCommand();
			}
		}

		[Test]
		public void Test_CalibrateWetToCurrentValueCommand()
		{
			using (var helper = new CalibrateCommandTestHelper())
			{
				helper.Label = "wet";
				helper.Letter = "W";
				helper.SimulatedSoilMoisturePercentage = 80;

				helper.DevicePort = GetDevicePort();
				helper.DeviceBaudRate = 9600; // TODO: Should this be configurable via environment variables?

				helper.SimulatorPort = GetSimulatorPort();
				helper.SimulatorBaudRate = 9600; // TODO: Should this be configurable via environment variables?

				helper.TestCalibrateCommand();
			}
		}

		[Test]
		public void Test_CalibrateWetToSpecifiedValueCommand()
		{
			using (var helper = new CalibrateCommandTestHelper())
			{
				helper.Label = "wet";
				helper.Letter = "W";
				helper.RawSoilMoistureValue = 880;

				helper.DevicePort = GetDevicePort();
				helper.DeviceBaudRate = 9600; // TODO: Should this be configurable via environment variables?

				helper.SimulatorPort = GetSimulatorPort();
				helper.SimulatorBaudRate = 9600; // TODO: Should this be configurable via environment variables?

				helper.TestCalibrateCommand();
			}
		}
	}
}
