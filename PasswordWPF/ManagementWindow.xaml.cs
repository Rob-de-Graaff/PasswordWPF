using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace PasswordWPF
{
    /// <summary>
    /// Interaction logic for ManagementWindow.xaml
    /// </summary>
    public partial class ManagementWindow : Window
    {
        private DataBaseConnection dataBaseConnection;
        private MainWindow mainWindow;
        private int currentUserID;

        public ManagementWindow(int value)
        {
            InitializeComponent();
            dataBaseConnection = new DataBaseConnection();
            mainWindow = new MainWindow();
            this.currentUserID = value;
        }

        private void PersonInfoButton_Click(object sender, RoutedEventArgs e)
        {
            if (personInfoButton.IsChecked == true)
            {
                //infoListbox.Items.Clear();
                infoListbox.ItemsSource = TryDisplayPersons();
            }
            else
            {
                infoListbox.ItemsSource = null;
            }
        }

        private void HubWindowButton_Click(object sender, RoutedEventArgs e)
        {
            HubWindow newWindow = new HubWindow(Convert.ToInt32(mainWindow.TryGetUserProperty(currentUserID, "ID")));
            this.Close();
            newWindow.ShowDialog();
        }

        private void CreateCommand_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CreateCommand_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            //create new person with dummy parameters
            Person selectedPerson = new Person(0, "", "", Convert.ToDateTime("1/1/0001"), "", 0, 0, "000000", false);
            UpdatePerson newWindow = new UpdatePerson(Convert.ToInt32(mainWindow.TryGetUserProperty(currentUserID, "ID")), selectedPerson, PersonStatus.Create);
            this.Close();
            newWindow.ShowDialog();
        }

        private void UpdateCommand_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {
            if (infoListbox != null && infoListbox.SelectedItem != null)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }

        private void UpdateCommand_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            //retrieve id from selected person
            int indexStart = infoListbox.SelectedItem.ToString().IndexOf("ID") + 4;
            int indexEnd = infoListbox.SelectedItem.ToString().IndexOf(",");
            int length = indexEnd - indexStart;
            string dataBasePrimaryKeyString = infoListbox.SelectedItem.ToString().Substring(indexStart, length);
            int dataBasePrimaryKey = Convert.ToInt32(dataBasePrimaryKeyString);
            Person selectedPerson = TryGetUser(dataBasePrimaryKey);
            UpdatePerson newWindow = new UpdatePerson(Convert.ToInt32(mainWindow.TryGetUserProperty(currentUserID, "ID")), selectedPerson, PersonStatus.Update);
            this.Close();
            newWindow.ShowDialog();
        }

        private void DeleteCommand_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {
            if (infoListbox != null && infoListbox.SelectedItem != null)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }

        private void DeleteCommand_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            //retrieve id from selected person
            int indexStart = infoListbox.SelectedItem.ToString().IndexOf("ID") + 4;
            int indexEnd = infoListbox.SelectedItem.ToString().IndexOf(",");
            int length = indexEnd - indexStart;
            string dataBasePrimaryKeyString = infoListbox.SelectedItem.ToString().Substring(indexStart, length);
            int dataBasePrimaryKey = Convert.ToInt32(dataBasePrimaryKeyString);
            Person selectedPerson = TryGetUser(dataBasePrimaryKey);
            UpdatePerson newWindow = new UpdatePerson(Convert.ToInt32(mainWindow.TryGetUserProperty(currentUserID, "ID")), selectedPerson, PersonStatus.Delete);
            this.Close();
            newWindow.ShowDialog();
        }

        private List<string> TryDisplayPersons()
        {
            List<string> personsResult = new List<string>();
            try
            {
                personsResult = dataBaseConnection.DisplayUsers();
            }
            catch (IOException IOex)
            {
                MessageBox.Show(IOex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return personsResult;
        }

        private Person TryGetUser(int selectedUserID)
        {
            Person personResult = new Person(0, "", "", Convert.ToDateTime("1/1/0001"), "", 0, 0, "000000", false);
            try
            {
                personResult = dataBaseConnection.GetUser(selectedUserID);
            }
            catch (IOException IOex)
            {
                MessageBox.Show(IOex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return personResult;
        }
    }
}