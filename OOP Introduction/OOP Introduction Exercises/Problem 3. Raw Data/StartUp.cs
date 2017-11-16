using System;
using System.Collections.Generic;
using System.Linq;

public class StartUp
{
    public static void Main(string[] args)
    {
        var number = int.Parse(Console.ReadLine());
        var cars = new List<Car>();

        for (int i = 0; i < number; i++)
        {
            var carArgs = Console.ReadLine().Split();

            var model = carArgs[0];
            var engine = new Engine(int.Parse(carArgs[1]), int.Parse(carArgs[2]));
            var cargo = new Cargo(int.Parse(carArgs[3]), carArgs[4]);
            var tires = new Tire[]
            {
                new Tire(double.Parse(carArgs[5]),int.Parse(carArgs[6])),
                new Tire(double.Parse(carArgs[7]),int.Parse(carArgs[8])),
                new Tire(double.Parse(carArgs[9]),int.Parse(carArgs[10])),
                new Tire(double.Parse(carArgs[11]),int.Parse(carArgs[12])),
            };

            var car = new Car(model, engine, cargo, tires);
            cars.Add(car);
        }

        var command = Console.ReadLine();

        if (command == "fragile")
        {
            var result = cars.Where(c => c.Cargo.Type == "fragile" && c.Tires.Any(x => x.Pressure < 1));
            foreach (var car in result)
            {
                Console.WriteLine(car.Model);
            }
        }
        else if (command == "flammable")
        {
            var result = cars.Where(c => c.Cargo.Type == "flammable").Where(ep => ep.Engine.Power > 250);
            foreach (var car in result)
            {
                Console.WriteLine(car.Model);
            }
        }
    }
}