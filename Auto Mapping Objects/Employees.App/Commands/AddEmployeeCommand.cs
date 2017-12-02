using Employees.DtoModels;
using Employees.Services;

namespace Employees.App.Commands
{
    public class AddEmployeeCommand : ICommand
    {
        private readonly EmployeeService EmployeeService;

        public AddEmployeeCommand(EmployeeService employeeService)
        {
            this.EmployeeService = employeeService;
        }

        public string Execute(params string[] args)
        {
            var firstName = args[0];
            var lastName = args[1];
            var salary = decimal.Parse(args[2]);

            var employeeDto = new EmployeeDto(firstName, lastName, salary);

            this.EmployeeService.AddEmployee(employeeDto);

            return $"Emloyee {firstName} {lastName} successfully added.";
        }
    }
}