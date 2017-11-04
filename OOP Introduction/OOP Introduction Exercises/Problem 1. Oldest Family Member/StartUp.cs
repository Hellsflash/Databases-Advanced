using System;
using System.Reflection;

public class StartUp
{
    public static void Main(string[] args)
    {
        MethodInfo oldestMemberMethod = typeof(Family).GetMethod("GetOldestMember");
        MethodInfo addMemberMethod = typeof(Family).GetMethod("AddMember");
        if (oldestMemberMethod == null || addMemberMethod == null)
        {
            throw new Exception();
        }

        var number = int.Parse(Console.ReadLine());
        var family = new Family();
        for (int i = 0; i < number; i++)
        {
            var personArgs = Console.ReadLine().Split();
            var person = new Person(personArgs[0], int.Parse(personArgs[1]));

            family.AddMember(person);
        }

        var oldestMember = family.GetOldestMember();

        Console.WriteLine($"{oldestMember.Name} {oldestMember.Age}");
    }
}