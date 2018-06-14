using System;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

namespace SoilMoistureSensorCalibratedSerial.Tests.Integration
{
	public class BaseTestFixture
	{
		public BaseTestFixture()
		{
		}

		[SetUp]
		public void Initialize()
		{
		}

		[TearDown]
		public void Finish()
		{
		}

		public string GetDevicePort()
		{
			var devicePort = Environment.GetEnvironmentVariable("MONITOR_PORT");

			if (String.IsNullOrEmpty(devicePort))
				devicePort = "/dev/ttyUSB0";

			Console.WriteLine("Device port: " + devicePort);

			return devicePort;
		}


		public string GetSimulatorPort()
		{
			var simulatorPort = Environment.GetEnvironmentVariable("MONITOR_SIMULATOR_PORT");

			if (String.IsNullOrEmpty(simulatorPort))
				simulatorPort = "/dev/ttyUSB1";

			Console.WriteLine("Simulator port: " + simulatorPort);

			return simulatorPort;
		}

		public int GetSerialBaudRate()
		{
			return 9600;
		}
	}
}
