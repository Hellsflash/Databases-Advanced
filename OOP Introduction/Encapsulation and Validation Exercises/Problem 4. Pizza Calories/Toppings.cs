using System;

class Topping
{
    private decimal caloriesPerGram;
    private string type;
    private decimal weight;

    public Topping(string type, decimal weight)
    {
        this.Type = type;
        this.Weight = weight;
    }
    public string Type
    {
        get { return this.type; }
        private set
        {
            if (value.ToLower() != "meat" && value.ToLower() != "veggies" && value.ToLower() != "cheese" && value.ToLower() != "sauce")
            {
                throw new Exception($"Cannot place {value} on top of your pizza.");
            }
            this.type = value.ToLower();
        }
    }

    public decimal Weight
    {
        get { return this.weight; }
        private set
        {
            if (value < 1 && value > 50)
            {
                throw new Exception($"{value} weight should be in the range [1..50].");
            }
            this.weight = value;
        }
    }


    public decimal CalculateCalories()
    {
        if (this.type == "meat")
        {
            caloriesPerGram = 1.2m;
        }
        else if (this.type == "veggies")
        {
            caloriesPerGram = 0.8m;
        }
        else if (this.type == "cheese")
        {
            caloriesPerGram = 1.1m;
        }
        else
        {
            caloriesPerGram = 0.9m;
        }
        return 2 * weight * caloriesPerGram;
    }
}