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

        protected abstract double GetData();

        protected void DataReadTimer_Tick(ThreadPoolTimer timer)
        {
            var data = GetData();

            if (DataReadEvent != null)
                DataReadEvent(this, new SensorEventArgs { Value = data });
        }
    }
}
