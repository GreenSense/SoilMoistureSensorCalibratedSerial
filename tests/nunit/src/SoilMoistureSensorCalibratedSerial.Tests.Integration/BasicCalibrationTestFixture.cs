using System;
using NUnit.Framework;
using duinocom;
using System.Threading;
using ArduinoSerialControllerClient;
using System.Collections.Generic;
using System.IO;

namespace SoilMoistureSensorCalibratedSerial.Tests.Integration
{
	//[TestFixture(Category="Integration")]
	public class BasicCalibrationTestFixture : BaseTestFixture
	{
		//[Test]
		public void Test_CalibrationAt0Percent()
		{
		  RunCalibrationTest(0, CalibrationIsReversedByDefault);
		}
		
		//[Test]
		public void Test_CalibrationAt50Percent()
		{
		  RunCalibrationTest(50, CalibrationIsReversedByDefault);
		}
		
		//[Test]
		public void Test_CalibrationAt100Percent()
		{
		  RunCalibrationTest(100, CalibrationIsReversedByDefault);
		}

		//[Test]
		public void Test_CalibrationAt0Percent_NotReversed()
		{
		  RunCalibrationTest(0, !CalibrationIsReversedByDefault);
		}
		
		/*[Test]
		public void Test_CalibrationAt50Percent_NotReversed()
		{
		  RunCalibrationTest(50, !CalibrationIsReversedByDefault);
		}
		
		[Test]
		public void Test_CalibrationAt100Percent_NotReversed()
		{
		  RunCalibrationTest(100, !CalibrationIsReversedByDefault);
		}*/
		
		public void RunCalibrationTest(int soilMoisturePercentage, bool calibrationIsReversed)
		{

      Console.WriteLine("");
      Console.WriteLine("==============================");
      Console.WriteLine("Starting calibration test");
      Console.WriteLine("==============================");
      Console.WriteLine("");
      
      SerialClient soilMoistureMonitor = null;
      ArduinoSerialDevice soilMoistureSimulator = null;
      
      int percentageValue = soilMoisturePercentage;
      
      try
      {
        soilMoistureMonitor = new SerialClient("/dev/ttyUSB0", 9600);
        soilMoistureSimulator = new ArduinoSerialDevice("/dev/ttyUSB1", 9600);
      
        soilMoistureMonitor.Open();
        soilMoistureSimulator.Connect();
        
        Thread.Sleep(5000);

        // Reset defaults on the board
        soilMoistureMonitor.WriteLine("X");

        // Reverse calibration if necessary
        if (calibrationIsReversed)
          soilMoistureMonitor.WriteLine("R");

        Thread.Sleep(5000);
        
        Console.WriteLine("");
        Console.WriteLine("Sending percentage to soil moisture simulator: " + percentageValue + "%");
        
        soilMoistureSimulator.AnalogWritePercentage(9, percentageValue);
        
        Thread.Sleep(5000);
        
        Console.WriteLine("");
        Console.WriteLine("Reading data from soil moisture monitor");
        Console.WriteLine("---------- Start Output ----------");
        
        var output = soilMoistureMonitor.Read();
        
        Console.WriteLine(output);
        Console.WriteLine("---------- End Output ----------");
        Console.WriteLine("");

        var dataLine = FindLastDataLine(output);

        Assert.IsNotNullOrEmpty(dataLine, "No output data detected");       

        Console.WriteLine("Identified last data line:");
        Console.WriteLine("  " + dataLine); 

        var data = ParseOutputLine(dataLine);
        

        Console.WriteLine("");
        Console.WriteLine("==============================");
        Console.WriteLine("Verifying Results");
        Console.WriteLine("==============================");
        Console.WriteLine("");
        var expectedCalibratedValue = percentageValue;
        
        if (calibrationIsReversed)
        {
          Console.WriteLine("");
          Console.WriteLine("Note: Calibration is configured to reverse values");
          Console.WriteLine("");
          expectedCalibratedValue = ArduinoConvert.ReversePercentage(percentageValue);
        }
        
        Console.WriteLine("");
        Console.WriteLine("Checking calibrated value (\"C\")");
        var calibratedValueIsWithinRange = IsWithinRange(expectedCalibratedValue, data["C"], 5);
        
        Assert.IsTrue(calibratedValueIsWithinRange, "Invalid value for 'C' (calibrated value).");
        
        Console.WriteLine("");
        Console.WriteLine("Checking raw value (\"R\")");
        
        var expectedRawValue = ArduinoConvert.PercentageToAnalog(percentageValue);
        
        var rawValueIsWithinRange = IsWithinRange(expectedRawValue, data["R"], 50);
        
        Assert.IsTrue(rawValueIsWithinRange, "Invalid value for 'R' (raw value).");
      }
      catch(Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
      finally
      {
        if (soilMoistureMonitor != null)
          soilMoistureMonitor.Close();
          
        if (soilMoistureSimulator != null)
          soilMoistureSimulator.Disconnect();
      }
      
      Console.WriteLine("");
      Console.WriteLine("==============================");
      Console.WriteLine("Finished calibration test");
      Console.WriteLine("==============================");
      Console.WriteLine("");
		}
		
		public Dictionary<string, int> ParseOutputLine(string outputLine)
		{
		  var dictionary = new Dictionary<string, int>();
		  
		  if (IsValidOutputLine(outputLine))
		  {
  		  foreach (var pair in outputLine.Split(';'))
  		  {
  		    var parts = pair.Split(':');
  		    
  		    if (parts.Length == 2)
  		    {
  		      var key = parts[0];
  		      var value = 0;
  		      try
  		      {
  		        value = Convert.ToInt32(parts[1]);
  		      
  		        dictionary[key] = value;
  		      }
  		      catch
  		      {
  		        Console.WriteLine("Warning: Invalid key/value pair '" + pair + "'");
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

    public string FindLastDataLine(string output)
    {
      var lastDataLine = String.Empty;
      foreach (var line in output.Split('\n'))
      {
        if (IsValidOutputLine(line))
          lastDataLine = line;
      }

      return lastDataLine;
    }
	}
}
