using System;

namespace PasswordWPF
{
    public partial class Person
    {
        private int personID;
        private string personFirstname;
        private string personLastname;
        private DateTime personDateOfBirth;
        private string personRole;
        private int personHoursWorked;
        private double personSalary;
        private string personCode;
        private bool personLockedOut;

        public Person(int id, string firstName, string lastName, DateTime dateOfBirth, string role, int hoursWorked, double salary, string code, bool lockedOut)
        {
            this.personID = id;
            this.personFirstname = firstName;
            this.personLastname = lastName;
            this.personDateOfBirth = dateOfBirth;
            this.personRole = role;
            this.personHoursWorked = hoursWorked;
            this.personSalary = salary;
            this.personCode = code;
            this.personLockedOut = lockedOut;
        }

        public override string ToString()
        {
            return String.Format($"ID: {ID}, First Name: {FirstName}, Last Name: {LastName}, DOB: {DateOfBirth.ToShortDateString()}, Role: {Role}, Hours: {HoursWorked}, Salary: {Salary}, Locked: {LockedOut}.");
        }

        public int ID
        {
            get { return personID; }
        }

        public string FirstName
        {
            get { return personFirstname; }
        }

        public string LastName
        {
            get { return personLastname; }
        }

        public DateTime DateOfBirth
        {
            get { return personDateOfBirth; }
        }

        public string Role
        {
            get { return personRole; }
        }

        public int HoursWorked
        {
            get { return personHoursWorked; }
            set { personHoursWorked = value; }
        }

        public double Salary
        {
            get { return personSalary; }
            set { personSalary = value; }
        }

        public string Code
        {
            get { return personCode; }
        }

        public bool LockedOut
        {
            get { return personLockedOut; }
            set { personLockedOut = value; }
        }
    }
}