using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Spi;

namespace SleepyBaby.RaspberryPi.Sensors
{
    class Vibration : BaseSensor
    {
        public Vibration(SpiDevice mcp3008, int channel, TimeSpan dataReadInterval) : base(mcp3008, channel, dataReadInterval)
        {
        }

        /// <summary>
        /// Gets reading of sensor switch to detect vibration
        /// </summary>
        /// <returns>Returns 1.0 if vibration detected, otherwise 0.0</returns>
        protected override double GetReading()
        {
            var sensorReading = TransferReadingFromMcp3008();

            // When vibration sensor switch is connected to 3.3V
            if (sensorReading < 1023)
            {
                // vibration detected
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
