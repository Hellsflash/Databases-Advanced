using System;
using System.Collections.Generic;
using System.Linq;

internal class StartUp
{
    private static void Main(string[] args)
    {
        var number = int.Parse(Console.ReadLine());
        var dict = new Dictionary<string, int>();

        for (var i = 0; i < number; i++)
        {
            var input = Console.ReadLine().Split().ToArray();

            var name = input[0];
            var age = int.Parse(input[1]);
            var person = new Person(name, age);

            if (person.Age > 30)
                dict.Add(name, age);
        }

        foreach (var person in dict.OrderBy(x => x.Key))
            Console.WriteLine($"{person.Key} - {person.Value}");
    }
}