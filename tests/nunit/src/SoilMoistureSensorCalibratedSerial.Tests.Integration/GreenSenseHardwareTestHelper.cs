﻿using System;
using System.Threading;
using NUnit.Framework;
namespace SoilMoistureSensorCalibratedSerial.Tests.Integration
{
	public class GreenSenseHardwareTestHelper : HardwareTestHelper
	{
		public int SoilMoistureSimulatorPin = 9;
		public int SoilMoistureSimulatorPowerPin = 3;

		public int DelayAfterTurningSoilMoistureSensorOn = 3;

		public int RawValueMarginOfError = 8;
		public int CalibratedValueMarginOfError = 3;

		public bool CalibrationIsReversedByDefault = true;

		public GreenSenseHardwareTestHelper()
		{
		}

		#region Enable Devices Functions
		public override void EnableDevices(bool enableSimulator)
		{
			base.EnableDevices(enableSimulator);

			PrepareDeviceForTest();
		}
		#endregion

		#region Prepare Device Functions
		public void PrepareDeviceForTest()
		{
			ResetDeviceSettings();

			SetDeviceReadInterval(1);

			if (CalibrationIsReversedByDefault)
				ReverseDeviceCalibration();

			ReadFromDeviceAndOutputToConsole();
		}
		#endregion

		#region General Device Command Settings
		public void SendDeviceCommand(string command)
		{
			WriteToDevice(command);

			WaitForMessageReceived(command);
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

					ConsoleWriteSerialOutput(output);
				}

				var hasTimedOut = DateTime.Now.Subtract(startTime).TotalSeconds > TimeoutWaitingForResponse;
				if (hasTimedOut && !wasMessageReceived)
				{
					ConsoleWriteSerialOutput(output);

					Assert.Fail("Timed out waiting for message received (" + TimeoutWaitingForResponse + " seconds)");
				}
			}
		}
		#endregion

		#region Specific Device Command Functions
		public void ResetDeviceSettings()
		{
			Console.WriteLine("");
			Console.WriteLine("Resetting device default settings...");
			Console.WriteLine("  Sending 'X' command to device");
			Console.WriteLine("");

			var cmd = "X";

			SendDeviceCommand(cmd);
		}

		public void SetDeviceReadInterval(int numberOfSeconds)
		{
			var cmd = "V" + numberOfSeconds;

			Console.WriteLine("");
			Console.WriteLine("Setting device read interval to " + numberOfSeconds + " second(s)...");
			Console.WriteLine("  Sending '" + cmd + "' command to device");
			Console.WriteLine("");

			SendDeviceCommand(cmd);
		}

		public void ReverseDeviceCalibration()
		{
			var cmd = "R";

			Console.WriteLine("");
			Console.WriteLine("Reversing device calibration settings...");
			Console.WriteLine("  Sending '" + cmd + "' command to device");
			Console.WriteLine("");

			SendDeviceCommand(cmd);
		}
		#endregion

		#region Soil Moisture Simulator Functions
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
		#endregion

		#region Simulator Read Functions
		public bool SimulatorDigitalRead(int pinNumber)
		{
			return SimulatorClient.DigitalRead(pinNumber);
		}
		#endregion
	}
}