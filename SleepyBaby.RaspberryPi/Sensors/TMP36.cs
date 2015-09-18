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
        public Tmp36(SpiDevice mcp3008, int channel, TimeSpan dataReadInterval) : base(mcp3008, channel, dataReadInterval)
        {
        }

        protected override double GetReading()
        {
            var sensorReading = TransferReadingFromMcp3008();

            //TMP36 == 10mV/1degC ... 3.3V = 3300.0 mV, 10 bit chip # steps is 2 exp 10 == 1024
            var millivolts = sensorReading * (3300.0 / 1024.0);
            var tempC = (millivolts - 500) / 10;
            var tempF = (tempC * 9.0 / 5.0) + 32;

            return Math.Round(tempF, 1);
        }
    }
}
