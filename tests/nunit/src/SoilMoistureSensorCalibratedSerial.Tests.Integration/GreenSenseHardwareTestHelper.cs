using System;
using System.Threading;
using NUnit.Framework;
namespace SoilMoistureSensorCalibratedSerial.Tests.Integration
{
	public class GreenSenseHardwareTestHelper : HardwareTestHelper
    {
		public int SoilMoistureSimulatorPin = 9;

		public int RawValueMarginOfError = 15;
		public int CalibratedValueMarginOfError = 5;

        public GreenSenseHardwareTestHelper()
        {
        }

		public void PrepareDeviceForTest()
		{
			ResetDeviceSettings();
			SetDeviceReadInterval(1);
			ReadFromDeviceAndOutputToConsole();
		}

		public void ResetDeviceSettings()
		{
			Console.WriteLine("");
            Console.WriteLine("Resetting device default settings...");
            Console.WriteLine("  Sending 'X' command to device");
            Console.WriteLine("");
   
            DeviceClient.WriteLine ("X");

            WaitForMessageReceived("X");
		}

		public void SetDeviceReadInterval(int numberOfSeconds)
		{
			var cmd = "V" + numberOfSeconds;

            Console.WriteLine("");
			Console.WriteLine("Setting device read interval to " + numberOfSeconds + " second(s)...");
            Console.WriteLine("  Sending '" + cmd + "' command to device");
            Console.WriteLine("");

            DeviceClient.WriteLine (cmd);
            
            WaitForMessageReceived(cmd);
		}
        
        public void SimulateSoilMoisture(int soilMoisturePercentage)
		{
			Console.WriteLine("");
			Console.WriteLine("Simulating soil moisture percentage");
			Console.WriteLine("  Sending analog percentage");
			Console.WriteLine("    PWM pin: " + SoilMoistureSimulatorPin);
			Console.WriteLine("    Soil Moisture Percentage: " + soilMoisturePercentage + "%");
            Console.WriteLine("");
   
			SimulatorClient.AnalogWritePercentage(SoilMoistureSimulatorPin, soilMoisturePercentage);
		}

		public void WaitForMessageReceived(string message)
        {
			Console.WriteLine("");
			Console.WriteLine("Waiting for received message");
			Console.WriteLine("  Message: " + message);
   
            var output = String.Empty;
            var wasMessageReceived = false;

            var startTime = DateTime.Now;

			while (!wasMessageReceived)
            {
                output += DeviceClient.ReadLine();

				var expectedText = "Received message: " + message;
				if (output.Contains(expectedText))
				{
					wasMessageReceived = true;
					Console.WriteLine("  Message was received");
					Console.WriteLine("------------------------------");
                    Console.WriteLine(output);
                    Console.WriteLine("------------------------------");
				}
                
                var hasTimedOut = DateTime.Now.Subtract(startTime).TotalSeconds > TimeoutWaitingForResponse;
				if (hasTimedOut && !wasMessageReceived)
				{               
                    Console.WriteLine("------------------------------");
                    Console.WriteLine(output);
                    Console.WriteLine("------------------------------");

					Assert.Fail("Timed out waiting for message received (" + TimeoutWaitingForResponse + " seconds)");

				}
            }
        }

    }
}
