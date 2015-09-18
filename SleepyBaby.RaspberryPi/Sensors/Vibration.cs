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

        protected override double GetReading()
        {
            var sensorReading = TransferReadingFromMcp3008();

            // When "Fast Vibration Sensor Switch" (SW-18010P) sensor connected to 3.3V with 3.3k pull-up resistor
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
