using System;
using System.Collections.Generic;
using System.Linq;

public class StartUp
{
    private static void Main(string[] args)
    {
        var number = int.Parse(Console.ReadLine());

        var cars = new List<Car>();

        for (var i = 0; i < number; i++)
        {
            var input = Console.ReadLine().Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            var model = input[0];
            var fuelAmount = double.Parse(input[1]);
            var consumption = double.Parse(input[2]);

            var car = new Car(model, fuelAmount, consumption);
            cars.Add(car);
        }

        var command = Console.ReadLine().Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);

        while (command[0] != "End")
        {
            var model = command[1];
            var kmToDrive = double.Parse(command[2]);
            var drivenCar = cars.FirstOrDefault(c => c.Model == model);
            drivenCar.Drive(kmToDrive);

            command = Console.ReadLine().Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
        }

        foreach (var car in cars)
            Console.WriteLine(car);
    }
}