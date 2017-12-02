using System;
using Employees.Models;

namespace Employees.DtoModels
{
    public class EmployeeByAgeDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public decimal Salary { get; set; }

        public Employee Manager { get; set; }
    }
}