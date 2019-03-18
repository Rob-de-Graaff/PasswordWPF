using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PasswordWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataBaseConnection dataBaseConnection;
        private int[] inputArray = new int[6];
        private int indexCounter = 0;
        private int numberOfAttempts = 3;

        //private string digitInput = "";
        private string[] displayArray = { "-", "-", "-", "-", "-", "-" };

        private Color[] displayColors = { Colors.ForestGreen, Colors.Orange, Colors.Red };
        private int currentUserID;

        public MainWindow()
        {
            InitializeComponent();

            dataBaseConnection = new DataBaseConnection();

            nummer0Knop.Click += Button_Click;
            nummer1Knop.Click += Button_Click;
            nummer2Knop.Click += Button_Click;
            nummer3Knop.Click += Button_Click;
            nummer4Knop.Click += Button_Click;
            nummer5Knop.Click += Button_Click;
            nummer6Knop.Click += Button_Click;
            nummer7Knop.Click += Button_Click;
            nummer8Knop.Click += Button_Click;
            nummer9Knop.Click += Button_Click;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (textboxID.Text.Count() <= 6)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(textboxID.Text, @"^[\d*]*$") && int.TryParse(textboxID.Text, out int resultID))
                {
                    currentUserID = resultID;
                    if (Convert.ToInt32(TryGetUserProperty(currentUserID, "ID")) == currentUserID)
                    {
                        CheckDay();
                        if (Convert.ToBoolean(TryGetUserProperty(currentUserID, "locked")) == false)
                        {
                            if (numberOfAttempts != 0)
                            {
                                inputArray[indexCounter] = Convert.ToInt32(((Button)sender).Content);
                                indexCounter++;
                                Display(displayArray, "check");
                                if (indexCounter == 6)
                                {
                                    string code = String.Join("", inputArray);
                                    if (TryGetUserProperty(currentUserID, "code") == code)
                                    {
                                        Display(displayArray, "correct");
                                        MessageBox.Show($"Welkom {TryGetUserProperty(currentUserID, "name")}");
                                        HubWindow newWindow = new HubWindow(Convert.ToInt32(TryGetUserProperty(currentUserID, "ID")));
                                        this.Hide(); //Mainwindow can't close else app closes
                                        newWindow.ShowDialog();
                                    }
                                    else
                                    {
                                        numberOfAttempts--;
                                        Display(displayArray, "incorrect");
                                        MessageBox.Show($"Incorrecte invoer\n{numberOfAttempts} poging(en) resterend.");
                                    }
                                    Array.Clear(inputArray, 0, inputArray.Length);
                                    indexCounter = 0;
                                }
                            }
                            else
                            {
                                TrySetUserLocked(currentUserID, true);
                            }
                        }
                        else
                        {
                            MessageBox.Show($"{TryGetUserProperty(currentUserID, "name")}, U heeft geen toegang");
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Het verkeerde ID is ingevoerd");
                    }
                }
                else
                {
                    MessageBox.Show($"Alleen nummers als ID invoeren A.U.B.");
                }
            }
            else
            {
                MessageBox.Show($"Het ID mag niet meer dan 6 cijfers lang zijn");
                textboxID.Clear();
            }
        }

        private void CheckDay()
        {
            DayOfWeek dayOfWeek = DateTime.Now.DayOfWeek;

            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                    if (TryGetUserProperty(currentUserID, "role") == "")
                    {
                        TrySetUserLocked(currentUserID, true);
                        Display(displayArray, "lock");
                    }
                    break;

                case DayOfWeek.Tuesday:
                    if (TryGetUserProperty(currentUserID, "role") == "Cafetaria")
                    {
                        TrySetUserLocked(currentUserID, true);
                        Display(displayArray, "lock");
                    }
                    break;

                case DayOfWeek.Wednesday:
                    if (TryGetUserProperty(currentUserID, "role") == "Cleaning")
                    {
                        TrySetUserLocked(currentUserID, true);
                        Display(displayArray, "lock");
                    }
                    break;

                case DayOfWeek.Thursday:
                    if (TryGetUserProperty(currentUserID, "role") == "")
                    {
                        TrySetUserLocked(currentUserID, true);
                        Display(displayArray, "lock");
                    }
                    break;

                case DayOfWeek.Friday:
                    if (TryGetUserProperty(currentUserID, "role") == "")
                    {
                        TrySetUserLocked(currentUserID, true);
                        Display(displayArray, "lock");
                    }
                    break;

                case DayOfWeek.Saturday:
                    if (TryGetUserProperty(currentUserID, "role") == "Cafetaria")
                    {
                        TrySetUserLocked(currentUserID, true);
                        Display(displayArray, "lock");
                    }
                    break;

                case DayOfWeek.Sunday:
                    if (TryGetUserProperty(currentUserID, "role") == "Cleaning" || TryGetUserProperty(currentUserID, "role") == "Cafetaria")
                    {
                        TrySetUserLocked(currentUserID, true);
                        Display(displayArray, "lock");
                    }
                    break;
            }
        }

        private void Display(string[] array, string status)
        {
            switch (status)
            {
                case "reset":
                    numberOfAttempts = 3;
                    for (int i = 0; i < indexCounter; i++)
                    {
                        displayArray[i] = "-";
                    }
                    codeLabel.Content = string.Join("", displayArray);
                    codeLabel.Background = new SolidColorBrush(displayColors[0]);
                    break;

                case "check":
                    displayArray[indexCounter - 1] = "*";
                    foreach (string number in displayArray)
                    {
                        codeLabel.Content = string.Join("", displayArray);
                    }
                    break;

                case "correct":
                    for (int i = 0; i < indexCounter; i++)
                    {
                        displayArray[i] = "-";
                    }
                    codeLabel.Content = string.Join("", displayArray);
                    codeLabel.Background = new SolidColorBrush(displayColors[0]);
                    break;

                case "incorrect":
                    for (int i = 0; i < indexCounter; i++)
                    {
                        displayArray[i] = "-";
                    }
                    codeLabel.Content = string.Join("", displayArray);
                    codeLabel.Background = new SolidColorBrush(displayColors[1]);
                    break;

                case "lock":
                    codeLabel.Background = new SolidColorBrush(displayColors[2]);
                    break;

                default:
                    numberOfAttempts = 3;
                    for (int i = 0; i < indexCounter; i++)
                    {
                        displayArray[i] = "-";
                    }
                    codeLabel.Content = string.Join("", displayArray);
                    codeLabel.Background = new SolidColorBrush(displayColors[0]);
                    break;
            }
        }

        public string TryGetUserProperty(int currentUserID, string property)
        {
            string propertyResult = "";
            try
            {
                propertyResult = dataBaseConnection.GetProperty(currentUserID, property);
            }
            catch (IOException IOex)
            {
                MessageBox.Show(IOex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return propertyResult;
        }

        private void TrySetUserLocked(int currentUserID, bool value)
        {
            try
            {
                dataBaseConnection.SetUserLocked(currentUserID, value);
                Display(displayArray, "lock");
                MessageBox.Show($"Code is 3X verkeerd ingevoerd");
                Display(displayArray, "reset");
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