using System.Text;
using Employees.Services;

namespace Employees.App.Commands
{
    public class ManagerInfoCommand : ICommand
    {
        private readonly EmployeeService employeeService;

        public ManagerInfoCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public string Execute(params string[] args)
        {
            int emplId = int.Parse(args[0]);

            var managerInfo = this.employeeService.ManagerInfo(emplId);

            var sb = new StringBuilder();

            sb.AppendLine($"{managerInfo.FirstName} {managerInfo.LastName} | Employees: {managerInfo.ManagedEmployeesCount}");

            foreach (var employee in managerInfo.ManagedEmployees)
            {
                sb.AppendLine($"    - {employee.FirstName} {employee.LastName} - ${employee.Salary:f2}");
            }

            return sb.ToString();
        }
    }
}