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
    public partial class MainWindow : Window  {
        SerialCommunication sc = new SerialCommunication();
        public MainWindow() {
            InitializeComponent();
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            getValues();
        }

        public async void getValues() {
            while (true) {
                
                await Task.Delay(1);
            }
        }

        private void btnMain_Click(object sender, RoutedEventArgs e) {
            sc.PortConnect();
            SettingsView settings = new SettingsView(sc);
            pnlSettingsContainer.Children.Add(settings);
            getValues();
        }

        private void chkSettingsToggle_Checked(object sender, RoutedEventArgs e) {
            DoubleAnimation widthAnimation=new DoubleAnimation(400, new Duration(TimeSpan.FromSeconds(0.1)));
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
