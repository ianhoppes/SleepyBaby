using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using Windows.ApplicationModel.Background;
using Windows.Devices.Gpio;
using Windows.Devices.Spi;
using Windows.System.Threading;
using Windows.Devices.Enumeration;
using System.Diagnostics;
using SleepyBaby.RaspberryPi.Sensors;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace SleepyBaby.RaspberryPi
{   
    public sealed class StartupTask : IBackgroundTask
    {
        private SpiDevice _mcp3008;
        private Tmp36 _tmp36;

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            InitMcp3008();
            InitSensors();
            while (true)
            {

            }
        }

        private async void InitMcp3008()
        {
            //using SPI0 on the Pi
            var spiSettings = new SpiConnectionSettings(0); //for spi bus index 0
            spiSettings.ClockFrequency = 10000; //10kHz-3.6MHz
            spiSettings.Mode = SpiMode.Mode0;

            var spiQuery = SpiDevice.GetDeviceSelector("SPI0");
            var deviceInfo = await DeviceInformation.FindAllAsync(spiQuery);
            if (deviceInfo != null && deviceInfo.Count > 0)
            {
                _mcp3008 = await SpiDevice.FromIdAsync(deviceInfo[0].Id, spiSettings);
            }
            else
            {
                throw new InvalidOperationException("No MCP3008 found at SPI0");
            }
        }

        private void InitSensors()
        {
            _tmp36 = new Tmp36(_mcp3008, 0, TimeSpan.FromSeconds(10));
            _tmp36.DataReadEvent += Tmp36_DataReadEvent;
        }

        private void Tmp36_DataReadEvent(object sender, SensorEventArgs e)
        {
            Debug.WriteLine("Tmp36: " + e.Reading);
        }
    }
}
