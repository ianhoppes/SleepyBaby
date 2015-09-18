using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Spi;
using Windows.System.Threading;

namespace SleepyBaby.RaspberryPi.Sensors
{
    abstract class BaseSensor
    {
        protected readonly SpiDevice _mcp3008;
        protected readonly int _channel;
        protected readonly ThreadPoolTimer _dataReadTimer;

        public event EventHandler<SensorEventArgs> DataReadEvent;

        public BaseSensor(SpiDevice mcp3008, int channel, TimeSpan dataReadInterval)
        {
            this._mcp3008 = mcp3008;
            this._channel = channel;
            this._dataReadTimer = ThreadPoolTimer.CreatePeriodicTimer(DataReadTimer_Tick, dataReadInterval);
        }

        protected abstract double GetReading();

        protected double TransferReadingFromMcp3008()
        {
            var channelByte = (byte)((8 + _channel) << 4);
            var transmitBuffer = new byte[3] { 1, channelByte, 0x00 };
            var receiveBuffer = new byte[3];

            _mcp3008.TransferFullDuplex(transmitBuffer, receiveBuffer);
            //first byte returned is 0 (00000000), 
            //second byte returned we are only interested in the last 2 bits 00000011 (mask of &3) 
            //then shift result 8 bits to make room for the data from the 3rd byte (makes 10 bits total)
            //third byte, need all bits, simply add it to the above result 
            double sensorData = ((receiveBuffer[1] & 3) << 8) + receiveBuffer[2];

            return sensorData;
        }

        protected void DataReadTimer_Tick(ThreadPoolTimer timer)
        {
            var reading = GetReading();

            if (DataReadEvent != null)
                DataReadEvent(this, new SensorEventArgs { Reading = reading });
        }
    }
}
