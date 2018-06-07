using System;
using NUnit.Framework;
using duinocom;
using System.Threading;
using ArduinoSerialControllerClient;
using System.Collections.Generic;
using System.IO;

namespace SoilMoistureSensorCalibratedSerial.Tests.Integration
{
	[TestFixture(Category="Integration")]
	public class FullScaleTestFixture : BaseTestFixture
	{
		[Test]
		public void Test_FullScaleTest()
		{
      using (var helper = new FullScaleMonitorTestHelper())
      {
        helper.DevicePort = GetDevicePort();
        helper.DeviceBaudRate = 9600; // TODO: Should this be configurable via environment variables?

        helper.SimulatorPort = GetSimulatorPort();
        helper.SimulatorBaudRate = 9600; // TODO: Should this be configurable via environment variables?

        helper.RunFullScaleTest();
      }
		}
	}
}
