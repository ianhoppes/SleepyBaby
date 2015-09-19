using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Spi;

namespace SleepyBaby.RaspberryPi.Sensors
{
    abstract class BaseSensor
    {
        protected readonly SpiDevice _mcp3008;
        protected readonly int _channel;

        public BaseSensor(SpiDevice mcp3008, int channel)
        {
            this._mcp3008 = mcp3008;
            this._channel = channel;
        }

        public abstract double GetReading();

        /// <summary>
        /// Gets value of sensor from the specified channel of the MCP3008. <a href="http://blog.falafel.com/mcp3008-analog-to-digital-conversion/">More information</a>
        /// </summary>
        /// <returns>Sensor reading as double</returns>
        protected double TransferReadingFromMcp3008()
        {
            var channelByte = (byte)((8 + _channel) << 4);
            var transmitBuffer = new byte[3] { 1, channelByte, 0x00 };
            var receiveBuffer = new byte[3];

            _mcp3008.TransferFullDuplex(transmitBuffer, receiveBuffer);

            double sensorData = ((receiveBuffer[1] & 3) << 8) + receiveBuffer[2];

            return sensorData;
        }
    }
}
