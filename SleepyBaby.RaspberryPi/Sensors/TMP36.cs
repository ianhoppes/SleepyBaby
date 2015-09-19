using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Spi;
using Windows.System.Threading;

namespace SleepyBaby.RaspberryPi.Sensors
{
    class Tmp36: BaseSensor
    {
        public Tmp36(SpiDevice mcp3008, int channel) : base(mcp3008, channel)
        {
        }

        /// <summary>
        /// Gets temperature from TMP36 sensor. <a href="https://learn.adafruit.com/tmp36-temperature-sensor/using-a-temp-sensor">More information</a>
        /// </summary>
        /// <returns>Returns temperature in Fahrenheit scale rounded to one decimal place</returns>
        public override double GetReading()
        {
            var sensorReading = TransferReadingFromMcp3008();

            var milliVolts = sensorReading * (3300.0 / 1024.0);
            var tempC = (milliVolts - 500) / 10;
            var tempF = (tempC * 9.0 / 5.0) + 32;

            return Math.Round(tempF, 1);
        }
    }
}
