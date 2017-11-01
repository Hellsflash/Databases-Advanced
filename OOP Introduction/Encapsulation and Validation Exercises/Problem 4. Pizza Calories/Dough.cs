using System;

class Dough
{
    private decimal caloriesPerGramFlour;
    private decimal caloriesPerGramBakingTechnique;
    private string flourType;
    private string bakingTechnique;
    private decimal weight;

    public Dough(string flourType, string bakingTechnique, decimal weight)
    {
        this.FlourType = flourType;
        this.BakingTechnique = bakingTechnique;
        this.Weight = weight;
    }
    public decimal Weight
    {
        get { return this.weight; }
        private set
        {
            if (weight < 1 && weight > 200)
            {
                throw new Exception("Dought weight should be in the range [1..200].");
            }
            this.weight = value;
        }
    }


    public string BakingTechnique
    {
        get { return this.bakingTechnique; }
        private set
        {
            if (value.ToLower() != "homemade" && value.ToLower() != "crispy" && value.ToLower() != "chewy")
            {
                throw new Exception("Invalid type of dough.");
            }

            this.bakingTechnique = value.ToLower();
        }
    }

    public string FlourType
    {
        get { return this.flourType; }
        private set
        {
            if (value.ToLower() != "white" && value.ToLower() != "wholegrain")
            {
                throw new Exception("Invalid type of dough.");
            }
            this.flourType = value.ToLower();
        }
    }

    public decimal CalculateCalories()
    {
        if (this.flourType == "white")
        {
            caloriesPerGramFlour = 1.5m;
        }
        else
        {
            caloriesPerGramFlour = 1.0m;
        }


        if (this.bakingTechnique == "crispy")
        {
            caloriesPerGramBakingTechnique = 0.9m;
        }
        else if (this.bakingTechnique == "chewy")
        {
            caloriesPerGramBakingTechnique = 1.1m;
        }
        else
        {
            caloriesPerGramBakingTechnique = 1.0m;
        }

        return (2 * weight) * caloriesPerGramBakingTechnique * caloriesPerGramFlour;
    }
}