﻿using System;
using System.Collections.Generic;

namespace Employees.Models
{
    public class Employee
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public decimal Salary { get; set; }

        public DateTime? BirthDay { get; set; }

        public int Age => (int)Math.Floor((DateTime.Now - this.BirthDay.Value).Days / 365.25);

        public string Address { get; set; }

        public int? ManagerId { get; set; }
        public Employee Manager { get; set; }

        public ICollection<Employee> ManagedEmployees { get; set; } = new List<Employee>();
    }
}