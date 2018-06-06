using System;
using System.Threading;

namespace SoilMoistureSensorCalibratedSerial.Tests.Integration
{
	public class CalibrateCommandTestHelper : GreenSenseHardwareTestHelper
    {
		public string Label;
		public string Letter;
		public int SimulatedSoilMoisturePercentage = -1;
		public int RawSoilMoistureValue = 0;
  
        public CalibrateCommandTestHelper()
        {
        }

		public void TestCalibrateCommand()
		{
			WriteTitleText("Starting calibrate " + Label + " command test");

			Console.WriteLine("Simulated soil moisture: " + SimulatedSoilMoisturePercentage + "%");

            if (RawSoilMoistureValue == 0)
				RawSoilMoistureValue = SimulatedSoilMoisturePercentage * AnalogPinMaxValue / 100;

			Console.WriteLine("Raw soil moisture value: " + RawSoilMoistureValue);
			Console.WriteLine("");

			var simulatorIsNeeded = SimulatedSoilMoisturePercentage > -1;

			EnableDevices(simulatorIsNeeded);

			PrepareDeviceForTest();

			if (SimulatorIsEnabled)
			{
				SimulateSoilMoisture(SimulatedSoilMoisturePercentage);

				var values = WaitForData(1);

				AssertDataValueIsWithinRange(values[values.Length - 1], "R", RawSoilMoistureValue, RawValueMarginOfError);
			}

			SendCalibrationCommand();
		}

        public void SendCalibrationCommand()
		{
			var command = Letter;

            // If the simulator isn't enabled then the raw value is passed as part of the command to specify it directly
            if (!SimulatorIsEnabled)
                command = command + RawSoilMoistureValue;

            Console.WriteLine("");
            Console.WriteLine("Sending '" + command + "' command to monitor device...");
            Console.WriteLine("");
            
			DeviceClient.WriteLine(command);

			WaitForMessageReceived(command);

			var data = WaitForData(1);

			AssertDataValueIsWithinRange(data[data.Length-1], Letter, RawSoilMoistureValue, RawValueMarginOfError);
		}
    }
}
