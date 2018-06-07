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
	[TestFixture(Category = "Integration")]
	public class ReadIntervalCommandTestFixture : BaseTestFixture
	{
		[Test]
		public void Test_SetReadIntervalCommand()
		{
			using (var helper = new ReadIntervalCommandTestHelper())
			{
				helper.ReadInterval = 5;

				helper.DevicePort = GetDevicePort();
				helper.DeviceBaudRate = 9600; // TODO: Should this be configurable via environment variables?

				helper.SimulatorPort = GetSimulatorPort();
				helper.SimulatorBaudRate = 9600; // TODO: Should this be configurable via environment variables?

				helper.TestSetReadIntervalCommand();
			}
		}
	}
}