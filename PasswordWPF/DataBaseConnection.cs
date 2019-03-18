using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace PasswordWPF
{
    internal class DataBaseConnection
    {
        private string connectionString = PasswordWPF.Properties.Settings.Default.ConnectionString;

        public void AddUser(int selectedUserID, string firstName, string lastName, DateTime dateOfBirth, string role, int hoursWorked, double salary, string code, bool lockedOut)
        {
            string query = "INSERT INTO Persons (Id, FirstName, LastName, DateOfBirth, Role, HoursWorked, Salary, Code, LockedOut) VALUES(@ID, @firstName, @lastName, @dateOfBirth, @role, @hoursWorked, @salary, @code, @lockedOut)";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ID", selectedUserID);
                        command.Parameters.AddWithValue("@firstName", firstName);
                        command.Parameters.AddWithValue("@lastName", lastName);
                        command.Parameters.AddWithValue("@dateOfBirth", dateOfBirth);
                        command.Parameters.AddWithValue("@role", role);
                        command.Parameters.AddWithValue("@hoursWorked", hoursWorked);
                        command.Parameters.AddWithValue("@salary", salary);
                        command.Parameters.AddWithValue("@code", code);
                        command.Parameters.AddWithValue("@lockedOut", lockedOut);

                        connection.Open();

                        command.ExecuteNonQuery();

                        connection.Close();
                    }
                }
            }
            catch (IOException IOex)
            {
                throw IOex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateUser(int selectedUserID, string firstName, string lastName, DateTime dateOfBirth, string role, int hoursWorked, double salary, string code, bool lockedOut)
        {
            string query = "UPDATE Persons SET FirstName = @firstName, LastName = @lastName, DateOfBirth = @dateOfBirth, Role = @role, HoursWorked = @HoursWorked, Salary = @salary, Code = @code, LockedOut = @lockedOut WHERE Id = @ID";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ID", selectedUserID);
                        command.Parameters.AddWithValue("@firstName", firstName);
                        command.Parameters.AddWithValue("@lastName", lastName);
                        command.Parameters.AddWithValue("@dateOfBirth", dateOfBirth);
                        command.Parameters.AddWithValue("@role", role);
                        command.Parameters.AddWithValue("@hoursWorked", hoursWorked);
                        command.Parameters.AddWithValue("@salary", salary);
                        command.Parameters.AddWithValue("@code", code);
                        command.Parameters.AddWithValue("@lockedOut", lockedOut);

                        connection.Open();

                        command.ExecuteNonQuery();

                        connection.Close();
                    }
                }
            }
            catch (IOException IOex)
            {
                throw IOex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteUser(int selectedUserID)
        {
            string query = "DELETE FROM Persons WHERE Id = @ID";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ID", selectedUserID);

                        connection.Open();

                        command.ExecuteNonQuery();

                        connection.Close();
                    }
                }
            }
            catch (IOException IOex)
            {
                throw IOex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetUserLocked(int currentUserID, bool value)
        {
            #region query + new SqlCommand

            string query = "UPDATE Persons SET LockedOut = @lockedOut WHERE Id = @ID";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ID", currentUserID);
                        command.Parameters.AddWithValue("@lockedOut", value);

                        connection.Open();

                        command.ExecuteNonQuery();

                        connection.Close();
                    }
                }
            }
            catch (IOException IOex)
            {
                throw IOex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            #endregion query + new SqlCommand

            #region connection.CreateCommand + command.CommandText

            //try
            //{
            //    using (SqlConnection connection = new SqlConnection(connectionString))
            //    {
            //        using (SqlCommand command = connection.CreateCommand())
            //        {
            //            command.CommandText = "UPDATE Persons SET LockedOut = @lockedOut WHERE Id = @ID";
            //            command.Parameters.AddWithValue("@ID", currentUserID);
            //            command.Parameters.AddWithValue("@lockedOut", value);

            //            connection.Open();

            //            command.ExecuteNonQuery();

            //            connection.Close();
            //        }
            //    }
            //}
            //catch (IOException IOex)
            //{
            //    throw IOex;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

            #endregion connection.CreateCommand + command.CommandText
        }

        public string GetProperty(int selectedUserID, string property)
        {
            string personString = "";
            string resultString = "";
            int indexStart = 0;
            int indexEnd = 0;
            int length = 0;
            string query = $"SELECT * FROM Persons WHERE Id = {selectedUserID}";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();

                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            personString = "ID" + reader[0] + ",firstName" + reader[1]
                                + ",lastName" + reader[2] + ",dateOfBirth" + Convert.ToDateTime(reader[3]).ToString("dd/MM/yyyy")
                                + ",role" + reader[4] + ",hoursWorked" + reader[5]
                                + ",salary" + reader[6] + ",code" + reader[7]
                                + ",locked" + reader[8];
                        }
                        reader.Close();
                    }
                    connection.Close();
                }
            }
            catch (IOException IOex)
            {
                throw IOex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            switch (property)
            {
                case "ID":
                    indexStart = personString.IndexOf("ID") + 2;
                    indexEnd = personString.IndexOf(",");
                    length = indexEnd - indexStart;
                    resultString = personString.Substring(indexStart, length);
                    break;

                case "name":
                    personString = personString.Remove(0, personString.IndexOf(",") + 1);
                    indexStart = personString.IndexOf("FirstName") + 10;
                    indexEnd = personString.IndexOf(",");
                    length = indexEnd - indexStart;
                    string resultString1 = personString.Substring(indexStart, length);
                    personString = personString.Remove(0, indexEnd + 1);
                    int indexStart2 = personString.IndexOf("LastName") + 9;
                    int indexEnd2 = personString.IndexOf(",");
                    int length2 = indexEnd2 - indexStart2;
                    resultString = resultString1 + " " + personString.Substring(indexStart2, length2);
                    break;

                case "role":
                    for (int i = 0; i < 4; i++)
                    {
                        personString = personString.Remove(0, personString.IndexOf(",") + 1);
                    }
                    indexStart = personString.IndexOf("Role") + 5;
                    indexEnd = personString.IndexOf(",");
                    length = indexEnd - indexStart;
                    resultString = personString.Substring(indexStart, length);
                    break;

                case "code":
                    for (int i = 0; i < 7; i++)
                    {
                        personString = personString.Remove(0, personString.IndexOf(",") + 1);
                    }
                    indexStart = personString.IndexOf("Code") + 5;
                    indexEnd = personString.IndexOf(",");
                    length = indexEnd - indexStart;
                    resultString = personString.Substring(indexStart, length);
                    break;

                case "locked":
                    for (int i = 0; i < 8; i++)
                    {
                        personString = personString.Remove(0, personString.IndexOf(",") + 1);
                    }
                    indexStart = personString.IndexOf("Locked") + 7;
                    indexEnd = personString.Length;
                    length = indexEnd - indexStart;
                    resultString = personString.Substring(indexStart, length);
                    break;

                default:
                    break;
            }
            return resultString;
        }

        public Person GetUser(int selectedUserID)
        {
            int selectedPersonID = 0;
            string selectedPersonFirstName = "";
            string selectedPersonLastName = "";
            DateTime selectedPersonDateOfBirth = Convert.ToDateTime("1/1/0001");
            string selectedPersonRole = "";
            int selectedPersonHoursWorked = 0;
            double selectedPersonSalary = 0;
            string selectedPersonCode = "";
            bool selectedPersonLocked = false;
            Person selectedPerson;
            string query = $"SELECT Id, FirstName, LastName, DateOfBirth, Role, HoursWorked, Salary, Code, LockedOut FROM Persons WHERE Id = {selectedUserID}";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();

                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            selectedPersonID = Convert.ToInt32(reader[0]);
                            selectedPersonFirstName = Convert.ToString(reader[1]);
                            selectedPersonLastName = Convert.ToString(reader[2]);
                            selectedPersonDateOfBirth = Convert.ToDateTime(reader[3]);
                            selectedPersonRole = Convert.ToString(reader[4]);
                            selectedPersonHoursWorked = Convert.ToInt32(reader[5]);
                            selectedPersonSalary = Convert.ToDouble(reader[6]);
                            selectedPersonCode = Convert.ToString(reader[7]);
                            selectedPersonLocked = Convert.ToBoolean(reader[8]);
                            //selectedPerson = (Person)reader[0]; //Cannot convert
                        }
                        reader.Close();
                    }
                    connection.Close();
                }
            }
            catch (IOException IOex)
            {
                throw IOex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            selectedPerson = new Person(selectedPersonID, selectedPersonFirstName, selectedPersonLastName, selectedPersonDateOfBirth.Date, selectedPersonRole, selectedPersonHoursWorked, selectedPersonSalary, selectedPersonCode, selectedPersonLocked);
            return selectedPerson;
        }

        public List<int> GetAllUserID()
        {
            List<int> IDs = new List<int>();
            string query = $"SELECT Id FROM Persons";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();

                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            IDs.Add(Convert.ToInt32(reader[0]));
                            //IDs.Add(Convert.ToInt32(reader["Id"]));
                        }
                        reader.Close();
                    }
                    connection.Close();
                }
            }
            catch (IOException IOex)
            {
                throw IOex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return IDs;
        }

        public List<string> GetAllUserCodes()
        {
            List<string> codes = new List<string>();
            string query = $"SELECT Code FROM Persons";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                codes.Add(Convert.ToString(reader[0]));
                                //codes.Add(Convert.ToString(reader["Code"]));
                            }
                            reader.Close();
                        }
                    }
                    connection.Close();
                }
            }
            catch (IOException IOex)
            {
                throw IOex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return codes;
        }

        public List<string> DisplayUsers()
        {
            int recordCount = 0;
            List<string> persons = new List<string>();
            string query = $"SELECT * FROM Persons";
            try
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connectionString))
                {
                    using (DataTable table = new DataTable())
                    {
                        recordCount = adapter.Fill(table);
                        foreach (DataRow row in table.Rows)
                        {
                            persons.Add("ID: " + row.ItemArray[0] + ", first name: " + row.ItemArray[1]
                                + ", last name: " + row.ItemArray[2] + ", date of birth: " + Convert.ToDateTime(row.ItemArray[3]).ToString("dd/MM/yyyy")
                                + ", role: " + row.ItemArray[4] + ", hours worked: " + row.ItemArray[5]
                                + ", salary: " + row.ItemArray[6] + ", code: " + row.ItemArray[7]
                                + ", locked: " + row.ItemArray[8]);
                        }
                    }
                }
            }
            catch (IOException IOex)
            {
                throw IOex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return persons;
        }
    }
}