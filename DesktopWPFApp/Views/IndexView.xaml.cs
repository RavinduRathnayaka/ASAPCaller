using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DesktopWPFApp.Models;
using System.Threading;

namespace DesktopWPFApp.Views {
    /// <summary>
    /// Interaction logic for IndexView.xaml
    /// </summary>
    public partial class IndexView : UserControl {
        SerialCommunication sc;
        public IndexView(SerialCommunication aSc) {
            InitializeComponent();
            sc = aSc;
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            getStatus();
            if (sc.SerialPorts != null && sc.SerialPorts.Length != 0) {
                sc.PortConnect();
            }
            else {
                btnMain.Content = "Device is not connected";
            }
        }
        #region Ui updates on trigger
        public async void getStatus() {
            while (true) {
                UpdateUi();
                await Task.Delay(1);
            }
        }


        private void UpdateUi() {
            if (sc.AccidentDetected) {
                if (!WaiitingForResponse) {
                    grdContainer.Style = Resources["backgroundAccidentStye"] as Style;
                    btnMain.Template = Resources["btnMainAccidentStyle"] as ControlTemplate;
                    Thread countDown = new Thread(CountDownToCall);
                    countDown.Start();
                    WaiitingForResponse = true;
                    btnMain.Click += BtnMain_Click;
                }
            }
            else if (sc.Conntected) {
                grdContainer.Style = Resources["backgroundConnectedStye"] as Style;
                btnMain.Template = Resources["btnMainConnectedStyle"] as ControlTemplate;
                btnMain.Content = "ASAP Caller Online";
                btnMain.Click -= BtnMain_Click;
                btnStopCall.Visibility = Visibility.Hidden;
                txtblkQ.Visibility = Visibility.Hidden;
                btnStopCall.Visibility = Visibility.Hidden;
                txtblkStatus.Visibility = Visibility.Hidden;
                txtblkCount.Visibility = Visibility.Hidden;
            }
            else if (!sc.Conntected) {
                grdContainer.Style = Resources["backgroundDisconnectedStye"] as Style;
                btnMain.Template = Resources["btnMainDisconnectedStyle"] as ControlTemplate;
                btnMain.Content = "ASAP Caller Offline";
                btnMain.Click -= BtnMain_Click;
            }
            if (sc.Status.Contains("Connect")) {
                AnimationConnecting();
            }
        }
        private bool IsCalling = false;
        private bool StopCalling = false;
        private bool WaiitingForResponse = false;

        private void CountDownToCall() {
            Dispatcher.Invoke(() => {
                txtblkQ.Visibility = Visibility.Visible;
                btnStopCall.Visibility = Visibility.Visible;
                txtblkStatus.Visibility = Visibility.Visible;
                btnMain.Content = "Collision Detected";
                txtblkStatus.Text = "Calling in..";
                txtblkCount.Visibility = Visibility.Visible;
            });
            for (int i = 10; i >= 0 && !IsCalling && !StopCalling; i--) {
                Dispatcher.Invoke(() => {
                    txtblkCount.Text = i.ToString();
                });
                Thread.Sleep(1000);
            }
            if (!StopCalling) {
                SendCall();
            }
            StopCalling = false;
        }
        private void StopCall() {
            StopCalling = true;
            btnStopCall.Visibility = Visibility.Hidden;
            ResetSerialData();
            WaiitingForResponse = false;
        }

        private void ResetSerialData() {
            sc.AccidentDetected = false;
            sc.SerialWrite("c");
            getStatus();
        }

        private void BtnMain_Click(object sender, RoutedEventArgs e) {
            SendCall();
        }

        private void SendCall() {
            IsCalling = true;
            Dispatcher.Invoke(() => {
                btnMain.Content = "Calling...";
                btnMain.Content = "Calling For Help...";
                btnStopCall.Visibility = Visibility.Hidden;
                txtblkQ.Visibility = Visibility.Hidden;
                btnStopCall.Visibility = Visibility.Hidden;
                txtblkStatus.Visibility = Visibility.Hidden;
                txtblkCount.Visibility = Visibility.Hidden;
            });
            //TODO: Serialcom Call..
        }

        private void btnStopCall_Click(object sender, RoutedEventArgs e) {
            StopCall();
        }
        private async void AnimationConnecting() {
            while (!sc.Conntected) {
                btnMain.Content = sc.Status;
                await Task.Delay(1);
            }
            btnMain.Content = "ASAP Caller Online";
        }
        #endregion
        


    }
}
