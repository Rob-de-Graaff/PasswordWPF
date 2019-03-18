using System;
using System.Windows;

namespace PasswordWPF
{
    /// <summary>
    /// Interaction logic for HubWindow.xaml
    /// </summary>
    public partial class HubWindow : Window
    {
        private MainWindow mainWindow;
        private int currentUserID;
        private string message = string.Empty;

        public HubWindow(int value)
        {
            InitializeComponent();

            mainWindow = new MainWindow();
            this.currentUserID = value;
            message = $"Beste {mainWindow.TryGetUserProperty(currentUserID, "name")} dit programma is nog in ontwikkeling, sommige eigenschappen zijn nog niet geïmplementeerd";
            CheckRole();
        }

        private void CheckRole()
        {
            switch (mainWindow.TryGetUserProperty(currentUserID, "role"))
            {
                case "Management":
                    managementButton.IsEnabled = true;
                    break;

                case "Secretariat":
                    secretariatButton.IsEnabled = true;
                    break;

                case "Cleaning":
                    cleaningButton.IsEnabled = true;
                    break;

                case "Cafetaria":
                    cafetariaButton.IsEnabled = true;
                    break;
            }
        }

        private void ManagementButton_Click(object sender, RoutedEventArgs e)
        {
            ManagementWindow newWindow = new ManagementWindow(Convert.ToInt32(mainWindow.TryGetUserProperty(currentUserID, "ID")));
            this.Close();
            newWindow.ShowDialog();
        }

        private void SecretariatButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(message);
        }

        private void CleaningButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(message);
        }

        private void CafetariaButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(message);
        }

        private void MainWindowButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow newWindow = new MainWindow();
            this.Close();
            newWindow.ShowDialog();
        }
    }
}