using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Spi;

namespace SleepyBaby.RaspberryPi.Sensors
{
    class LaserBreakBeam : BaseSensor
    {
        public LaserBreakBeam(SpiDevice mcp3008, int channel, TimeSpan dataReadInterval) : base(mcp3008, channel, dataReadInterval)
        {
        }

        /// <summary>
        /// Gets reading of photocell at end of laser beam to determine if beam is broken
        /// </summary>
        /// <returns>Returns 1.0 if beam broken, otherwise 0.0</returns>
        protected override double GetReading()
        {
            var photocellReading = TransferReadingFromMcp3008();

            // When photocell connected to 3.3V with 20k pull-down resistor:
            // Dim room = ~40-50
            // Overhead 100W light = ~500
            // Direct LED flashlight = ~1000
            // Laser = >1000

            if (photocellReading < 1000)
            {
                // beam broken
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
