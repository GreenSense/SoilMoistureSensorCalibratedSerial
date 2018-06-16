using System;
namespace SoilMoistureSensorCalibratedSerial.Tests.Integration
{
	public class SerialOutputTimeTestHelper : GreenSenseHardwareTestHelper
	{
		public int ReadInterval = 1;

		public void TestReadInterval()
		{
			WriteTitleText("Starting read interval test");

			Console.WriteLine("Read interval: " + ReadInterval);

			EnableDevices(false);

			SetDeviceReadInterval(ReadInterval);

			// Wait for the first data line before starting
			WaitUntilDataLine();

			// Get the time until the next data line
			var secondsBetweenDataLines = WaitUntilDataLine();

			Console.WriteLine("Time between data lines: " + secondsBetweenDataLines + " seconds");

			AssertIsWithinRange("serial output time", ReadInterval, secondsBetweenDataLines, TimeErrorMargin);
		}
	}
}
