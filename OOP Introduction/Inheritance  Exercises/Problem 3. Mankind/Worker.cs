using System;

public class Worker : Human
{
    private const int WorkDays = 5;

    private double weekSalary;
    private double hoursPerDay;
    private double salaryPerHour;

    public Worker(string firstName, string lastName, double weekSalary, double hoursPerDay)
        : base(firstName, lastName)
    {
        this.WeekSalary = weekSalary;
        this.HoursPerDay = hoursPerDay;
        this.salaryPerHour = (weekSalary / hoursPerDay) / WorkDays;
    }

    public double WeekSalary
    {
        get { return this.weekSalary; }
        set
        {
            if (value <= 10)
            {
                throw new ArgumentException("Expected value mismatch!Argument: weekSalary");
            }
            this.weekSalary = value;
        }
    }

    public double HoursPerDay
    {
        get { return this.hoursPerDay; }
        set
        {
            if (value < 1 || value > 12)
            {
                throw new ArgumentException("Expected value mismatch! Argument: workHoursPerDay");
            }
            this.hoursPerDay = value;
        }
    }

    public override string ToString()
    {
        return base.ToString() + Environment.NewLine +
               $"Week Salary: {this.weekSalary:F2}" + Environment.NewLine +
               $"Hours per day: {this.hoursPerDay:F2}" + Environment.NewLine +
               $"Salary per hour: {this.salaryPerHour:F2}";
    }
}