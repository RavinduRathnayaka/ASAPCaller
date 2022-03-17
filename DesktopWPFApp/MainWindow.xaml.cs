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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DesktopWPFApp {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        Models.SerialCommunication sc = new Models.SerialCommunication();
        public MainWindow() {
            InitializeComponent();
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            Models.SerialCommunication sc = new Models.SerialCommunication();
            string[] serialPorts=sc.SerialPorts;
            cmbPortNames.ItemsSource = serialPorts;
            if (serialPorts.Length > 0) {
                cmbPortNames.SelectedIndex = 0;
                pitch.Text =cmbPortNames.Text;
            }
        }
        private void cmbPortNames_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            sc.ChangePort("COM3");
            
        }
        private async void ReadData() {
            while (!sc.AccidentDetected) {
                pitch.Text = sc.Pitch.ToString();
                roll.Text = sc.Roll.ToString();
                await Task.Delay(1);
            }
            
            
        }

        private void btnSettingsToggle_Click(object sender, RoutedEventArgs e) {
            ReadData();
        }

        private void chbxConnection_Checked(object sender, RoutedEventArgs e) {
            sc.PortConnect();
        }

        private void chbxConnection_Unchecked(object sender, RoutedEventArgs e) {
            sc.PortDisConnect();
        }
    }
}
