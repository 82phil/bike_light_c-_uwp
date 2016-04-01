using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using BikeLight.Lighting;
using Windows.UI.Core;
using Windows.Devices.Sensors;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BikeLight
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        // Lighting control
        bike01 bike_lighting;

        private LightSensor _lightsensor;

        public MainPage()
        {
            this.InitializeComponent();

            bike_lighting = new bike01(App.Firmata);

            _lightsensor = LightSensor.GetDefault();

            if (_lightsensor != null)
            {
                // Establish the report interval
                uint minReportInterval = _lightsensor.MinimumReportInterval;
                uint reportInterval = minReportInterval > 16 ? minReportInterval : 16;
                _lightsensor.ReportInterval = reportInterval;

                // Establish the event handler
                _lightsensor.ReadingChanged += new TypedEventHandler<LightSensor, LightSensorReadingChangedEventArgs>(ReadingChanged);
            }
            else
            {
                txtLuxValue.Text = "Not supported!";
            }
        }

        private void ReadingChanged(object sender, LightSensorReadingChangedEventArgs e)
        {
            var action = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                LightSensorReading reading = e.Reading;
                txtLuxValue.Text = String.Format("{0,5:0.00}", reading.IlluminanceInLux);
                //if (ct_Slider.Value < reading.IlluminanceInLux)
                //{
                //    bike_lighting.frontRight.segment_color(0, 0, 0);
                //}
                //else if ((nt_Slider.Value < reading.IlluminanceInLux) && (reading.IlluminanceInLux <= ct_Slider.Value))
                //{
                //    bike_lighting.frontRight.segment_color(200, 0, 0);
                //}
                //else if ((at_Slider.Value < reading.IlluminanceInLux) && (reading.IlluminanceInLux <= nt_Slider.Value))
                //{
                //    bike_lighting.frontRight.segment_color(255, 0, 0);
                //}
                //else {
                //    bike_lighting.frontRight.segment_color(150, 0, 0);
                //}
                //bike_lighting.frontRight.update();
            });
        }

        private void FrontRight_Toggled(object sender, RoutedEventArgs e)
        {
            if (FrontRight.IsOn)
            {
                bike_lighting.frontRight.segment_color(255, 255, 255);
            } else
            {
                bike_lighting.frontRight.segment_color(0, 0, 0);
            }
            bike_lighting.frontRight.update();
        }

        private void FrontLeft_Toggled(object sender, RoutedEventArgs e)
        {
            if (FrontLeft.IsOn)
            {
                bike_lighting.frontLeft.segment_color(255, 255, 255);
            }
            else
            {
                bike_lighting.frontLeft.segment_color(0, 0, 0);
            }
            bike_lighting.frontLeft.update();
        }

        private void FrontSideRight_Toggled(object sender, RoutedEventArgs e)
        {
            if (FrontSideRight.IsOn)
            {
                bike_lighting.frontSideRight.segment_color(255, 255, 0);
            }
            else
            {
                bike_lighting.frontSideRight.segment_color(0, 0, 0);
            }
            bike_lighting.frontSideRight.update();
        }

        private void FrontSideLeft_Toggled(object sender, RoutedEventArgs e)
        {
            if (FrontSideLeft.IsOn)
            {
                bike_lighting.frontSideLeft.segment_color(255, 255, 0);
            }
            else
            {
                bike_lighting.frontSideLeft.segment_color(0, 0, 0);
            }
            bike_lighting.frontSideLeft.update();
        }

        private void SideRight_Toggled(object sender, RoutedEventArgs e)
        {
            if (SideRight.IsOn)
            {
                bike_lighting.SideRight.segment_color(255, 255, 255);
            }
            else
            {
                bike_lighting.SideRight.segment_color(0, 0, 0);
            }
            bike_lighting.SideRight.update();
        }

        private void SideLeft_Toggled(object sender, RoutedEventArgs e)
        {
            if (SideLeft.IsOn)
            {
                bike_lighting.SideLeft.segment_color(255, 255, 255);
            }
            else
            {
                bike_lighting.SideLeft.segment_color(0, 0, 0);
            }
            bike_lighting.SideLeft.update();
        }

        private void Rear_Toggled(object sender, RoutedEventArgs e)
        {
            if (Rear.IsOn)
            {
                bike_lighting.Rear.segment_color(255, 0, 0);
            }
            else
            {
                bike_lighting.Rear.segment_color(0, 0, 0);
            }
            bike_lighting.Rear.update();
        }
    }
}
