using System.Linq;
using Employees.Services;

namespace Employees.App.Commands
{
    public class SetAddressCommand : ICommand
    {
        private readonly EmployeeService employeeService;

        public SetAddressCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public string Execute(params string[] args)
        {
            int employeeId = int.Parse(args[0]);
            string address = string.Join(" ", args.Skip(1));

            string employeeName = this.employeeService.SetAddress(employeeId, address);

            return $"{employeeName}'s address successfully set.";
        }
    }
}