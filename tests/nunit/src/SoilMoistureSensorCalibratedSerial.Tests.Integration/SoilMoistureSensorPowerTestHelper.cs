using System;
using System.Threading;
using NUnit.Framework;
namespace SoilMoistureSensorCalibratedSerial.Tests.Integration
{
	public class SoilMoistureSensorPowerTestHelper : GreenSenseHardwareTestHelper
	{
		public int ReadInterval = 1;

		public void TestSoilMoistureSensorPower()
		{
			WriteTitleText("Starting soil moisture sensor power test");

			EnableDevices();

			SetDeviceReadInterval(ReadInterval);

			var data = WaitForData();

			AssertDataValueEquals(data, "V", ReadInterval);

			var sensorDoesTurnOff = ReadInterval > DelayAfterTurningSoilMoistureSensorOn;

			if (sensorDoesTurnOff)
			{
				Console.WriteLine("The soil moisture sensor should turn off when not in use.");

				TestSoilMoistureSensorPowerTurnsOnAndOff();
			}
			else
			{
				Console.WriteLine("The soil moisture sensor should stay on permanently.");

				TestSoilMoistureSensorPowerStaysOn();
			}
		}

		public void TestSoilMoistureSensorPowerStaysOn()
		{
			WriteParagraphTitleText("Waiting until the soil moisture sensor is on before starting the test...");

			WaitUntilSoilMoistureSensorPowerPinIs(On);

			var durationInSeconds = ReadInterval * 5;

			WriteParagraphTitleText("Checking that soil moisture sensor power pin stays on...");

			CheckSoilMoistureSensorPowerPinForDuration(On, durationInSeconds);
		}

		public void TestSoilMoistureSensorPowerTurnsOnAndOff()
		{
			WriteParagraphTitleText("Waiting until the soil moisture sensor is on before starting the test...");

			WaitUntilSoilMoistureSensorPowerPinIs(On);
			WaitUntilSoilMoistureSensorPowerPinIs(Off);

			CheckSoilMoistureSensorOnDuration();
			CheckSoilMoistureSensorOffDuration();
		}

		public void CheckSoilMoistureSensorOnDuration()
		{
			WriteParagraphTitleText("Getting the total on time...");

			var totalOnTime = WaitWhileSoilMoistureSensorPowerPinIs(On);

			WriteParagraphTitleText("Checking the total on time is correct...");

			var expectedOnTime = DelayAfterTurningSoilMoistureSensorOn;

			Assert.IsTrue(IsWithinRange(expectedOnTime, totalOnTime, 0.2), "Total on time is incorrect");
		}

		public void CheckSoilMoistureSensorOffDuration()
		{
			WriteParagraphTitleText("Getting the total off time...");

			var totalOffTime = WaitWhileSoilMoistureSensorPowerPinIs(Off);

			WriteParagraphTitleText("Checking the total off time is correct...");

			var expectedOffTime = ReadInterval;
			Assert.IsTrue(IsWithinRange(expectedOffTime, totalOffTime, 0.2), "Total off time is incorrect");
		}

		public double WaitWhileSoilMoistureSensorPowerPinIs(bool expectedValue)
		{
			Console.WriteLine("Waiting while soil moisture sensor power pin is " + GetOnOffString(expectedValue));

			bool powerPinValue = !expectedValue;

			var startTime = DateTime.MinValue;
			var finishTime = DateTime.MinValue;

			bool isStarted = false;
			bool isFinished = false;

			while (!isFinished)
			{
				Console.Write(".");
				powerPinValue = SimulatorDigitalRead(SoilMoistureSimulatorPowerPin);

				if (startTime == DateTime.MinValue && powerPinValue == expectedValue)
				{
					startTime = DateTime.Now;
					isStarted = true;
				}

				if (isStarted && powerPinValue != expectedValue)
				{
					finishTime = DateTime.Now;
					isFinished = true;
				}
			}
			Console.WriteLine("");

			var waitTimeInSeconds = finishTime.Subtract(startTime).TotalSeconds;

			Console.WriteLine("  " + waitTimeInSeconds + " seconds");
			Console.WriteLine("");

			return waitTimeInSeconds;
		}

		public int WaitUntilSoilMoistureSensorPowerPinIs(bool expectedValue)
		{
			Console.WriteLine("Waiting until the soil moisture sensor power pin is " + GetOnOffString(expectedValue));

			bool powerPinValue = !expectedValue;

			var startTime = DateTime.Now;

			while (powerPinValue != expectedValue)
			{
				Console.Write(".");
				powerPinValue = SimulatorDigitalRead(SoilMoistureSimulatorPowerPin);
			}
			Console.WriteLine("");

			var waitTimeInSeconds = DateTime.Now.Subtract(startTime).TotalSeconds;

			Console.WriteLine("  " + waitTimeInSeconds + " seconds");
			Console.WriteLine("");

			return (int)waitTimeInSeconds;
		}

		public void CheckSoilMoistureSensorPowerPinForDuration(bool expectedPowerPinValue, int durationInSeconds)
		{
			Console.WriteLine("Checking soil moisture sensor power pin for specified duration...");
			Console.WriteLine("  Expected value: " + expectedPowerPinValue);
			Console.WriteLine("  Duration: " + durationInSeconds);

			var startTime = DateTime.Now;

			var waitTimeIsFinished = false;

			while (!waitTimeIsFinished)
			{
				Console.Write(".");
				waitTimeIsFinished = DateTime.Now.Subtract(startTime).TotalSeconds > durationInSeconds;

				bool powerPinValue = SimulatorDigitalRead(SoilMoistureSimulatorPowerPin);

				if (expectedPowerPinValue)
					Assert.AreEqual(true, powerPinValue, "Soil moisture sensor power pin is off when it should be on.");
				else
					Assert.AreEqual(false, powerPinValue, "Soil moisture sensor power pin is on when it should be off.");
			}

			Console.WriteLine("");
			Console.WriteLine("Soil moisture sensor power works as expected.");
			Console.WriteLine("");
		}
	}
}
