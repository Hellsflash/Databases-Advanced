using System;

public class Car
{
    public Car(string model, double fuelAmount, double fuelConsumption)
    {
        Model = model;
        FuelAmount = fuelAmount;
        FuelConsumption = fuelConsumption;
        DistanceTraveled = 0;
    }

    public string Model { get; }

    public double FuelAmount { get; private set; }

    public double FuelConsumption { get; }

    public double DistanceTraveled { get; private set; }

    public void Drive(double kmToDrive)
    {
        var total = kmToDrive * FuelConsumption;

        if (total > FuelAmount)
        {
            Console.WriteLine("Insufficient fuel for the drive");
        }
        else
        {
            DistanceTraveled += kmToDrive;
            FuelAmount -= total;
        }
    }

    public override string ToString()
    {
        return $"{Model} {FuelAmount:F2} {DistanceTraveled}";
    }
}