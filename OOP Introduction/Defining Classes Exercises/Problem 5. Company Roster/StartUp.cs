using System;
using System.Collections.Generic;
using System.Linq;

public class Startup
{
    private static void Main(string[] args)
    {
        var number = int.Parse(Console.ReadLine());

        var employees = new List<Employee>();

        for (var i = 0; i < number; i++)
        {
            var input = Console.ReadLine().Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            var name = input[0];
            var salary = double.Parse(input[1]);
            var position = input[2];
            var department = input[3];

            if (input.Length == 6)
            {
                var email = input[4];
                var age = int.Parse(input[5]);

                var employee = new Employee(name, salary, position, department, email, age);
                employees.Add(employee);
            }
            else if (input.Length == 5)
            {
                if (int.TryParse(input[4], out int isAge))
                {
                    var employee = new Employee(name, salary, position, department, isAge);
                    employees.Add(employee);
                }
                else
                {
                    var email = input[4];
                    var employee = new Employee(name, salary, position, department, email);
                    employees.Add(employee);
                }
            }

            else if (input.Length == 4)
            {
                var employee = new Employee(name, salary, position, department);
                employees.Add(employee);
            }
        }

        var departments = employees.GroupBy(em => em.Department).Select(gr => new
        {
            Name = gr.Key,
            AvarageSalary = gr.Average(em => em.Salary),
            employees = gr
        }).OrderByDescending(gr => gr.AvarageSalary).FirstOrDefault();

        Console.WriteLine($"Highest Average Salary: {departments.Name}");
        foreach (var emp in departments.employees.OrderByDescending(em => em.Salary))
            Console.WriteLine(emp);
    }
}