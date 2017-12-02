using Employees.Services;

namespace Employees.App.Commands
{
    public class EmployeeInfoCommand : ICommand
    {
        private readonly EmployeeService employeeService;

        public EmployeeInfoCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public string Execute(params string[] args)
        {
            int employeeId = int.Parse(args[0]);

            var emplDto = this.employeeService.ById(employeeId);

            return $"ID: {emplDto.Id} - {emplDto.FirstName} {emplDto.LastName} - ${emplDto.Salary:f2}";
        }
    }
}