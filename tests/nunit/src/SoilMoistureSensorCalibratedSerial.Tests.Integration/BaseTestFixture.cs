using System;
using NUnit.Framework;

namespace SoilMoistureSensorCalibratedSerial.Tests.Integration
{
	public class BaseTestFixture
	{
		public int PwmHigh = 255;
		public int AnalogHigh = 1023;
		
		public bool CalibrationIsReversedByDefault = true; // Reversed by default to matches common soil moisture sensors
		
		public BaseTestFixture ()
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
		
		public bool IsWithinRange(int expectedValue, int actualValue, int allowableMarginOfError)
		{
			Console.WriteLine("Checking value is within range...");
			Console.WriteLine("Expected value: " + expectedValue);
			Console.WriteLine("Actual value: " + actualValue);
			Console.WriteLine("Allowable margin of error: " + allowableMarginOfError);
			
			var minAllowableValue = expectedValue-allowableMarginOfError;
			if (minAllowableValue < 0)
				minAllowableValue = 0;
			var maxAllowableValue = expectedValue+allowableMarginOfError;
			
			Console.WriteLine("Max allowable value: " + maxAllowableValue);
			Console.WriteLine("Min allowable value: " + minAllowableValue);
			
			var isWithinRange = actualValue <= maxAllowableValue &&
				actualValue >= minAllowableValue;
		
			Console.WriteLine("Is within range: " + isWithinRange);
		
			return isWithinRange;
		}
	}
}
