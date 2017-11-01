using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class StartUp
{
    private static void Main(string[] args)
    {
        Type boxType = typeof(Box);
        FieldInfo[] fields = boxType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
        Console.WriteLine(fields.Count());

        var param = new List<double>();

        for (int i = 0; i < 3; i++)
        {
            var input = double.Parse(Console.ReadLine());

            param.Add(input);
        }

        var box = new Box(param[0], param[1], param[2]);

        Console.WriteLine($"Surface Area - {box.SurfaceArea():F2}");
        Console.WriteLine($"Lateral Surface Area - {box.LateralSurfaceArea():F2}");
        Console.WriteLine($"Volume - {box.Volume():F2}");
    }
}