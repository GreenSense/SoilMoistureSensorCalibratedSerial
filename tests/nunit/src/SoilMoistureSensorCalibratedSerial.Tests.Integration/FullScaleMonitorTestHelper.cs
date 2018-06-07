using System;
namespace SoilMoistureSensorCalibratedSerial.Tests.Integration
{
	public class FullScaleMonitorTestHelper : GreenSenseHardwareTestHelper
	{
		public FullScaleMonitorTestHelper()
		{
		}
		public void RunFullScaleTest()
		{
			WriteTitleText("Starting full scale test");

			EnableDevices();

			PrepareDeviceForTest();

			int step = 25;

			for (int i = 100; i >= 0; i -= step)
			{
				RunFullScaleTestSegment(i);
			}

			for (int i = 0; i <= 100; i += step)
			{
				RunFullScaleTestSegment(i);
			}
		}

		public void RunFullScaleTestSegment(int soilMoisturePercentage)
		{
			WriteSubTitleText("Starting full scale test segment");

			Console.WriteLine("Soil moisture: " + soilMoisturePercentage + "%");
			Console.WriteLine("");

			SimulateSoilMoisture(soilMoisturePercentage);

			var data = WaitForData();

			Console.WriteLine("");
			Console.WriteLine("Checking calibrated value");
			Console.WriteLine("");

			AssertDataValueIsWithinRange(data, "C", soilMoisturePercentage, CalibratedValueMarginOfError);

			Console.WriteLine("");
			Console.WriteLine("Checking raw value");
			Console.WriteLine("");

			var expectedRawValue = soilMoisturePercentage * AnalogPinMaxValue / 100;

			AssertDataValueIsWithinRange(data, "R", expectedRawValue, RawValueMarginOfError);
		}
	}
}
