using System;
using System.Linq;

namespace Employees.App
{
    public class Engine
    {
        private readonly IServiceProvider ServiceProvider;

        public Engine(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public void Run()
        {
            while (true)
            {
                try
                {
                    var input = Console.ReadLine().Trim();
                    var tokens = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    var commandName = tokens[0];
                    var args = tokens.Skip(1).ToArray();

                    var command = CommandParser.Parse(ServiceProvider, commandName);

                    var result = command.Execute(args);

                    Console.WriteLine(result);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}