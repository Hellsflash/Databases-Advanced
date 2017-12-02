using Employees.Services;

namespace Employees.App.Commands
{
    public class SetManagerCommand : ICommand
    {
        private readonly EmployeeService employeeService;

        public SetManagerCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public string Execute(params string[] args)
        {
            int employeeId = int.Parse(args[0]);
            int managerId = int.Parse(args[1]);

            string names = this.employeeService.SetManager(employeeId, managerId);

            string[] tokens = names.Split();
            string emplName = $"{tokens[0]} {tokens[1]}";
            string manName = $"{tokens[2]} {tokens[3]}";

            return $"{manName} successfully appointed as {emplName}'s manager.";
        }
    }
}