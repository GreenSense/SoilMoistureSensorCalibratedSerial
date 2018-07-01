using NUnit.Framework;

namespace SoilMoistureSensorCalibratedSerial.Tests.Integration
{
	[TestFixture(Category = "Integration")]
	public class ReadIntervalCommandTestFixture : BaseTestFixture
	{
		[Test]
		public void Test_SetReadIntervalCommand_2Seconds()
		{
			using (var helper = new ReadIntervalCommandTestHelper())
			{
				helper.ReadInterval = 2;

				helper.DevicePort = GetDevicePort();
				helper.DeviceBaudRate = GetDeviceSerialBaudRate();

				helper.SimulatorPort = GetSimulatorPort();
				helper.SimulatorBaudRate = GetSimulatorSerialBaudRate();

				helper.TestSetReadIntervalCommand();
			}
		}

		[Test]
		public void Test_SetReadIntervalCommand_5Seconds()
		{
			using (var helper = new ReadIntervalCommandTestHelper())
			{
				helper.ReadInterval = 5;

				helper.DevicePort = GetDevicePort();
				helper.DeviceBaudRate = GetDeviceSerialBaudRate();

				helper.SimulatorPort = GetSimulatorPort();
				helper.SimulatorBaudRate = GetSimulatorSerialBaudRate();

				helper.TestSetReadIntervalCommand();
			}
		}
	}
}
