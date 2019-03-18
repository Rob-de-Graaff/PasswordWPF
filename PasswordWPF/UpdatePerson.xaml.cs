using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PasswordWPF
{
    /// <summary>
    /// Interaction logic for UpdatePerson.xaml
    /// </summary>
    public partial class UpdatePerson : Window
    {
        private DataBaseConnection dataBaseConnection;
        private MainWindow mainWindow;
        private int currentUserID;
        private Person selectedPerson;
        private PersonStatus personStatus;
        private string inputCode = "";

        public UpdatePerson(int currentUserID, Person selectedPerson, PersonStatus personStatus)
        {
            InitializeComponent();
            dataBaseConnection = new DataBaseConnection();
            mainWindow = new MainWindow();
            this.currentUserID = currentUserID;
            this.selectedPerson = selectedPerson;
            this.personStatus = personStatus;
            FillFields(selectedPerson);
        }

        private string CheckID(int selectedUserID)
        {
            if (selectedUserID == 0) //checks if person is new
            {
                //search for missing keys in squence, if so id becomes this value
                IEnumerable<int> existingIDs = Enumerable.Range(1, TryGetAllUserID().Max()).Except(TryGetAllUserID());
                selectedUserID = existingIDs.FirstOrDefault();
                if (selectedUserID == 0)
                {
                    selectedUserID = TryGetAllUserID().Max() + 1;
                }
            }
            return Convert.ToString(selectedUserID);
        }

        private void FillFields(Person person)
        {
            //fill fields with date values in UpdatePersonWindow
            for (int i = 1; i < 32; i++)
            {
                ComboBoxItem item = new ComboBoxItem
                {
                    Content = i
                };
                comboboxDay.Items.Add(item);
            }

            for (int i = DateTime.Today.Year - 18; i >= DateTime.Today.Year - 100; i--)
            {
                ComboBoxItem item = new ComboBoxItem
                {
                    Content = i
                };
                comboboxYear.Items.Add(item);
            }

            //fill fields with parameters from person object
            textboxID.Text = CheckID(person.ID);
            textboxFirstname.Text = person.FirstName;
            textboxLastname.Text = person.LastName;
            comboboxDay.Text = Convert.ToString(person.DateOfBirth.Day);
            comboboxMonth.Text = Convert.ToString(person.DateOfBirth.Month);
            comboboxYear.Text = Convert.ToString(person.DateOfBirth.Year);
            comboboxRole.Text = person.Role;
            textboxHoursWorked.Text = Convert.ToString(person.HoursWorked);
            textboxSalary.Text = Convert.ToString(person.Salary);
            textboxCode.Text = String.Join("", person.Code);
            comboboxLocked.SelectedItem = person.LockedOut;
        }

        private void ButtonUpdatePerson_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textboxFirstname.Text) && !string.IsNullOrWhiteSpace(textboxLastname.Text))
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(textboxFirstname.Text, @"^[\s*a-zA-Z]*$") && System.Text.RegularExpressions.Regex.IsMatch(textboxLastname.Text, @"^[\s*a-zA-Z]*$"))
                {
                    string DateOfBirth = Convert.ToString($"{comboboxDay.Text}/{comboboxMonth.Text}/{comboboxYear.Text}");
                    if (DateTime.TryParse(DateOfBirth, out DateTime resultDateOfBirth))
                    {
                        if (comboboxDay.SelectedItem != null && comboboxMonth.SelectedItem != null && comboboxYear.SelectedItem != null && comboboxRole.SelectedItem != null && comboboxLocked.SelectedItem != null)
                        {
                            if (System.Text.RegularExpressions.Regex.IsMatch(textboxHoursWorked.Text, @"^[\d*]*$") && int.TryParse(textboxHoursWorked.Text, out int resultHoursWorked))
                            {
                                if (textboxHoursWorked.Text.Count() <= 5)
                                {
                                    if (System.Text.RegularExpressions.Regex.IsMatch(textboxSalary.Text, @"^[0-9]*$") && double.TryParse(textboxSalary.Text, out double resultSalary))
                                    {
                                        if (resultSalary >= SetMinimumWage(resultDateOfBirth))
                                        {
                                            if (resultSalary <= 99999999.99)
                                            {
                                                if (System.Text.RegularExpressions.Regex.IsMatch(textboxCode.Text, @"^[0-9]*$") && int.TryParse(textboxCode.Text, out int resultCode))
                                                {
                                                    if (textboxCode.Text.Count() == 6)
                                                    {
                                                        inputCode = textboxCode.Text;
                                                        if (CheckCode())
                                                        {
                                                            int inputUserID = Convert.ToInt32(textboxID.Text);
                                                            string firstName = $"{textboxFirstname.Text}";
                                                            string lastName = $"{textboxLastname.Text}";
                                                            DateTime DOB = resultDateOfBirth;
                                                            string role = Convert.ToString($"{comboboxRole.Text}");
                                                            int hoursWorked = Convert.ToInt32(textboxHoursWorked.Text);
                                                            double salary = Convert.ToDouble(textboxSalary.Text);
                                                            bool locked = Convert.ToBoolean($"{comboboxLocked.Text}");
                                                            if (MessageBox.Show($"Weet u zeker dat u de gegevens wilt aanpassen?", "Waarschuwing", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                                                            {
                                                                switch (personStatus)
                                                                {
                                                                    case PersonStatus.Create:
                                                                        //updateWindow.Title = "Nieuw Persoon";
                                                                        TryAddUser(inputUserID, firstName, lastName, DOB, role, hoursWorked, salary, inputCode, locked);
                                                                        break;

                                                                    case PersonStatus.Update:
                                                                        //updateWindow.Title = "Update Persoon";
                                                                        TryUpdateUser(inputUserID, firstName, lastName, DOB, role, hoursWorked, salary, inputCode, locked);
                                                                        break;

                                                                    case PersonStatus.Delete:
                                                                        //updateWindow.Title = "Verwijder Persoon";
                                                                        TryDeleteUser(selectedPerson.ID);
                                                                        break;

                                                                    default:
                                                                        //updateWindow.Title = "Onbekend Persoon";
                                                                        break;
                                                                }
                                                                UpdateWindow();
                                                            }
                                                        }
                                                        else
                                                        {
                                                            MessageBox.Show($"De ingevoerde code is al gereserveerd.");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show($"De code moet 6 characters lang zijn");
                                                        textboxCode.Clear();
                                                    }
                                                }
                                                else
                                                {
                                                    MessageBox.Show($"Alleen nummers als code invoeren A.U.B.");
                                                }
                                            }
                                            else
                                            {
                                                MessageBox.Show($"De ingevoerde salaris heeft een te hoge waarde.");
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show($"De ingevoerde salaris heeft een te lage waarde.");
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show($"Alleen nummers als salaris invoeren A.U.B.");
                                    }
                                }
                                else
                                {
                                    MessageBox.Show($"De ingevoerde aantal uren heeft een te hoge waarde.");
                                }
                            }
                            else
                            {
                                MessageBox.Show($"Alleen nummers als uren gewerkt invoeren A.U.B.");
                            }
                        }
                        else
                        {
                            MessageBox.Show($"Alle drop down menus moeten waarden bevatten");
                        }
                    }
                    else
                    {
                        MessageBox.Show($"De ingevoerde datum bestaat niet.");
                    }
                }
                else
                {
                    MessageBox.Show($"Alleen alfabetische karakters als namen invoeren A.U.B.");
                }
            }
            else
            {
                MessageBox.Show($"De voor en achternaam velden moeten ingevuld zijn");
            }
        }

        private void ManagementWindowButton_Click(object sender, RoutedEventArgs e)
        {
            ManagementWindow newWindow = new ManagementWindow(Convert.ToInt32(mainWindow.TryGetUserProperty(currentUserID, "ID")));
            this.Close();
            newWindow.ShowDialog();
        }

        private double SetMinimumWage(DateTime birthDate)
        {
            double minimumWage = 0;
            DateTime today = DateTime.Today;
            // Calculate the age.
            int age = today.Year - birthDate.Year;
            // Go back to the year the person was born in case of a leap year
            if (birthDate > today.AddYears(-age)) age--;

            switch (age)
            {
                case 18:
                    minimumWage = 4.33;
                    break;

                case 19:
                    minimumWage = 5.01;
                    break;

                case 20:
                    minimumWage = 6.38;
                    break;

                case 21:
                    minimumWage = 7.74;
                    break;

                case int i when (age >= 22):
                    minimumWage = 9.11;
                    break;

                default:
                    minimumWage = 4.33;
                    break;
            }
            return minimumWage;
        }

        private bool CheckCode()
        {
            int hitCounter = 0;
            bool result;
            switch (personStatus)
            {
                //if person doesn't exist check if code exist
                case PersonStatus.Create:
                    foreach (string userCode in TryGetAllUserCodes())
                    {
                        if (inputCode == userCode || inputCode == "000000")
                        {
                            hitCounter++;
                        }
                    }
                    if (hitCounter == 0)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                    break;

                //if person exist, check if code is changed, if so check if code exsist
                case PersonStatus.Update:
                    if (inputCode != selectedPerson.Code)
                    {
                        foreach (string userCode in TryGetAllUserCodes())
                        {
                            if (inputCode == userCode)
                            {
                                hitCounter++;
                            }
                        }
                        if (hitCounter == 0)
                        {
                            result = true;
                        }
                        else
                        {
                            result = false;
                        }
                    }
                    else
                    {
                        result = true;
                    }
                    break;

                case PersonStatus.Delete:
                    result = true;
                    break;

                default:
                    result = false;
                    break;
            }
            return result;
        }

        private void UpdateWindow()
        {
            textboxID.Clear();
            textboxFirstname.Clear();
            textboxLastname.Clear();
            comboboxDay.SelectedIndex = 0;
            comboboxMonth.SelectedIndex = 0;
            comboboxYear.SelectedIndex = 0;
            comboboxRole.SelectedIndex = 0;
            textboxHoursWorked.Clear();
            textboxSalary.Clear();
            textboxCode.Clear();
            comboboxLocked.SelectedIndex = 0;
        }

        private List<int> TryGetAllUserID()
        {
            List<int> IDsResult = new List<int>();
            try
            {
                IDsResult = dataBaseConnection.GetAllUserID();
            }
            catch (IOException IOex)
            {
                MessageBox.Show(IOex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return IDsResult;
        }

        private List<string> TryGetAllUserCodes()
        {
            List<string> codesResult = new List<string>();
            try
            {
                codesResult = dataBaseConnection.GetAllUserCodes();
            }
            catch (IOException IOex)
            {
                MessageBox.Show(IOex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return codesResult;
        }

        private void TryAddUser(int selectedUserID, string firstName, string lastName, DateTime dateOfBirth, string role, int hoursWorked, double salary, string code, bool lockedOut)
        {
            try
            {
                dataBaseConnection.AddUser(selectedUserID, firstName, lastName, dateOfBirth, role, hoursWorked, salary, inputCode, lockedOut);
            }
            catch (IOException IOex)
            {
                MessageBox.Show(IOex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TryUpdateUser(int selectedUserID, string firstName, string lastName, DateTime dateOfBirth, string role, int hoursWorked, double salary, string code, bool lockedOut)
        {
            try
            {
                dataBaseConnection.UpdateUser(selectedUserID, firstName, lastName, dateOfBirth, role, hoursWorked, salary, inputCode, lockedOut);
            }
            catch (IOException IOex)
            {
                MessageBox.Show(IOex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TryDeleteUser(int selectedUserID)
        {
            try
            {
                dataBaseConnection.DeleteUser(selectedUserID);
            }
            catch (IOException IOex)
            {
                MessageBox.Show(IOex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}