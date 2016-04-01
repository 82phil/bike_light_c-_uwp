using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Devices.Enumeration;
using System.Threading;
using System.Threading.Tasks;
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

using Communication;

using Microsoft.Maker.Serial;
using Microsoft.Maker.Firmata;
using Microsoft.Maker.RemoteWiring;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BikeLight
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CommPage : Page
    {

        private DispatcherTimer timeout;

        // Arduino Remote
        // RemoteDevice Arduino;

        private CancellationTokenSource cancelTokenSource;

        public CommPage()
        {
            this.InitializeComponent();

            App.Firmata = new UwpFirmata();
        }

        private void On_Attempt_Connect(object sender, RoutedEventArgs e)
        {
            DeviceInformation device = null;
            if (DeviceList.SelectedItem != null)
            {
                var selectedConnection = DeviceList.SelectedItem as Connection;
                device = selectedConnection.Source as DeviceInformation;
            }
            else
            {
                ConnectMessage.Text = "You must select an item to proceed.";
                return;
            }

            App.blueSerial = new BluetoothSerial(device);

            App.blueSerial.ConnectionEstablished += OnConnectionEstablished;
            App.blueSerial.ConnectionFailed += DeviceConnectionFailed;
            App.blueSerial.ConnectionLost += DeviceConnectionLost;

            ConnectMessage.Text = "Attempting to connect...";

            App.blueSerial.begin();

            //start a timer for connection timeout
            timeout = new DispatcherTimer();
            timeout.Interval = new TimeSpan(0, 0, 30);
            timeout.Tick += ConnectionTimeout;
            timeout.Start();

        }

        private void On_Refresh(object sender, RoutedEventArgs e)
        {
            RefreshList();
        }

        private void RefreshList()
        {
            Task<DeviceInformationCollection> task = null;
            cancelTokenSource = new CancellationTokenSource();
            cancelTokenSource.Token.Register(() => OnConnectionCancelled());

            task = BluetoothSerial.listAvailableDevicesAsync().AsTask<DeviceInformationCollection>(cancelTokenSource.Token);

            if (task != null)
            {
                //store the returned DeviceInformation items when the task completes
                task.ContinueWith(listTask =>
                {
                    //store the result and populate the device list on the UI thread
                    var action = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, new Windows.UI.Core.DispatchedHandler(() =>
                    {
                        Connections connections = new Connections();

                        var result = listTask.Result;
                        if (result == null || result.Count == 0)
                        {
                            ConnectMessage.Text = "No items found.";
                            DeviceList.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            foreach (DeviceInformation device in result)
                            {
                                connections.Add(new Connection(device.Name, device));
                            }
                            ConnectMessage.Text = "Select an item and press \"Connect\" to connect.";
                        }

                        DeviceList.ItemsSource = connections;
                    }));
                });
            }

        }

        private void OnConnectionEstablished()
        {
            timeout.Stop();
            ConnectMessage.Text = "Connection successful";
            // Hand off to Firmata
            App.Firmata.begin(App.blueSerial);
            this.Frame.Navigate(typeof(MainPage));
        }

        private void DeviceConnectionFailed(string message)
        {
            timeout.Stop();
            ConnectMessage.Text = message;
        }

        private void DeviceConnectionLost(string message)
        {
            timeout.Stop();
            ConnectMessage.Text = message;
        }

        private void ConnectionTimeout(object sender, object e)
        {
            var action = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, new Windows.UI.Core.DispatchedHandler(() =>
            {
                timeout.Stop();
                ConnectMessage.Text = "Connection attempt timed out.";
            }));
        }

        private void OnConnectionCancelled()
        {
            timeout.Stop();
            ConnectMessage.Text = "Connection attempt cancelled.";
        }

        
    }
}
