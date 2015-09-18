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

        protected override double GetData()
        {
            var channelByte = (byte)((8 + _channel) << 4);
            var transmitBuffer = new byte[3] { 1, channelByte, 0x00 };
            var receiveBuffer = new byte[3];

            _mcp3008.TransferFullDuplex(transmitBuffer, receiveBuffer);
            //first byte returned is 0 (00000000), 
            //second byte returned we are only interested in the last 2 bits 00000011 (mask of &3) 
            //then shift result 8 bits to make room for the data from the 3rd byte (makes 10 bits total)
            //third byte, need all bits, simply add it to the above result 
            var sensorData = ((receiveBuffer[1] & 3) << 8) + receiveBuffer[2];
            //TMP36 == 10mV/1degC ... 3.3V = 3300.0 mV, 10 bit chip # steps is 2 exp 10 == 1024
            var millivolts = sensorData * (3300.0 / 1024.0);
            var tempC = (millivolts - 500) / 10;
            var tempF = (tempC * 9.0 / 5.0) + 32;

            return Math.Round(tempF, 1);
        }
    }
}
