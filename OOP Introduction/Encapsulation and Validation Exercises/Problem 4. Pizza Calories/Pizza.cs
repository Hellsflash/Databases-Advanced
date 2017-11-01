using System;
using System.Collections.Generic;
using System.Linq;

class Pizza
{
    public Pizza(string name, Dough dough)
    {
        this.Name = name;
        this.Dough = dough;
        this.Toppings = new List<Topping>();
    }
    private string name;
    private List<Topping> toppings;
    private Dough dough;
    private int toppingCount;

    public string Name
    {
        get { return this.name; }
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new Exception($"Pizza name should be between 1 and 15 symbols.");
            }
            this.name = value;
        }
    }

    public decimal TotalCalories
    {
        get { return this.CalculateTotalCalories(); }
    }

    public int ToppingCount
    {
        get { return this.toppingCount; }
        set
        {
            if (value < 0 || value > 10)
            {
                throw new Exception("Number of toppings should be in range[0..10].");
            }
            this.toppingCount = value;
        }
    }

    public Dough Dough
    {
        get { return this.dough; }
        set { this.dough = value; }
    }

    public List<Topping> Toppings
    {
        get { return this.toppings; }
        set { this.toppings = value; }
    }

    public decimal CalculateTotalCalories()
    {
        return dough.CalculateCalories() + toppings.Sum(c => c.CalculateCalories());
    }

    public void AddTopping(Topping topping)
    {
        toppings.Add(topping);
    }
}