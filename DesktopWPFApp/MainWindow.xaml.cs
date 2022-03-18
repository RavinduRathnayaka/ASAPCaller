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
            getValues();
            if(sc.SerialPorts!=null && sc.SerialPorts.Length!=0) {
                sc.PortConnect();
            }
            else {
                btnMain.Content = "Device is not connected";
            }
        }
        public async void getValues() {
            while (true) {
                UpdateUi();
                await Task.Delay(1);
            }
        }
        private void UpdateUi() {
            if (sc.AccidentDetected) {
                grdContainer.Background = Brushes.Red;
            }
            else if (sc.Conntected) {
                grdContainer.Background = Brushes.Lime;
            }
            else if (!sc.Conntected) {
                grdContainer.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#484848"));
            }
            if (sc.Status.Contains("Connect")) {
                AnimationConnecting();
            }
        }
        private async void AnimationConnecting() {
            while (!sc.Conntected) {
                btnMain.Content = sc.Status;
                await Task.Delay(1);
            }
           btnMain.Content = "ASAP Caller Online";
        }
        private void btnMain_Click(object sender, RoutedEventArgs e) {
            sc.PortConnect();
            SettingsView settings = new SettingsView(sc);
            pnlSettingsContainer.Children.Add(settings);
            getValues();
        }
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
    }
}
