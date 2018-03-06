using System;
using NUnit.Framework;
using duinocom;
using System.Threading;
using ArduinoSerialControllerClient;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SoilMoistureSensorCalibratedSerial.Tests.Integration
{
	[TestFixture(Category="Integration")]
	public class CalibrateCommandTestFixture : BaseTestFixture
	{
		[Test]
		public void Test_CalibrateDryCommand()
		{
			var percentage = 20;

			var raw = 219;//(percentage * 1024 / 100)-1;

			TestCalibrationCommand ("dry", "D", percentage, raw);
		}

		[Test]
		public void Test_CalibrateWetCommand()
		{
			var percentage = 80;

			var raw = 880;//percentage * 1023 / 100;

			TestCalibrationCommand ("wet", "W", percentage, raw);
		}

		public void TestCalibrationCommand(string label, string command, int percentageIn, int expectedRaw)
		{

			Console.WriteLine ("");
			Console.WriteLine ("==============================");
			Console.WriteLine ("Starting calibrate " + label + " command test");
			Console.WriteLine ("");
			Console.WriteLine ("Percentage in: " + percentageIn);
			Console.WriteLine ("Expected raw: " + expectedRaw);

			SerialClient soilMoistureMonitor = null;
			ArduinoSerialDevice soilMoistureSimulator = null;

			try {
				soilMoistureMonitor = new SerialClient ("/dev/ttyUSB0", 9600);
				soilMoistureSimulator = new ArduinoSerialDevice ("/dev/ttyUSB1", 9600);

				Console.WriteLine("");
				Console.WriteLine("Connecting to serial devices...");
				Console.WriteLine("");

				soilMoistureMonitor.Open ();
				soilMoistureSimulator.Connect ();

				Thread.Sleep (4000);


				Console.WriteLine("");
				Console.WriteLine("Sending 'X' command to device to reset to defaults...");
				Console.WriteLine("");

				// Reset defaults
				soilMoistureMonitor.WriteLine ("X");

				Thread.Sleep(5000);

				Console.WriteLine("");
				Console.WriteLine("Reading the output from the monitor...");
				Console.WriteLine("");

				// Read the output
				var outputLine = soilMoistureMonitor.Read ();

				Console.WriteLine (outputLine);
				Console.WriteLine ("");

				Thread.Sleep(2000);

				Console.WriteLine("");
				Console.WriteLine("Sending analog percentage to simulator: " + percentageIn);
				Console.WriteLine("");

				// Set the simulated soil moisture
				soilMoistureSimulator.AnalogWritePercentage (9, percentageIn);

				Thread.Sleep(12000);

				Console.WriteLine("");
				Console.WriteLine("Reading output from the monitor device...");
				Console.WriteLine("");
				// Read the output
				outputLine = soilMoistureMonitor.Read ();

				Console.WriteLine (outputLine);
				Console.WriteLine ("");

				// Extract the data line
				var dataLine = "";

				var dataLines = outputLine.Split('\n');

				for (int i = dataLines.Length-1; i>=0; i--)
				{
					if (dataLines[i].StartsWith("D;"))
					{
						dataLine = dataLines[i];
						break;
					}
				}

				Console.WriteLine ("Data line:");
				Console.WriteLine (dataLine);
				Console.WriteLine ("");

				// Parse the values in the data line
				var values = ParseOutputLine(dataLine);

				// Get the raw soil moisture value
				var rawValue = values["R"];


				Console.WriteLine("");
				Console.WriteLine("Checking the values from the monitor device...");
				Console.WriteLine("");

				// Ensure the raw value is in the valid range
				Assert.IsTrue(rawValue >= expectedRaw-3 && rawValue <= expectedRaw+3, "Raw value is outside the valid range: " + rawValue);


				Console.WriteLine("");
				Console.WriteLine("Sending '" + command + "' command to monitor device...");
				Console.WriteLine("");

				// Send the command
				soilMoistureMonitor.WriteLine (command);

				Thread.Sleep(2000);

				Console.WriteLine("");
				Console.WriteLine("Reading the output from the monitor device...");
				Console.WriteLine("");

				// Read the output
				outputLine = soilMoistureMonitor.Read ();

				Console.WriteLine (outputLine);
				Console.WriteLine ("");

				Console.WriteLine("");
				Console.WriteLine("Checking the output...");
				Console.WriteLine("");

				// Check the output
				var expected = "Setting " + label + " soil moisture sensor calibration value:";
				Assert.IsTrue(outputLine.Contains(expected), "Didn't find expected output");

				var lastLine = "";

				var lines = outputLine.Split('\n');

				// Extract the line containing the calibration value
				for (int i = lines.Length-1; i>=0; i--)
				{
					var line = lines[i];
					if (line.StartsWith(expected))
					{
						lastLine = line;
						break;
					}
				}

				Console.WriteLine("Last line");
				Console.WriteLine(lastLine);
				Console.WriteLine("");

				// Extraction the calibration value
				var cvString = lastLine.Substring(lastLine.IndexOf(":")+2, 3);
				var calibrationValue = Convert.ToInt32(cvString);

				Console.WriteLine("Calibration value: " + calibrationValue);
				Console.WriteLine("");

				// Ensure the calibration value is in the valid range
				Assert.IsTrue(calibrationValue >= expectedRaw-3 && calibrationValue <= expectedRaw+3, "Calibration value is outside the valid range: " + calibrationValue);

			} catch (IOException ex) {
				Console.WriteLine (ex.ToString ());
			} finally {
				if (soilMoistureMonitor != null)
					soilMoistureMonitor.Close ();

				if (soilMoistureSimulator != null)
					soilMoistureSimulator.Disconnect ();
			}
		}

		/*[Test]
		public void Test_CalibrateWetCommand()
		{
			Console.WriteLine ("");
			Console.WriteLine ("==============================");
			Console.WriteLine ("Starting calibrate wet command test");
			Console.WriteLine ("");

			SerialClient soilMoistureMonitor = null;
			ArduinoSerialDevice soilMoistureSimulator = null;

			try {
				soilMoistureMonitor = new SerialClient ("/dev/ttyUSB0", 9600);
				soilMoistureSimulator = new ArduinoSerialDevice ("/dev/ttyUSB1", 9600);

				soilMoistureMonitor.Open ();
				soilMoistureSimulator.Connect ();

				Thread.Sleep (4000);

				// Reset defaults
				soilMoistureMonitor.WriteLine ("X");

				// Set the simulated soil moisture to 20%
				soilMoistureSimulator.AnalogWritePercentage (9, 80);

				Thread.Sleep(1000);

				// Read the output
				var outputLine = soilMoistureMonitor.Read ();

				Console.WriteLine (outputLine);
				Console.WriteLine ("");

				// Extract the data line
				var dataLine = (from line in outputLine.Split('\n')
					where line.StartsWith("D;")
					select line).FirstOrDefault();

				// Parse the values in the data line
				var values = ParseOutputLine(dataLine);

				// Get the raw soil moisture value
				var rawValue = values["R"];

				// Ensure the raw value is in the valid range
				Assert.IsTrue(rawValue >= 218 && rawValue <= 222, "Raw value is outside the valid range.");

				// Send the "D" (dry) command
				soilMoistureMonitor.WriteLine ("W");

				Thread.Sleep(1000);

				// Read the output
				outputLine = soilMoistureMonitor.Read ();

				Console.WriteLine (outputLine);
				Console.WriteLine ("");

				// Check the output
				var expected = "Setting dry soil moisture sensor calibration value:";
				Assert.IsTrue(outputLine.Contains(expected), "Didn't find expected output");

				var lastLine = "";

				var lines = outputLine.Split('\n');

				// Extract the line containing the calibration value
				for (int i = lines.Length-1; i>=0; i--)
				{
					var line = lines[i];
					if (line.StartsWith(expected))
					{
						lastLine = line;
						break;
					}
				}

				Console.WriteLine("Last line");
				Console.WriteLine(lastLine);
				Console.WriteLine("");

				// Extraction the calibration value
				var cvString = lastLine.Substring(lastLine.IndexOf(":")+2, 3);
				var calibrationValue = Convert.ToInt32(cvString);

				Console.WriteLine("Calibration value: " + calibrationValue);
				Console.WriteLine("");

				// Ensure the calibration value is in the valid range
				Assert.IsTrue(calibrationValue >= 218 && calibrationValue <= 222, "Calibration value is outside the valid range.");

			} catch (IOException ex) {
				Console.WriteLine (ex.ToString ());
			} finally {
				if (soilMoistureMonitor != null)
					soilMoistureMonitor.Close ();

				if (soilMoistureSimulator != null)
					soilMoistureSimulator.Disconnect ();
			}
		}*/

		public Dictionary<string, int> ParseOutputLine(string outputLine)
		{
			var dictionary = new Dictionary<string, int> ();

			if (IsValidOutputLine (outputLine)) {
				foreach (var pair in outputLine.Split(';')) {
					var parts = pair.Split (':');

					if (parts.Length == 2) {
						var key = parts [0];
						var value = 0;
						try {
							value = Convert.ToInt32 (parts [1]);

							dictionary [key] = value;
						} catch {
							Console.WriteLine ("Warning: Invalid key/value pair '" + pair + "'");
						}
					}
				}
			}

			return dictionary;
		}

		public bool IsValidOutputLine(string outputLine)
		{
			var dataPrefix = "D;";

			return outputLine.StartsWith(dataPrefix);
		}
	}
}
