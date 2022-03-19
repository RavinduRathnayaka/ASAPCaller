using DesktopWPFApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace DesktopWPFApp.Views {
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl {
        public SerialCommunication sc { get; set; }
        public SettingsView(SerialCommunication aSc) {
            sc = aSc;
            InitializeComponent();
            UserControl_Loaded(null, null);
        }
        private void UserControl_Loaded(object? sender, RoutedEventArgs ?e) {
            UpdateControllers();
            Readdata();
        }

        private async void Readdata() {
            while (true) {
                sc.PortUpdate();
                UpdateControllers();
                await Task.Delay(1);
            }
        }
        private void UpdateControllers() {
            string[] serialPorts = sc.SerialPorts;
            cmbPortNames.ItemsSource = serialPorts;
            cmbPortNames.SelectedIndex = 0;
            chbxConnection.IsChecked = sc.Conntected ? true : false;
            cmbPortNames.IsEnabled = sc.Conntected ? false : true;
            chbxConnection.IsEnabled = cmbPortNames.Items.IsEmpty ? false : true;
        }
        private void cmbPortNames_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (!cmbPortNames.Items.IsEmpty && !sc.Conntected) {
                sc.ChangePort(cmbPortNames.SelectedItem.ToString());
            }
            else {
                chbxConnection.IsEnabled = false;
            }
        }
        private void chbxConnection_Checked(object sender, RoutedEventArgs e) {
            if (!sc.Conntected) {
                sc.Reset();
                sc.PortConnect();
                cmbPortNames.IsEnabled = false;
            }
        }
        private void chbxConnection_Unchecked(object sender, RoutedEventArgs e) {
            if (sc.Conntected) {
                sc.PortDisConnect();
                cmbPortNames.IsEnabled = true;
            }
        }


    }
}
