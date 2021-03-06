﻿using System;
using Employees.Services;

namespace Employees.App.Commands
{
    public class SetBirthdayCommand : ICommand
    {
        private readonly EmployeeService employeeService;

        public SetBirthdayCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public string Execute(params string[] args)
        {
            int employeeId = int.Parse(args[0]);
            DateTime birthday = DateTime.ParseExact(args[1], "dd-MM-yyyy", null);

            string employeeFullName = this.employeeService.SetBirthday(employeeId, birthday);

            return $"Date {args[1]} successfully set as {employeeFullName}'s birthday.";
        }
    }
}