using System;
using System.Reflection;

internal class StartUp
{
    private static void Main(string[] args)
    {
        var personType = typeof(Person);
        var properties = personType.GetProperties
            (BindingFlags.Public | BindingFlags.Instance);
        Console.WriteLine(properties.Length);
    }
}