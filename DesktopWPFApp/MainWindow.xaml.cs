using DesktopWPFApp.Models;
using DesktopWPFApp.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DesktopWPFApp {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        SerialCommunication sc = new SerialCommunication();
        public MainWindow() {
            InitializeComponent();

        }
        private void Window_Loaded(object sender, RoutedEventArgs e) {
            SettingsView settings = new SettingsView(sc);
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
                grdContainer.Style = Resources["backgroundAccidentStye"] as Style;
                btnMain.Template = Resources["btnMainAccidentStyle"] as ControlTemplate;
                CountDownToCall();
                btnMain.Click += BtnMain_Click;
            }
            else if (sc.Conntected) {
                grdContainer.Style = Resources["backgroundConnectedStye"] as Style;
                btnMain.Template = Resources["btnMainConnectedStyle"] as ControlTemplate;
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
                btnMain.Click -= BtnMain_Click;
            }
            if (sc.Status.Contains("Connect")) {
                AnimationConnecting();
            }
        }
        private bool IsCalling = false;
        private bool StopCalling=false;
        private void CountDownToCall() {
            txtblkQ.Visibility = Visibility.Visible;
            btnStopCall.Visibility = Visibility.Visible;
            txtblkStatus.Visibility = Visibility.Visible;
            btnMain.Content = "Collision Detected";
            txtblkStatus.Text = "Calling in..";
            txtblkCount.Visibility = Visibility.Visible;
            for (int i = 10; i > 0 && !IsCalling && !StopCalling; i--) {
                //TODO: add Countdown and animation
            }
        }
        private void StopCall() {
            StopCalling = true;
            btnStopCall.Visibility = Visibility.Hidden;
            //TODO: Reset app?
        }
        private void BtnMain_Click(object sender, RoutedEventArgs e) {
            btnMain.Content = "Calling...";
            IsCalling=true;
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
        #region Animations
        private void chkSettingsToggle_Checked(object sender, RoutedEventArgs e) {
            DoubleAnimation widthAnimation = new DoubleAnimation(400, new Duration(TimeSpan.FromSeconds(0.1)));
            pnlSettingsContainer.Children.Add(new SettingsView(sc));
            pnlSettingsContainer.BeginAnimation(WidthProperty, widthAnimation);
        }
        private void chkSettingsToggle_Unchecked(object sender, RoutedEventArgs e) {
            DoubleAnimation widthAnimation = new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(0.1)));
            widthAnimation.Completed += WidthAnimation_Completed;
            pnlSettingsContainer.BeginAnimation(WidthProperty, widthAnimation);
        }
        private void WidthAnimation_Completed(object? sender, EventArgs e) {
            pnlSettingsContainer.Children.Clear();
        }
        #endregion

        
    }
}
