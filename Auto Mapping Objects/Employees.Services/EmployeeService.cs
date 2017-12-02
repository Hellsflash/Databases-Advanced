using System;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Employees.Data;
using Employees.DtoModels;
using Employees.Models;
using Microsoft.EntityFrameworkCore;

namespace Employees.Services
{
    public class EmployeeService
    {
        private readonly EmployeeContext Context;

        public EmployeeService(EmployeeContext context)
        {
            this.Context = context;
        }

        public EmployeeDto ById(int employeeId)
        {
            var employee = Context.Employees.Find(employeeId);

            if (employee == null)
            {
                throw new ArgumentException("Invalid EmployeeId!");
            }

            var employeeDto = Mapper.Map<EmployeeDto>(employee);

            return employeeDto;
        }

        public PersonalInfoDto FullPersonalInfoById(int employeeId)
        {
            var employee = Context.Employees.Find(employeeId);

            if (employee == null)
            {
                throw new ArgumentException("Invalid EmployeeId!");
            }

            var personalInfoDto = Mapper.Map<PersonalInfoDto>(employee);

            return personalInfoDto;
        }

        public void AddEmployee(EmployeeDto dto)
        {
            var employee = Mapper.Map<Employee>(dto);

            Context.Employees.Add(employee);
            Context.SaveChanges();
        }

        public string SetBirthday(int employeeId, DateTime birthdayDate)
        {
            var employee = Context.Employees.Find(employeeId);

            if (employee == null)
            {
                throw new ArgumentException("Invalid EmployeeId!");
            }

            employee.BirthDay = birthdayDate;

            Context.SaveChanges();

            return $"{employee.FirstName} {employee.LastName}";
        }

        public string SetAddress(int employeeId, string address)
        {
            var employee = Context.Employees.Find(employeeId);

            if (employee == null)
            {
                throw new ArgumentException("Invalid EmployeeId!");
            }

            employee.Address = address;

            Context.SaveChanges();

            return $"{employee.FirstName} {employee.LastName}";
        }

        public string SetManager(int employeeId, int managerId)
        {
            var employee = Context.Employees.Find(employeeId);
            var manager = Context.Employees.Find(managerId);

            if (employee == null || manager == null)
            {
                throw new ArgumentException("Invalid EmployeeId or ManagerId!");
            }

            employee.ManagerId = managerId;
            Context.SaveChanges();

            string employeeManagerNames = $"{employee.FirstName} {employee.LastName} {manager.FirstName} {manager.LastName}";
            return employeeManagerNames;
        }

        public ManagerDto ManagerInfo(int employeeId)
        {
            var manager = Context.Employees
                .Include(m => m.ManagedEmployees)
                .FirstOrDefault(m => m.Id == employeeId);

            if (manager == null)
            {
                throw new ArgumentException("Invalid ManagerId!");
            }

            var manDto = Mapper.Map<ManagerDto>(manager);
            return manDto;
        }

        public EmployeeByAgeDto[] ListEmployeesOlder(int age)
        {
            var employeesList = Context.Employees
                .Where(e => e.Age > age)
                .Include(e => e.Manager)
                .ProjectTo<EmployeeByAgeDto>()
                .ToArray();

            return employeesList;
        }
    }
}