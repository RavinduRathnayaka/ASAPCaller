using DesktopWPFApp.Models;
using DesktopWPFApp.Views;
using System;
using System.Windows;
using System.Windows.Media.Animation;

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
            pnlContainer.Children.Add(new IndexView(sc));
        }
        #region Side Pannel Animations
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
