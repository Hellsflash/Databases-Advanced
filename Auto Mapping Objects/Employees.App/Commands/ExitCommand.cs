using System;

namespace Employees.App.Commands
{
    public class ExitCommand : ICommand
    {
        public string Execute(params string[] args)
        {
            Console.WriteLine("Good Bye!");
            Environment.Exit(0);

            return string.Empty;
        }
    }
}